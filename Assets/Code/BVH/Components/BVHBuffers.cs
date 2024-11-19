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
        public readonly int Size;

        public BVHBuffers(int size)
        {
            Nodes = new ComputeBuffer(size + (size - 1) + 1, BVHNode.GetSize()); // leaves + innerNodes + root 
            MortonCodes = new ComputeBuffer(size, MortonCode.GetSize());
            ParentIds = new ComputeBuffer(size, sizeof(uint));
            Boxes = new ComputeBuffer(size, AABB.GetSize());
            Root = new ComputeBuffer(1, sizeof(uint));
            Size = size;
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