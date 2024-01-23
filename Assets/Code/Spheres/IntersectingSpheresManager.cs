using System.Collections.Generic;
using UnityEngine;
#if  UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.RenderFeature
{
    [ExecuteInEditMode]
    public class IntersectingSpheresManager : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private List<IntersectingSphere> _spheresView;
        [SerializeField] private List<SphereData> _spheresData;
        private ComputeBuffer _buffer;

        private void OnValidate()
        {
            foreach (IntersectingSphere sphere in _spheresView)
            {
                sphere?.InjectOnChangedCallback(UpdateBuffer);
            }
        }
        
        #if  UNITY_EDITOR
        private void OnEnable()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnDestroy;
        }

        private void OnDisable()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnDestroy;
        }
        #endif

        private void UpdateBuffer()
        {
            _spheresData.Clear();
            
            foreach (IntersectingSphere sphere in _spheresView)
            {
                _spheresData.Add(sphere.Data);
            }
            
            _buffer ??= new ComputeBuffer(100, SphereData.GetSize());
            _buffer.SetData(_spheresData);
            _material.SetBuffer("_Spheres", _buffer);
            _material.SetInt("_SpheresCount", _spheresData.Count);
        }

        private void OnDestroy()
        {
            _buffer?.Release();
        }
    }
}