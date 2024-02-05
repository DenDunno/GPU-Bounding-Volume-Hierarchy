using UnityEngine.Rendering;

namespace Code.RenderFeature
{
    public class DebugPass
    {
        public void TryDraw(CommandBuffer commandBuffer, RTHandle copiedColor, RTHandle source)
        {
            // if (_debugMaterial != null && _debug)
            // {
            //     _debugMaterial.SetBuffer("_ActiveTiles", _buffers.SpheresInTileCount);
            //     _debugMaterial.SetInt("_TilesCountX", _tilesX);
            //     _debugMaterial.SetInt("_TilesCountY", _tilesY);
            //     _debugMaterial.SetInt("_SpheresCount", _buffers.SpheresCount);
            //     
            //     Blitter.BlitCameraTexture(commandBuffer, source, copiedColor);
            //     Blitter.BlitCameraTexture(commandBuffer, copiedColor, source, _debugMaterial, 0);    
            // }
        }
    }
}