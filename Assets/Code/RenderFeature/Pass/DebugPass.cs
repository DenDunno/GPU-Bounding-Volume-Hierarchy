using Code.RenderFeature.Data;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.RenderFeature.Pass
{
    public class DebugPass
    {
        private readonly IntersectingSpheresBuffers _buffers;
        private readonly Vector2Int _tiles;
        private readonly Material _material;
        private readonly bool _useDebug;

        public DebugPass(Vector2Int tiles, IntersectingSpheresBuffers buffers, Material debugMaterial, bool useDebug)
        {
            _material = debugMaterial;
            _useDebug = useDebug;
            _buffers = buffers;
            _tiles = tiles;
        }
        
        public void PassDataToMaterial()
        {
            _material.SetBuffer("_ActiveTiles", _buffers.SpheresInTileCount);
            _material.SetInt("_TilesCountX", _tiles.x);
            _material.SetInt("_TilesCountY", _tiles.y);
        }
        
        public void TryDraw(CommandBuffer commandBuffer, RTHandle copiedColor, RTHandle source)
        {
            if (_material != null && _useDebug)
            {
                _material.SetInt("_SpheresCount", _buffers.SpheresCount);
                Blitter.BlitCameraTexture(commandBuffer, source, copiedColor);
                Blitter.BlitCameraTexture(commandBuffer, copiedColor, source, _material, 0);    
            }
        }
    }
}