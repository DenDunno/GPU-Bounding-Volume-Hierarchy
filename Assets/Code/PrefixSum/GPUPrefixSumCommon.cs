using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPUPrefixSumCommon
    {
        public readonly IShaderBridge<string> Bridge;
        public readonly Kernel BlockSumAdditionKernel;
        public readonly Kernel ChunksScanKernel;
        public readonly Kernel SingleScanKernel;

        public GPUPrefixSumCommon(ComputeShader shader)
        {
            Bridge = new CachedShaderBridge(new ComputeShaderBridge(shader));
            BlockSumAdditionKernel = new Kernel(shader, "BlockSumAddition");
            ChunksScanKernel = new Kernel(shader, "ChunkPrefixSum");
            SingleScanKernel = new Kernel(shader, "SinglePrefixSum");
        }

        public int ThreadsPerGroup => ChunksScanKernel.ThreadsPerGroup.x;
    }
}