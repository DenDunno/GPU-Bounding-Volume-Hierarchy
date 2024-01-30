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
        [SerializeField] private List<IntersectingSphere> _spheresView;
        [SerializeField] private Material _material;
        [SerializeField] private int _maxSpheres = 500;
        private readonly List<SphereData> _spheresData = new();
        private ComputeBuffer _buffer;
        private bool _isDestroying;
        
        private void OnValidate()
        {
            if (_spheresView != null)
            {
                foreach (IntersectingSphere sphere in _spheresView)
                {
                    sphere?.InjectOnChangedCallback(this);
                }
            }
        }
        
        #if  UNITY_EDITOR
        private void OnEnable()
        {
            AssemblyReloadEvents.beforeAssemblyReload += Dispose;
        }

        private void OnDisable()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= Dispose;
        }
        #endif

        public void Add(IntersectingSphere sphere)
        {
            _spheresView.Add(sphere);
        }
        
        public void Remove(IntersectingSphere sphere)
        {
            _spheresView.Remove(sphere);
        }

        public void UpdateBuffer()
        {
            if (_isDestroying == false)
            {
                _spheresData.Clear();
            
                foreach (IntersectingSphere sphere in _spheresView)
                {
                    _spheresData.Add(sphere.Data);
                }
                
                _buffer ??= new ComputeBuffer(_maxSpheres, SphereData.GetSize());
                _buffer.SetData(_spheresData);
                _material.SetBuffer("_Spheres", _buffer);
                _material.SetInt("_SpheresCount", _spheresData.Count);
            }
        }

        private void OnDestroy()
        {
            _isDestroying = true;
            Dispose();
        }

        public void Dispose()
        {
            _spheresData?.Clear();
            _buffer?.Release();
        }
    }
}