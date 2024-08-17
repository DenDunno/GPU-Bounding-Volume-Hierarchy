using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPUPrefixSum
    {
        private readonly IShaderBridge<string> _shaderBridge;
        private readonly Kernel _chunkPrefixSumKernel;
        private readonly ComputeBuffer _blockSum;
        private readonly ComputeBuffer _input;
        private readonly int _size;

        public GPUPrefixSum(ComputeBuffer input, ComputeShader shader)
        {
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(shader));
            _chunkPrefixSumKernel = new Kernel(shader, "ChunkPrefixSum");
            _blockSum = new ComputeBuffer(2, sizeof(int)); 
            _size = input.count;
            _input = input;
        }

        public void Initialize()
        {
            _shaderBridge.SetBuffer(_chunkPrefixSumKernel.ID, "BlockSum", _input);
            _shaderBridge.SetBuffer(_chunkPrefixSumKernel.ID, "Result", _input);
            _shaderBridge.SetInt("InputSize", _size);
        }

        public void Dispatch()
        {
            _chunkPrefixSumKernel.Dispatch(new Vector3Int(1, 1, 1));
        }
    }
}