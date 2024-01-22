using UnityEngine;
using UnityEngine.Rendering.Universal;

internal class ColorBlitRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private Material _passMaterial;
    [SerializeField] private RenderPassEvent _injectionPoint = RenderPassEvent.AfterRenderingPostProcessing;
    [SerializeField] private float _intensity;
    private ColorBlitPass _renderPass;
 
    public override void Create()
    {
        _renderPass = new ColorBlitPass
        {
            renderPassEvent = (RenderPassEvent)_injectionPoint
        };

        _renderPass.ConfigureInput(ScriptableRenderPassInput.Color);
    }
 
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_passMaterial != null)
        {
            _renderPass.Setup(_passMaterial, ref renderingData, _intensity);
            renderer.EnqueuePass(_renderPass);
        }
    }
    
    protected override void Dispose(bool disposing)
    {
        _renderPass.Dispose();
    }
}