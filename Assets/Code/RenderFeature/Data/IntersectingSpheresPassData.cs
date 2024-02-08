using System;
using UnityEngine;

namespace Code.RenderFeature.Data
{
    [Serializable]
    public class IntersectingSpheresPassData
    {
        [SerializeField] [Min(1)] private int _maxSpheres = 500;
        [SerializeField] private InjectionPoint _injectionPoint = InjectionPoint.AfterRenderingPostProcessing;
        [SerializeField] private ComputeShader _frustumCullingShader;
        [SerializeField] private Material _raytracingMaterial;
        [SerializeField] private Material _debugMaterial;
        [SerializeField] private bool _debug;

        public ComputeShader FrustumCullingShader => _frustumCullingShader;
        public Material RaytracingMaterial => _raytracingMaterial;
        public InjectionPoint InjectionPoint => _injectionPoint;
        public Material DebugMaterial => _debugMaterial;
        public int MaxSpheres => _maxSpheres;
        public bool UseDebug => _debug;


        public bool IsValid => _raytracingMaterial != null &&
                               _frustumCullingShader != null && 
                               _debugMaterial != null;
    }
}