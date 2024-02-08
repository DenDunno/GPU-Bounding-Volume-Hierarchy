using System;
using Code.RenderFeature.Data;
using Code.Utils.SubFrustums;
using UnityEngine;

namespace Code.RenderFeature.ComputeShaders
{
    public class VisibleSpheresProjection : IDisposable
    {
        private readonly ComputeBuffer _visibleSpheresCount;
        private readonly ComputeBuffer _cameraFrustumBuffer;
        private readonly int[] _fetchedVisibleSpheresCount;
        private readonly SharedBuffers _buffers;
        private readonly ComputeShader _shader;

        public VisibleSpheresProjection(ComputeShader shader, SharedBuffers buffers)
        {
            _buffers = buffers;
            _shader = shader;
            _visibleSpheresCount = new ComputeBuffer(1, sizeof(int), ComputeBufferType.IndirectArguments);
            _cameraFrustumBuffer = new ComputeBuffer(1, Frustum.GetSize());
            _fetchedVisibleSpheresCount = new int[1];
        }

        public void PassData(Frustum[] cameraFrustum)
        {
            _cameraFrustumBuffer.SetData(cameraFrustum);
            
            _shader.SetBuffer(0, "_VisibleSpheres", _buffers.VisibleSpheres);
            _shader.SetBuffer(0, "_Frustum", _cameraFrustumBuffer);
            _shader.SetBuffer(0, "_Spheres", _buffers.Spheres);
            _shader.SetBuffer(0, "_Circles", _buffers.Cirlces);
        }
        
        public int Dispatch(Camera camera)
        {
            _buffers.VisibleSpheres.SetCounterValue(0);
            _shader.SetMatrix("_CameraWorldToLocal", camera.transform.worldToLocalMatrix);
            _shader.SetVector("_CameraPosition", camera.transform.position);
            _shader.SetMatrix("_ProjectionMatrix", camera.projectionMatrix);
            _shader.SetInt("_SpheresCount", _buffers.SpheresCount);

            _shader.Dispatch(0, Mathf.CeilToInt(_buffers.SpheresCount / 8f), 1, 1);

            ComputeBuffer.CopyCount(_buffers.VisibleSpheres, _visibleSpheresCount, 0);
            _visibleSpheresCount.GetData(_fetchedVisibleSpheresCount);

            return _fetchedVisibleSpheresCount[0];
        }

        public void Dispose()
        {
            _visibleSpheresCount.Dispose();
            _cameraFrustumBuffer.Dispose();
        }
    }
}