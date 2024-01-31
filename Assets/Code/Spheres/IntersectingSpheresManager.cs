using System;
using System.Collections.Generic;
using Code.Utils.SubFrustums;
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
        [SerializeField] private ComputeShader _cullingShader;
        [SerializeField] private int _maxSpheres = 500;
        [SerializeField] private int _tileSizeX = 10;
        [SerializeField] private int _tileSizeY = 10;
        [SerializeField] private Material _material;
        private readonly List<SphereData> _spheresData = new();
        private SphereCullingComputeShader _cullingShaderFacade;
        private ComputeBuffer _spheresBuffer;
        private bool _isDisposed;

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
            if (_isDisposed == false)
            {
                _spheresData.Clear();
            
                foreach (IntersectingSphere sphere in _spheresView)
                {
                    _spheresData.Add(sphere.Data);
                }
                
                _spheresBuffer?.Dispose();
                _spheresBuffer = new ComputeBuffer(_maxSpheres, SphereData.GetSize());
                _spheresBuffer.SetData(_spheresData);
                _material.SetBuffer("_Spheres", _spheresBuffer);
                _material.SetInt("_SpheresCount", _spheresData.Count);

                SubFrustumsCalculator subFrustumsCalculator = new(Camera.main, _tileSizeX, _tileSizeY);
                Frustum[] subFrustums = subFrustumsCalculator.Evaluate();
                
                _cullingShaderFacade?.Dispose();
                _cullingShaderFacade = new SphereCullingComputeShader(_cullingShader, _spheresBuffer, _tileSizeX * _tileSizeY);
                _cullingShaderFacade.PassParameters(subFrustums, _spheresData.Count);
                _cullingShaderFacade.Dispatch(Camera.main.transform);
            }
        }

        private void OnDestroy()
        {
            _isDisposed = true;
            Dispose();
        }

        public void Dispose()
        {
            _spheresData?.Clear();
            _spheresBuffer?.Release();
            _cullingShaderFacade?.Dispose();
        }
    }
}