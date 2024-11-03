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
        Start();
    }

    private void Start()
    {
        Dispose();
        Reassemble();
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

    private void OnDestroy()
    {
        Dispose();
    }
    
    protected abstract void Reassemble();
    protected abstract void Dispose();
}