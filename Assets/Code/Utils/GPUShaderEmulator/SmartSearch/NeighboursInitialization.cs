using Code.Data;

namespace Code.Utils.GPUShaderEmulator
{
    public struct NeighboursInitialization : IBlockTask
    {
        private PlocPlusPlusSmartSearchData _data;

        public NeighboursInitialization(PlocPlusPlusSmartSearchData data)
        {
            _data = data;
        }

        public void Execute(int threadsPerBlock, ThreadId threadId)
        {
            int blockOffset = threadId.Group * threadsPerBlock;

            for (int rangeId = threadId.Local; rangeId < _data.PLOCRange; rangeId += _data.BlockSize)
            {
                int globalId = rangeId - 2 * _data.Radius + blockOffset;
                _data.Neighbours[rangeId] = uint.MaxValue;

                if (_data.IsInBounds(globalId))
                {
                    _data.NeighboursBoxes[rangeId] = _data.Nodes[_data.ComputeLeafIndex(globalId)].Box;
                }
                else
                {
                    _data.NeighboursBoxes[rangeId] = AABB.CreateMaxBox();
                }
            }
        }
    }
}