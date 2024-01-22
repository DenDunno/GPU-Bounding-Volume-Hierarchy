using UnityEngine.Rendering.Universal;

public enum InjectionPoint
{
    BeforeRenderingTransparents = RenderPassEvent.BeforeRenderingTransparents,
    BeforeRenderingPostProcessing = RenderPassEvent.BeforeRenderingPostProcessing,
    AfterRenderingPostProcessing = RenderPassEvent.AfterRenderingPostProcessing
}