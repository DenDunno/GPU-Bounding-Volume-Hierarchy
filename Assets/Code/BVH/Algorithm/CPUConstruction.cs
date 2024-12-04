using Code.Utils.Extensions;
using Code.Utils.GPUShaderEmulator;
using Unity.Collections;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class CPUConstruction : IBVHConstructionAlgorithm
    {
        private readonly ComputeBuffer _nodesBuffer;
        private readonly bool _isStupidSearch;

        public CPUConstruction(ComputeBuffer nodesBuffer, bool isStupidSearch)
        {
            _isStupidSearch = isStupidSearch;
            _nodesBuffer = nodesBuffer;
        }

        public void Prepare()
        {
        }

        public void Execute(int leavesCount)
        {
            int blockSize = leavesCount;
            int radiusShift = 2;
            int groups = Mathf.CeilToInt((float)leavesCount / blockSize);
            NativeArray<BVHNode> nodes = new(_nodesBuffer.FetchData<BVHNode>(_nodesBuffer.count), Allocator.TempJob);

            if (_isStupidSearch)
            {
                Execute(blockSize, groups, new PlocPlusStupidSearch(nodes, leavesCount, radiusShift));
            }
            else
            {
                PlocPlusPlusSmartSearchData data = new(nodes, leavesCount, blockSize, radiusShift);
                Execute(blockSize, groups, new NeighboursInitialization(data));
                Execute(blockSize, groups, new PlocPlocSmartSearch(data));
                
                for (int i = 0; i < leavesCount; ++i)
                {
                    BVHNode bvhNode = nodes[i];
                    bvhNode.X = (uint)new PlocPlocSmartSearch(data).FindNearestNeighbour(i, 0);
                    nodes[i] = bvhNode;
                }
                data.Dispose();
            }

            _nodesBuffer.SetData(nodes);
            nodes.Dispose();
        }

        private void Execute<TBlockTask>(int blockSize, int groups, TBlockTask blockTask)
            where TBlockTask : struct, IBlockTask
        {
            GPUShaderEmulator<TBlockTask> test = new(blockSize, groups, blockTask);
            test.Execute();
            blockTask.Dispose();
        }
    }
}