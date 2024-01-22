using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class ColorBlitPass : ScriptableRenderPass
{
    Material m_Material;
    float m_Intensity;
    private RTHandle m_CopiedColor;

    public ColorBlitPass(Material material)
    {
        m_Material = material;
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public void SetTarget(float intensity, RenderingData renderingData)
    {
        m_Intensity = intensity;
        
        RenderTextureDescriptor colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
        RenderingUtils.ReAllocateIfNeeded(ref m_CopiedColor, colorCopyDescriptor, name: "ColorBlitPass");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ResetTarget();
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        ref CameraData cameraData = ref renderingData.cameraData;
        CommandBuffer cmd = CommandBufferPool.Get();
        
        if (m_Material == null)
        {
            return;
        }

        using (new ProfilingScope(cmd, profilingSampler))
        {
            RTHandle source = cameraData.renderer.cameraColorTargetHandle;
 
            Blitter.BlitCameraTexture(cmd, source, m_CopiedColor);
            m_Material.SetTexture("_BlitTexture", m_CopiedColor);
            
            CoreUtils.SetRenderTarget(cmd, cameraData.renderer.cameraColorTargetHandle);
            CoreUtils.DrawFullScreen(cmd, m_Material);
        }
 
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
 
        CommandBufferPool.Release(cmd);
    }
    
    public void Dispose()
    {
        m_CopiedColor?.Release();
    }
}