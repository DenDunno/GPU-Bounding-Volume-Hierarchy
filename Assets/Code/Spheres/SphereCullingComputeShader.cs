using UnityEngine;

namespace Code.RenderFeature
{
    public class SphereCullingComputeShader
    {
        private readonly IntersectingSpheresBuffers _buffers;
        private readonly ComputeShader _shader;

        public SphereCullingComputeShader(ComputeShader shader, IntersectingSpheresBuffers buffers)
        {
            _shader = shader;
            _buffers = buffers;
        }

        public void PassData()
        {
            _shader.SetBuffer(0, "_SpheresInTileCount", _buffers.SpheresInTileCount);
            _shader.SetBuffer(0, "_Spheres", _buffers.Spheres);
            _shader.SetBuffer(0, "_SubFrustums", _buffers.SubFrustums);
            _shader.SetBuffer(0, "_SpheresInTile", _buffers.SpheresInTile);
            _shader.SetInt("_SpheresCount", _buffers.SpheresCount);
            _shader.SetInt("_MaxSpheresInTile", _buffers.MaxSpheresInTile);
        }

        public void Dispatch(Transform cameraTransform)
        {
            ClearActiveTiles();
            _shader.SetMatrix("_CameraWorldToLocal", cameraTransform.worldToLocalMatrix);
            _shader.Dispatch(0, Mathf.CeilToInt(_buffers.TilesCount / 8f), Mathf.CeilToInt(_buffers.SpheresCount / 8f), 1);
        }

        private void ClearActiveTiles()
        {
            int[] activeTiles = new int[_buffers.TilesCount];
            _buffers.SpheresInTileCount.SetData(activeTiles);
        }
    }
}