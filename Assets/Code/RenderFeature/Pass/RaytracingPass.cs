

namespace Code.RenderFeature.Pass
{
    public class RaytracingPass
    {
        // private readonly SharedBuffers _buffer;
        // private readonly Material _material;
        //
        // public RaytracingPass(SharedBuffers buffer, Material material)
        // {
        //     _buffer = buffer;
        //     _material = material;
        // }
        //
        // public void PassDataToMaterial()
        // {
        //     _material.SetBuffer("_VisibleSpheres", _buffer.VisibleSpheres);
        //     _material.SetBuffer("_Spheres", _buffer.Spheres);
        // }
        //
        // public void Draw(BlitArguments blitArgs, Camera camera, int visibleSpheresCount)
        // {
        //     _material.SetVector("_CameraParams", camera.GetNearClipPlaneParams());
        //     _material.SetInt("_VisibleSpheresCount", visibleSpheresCount);
        //
        //     Blitter.BlitCameraTexture(blitArgs.CommandBuffer, blitArgs.Source, blitArgs.Destination);
        //     Blitter.BlitCameraTexture(blitArgs.CommandBuffer, blitArgs.Destination, blitArgs.Source, _material, 0);
        //}
    }
}