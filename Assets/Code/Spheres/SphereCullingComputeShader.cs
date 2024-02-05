using UnityEngine;

namespace Code.RenderFeature
{
    public class SphereCullingComputeShader
    {
        private readonly IntersectingSpheresData _data;
        private readonly ComputeShader _shader;

        public SphereCullingComputeShader(ComputeShader shader, IntersectingSpheresData data)
        {
            _shader = shader;
            _data = data;
        }

        public void Setup()
        {
            _shader.SetBuffer(0, "_SpheresInTileCount", _data.SpheresInTileCount);
            _shader.SetBuffer(0, "_Spheres", _data.Spheres);
            _shader.SetBuffer(0, "_SubFrustums", _data.SubFrustums);
            _shader.SetBuffer(0, "_SpheresInTile", _data.SpheresInTile);
            _shader.SetInt("_SpheresCount", _data.SpheresCount);
            _shader.SetInt("_MaxSpheresInTile", _data.MaxSpheresInTile);
        }

        public void Dispatch(Transform cameraTransform)
        {
            ClearActiveTiles();
            _shader.SetMatrix("_CameraWorldToLocal", cameraTransform.worldToLocalMatrix);
            _shader.Dispatch(0, Mathf.CeilToInt(_data.TilesCount / 8f), Mathf.CeilToInt(_data.SpheresCount / 8f), 1);
        }

        private void ClearActiveTiles()
        {
            int[] activeTiles = new int[_data.TilesCount];
            _data.SpheresInTileCount.SetData(activeTiles);
        }
    }
}