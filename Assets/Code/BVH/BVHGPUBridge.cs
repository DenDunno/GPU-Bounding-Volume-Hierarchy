using System.Collections.Generic;
using Code.Data;
using Code.Utils.Extensions;
using Unity.Collections;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHGPUBridge
    {
        public readonly ComputeBuffer Nodes;
        public readonly ComputeBuffer Root;
        private readonly int _innerNodesCount;
        private List<AABB> _boxes;

        public BVHGPUBridge(ComputeBuffer nodes, ComputeBuffer root, int innerNodesCount, List<AABB> boxes)
        {
            _innerNodesCount = innerNodesCount;
            _boxes = boxes;
            Nodes = nodes;
            Root = root;
        }

        public void SendBoxesToGPU() => Nodes.SetData(_boxes);
        public BVHNode[] FetchInnerNodes() => Nodes.FetchData<BVHNode>(_innerNodesCount);
        public int FetchRoot() => Root.FetchValue<int>();
    }
}