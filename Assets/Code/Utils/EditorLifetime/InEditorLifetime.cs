using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[DefaultExecutionOrder(-10)]
public abstract class InEditorLifetime : MonoBehaviour 
{
    [SerializeField, HideInInspector] private bool _isInitialized;

    [Button]
    protected void OnValidate()
    {
        Regenerate();
    }

    private void Start()
    {
        Regenerate();
    }

    #if UNITY_EDITOR
    private void OnEnable()
    {
        if (_isInitialized == false)
        {
            _isInitialized = true;
            OnScriptAttached();
        }
        
        AssemblyReloadEvents.beforeAssemblyReload += Dispose;
    }

    private void OnDisable()
    {
        AssemblyReloadEvents.beforeAssemblyReload -= Dispose;
    }
    #endif

    protected virtual void Regenerate()
    {
        Dispose();
        Reassemble();
    }
    
    private void OnDestroy()
    {
        Dispose();
    }

    protected virtual void OnScriptAttached() {} 
    protected virtual void Reassemble() {}
    protected virtual void Dispose() {}
}