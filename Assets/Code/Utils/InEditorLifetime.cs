using Code.Components.MortonCodeAssignment;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[DefaultExecutionOrder(-10)]
public abstract class InEditorLifetime<TData> : MonoBehaviour where TData : IValidatedData
{
    [SerializeField] private TData _data;

    public TData Data => _data;
    
    protected void OnValidate()
    {
        Start();
    }

    private void Start()
    {
        if (_data != null)
        {
            _data.OnValidate();

            if (_data.IsValid())
            {
                Dispose();
                Reassemble(_data);   
            }
        }
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
    
    protected abstract void Reassemble(TData data);
    protected abstract void Dispose();
}