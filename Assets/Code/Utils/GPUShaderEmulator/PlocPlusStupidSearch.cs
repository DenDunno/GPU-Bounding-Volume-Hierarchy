using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Code.Utils.GPUShaderEmulator
{
    public struct PlocPlusStupidSearch : IBlockTask
    {
        [NativeDisableContainerSafetyRestriction] private NativeArray<BVHNode> _nodes;
        private readonly int _leavesCount;
        private readonly int _radius;
        
        public PlocPlusStupidSearch(NativeArray<BVHNode> nodes, int leavesCount, int radiusShift)
        {
            _leavesCount = leavesCount;
            _radius = 1 << radiusShift;
            _nodes = nodes;
        }

        private bool IsInBounds(int id)
        {
            return id >= 0 && id < _leavesCount;
        }
        
        int RunStupidSearch(int globalId)
        {
            int minDistanceIndex = int.MaxValue;
            float minDistance = float.MaxValue;

            for (int globalNeighbourId = globalId - _radius; globalNeighbourId <= globalId + _radius; ++globalNeighbourId)
            {
                if (IsInBounds(globalNeighbourId) && globalNeighbourId != globalId)
                {
                    float distance = _nodes[globalId].Box.Union(_nodes[globalNeighbourId].Box).ComputeSurfaceArea();

                    if (distance < minDistance)
                    {
                        minDistanceIndex = globalNeighbourId;
                        minDistance = distance;
                    }   
                }
            }

            return minDistanceIndex;
        }

        public void Execute(int threadsPerBlock, ThreadId threadId)
        {
            if (IsInBounds(threadId.Global))
            {
                BVHNode bvhNode = _nodes[threadId.Global];
                bvhNode.Left = (uint)RunStupidSearch(threadId.Global);
                _nodes[threadId.Global] = bvhNode;   
            }
        }
    }
}