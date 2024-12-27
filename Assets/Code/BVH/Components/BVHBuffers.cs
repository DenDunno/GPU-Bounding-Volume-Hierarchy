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
        public readonly ComputeBuffer TreeSize;
        public readonly ComputeBuffer MortonCodes;
        public readonly ComputeBuffer ParentIds;
        public readonly ComputeBuffer Boxes;
        public readonly ComputeBuffer Nodes;
        public readonly ComputeBuffer Root;
        public readonly ComputeBuffer Test;
        public readonly int Size;

        public BVHBuffers(int size)
        {
            Nodes = new ComputeBuffer(size + (size - 1), BVHNode.GetSize());
            MortonCodes = new ComputeBuffer(size, MortonCode.GetSize());
            MergedNodesCount = new ComputeBuffer(1, sizeof(uint));
            ValidNodesCount = new ComputeBuffer(1, sizeof(uint));
            ParentIds = new ComputeBuffer(size, sizeof(uint));
            BlockCounter = new ComputeBuffer(1, sizeof(uint));
            Test = new ComputeBuffer(size, sizeof(uint) * 2);
            Boxes = new ComputeBuffer(size, AABB.GetSize());
            TreeSize = new ComputeBuffer(1, sizeof(uint));
            Root = new ComputeBuffer(1, sizeof(uint));
            Size = size;
        }

        public void Dispose()
        {
            ValidNodesCount.Dispose();
            BlockCounter.Dispose();
            MergedNodesCount.Dispose();
            MortonCodes.Dispose();
            ParentIds.Dispose();
            TreeSize.Dispose();
            Boxes.Dispose();
            Nodes.Dispose();
            Root.Dispose();
            Test.Dispose();
        }
    }
}