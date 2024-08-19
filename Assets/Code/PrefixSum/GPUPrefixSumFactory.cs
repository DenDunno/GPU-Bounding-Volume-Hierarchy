using Code.Utils.Factory;
using UnityEngine;

namespace Code
{
    public class GPUPrefixSumFactory : IFactory<IGPUPrefixSum>
    {
        private readonly ComputeShader _shader;
        private readonly ComputeBuffer _input;

        public GPUPrefixSumFactory(ComputeShader shader, ComputeBuffer input)
        {
            _shader = shader;
            _input = input;
        }

        public IGPUPrefixSum Create()
        {
            GPUPrefixSumCommon common = new(_shader);
            return BuildChain(common, _input);
        }

        private IGPUPrefixSum BuildChain(GPUPrefixSumCommon common, ComputeBuffer input)
        {
            if (input.count <= common.ThreadsPerGroup)
            {
                return new GPUPrefixSum(input, common.Bridge, common.SingleScanKernel);
            }

            int blockSumBufferSize = Mathf.CeilToInt(input.count / (float)common.ThreadsPerGroup);
            ComputeBuffer blockSumBuffer = new(blockSumBufferSize, sizeof(int));
            IGPUPrefixSum childNode = BuildChain(common, blockSumBuffer);
            
            return new ChunkGPUPrefixSum(input, blockSumBuffer, common, childNode);
        }
    }
}