using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPUPrefixSum : IGPUPrefixSum
    {
        public readonly Vector3Int ThreadGroups;
        private readonly IShaderBridge<string> _shaderBridge;
        private readonly ComputeBuffer _input;
        private readonly Kernel _kernel;

        public GPUPrefixSum(ComputeBuffer input, IShaderBridge<string> shaderBridge, Kernel kernel)
        {
            _input = input;
            _kernel = kernel;
            _shaderBridge = shaderBridge;
            ThreadGroups = kernel.ComputeThreadGroups(input.count);
        }

        public void Dispatch()
        {
            _shaderBridge.SetBuffer(_kernel.ID, "Result", _input);
            _kernel.Dispatch(ThreadGroups);
        }

        public void Dispose()
        {
        }
    }
}