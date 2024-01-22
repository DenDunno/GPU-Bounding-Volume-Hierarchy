using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class ColorBlitRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private Material _material;
    public float m_Intensity;

    ColorBlitPass m_RenderPass = null;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_RenderPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        m_RenderPass.ConfigureInput(ScriptableRenderPassInput.Color);
        m_RenderPass.SetTarget(m_Intensity, renderingData);
    }

    public override void Create()
    {
        m_RenderPass = new ColorBlitPass(_material);
    }

    protected override void Dispose(bool disposing)
    {
        m_RenderPass.Dispose();
    }
}