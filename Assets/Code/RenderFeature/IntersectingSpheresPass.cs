using System.Collections.Generic;
using Code.Utils.SubFrustums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Code.RenderFeature
{
    public class IntersectingSpheresPass : ScriptableRenderPass
    {
        private readonly IntersectingSpheresPassData _data;
        private SphereCullingComputeShader _cullingShader;
        private IntersectingSpheresBuffers _buffers;
        private RTHandle _copiedColor;

        public IntersectingSpheresPass(IntersectingSpheresPassData data)
        {
            renderPassEvent = (RenderPassEvent)data.InjectionPoint;
            _data = data;
        }
        
        public void Setup(ref CameraData cameraData, List<SphereData> sphereData)
        {
            _buffers?.Dispose();
            _buffers = new IntersectingSpheresBuffers(_data.TilesCount, _data.MaxSpheres, _data.MaxSpheresInTile);

            _cullingShader = new SphereCullingComputeShader(_data.CullingShader, _buffers);
            
            SubFrustumsCalculator subFrustumsCalculator = new(cameraData.camera, _data.TileSizeX, _data.TileSizeY);
            Frustum[] subFrustums = subFrustumsCalculator.Evaluate();
            _buffers.Update(subFrustums, sphereData);
            
            _cullingShader.Setup();
            
            RenderTextureDescriptor colorCopyDescriptor = cameraData.cameraTargetDescriptor;
            colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
            RenderingUtils.ReAllocateIfNeeded(ref _copiedColor, colorCopyDescriptor, name: "FullscreenPassColorCopy");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ResetTarget();
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (IsValidState(in renderingData))
            {
                CommandBuffer commandBuffer = CommandBufferPool.Get();
            
                using (new ProfilingScope(commandBuffer, profilingSampler))
                {
                    Draw(in renderingData, commandBuffer);
                }
 
                Release(context, commandBuffer);
            }
        }

        private bool IsValidState(in RenderingData renderingData)
        {
            return _data.RaytracingMaterial != null && renderingData.cameraData.isPreviewCamera == false;
        }

        private void Draw(in RenderingData renderingData, CommandBuffer commandBuffer)
        { 
            Camera camera = renderingData.cameraData.camera;
            
            _data.RaytracingMaterial.SetVector("_CameraParams", camera.GetNearClipPlaneParams());
            _data.RaytracingMaterial.SetBuffer("_Spheres", _buffers.Spheres);
            _data.RaytracingMaterial.SetBuffer("_SpheresInTileCount", _buffers.SpheresInTileCount);
            _data.RaytracingMaterial.SetBuffer("_SpheresInTile", _buffers.SpheresInTile);
            _data.RaytracingMaterial.SetInt("_SpheresCount", _buffers.SpheresCount);
            _data.RaytracingMaterial.SetInt("_TilesCountX", _data.TileSizeX);
            _data.RaytracingMaterial.SetInt("_TilesCountY", _data.TileSizeY);
            _data.RaytracingMaterial.SetInt("_MaxSpheresInTile", _buffers.MaxSpheresInTile);
            _cullingShader.Dispatch(camera.transform);
            
            RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            Blitter.BlitCameraTexture(commandBuffer, source, _copiedColor);
            Blitter.BlitCameraTexture(commandBuffer, _copiedColor, source, _data.RaytracingMaterial, 0);

            if (_data.DebugMaterial != null && _data.UseDebug)
            {
                _data.DebugMaterial.SetBuffer("_ActiveTiles", _buffers.SpheresInTileCount);
                _data.DebugMaterial.SetInt("_TilesCountX", _data.TileSizeX);
                _data.DebugMaterial.SetInt("_TilesCountY", _data.TileSizeY);
                _data.DebugMaterial.SetInt("_SpheresCount", _buffers.SpheresCount);
                
                Blitter.BlitCameraTexture(commandBuffer, source, _copiedColor);
                Blitter.BlitCameraTexture(commandBuffer, _copiedColor, source, _data.DebugMaterial, 0);    
            }
        }

        private void Release(ScriptableRenderContext context, CommandBuffer commandBuffer)
        {
            context.ExecuteCommandBuffer(commandBuffer);
            commandBuffer.Clear();
            CommandBufferPool.Release(commandBuffer);
        }

        public void Dispose()
        {
            _copiedColor?.Release();
            _buffers?.Dispose();
        }
    }
}