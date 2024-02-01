using System;
using System.Collections.Generic;
using Code.Utils.SubFrustums;
using UnityEngine;

namespace Code.RenderFeature
{
    public class IntersectingSpheresData : IDisposable
    {
        public readonly ComputeBuffer ActiveTiles;
        public readonly ComputeBuffer SubFrustums;
        public readonly ComputeBuffer Spheres;
        public readonly int TilesCount;

        public IntersectingSpheresData(int tiles, int maxSpheres)
        {
            TilesCount = tiles;
            Spheres = new ComputeBuffer(maxSpheres, SphereData.GetSize());
            ActiveTiles = new ComputeBuffer(tiles, sizeof(int));
            SubFrustums = new ComputeBuffer(tiles, Frustum.GetSize());
        }

        public int SpheresCount { get; private set; }

        public void Update(Frustum[] subFrustums, List<SphereData> sphereData)
        {
            SpheresCount = sphereData.Count;
            SubFrustums.SetData(subFrustums);
            Spheres.SetData(sphereData);
        }

        public void Dispose()
        {
            Spheres.Dispose();
            ActiveTiles.Dispose();
            SubFrustums.Dispose();
        }
    }
}