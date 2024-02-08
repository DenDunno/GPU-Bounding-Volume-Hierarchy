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
        private readonly VisibleSpheresProjection _frustumCulling;
        private readonly RaytracingPass _raytracingPass;
        private readonly CopiedColor _copiedColor;
        private readonly SharedBuffers _buffers;
        private readonly DebugPass _debugPass;

        public IntersectingSpheresPass(IntersectingSpheresPassData data)
        {
            _buffers = new SharedBuffers(data.MaxSpheres);
            _frustumCulling = new VisibleSpheresProjection(data.FrustumCullingShader, _buffers);
            _debugPass = new DebugPass(data.DebugMaterial, data.UseDebug, _buffers.Cirlces);
            _raytracingPass = new RaytracingPass(_buffers, data.RaytracingMaterial);
            _copiedColor = new CopiedColor("FullscreenPassColorCopy");
            renderPassEvent = (RenderPassEvent)data.InjectionPoint;
        }

        public void Setup(ref CameraData cameraData, List<SphereData> sphereData)
        {
            Frustum[] frustum = cameraData.camera.GetFrustum();
            
            _buffers.Update(sphereData);
            _copiedColor.ReAllocateIfNeeded(cameraData.cameraTargetDescriptor);
            _frustumCulling.PassData(frustum);
            _raytracingPass.PassDataToMaterial();
            _debugPass.PassDataToMaterial();
        }

        protected override void Draw(in RenderingData renderingData, CommandBuffer commandBuffer)
        {
            Camera camera = renderingData.cameraData.camera;
            RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            BlitArguments blitArguments = new(commandBuffer, source, _copiedColor.RTHandle);

            int visibleSpheresCount = _frustumCulling.Dispatch(camera);
            _raytracingPass.Draw(blitArguments, camera, visibleSpheresCount);
            _debugPass.TryDraw(blitArguments, visibleSpheresCount, camera);
        }
        
        public void Dispose()
        {
            _frustumCulling?.Dispose();
            _copiedColor?.Dispose();
            _buffers?.Dispose();
        }
    }
}

// Circle[] circles = new Circle[visibleSpheresCount];
// SphereData[] spheres = new SphereData[visibleSpheresCount];
//
// _buffers.Spheres.GetData(spheres);
//
// for (int i = 0; i < visibleSpheresCount; ++i)
// {
//     var clipSpace = WorldToScreenPoint(spheres[i].Position, camera);
//     Debug.Log(spheres[i].Position);
//     Debug.Log(clipSpace);
//     
//     circles[i].Position = clipSpace;
//     circles[i].Radius = 0.25f;
// }
//
// _buffers.Cirlces.SetData(circles);


// Circle[] circles = new Circle[visibleSpheresCount];
// _buffers.Cirlces.GetData(circles);
//
// string a = "";
//
// foreach (Circle circle in circles)
// {
//     a += circle.ToString();
// }
//
// Debug.Log(a);