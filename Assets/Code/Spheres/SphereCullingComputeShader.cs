using System;
using Code.Utils.SubFrustums;
using UnityEngine;

namespace Code.RenderFeature
{
    public class SphereCullingComputeShader : IDisposable
    {
        private readonly ComputeBuffer _subFrustums;
        private readonly ComputeBuffer _activeTiles;
        private readonly ComputeBuffer _spheres;
        private readonly ComputeShader _shader;

        public SphereCullingComputeShader(ComputeShader shader, ComputeBuffer spheres, int tiles)
        {
            _subFrustums = new ComputeBuffer(tiles, Frustum.GetSize());
            _activeTiles = new ComputeBuffer(tiles, sizeof(int));
            _spheres = spheres;
            _shader = shader;
        }

        public void PassParameters(Frustum[] subFrustums, int spheresCount)
        {
            _subFrustums.SetData(subFrustums);
            _shader.SetBuffer(0, "_ActiveTiles", _activeTiles);
            _shader.SetBuffer(0, "_Spheres", _spheres);
            _shader.SetBuffer(0, "_SubFrustums", _subFrustums);
            _shader.SetInt("_SpheresCount", spheresCount);
        }

        public void Dispatch(Transform cameraTransform)
        {
            _shader.SetMatrix("_CameraWorldToLocal", cameraTransform.worldToLocalMatrix);
            _shader.Dispatch(0, _subFrustums.count, 1, 1);
        }

        public void Dispose()
        {
            _subFrustums.Dispose();
            _activeTiles.Dispose();
        }
    }
}