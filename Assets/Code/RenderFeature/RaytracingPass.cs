using Code.Utils.SubFrustums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.RenderFeature
{
    public class RaytracingPass
    {
        private readonly IntersectingSpheresBuffers _buffer;
        private readonly Material _material;
        private readonly Vector2Int _tiles;

        public RaytracingPass(IntersectingSpheresBuffers buffer, Material material, Vector2Int tiles)
        {
            _buffer = buffer;
            _material = material;
            _tiles = tiles;
        }

        public void PassDataToMaterial()
        {
            _material.SetBuffer("_Spheres", _buffer.Spheres);
            _material.SetBuffer("_SpheresInTileCount", _buffer.SpheresInTileCount);
            _material.SetBuffer("_SpheresInTile", _buffer.SpheresInTile);
            _material.SetInt("_TilesCountX", _tiles.x);
            _material.SetInt("_TilesCountY", _tiles.y);
            _material.SetInt("_MaxSpheresInTile", _buffer.MaxSpheresInTile);
        }

        public void Draw(CommandBuffer commandBuffer, RTHandle copiedColor, RTHandle source, Camera camera)
        {
            _material.SetVector("_CameraParams", camera.GetNearClipPlaneParams());
            _material.SetInt("_SpheresCount", _buffer.SpheresCount);
            
            Blitter.BlitCameraTexture(commandBuffer, source, copiedColor);
            Blitter.BlitCameraTexture(commandBuffer, copiedColor, source, _material, 0);
        }
    }
}