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
        private readonly int _size;

        public GPUPrefixSum(ComputeBuffer input, IShaderBridge<string> shaderBridge, Kernel kernel)
        {
            _input = input;
            _kernel = kernel;
            _size = input.count;
            _shaderBridge = shaderBridge;
            ThreadGroups = kernel.ComputeThreadGroups(_size);
        }

        private void SetupShader()
        {
            _shaderBridge.SetBuffer(_kernel.ID, "Result", _input);
            _shaderBridge.SetInt("InputSize", _size);
        }

        public void Dispatch()
        {
            SetupShader();
            _kernel.Dispatch(ThreadGroups);
        }
    }
}