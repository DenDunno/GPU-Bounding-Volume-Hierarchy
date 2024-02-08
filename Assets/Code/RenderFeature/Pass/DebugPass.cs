using Code.RenderFeature.Data;
using Code.Utils.SubFrustums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.RenderFeature.Pass
{
    public class DebugPass
    {
        private readonly ComputeBuffer _circles;
        private readonly Material _material;
        private readonly bool _useDebug;

        public DebugPass(Material debugMaterial, bool useDebug, ComputeBuffer circles)
        {
            _material = debugMaterial;
            _useDebug = useDebug;
            _circles = circles;
        }
        
        public void PassDataToMaterial()
        {
            _material.SetBuffer("_Circles", _circles);
        }
        
        public void TryDraw(BlitArguments blitArgs, int visibleCircles, Camera camera)
        {
            if (_material != null && _useDebug)
            {
                _material.SetInt("_VisibleCirclesCount", visibleCircles);
                _material.SetVector("_CameraParams", camera.GetNearClipPlaneParams());

                Blitter.BlitCameraTexture(blitArgs.CommandBuffer, blitArgs.Source, blitArgs.Destination);
                Blitter.BlitCameraTexture(blitArgs.CommandBuffer, blitArgs.Destination, blitArgs.Source, _material, 0);
            }
        }
    }
}