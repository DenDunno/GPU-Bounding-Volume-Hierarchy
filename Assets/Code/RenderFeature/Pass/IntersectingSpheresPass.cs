using System.Collections.Generic;
using Code.RenderFeature.ComputeShaders;
using Code.RenderFeature.Data;
using Code.Utils;
using Code.Utils.SubFrustums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Code.RenderFeature.Pass
{
    public class IntersectingSpheresPass : BaseRenderPass
    {
        private readonly ResetTilesDataComputeShader _resetTilesShader;
        private readonly SphereCullingComputeShader _cullingShader;
        private readonly GPUBasedFrustumCulling _frustumCulling;
        private readonly IntersectingSpheresBuffers _buffers;
        private readonly IntersectingSpheresPassData _data;
        private readonly RaytracingPass _raytracingPass;
        private readonly CopiedColor _copiedColor;
        private readonly DebugPass _debugPass;

        public IntersectingSpheresPass(IntersectingSpheresPassData data)
        {
            _buffers = new IntersectingSpheresBuffers(data.TilesCount, data.MaxSpheres, data.MaxSpheresInTile);
            _resetTilesShader = new ResetTilesDataComputeShader(data.ResetTilesDataShader, _buffers.SpheresInTileCount, data.TilesCount);
            _debugPass = new DebugPass(data.TilesSize, _buffers, data.DebugMaterial, data.UseDebug);
            _raytracingPass = new RaytracingPass(_buffers, data.RaytracingMaterial, data.TilesSize);
            _frustumCulling = new GPUBasedFrustumCulling(data.FrustumCullingShader, _buffers);
            _cullingShader = new SphereCullingComputeShader(data.CullingShader, _buffers);
            _copiedColor = new CopiedColor("FullscreenPassColorCopy");
            renderPassEvent = (RenderPassEvent)data.InjectionPoint;
            _data = data;
        }
        
        public void Setup(ref CameraData cameraData, List<SphereData> sphereData)
        {
            Frustum[] subFrustums = new SubFrustumsCalculator(cameraData.camera, _data.TilesSize).Evaluate();
            Frustum[] cameraFrustum = new SubFrustumsCalculator(cameraData.camera, Vector2Int.one).Evaluate();
            
            _buffers.Update(subFrustums, sphereData);
            _copiedColor.ReAllocateIfNeeded(cameraData.cameraTargetDescriptor);
            _frustumCulling.PassData(cameraFrustum);
            _raytracingPass.PassDataToMaterial();
            _debugPass.PassDataToMaterial();
            _resetTilesShader.PassData();
            _cullingShader.PassData();
        }

        protected override void Draw(in RenderingData renderingData, CommandBuffer commandBuffer)
        {
            RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            Camera camera = renderingData.cameraData.camera;
            
            int visibleSpheresCount = _frustumCulling.Dispatch(camera.transform);
            _resetTilesShader.Dispatch();
            _cullingShader.Dispatch(camera.transform);
            _raytracingPass.Draw(commandBuffer, _copiedColor.RTHandle, source, camera);
            _debugPass.TryDraw(commandBuffer, _copiedColor.RTHandle, source);
        }

        public void Dispose()
        {
            _frustumCulling?.Dispose();
            _copiedColor?.Dispose();
            _buffers?.Dispose();
        }
    }
}