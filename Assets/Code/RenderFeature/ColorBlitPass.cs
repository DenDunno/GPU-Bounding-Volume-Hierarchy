using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorBlitPass : ScriptableRenderPass
{
    private static Material _material;
    private RTHandle _copiedColor;
    private float _intensity;
    private static readonly int _intensityID = Shader.PropertyToID("_Intensity");

    public void Setup(Material material, ref RenderingData renderingData, float intensity)
    {
        _material = material;
        _intensity = intensity;
        
        RenderTextureDescriptor colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
        RenderingUtils.ReAllocateIfNeeded(ref _copiedColor, colorCopyDescriptor, name: "FullscreenPassColorCopy");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ResetTarget();
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (IsValidState(in renderingData))
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get();
            
            using (new ProfilingScope(commandBuffer, profilingSampler))
            {
                Draw(in renderingData, commandBuffer);
            }
 
            Release(context, commandBuffer);
        }
    }

    private static bool IsValidState(in RenderingData renderingData)
    {
        return _material != null && renderingData.cameraData.isPreviewCamera == false;
    }

    private void Draw(in RenderingData renderingData, CommandBuffer commandBuffer)
    {
        RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;

        Blitter.BlitCameraTexture(commandBuffer, source, _copiedColor);
        
        Vector4 cameraParameters = GetCameraParameters(renderingData.cameraData.camera);
        _material.SetFloat(_intensityID, _intensity);
        _material.SetVector("_CameraParams", cameraParameters);

        Blitter.BlitCameraTexture(commandBuffer, _copiedColor, source, _material, 0);
    }
    
    private Vector4 GetCameraParameters(Camera camera)
    {
        Vector3 bottomLeftPoint = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 bottomRightPoint = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
        Vector3 upperLeftPoint = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));

        float planeHeight = (bottomLeftPoint - upperLeftPoint).magnitude;
        float planeWidth = (bottomLeftPoint - bottomRightPoint).magnitude;
        
        return new Vector4(planeWidth, planeHeight, camera.nearClipPlane, 0);
    }

    private static void Release(ScriptableRenderContext context, CommandBuffer commandBuffer)
    {
        context.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();
        CommandBufferPool.Release(commandBuffer);
    }

    public void Dispose()
    {
        _copiedColor?.Release();
    }
}