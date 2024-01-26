using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorBlitPass : ScriptableRenderPass
{
    private static Material _material;
    private RTHandle _copiedColor;

    public void Setup(Material material, ref RenderingData renderingData)
    {
        _material = material;
        
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
                CoreUtils.DrawFullScreen(commandBuffer, _material);
            }
 
            Release(context, commandBuffer);
        }
    }

    private static bool IsValidState(in RenderingData renderingData)
    {
        return _material != null && renderingData.cameraData.isPreviewCamera == false;
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