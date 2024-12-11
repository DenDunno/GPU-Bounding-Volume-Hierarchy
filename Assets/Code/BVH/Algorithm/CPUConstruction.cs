using System.Collections.Generic;
using System.Linq;
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
            int blockSize = 1024;
            int radiusShift = 4;
            int groups = Mathf.CeilToInt((float)leavesCount / blockSize);
            NativeArray<BVHNode> nodes = new(_nodesBuffer.FetchData<BVHNode>(_nodesBuffer.count), Allocator.TempJob);

            if (_isStupidSearch)
            {
                Execute(blockSize, groups, new PlocPlusStupidSearch(nodes, leavesCount, radiusShift));
            }
            else
            {
                PlocPlusPlusSmartSearchData data = new(nodes, leavesCount, blockSize, radiusShift);
                GPUShaderEmulator<NeighboursInitialization> initialization = new(blockSize, groups, new NeighboursInitialization(data));
                GPUShaderEmulator<PlocPlocSmartSearch> search = new(blockSize, groups, new PlocPlocSmartSearch(data));
                
                int[] groupsArray = EnumerableExtensions.GetRandomOrderedArray(groups);

                foreach (int groupId in groupsArray)
                {
                    initialization.Execute(groupId);
                    search.Execute(groupId);
                    
                    for (int threadId = 0; threadId < blockSize; ++threadId)
                    {
                        if (threadId < leavesCount)
                        {
                            int globalId = groupId * blockSize + threadId;
                            BVHNode bvhNode = nodes[globalId];
                            bvhNode.X = (uint)new PlocPlocSmartSearch(data).FindNearestNeighbour(threadId, groupId * blockSize);
                            nodes[globalId] = bvhNode;   
                        }
                    }
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