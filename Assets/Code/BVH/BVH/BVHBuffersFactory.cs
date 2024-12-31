using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHBuffersFactory 
    {
        private readonly IReadOnlyList<TopLevelAccelerationStructure> _topLevelStructures;

        public BVHBuffersFactory(IReadOnlyList<TopLevelAccelerationStructure> topLevelStructures)
        {
            _topLevelStructures = topLevelStructures;
        }

        public BVHBuffers Create()
        {
            return new BVHBuffers(
                bottomLevel: CreateBottomLevelBuffer(),
                rootNodeIndices: CreateRootNodesBuffer(),
                modelMatrices: CreateModelMatricesBuffer());
        }

        private ComputeBuffer CreateBottomLevelBuffer()
        {
            int bufferSize = _topLevelStructures.Sum(x => x.Cluster.Tree.Length);
            ComputeBuffer bottomLevelsBuffer = new(bufferSize, BVHNode.GetSize());

            int offset = 0;
            foreach (TopLevelAccelerationStructure structure in _topLevelStructures)
            {
                int treeSize = structure.Cluster.Tree.Length;
                bottomLevelsBuffer.SetData(structure.Cluster.Tree, 0, offset, treeSize);
                offset += treeSize;
            }

            return bottomLevelsBuffer;
        }

        private ComputeBuffer CreateRootNodesBuffer()
        {
            int[] rootNodesIndices = new int[_topLevelStructures.Count];
            ComputeBuffer rootNodesBuffer = new(_topLevelStructures.Count, sizeof(uint));

            int offset = 0;
            for (int i = 0; i < _topLevelStructures.Count; ++i)
            {
                rootNodesIndices[i] = offset;
                offset += _topLevelStructures[i].Cluster.Tree.Length;
            }

            rootNodesBuffer.SetData(rootNodesIndices);

            return rootNodesBuffer;
        }

        private ComputeBuffer CreateModelMatricesBuffer()
        {
            return new ComputeBuffer(_topLevelStructures.Count, sizeof(float) * 16);
        }
    }
}