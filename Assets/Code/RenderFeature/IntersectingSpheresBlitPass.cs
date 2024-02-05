using System.Collections.Generic;
using Code.Utils.SubFrustums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Code.RenderFeature
{
    public class IntersectingSpheresBlitPass : ScriptableRenderPass
    {
        private SphereCullingComputeShader _cullingShader;
        private IntersectingSpheresData _data;
        private Material _material;
        private RTHandle _copiedColor;
        private Material _debugMaterial;
        private int _tilesX;
        private int _tilesY;
        private bool _debug;

        public void Setup(Material material, ref RenderingData renderingData, int tilesX, int tilesY, int maxSpheres,
            ComputeShader cullingShader, Material debugMaterial, List<SphereData> sphereData, bool debug)
        {
            _debug = debug;
            _data?.Dispose();
            _data = new IntersectingSpheresData(tilesX * tilesY, maxSpheres, 100);
            _tilesX = tilesX;
            _tilesY = tilesY;
            _material = material;
            _debugMaterial = debugMaterial;

            SubFrustumsCalculator subFrustumsCalculator = new(renderingData.cameraData.camera, tilesX, tilesY);
            Frustum[] subFrustums = subFrustumsCalculator.Evaluate();
            _data.Update(subFrustums, sphereData);

            _cullingShader = new SphereCullingComputeShader(cullingShader, _data);
            _cullingShader.Setup();
            
            RenderTextureDescriptor colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
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
            return _material != null && renderingData.cameraData.isPreviewCamera == false;
        }

        private void Draw(in RenderingData renderingData, CommandBuffer commandBuffer)
        { 
            Camera camera = renderingData.cameraData.camera;
            
            _material.SetVector("_CameraParams", camera.GetNearClipPlaneParams());
            _material.SetBuffer("_Spheres", _data.Spheres);
            _material.SetBuffer("_SpheresInTileCount", _data.SpheresInTileCount);
            _material.SetBuffer("_SpheresInTile", _data.SpheresInTile);
            _material.SetInt("_SpheresCount", _data.SpheresCount);
            _material.SetInt("_TilesCountX", _tilesX);
            _material.SetInt("_TilesCountY", _tilesY);
            _material.SetInt("_MaxSpheresInTile", _data.MaxSpheresInTile);
            _cullingShader.Dispatch(camera.transform);
            
            RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            Blitter.BlitCameraTexture(commandBuffer, source, _copiedColor);
            Blitter.BlitCameraTexture(commandBuffer, _copiedColor, source, _material, 0);

            if (_debugMaterial != null && _debug)
            {
                _debugMaterial.SetBuffer("_ActiveTiles", _data.SpheresInTileCount);
                _debugMaterial.SetInt("_TilesCountX", _tilesX);
                _debugMaterial.SetInt("_TilesCountY", _tilesY);
                _debugMaterial.SetInt("_SpheresCount", _data.SpheresCount);
                
                Blitter.BlitCameraTexture(commandBuffer, source, _copiedColor);
                Blitter.BlitCameraTexture(commandBuffer, _copiedColor, source, _debugMaterial, 0);    
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
            _data?.Dispose();
        }
    }
}