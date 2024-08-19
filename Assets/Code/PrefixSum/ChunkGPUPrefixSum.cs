using UnityEngine;

namespace Code
{
    public class ChunkGPUPrefixSum : IGPUPrefixSum
    {
        private readonly IGPUPrefixSum _blockSumScan;
        private readonly GPUPrefixSum _scanPerChunk;
        private readonly GPUPrefixSumCommon _common;
        private readonly ComputeBuffer _blockSum;
        private readonly ComputeBuffer _input;

        public ChunkGPUPrefixSum(ComputeBuffer input, ComputeBuffer blockSum, 
            GPUPrefixSumCommon common, IGPUPrefixSum blockSumScan)
        {
            _scanPerChunk = new GPUPrefixSum(input, common);
            _blockSumScan = blockSumScan;
            _blockSum = blockSum;
            _common = common;
            _input = input;
        }

        private void Setup()
        {
            _common.Bridge.SetBuffer(_common.ScanKernel.ID, "BlockSum", _blockSum);
            _common.Bridge.SetBuffer(_common.BlockSumAdditionKernel.ID, "BlockSum", _blockSum);
            _common.Bridge.SetBuffer(_common.BlockSumAdditionKernel.ID, "Result", _input);
        }

        public void Dispatch()
        {
            Setup();
            _scanPerChunk.Dispatch();
            _blockSumScan.Dispatch();
            _common.BlockSumAdditionKernel.Dispatch(_scanPerChunk.ThreadGroups);
        }
    }
}