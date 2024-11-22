using Code.Utils.Extensions;
using Code.Utils.GPUShaderEmulator;
using Unity.Collections;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class CPUConstruction : IBVHConstructionAlgorithm
    {
        private readonly ComputeBuffer _nodesBuffer;

        public CPUConstruction(ComputeBuffer nodesBuffer)
        {
            _nodesBuffer = nodesBuffer;
        }

        public void Prepare()
        {
        }

        public void Execute(int leavesCount)
        {
            int blockSize = 4;
            int groups = Mathf.CeilToInt((float)leavesCount / blockSize);
            NativeArray<BVHNode> nodes = new(_nodesBuffer.FetchData<BVHNode>(_nodesBuffer.count), Allocator.TempJob);
            GPUShaderEmulator<PlocPlusPlusCPUTest> test = new(blockSize, groups, new PlocPlusPlusCPUTest(nodes, leavesCount));
            test.Execute();
            _nodesBuffer.SetData(nodes);
            nodes.Dispose();
        }
    }
}