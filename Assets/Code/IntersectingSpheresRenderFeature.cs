using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IntersectingSpheresRenderFeature : ScriptableRendererFeature
{
    [SerializeField] private int _a;
    
    public override void Create()
    {
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
    }
}