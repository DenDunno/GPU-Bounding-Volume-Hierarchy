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
        RenderingUtils.ReAllocateIfNeeded(ref _copiedColor, colorCopyDescriptor, name: "_FullscreenPassColorCopy");
    }

    public void Dispose()
    {
        _copiedColor?.Release();
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
                Draw(renderingData, commandBuffer);
            }
 
            Release(context, commandBuffer);
        }
    }

    private static bool IsValidState(in RenderingData renderingData)
    {
        return _material != null && renderingData.cameraData.isPreviewCamera == false;
    }

    private void Draw(RenderingData renderingData, CommandBuffer commandBuffer)
    {
        RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;

        Blitter.BlitCameraTexture(commandBuffer, source, _copiedColor);
        _material.SetFloat(_intensityID, _intensity);

        Blitter.BlitCameraTexture(commandBuffer, _copiedColor, source, _material, 0);
    }

    private static void Release(ScriptableRenderContext context, CommandBuffer commandBuffer)
    {
        context.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();
        CommandBufferPool.Release(commandBuffer);
    }
}