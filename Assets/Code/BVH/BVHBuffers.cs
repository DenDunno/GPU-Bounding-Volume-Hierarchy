using System;
using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHBuffers : IDisposable
    {
        public readonly ComputeBuffer MortonCodes;
        public readonly ComputeBuffer Boxes;
        public readonly ComputeBuffer Nodes;

        public BVHBuffers(int count)
        {
            MortonCodes = new ComputeBuffer(8, count);
            Boxes = new ComputeBuffer(count, AABB.GetSize());
            Nodes = new ComputeBuffer(count + (count - 1), 40);
        }
        
        public void Dispose()
        {
            Boxes.Dispose();
            Nodes.Dispose();
        }
    }
}