using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPUPrefixSumCommon
    {
        public readonly IShaderBridge<string> Bridge;
        public readonly Kernel BlockSumAdditionKernel;
        public readonly Kernel ScanKernel;

        public GPUPrefixSumCommon(ComputeShader shader)
        {
            Bridge = new CachedShaderBridge(new ComputeShaderBridge(shader));
            BlockSumAdditionKernel = new Kernel(shader, "BlockSumAddition");
            ScanKernel = new Kernel(shader, "ChunkPrefixSum");
        }

        public int ThreadsPerGroup => ScanKernel.ThreadsPerGroup.x;
    }
}