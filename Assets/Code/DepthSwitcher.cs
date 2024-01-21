using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class DepthSwitcher : MonoBehaviour
{
    [SerializeField] private ScriptableRendererFeature _feature;
    
    [ContextMenu("Toggle")]
    private void Toggle()
    {
        _feature.SetActive(_feature.isActive == false);
    }
}