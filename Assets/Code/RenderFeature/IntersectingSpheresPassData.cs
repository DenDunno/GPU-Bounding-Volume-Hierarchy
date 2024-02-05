using System;
using UnityEngine;

namespace Code.RenderFeature
{
    [Serializable]
    public class IntersectingSpheresPassData
    {
        [SerializeField] private InjectionPoint _injectionPoint = InjectionPoint.AfterRenderingPostProcessing;
        [SerializeField] private Material _raytracingMaterial;
        [SerializeField] private Material _debugMaterial;
        [SerializeField] private bool _debug;
        [SerializeField] private ComputeShader _resetTilesDataShader;
        [SerializeField] private ComputeShader _cullingShader;
        [SerializeField] [Min(1)] private int _tileSizeX = 10;
        [SerializeField] [Min(1)] private int _tileSizeY = 10;
        [SerializeField] [Min(1)] private int _maxSpheres = 500;
        [SerializeField] [Min(1)] private int _maxSpheresInTile = 20;

        public ComputeShader ResetTilesDataShader => _resetTilesDataShader;
        public Material RaytracingMaterial => _raytracingMaterial;
        public InjectionPoint InjectionPoint => _injectionPoint;
        public ComputeShader CullingShader => _cullingShader;
        public int TilesCount => _tileSizeX * _tileSizeY;
        public Material DebugMaterial => _debugMaterial;
        public int MaxSpheresInTile=> _maxSpheresInTile;
        public int MaxSpheres => _maxSpheres;
        public int TileSizeX => _tileSizeX;
        public int TileSizeY => _tileSizeY;
        public bool UseDebug => _debug;

        public bool IsValid => _raytracingMaterial != null && 
                               _cullingShader != null &&
                               _resetTilesDataShader != null;
    }
}