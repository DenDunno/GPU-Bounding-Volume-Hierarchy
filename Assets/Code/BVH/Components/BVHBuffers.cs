using System;
using Code.Data;
using DefaultNamespace;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHBuffers : IDisposable
    {
        public readonly ComputeBuffer MergedNodesCount;
        public readonly ComputeBuffer ValidNodesCount;
        public readonly ComputeBuffer BlockCounter;
        public readonly ComputeBuffer MortonCodes;
        public readonly ComputeBuffer ParentIds;
        public readonly ComputeBuffer Boxes;
        public readonly ComputeBuffer Nodes;
        public readonly ComputeBuffer Root;
        public readonly int Size;

        public BVHBuffers(int size)
        {
            Nodes = new ComputeBuffer(size + (size - 1), BVHNode.GetSize());
            MortonCodes = new ComputeBuffer(size, MortonCode.GetSize());
            MergedNodesCount = new ComputeBuffer(1, sizeof(uint));
            ValidNodesCount = new ComputeBuffer(1, sizeof(uint));
            ParentIds = new ComputeBuffer(size, sizeof(uint));
            BlockCounter = new ComputeBuffer(1, sizeof(uint));
            Boxes = new ComputeBuffer(size, AABB.GetSize());
            Root = new ComputeBuffer(1, sizeof(uint));
            Size = size;
        }

        public void Dispose()
        {
            MergedNodesCount.Dispose();
            ValidNodesCount.Dispose();
            BlockCounter.Dispose();
            MortonCodes.Dispose();
            ParentIds.Dispose();
            Boxes.Dispose();
            Nodes.Dispose();
            Root.Dispose();
        }
    }
}