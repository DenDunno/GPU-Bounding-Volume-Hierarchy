using System;
using Code.Data;
using Code.Utils.BuffersUtls;
using UnityEngine;

namespace Code.Core
{
    public class SphereBuffers : IDisposable
    {
        public readonly SubUpdatesBuffer<AABB> BoundingBoxes;
        public readonly ComputeBuffer Nodes;

        public SphereBuffers(int maxSpheres)
        {
            BoundingBoxes = new SubUpdatesBuffer<AABB>(maxSpheres, AABB.GetSize());
            Nodes = new ComputeBuffer(maxSpheres, AABBNode.GetSize());
        }

        public void Dispose()
        {
            BoundingBoxes.Dispose();
            Nodes.Dispose();
        }
    }
}