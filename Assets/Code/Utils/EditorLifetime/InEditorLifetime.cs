using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[DefaultExecutionOrder(-10)]
public abstract class InEditorLifetime : MonoBehaviour 
{
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

    protected abstract void Reassemble();
    protected virtual void Dispose() {}
}