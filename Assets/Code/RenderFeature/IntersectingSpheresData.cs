using System;
using System.Collections.Generic;
using Code.Utils.SubFrustums;
using UnityEngine;

namespace Code.RenderFeature
{
    public class IntersectingSpheresData : IDisposable
    {
        public readonly ComputeBuffer SpheresInTileCount;
        public readonly ComputeBuffer SpheresInTile;
        public readonly ComputeBuffer SubFrustums;
        public readonly ComputeBuffer Spheres;
        public readonly int MaxSpheresInTile;
        public readonly int TilesCount;

        public IntersectingSpheresData(int tiles, int maxSpheres, int maxSpheresInTile)
        {
            TilesCount = tiles;
            MaxSpheresInTile = maxSpheresInTile;
            Spheres = new ComputeBuffer(maxSpheres, SphereData.GetSize());
            SpheresInTileCount = new ComputeBuffer(tiles, sizeof(int));
            SubFrustums = new ComputeBuffer(tiles, Frustum.GetSize());
            SpheresInTile = new ComputeBuffer(tiles * maxSpheresInTile, sizeof(int));
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
            SpheresInTileCount.Dispose();
            SubFrustums.Dispose();
            SpheresInTile.Dispose();
        }
    }
}