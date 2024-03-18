using Code.Utils;
using UnityEditor;
using UnityEngine;

namespace Code.Core
{
    [ExecuteInEditMode]
    public class SpheresManager : MonoBehaviour
    {
        [SerializeField] private SpheresData _data;
        private SpheresComponents _components;
        private SphereBuffers _buffers;
        
        private void Start()
        {
            Reassemble();
        }
        
        private void OnValidate()
        {
            Reassemble();
        }
        
        private void Reassemble()
        {
            if (_data != null)
            {
                Dispose();
                _data.Initialize();
                _buffers = new SphereBuffers(_data.SpheresCount);
                _components = new SpheresComponents(_data, _buffers);
                _components.Initialize();
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

        private void Update()
        {
            _components.SpheresBoundUpdate.UpdateBuffer();
            _components.MortonCodeAssignment.Dispatch(_data.SpheresCount);
        }
        
        private void OnDrawGizmos()
        {
           SphereDebugUtils.DrawSpheresBounds(_data);
        }

        private void Dispose()
        {
            _components?.Dispose();
            _buffers?.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}