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
            NativeArray<BVHNode> nodes = new(_nodesBuffer.FetchData<BVHNode>(_nodesBuffer.count), Allocator.TempJob);
            GPUShaderEmulator<PlocPlusPlusCPUTest> test = new(2, 4, new PlocPlusPlusCPUTest(nodes, _nodesBuffer.count));
            test.Execute();
            _nodesBuffer.SetData(nodes);
            nodes.Dispose();
        }
    }
}