using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace Code.View
{
    [ExecuteInEditMode]
    public class SpheresManager : MonoBehaviour
    {
        // [SerializeField] private SpheresData _data;
        // private SpheresComponents _components;
        // private NativeArray<BoundingBox> _bounds;
        //
        // private void Start()
        // {
        //     Reassemble();
        // }
        //
        // private void OnValidate()
        // {
        //     Reassemble();
        // }
        //
        // private void Reassemble()
        // {
        //     if (_data != null)
        //     {
        //         _components?.Dispose();
        //         _components = new SpheresComponents(_data);
        //     }
        // }
        //
        // #if UNITY_EDITOR        
        // private void OnEnable()
        // {
        //     AssemblyReloadEvents.beforeAssemblyReload += Dispose;
        // }
        //
        // private void OnDisable()
        // {
        //     AssemblyReloadEvents.beforeAssemblyReload -= Dispose;
        // }
        // #endif
        //
        // private void Dispose()
        // {
        //     _components?.Dispose();
        // }
        //
        // private void OnDrawGizmos()
        // {
        //     for (int i = 0; i < _data.SpheresCount; ++i)
        //     {
        //         // BoundingBox bound = _bounds[i];
        //         // Gizmos.DrawWireCube(bound.Centre, bound.Size);
        //         // Gizmos.DrawSphere(bound.Max, 0.1f);
        //         // Gizmos.DrawSphere(bound.Min, 0.1f);
        //     }
        // }
        //
        // private void OnDestroy()
        // {
        //     Dispose();
        // }
    }
}