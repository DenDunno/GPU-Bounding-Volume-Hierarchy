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
            _scanPerChunk = new GPUPrefixSum(input, common.Bridge, common.ChunksScanKernel);
            _blockSumScan = blockSumScan;
            _blockSum = blockSum;
            _common = common;
            _input = input;
        }

        private void Setup()
        {
            _common.Bridge.SetBuffer(_common.ChunksScanKernel.ID, "BlockSum", _blockSum);
            _common.Bridge.SetBuffer(_common.BlockSumAdditionKernel.ID, "Result", _input);
            _common.Bridge.SetBuffer(_common.BlockSumAdditionKernel.ID, "BlockSum", _blockSum);
            _common.Bridge.SetInt("BlockSumSize", _blockSum.count);
        }

        public void Dispatch()
        {
            Setup();
            _scanPerChunk.Dispatch();
            _blockSumScan.Dispatch();
            Setup();
            _common.BlockSumAdditionKernel.Dispatch(_scanPerChunk.ThreadGroups);
        }

        public void Dispose()
        {
            _blockSumScan.Dispose();
            _scanPerChunk.Dispose();
            _blockSum.Dispose();
        }
    }
}