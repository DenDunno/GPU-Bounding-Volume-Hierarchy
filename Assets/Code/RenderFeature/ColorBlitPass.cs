using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorBlitPass : ScriptableRenderPass
{
    private static readonly int _blitTextureShaderID = Shader.PropertyToID("_BlitTexture");
    private static Material _material;
    private RTHandle _copiedColor;
    private float _intensity;

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
            RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            PassDataToMaterial(commandBuffer, source);
            Draw(context, commandBuffer, source);
            Release(commandBuffer);
        }
    }

    private static bool IsValidState(in RenderingData renderingData)
    {
        return _material != null && renderingData.cameraData.isPreviewCamera == false;
    }

    private void PassDataToMaterial(CommandBuffer commandBuffer, RTHandle source)
    {
        Blitter.BlitCameraTexture(commandBuffer, source, _copiedColor);
        _material.SetTexture(_blitTextureShaderID, _copiedColor);
        _material.SetFloat("_Intensity", _intensity);
    }

    private static void Draw(ScriptableRenderContext context, CommandBuffer commandBuffer, RTHandle source)
    {
        CoreUtils.SetRenderTarget(commandBuffer, source);
        CoreUtils.DrawFullScreen(commandBuffer, _material);
        context.ExecuteCommandBuffer(commandBuffer);
    }

    private static void Release(CommandBuffer commandBuffer)
    {
        commandBuffer.Clear();
        CommandBufferPool.Release(commandBuffer);
    }
}