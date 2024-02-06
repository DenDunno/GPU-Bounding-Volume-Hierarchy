using System.Collections.Generic;
using Code.Utils;
using Code.Utils.SubFrustums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Code.RenderFeature
{
    public class IntersectingSpheresPass : BaseRenderPass
    {
        private readonly SphereCullingComputeShader _cullingShader;
        private readonly IntersectingSpheresBuffers _buffers;
        private readonly IntersectingSpheresPassData _data;
        private readonly RaytracingPass _raytracingPass;
        private readonly CopiedColor _copiedColor;
        private readonly DebugPass _debugPass;

        public IntersectingSpheresPass(IntersectingSpheresPassData data)
        {
            _buffers = new IntersectingSpheresBuffers(data.TilesCount, data.MaxSpheres, data.MaxSpheresInTile);
            _debugPass = new DebugPass(data.TilesSize, _buffers, data.DebugMaterial, data.UseDebug);
            _raytracingPass = new RaytracingPass(_buffers, data.RaytracingMaterial, data.TilesSize);
            _cullingShader = new SphereCullingComputeShader(data.CullingShader, _buffers);
            _copiedColor = new CopiedColor("FullscreenPassColorCopy");
            renderPassEvent = (RenderPassEvent)data.InjectionPoint;
            _data = data;
        }
        
        public void Setup(ref CameraData cameraData, List<SphereData> sphereData)
        {
            Frustum[] subFrustums = new SubFrustumsCalculator(cameraData.camera, _data.TilesSize).Evaluate();
            _buffers.Update(subFrustums, sphereData);
            _cullingShader.PassData();
            _debugPass.PassDataToMaterial();
            _raytracingPass.PassDataToMaterial();
            _copiedColor.ReAllocateIfNeeded(cameraData.cameraTargetDescriptor);
        }

        protected override void Draw(in RenderingData renderingData, CommandBuffer commandBuffer)
        {
            RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            Camera camera = renderingData.cameraData.camera;
            
            _cullingShader.Dispatch(camera.transform);
            _raytracingPass.Draw(commandBuffer, _copiedColor.RTHandle, source, camera);
            _debugPass.TryDraw(commandBuffer, _copiedColor.RTHandle, source);
        }

        public void Dispose()
        {
            _copiedColor?.Dispose();
            _buffers?.Dispose();
        }
    }
}