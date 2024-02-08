using System;
using Code.RenderFeature.Data;
using Code.Utils.SubFrustums;
using UnityEngine;

namespace Code.RenderFeature.ComputeShaders
{
    public class GPUBasedFrustumCulling : IDisposable
    {
        private readonly IntersectingSpheresBuffers _buffers;
        private readonly ComputeBuffer _visibleSpheresCount;
        private readonly int[] _fetchedVisibleSpheresCount;
        private readonly ComputeShader _shader;

        public GPUBasedFrustumCulling(ComputeShader shader, IntersectingSpheresBuffers buffers)
        {
            _buffers = buffers;
            _shader = shader;
            _visibleSpheresCount = new ComputeBuffer(1, sizeof(int), ComputeBufferType.IndirectArguments);
            _fetchedVisibleSpheresCount = new int[1];
        }

        public void PassData(Frustum[] cameraFrustum)
        {
            _buffers.CameraFrustumBuffer.SetData(cameraFrustum);
            
            _shader.SetBuffer(0, "_VisibleSpheres", _buffers.VisibleSpheres);
            _shader.SetBuffer(0, "_Spheres", _buffers.Spheres);
            _shader.SetBuffer(0, "_Frustum", _buffers.CameraFrustumBuffer);
        }
        
        public int Dispatch(Transform cameraTransform)
        {
            _buffers.VisibleSpheres.SetCounterValue(0);
            _shader.SetMatrix("_CameraWorldToLocal", cameraTransform.worldToLocalMatrix);
            _shader.SetInt("_SpheresCount", _buffers.SpheresCount);
            _shader.Dispatch(0, Mathf.CeilToInt(_buffers.SpheresCount / 8f), 1, 1);

            ComputeBuffer.CopyCount(_buffers.VisibleSpheres, _visibleSpheresCount, 0);
            _visibleSpheresCount.GetData(_fetchedVisibleSpheresCount);

            return _fetchedVisibleSpheresCount[0];
        }

        public void Dispose()
        {
            _visibleSpheresCount?.Dispose();
        }
    }
}