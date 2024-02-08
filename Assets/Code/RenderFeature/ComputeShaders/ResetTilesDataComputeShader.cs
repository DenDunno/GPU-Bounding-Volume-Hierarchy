using UnityEngine;

namespace Code.RenderFeature.ComputeShaders
{
    public class ResetTilesDataComputeShader
    {
        private readonly ComputeBuffer _spheresInTileCount;
        private readonly ComputeShader _shader;
        private readonly int _tilesCount;

        public ResetTilesDataComputeShader(ComputeShader shader, ComputeBuffer spheresInTileCount, int tilesCount)
        {
            _spheresInTileCount = spheresInTileCount;
            _tilesCount = tilesCount;
            _shader = shader;
        }

        public void PassData()
        {
            _shader.SetBuffer(0, "_SpheresInTileCount", _spheresInTileCount);
        }

        public void Dispatch()
        {
            _shader.Dispatch(0, Mathf.CeilToInt(_tilesCount / 8f), 1, 1);
        }
    }
}