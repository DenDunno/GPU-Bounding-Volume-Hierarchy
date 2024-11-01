using System;
using Code.Data;
using DefaultNamespace;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHBuffers : IDisposable
    {
        public readonly ComputeBuffer MortonCodes;
        public readonly ComputeBuffer ParentIds;
        public readonly ComputeBuffer Boxes;
        public readonly ComputeBuffer Nodes;
        public readonly ComputeBuffer Root;
        
        public BVHBuffers(int count)
        {
            MortonCodes = new ComputeBuffer(count, MortonCode.GetSize());
            ParentIds = new ComputeBuffer(count, sizeof(uint));
            Boxes = new ComputeBuffer(count, AABB.GetSize());
            Nodes = new ComputeBuffer(count + (count - 1), 32);
            Root = new ComputeBuffer(1, sizeof(uint));
        }

        public void Dispose()
        {
            MortonCodes.Dispose();
            ParentIds.Dispose();
            Boxes.Dispose();
            Nodes.Dispose();
            Root.Dispose();
        }
    }
}