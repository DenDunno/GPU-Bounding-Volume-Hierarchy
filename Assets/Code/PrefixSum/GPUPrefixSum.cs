using System;
using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPUPrefixSum : IDisposable
    {
        private readonly KernelConstantDispatch _chunkPrefixSumDispatch;
        private readonly IShaderBridge<string> _shaderBridge;
        private readonly ComputeBuffer _blockSum;
        private readonly ComputeBuffer _input;
        private readonly Kernel _kernel;
        private readonly int _size;

        public GPUPrefixSum(ComputeBuffer input, ComputeShader shader)
        {
            _input = input;
            _size = input.count;
            Vector3Int payload = new(_size, 1, 1);
            _kernel = new Kernel(shader, "ChunkPrefixSum");
            _chunkPrefixSumDispatch = new KernelConstantDispatch(_kernel, payload);
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(shader));
            _blockSum = new ComputeBuffer(_chunkPrefixSumDispatch.ThreadGroups.x, sizeof(int));
        }

        public void Initialize()
        {
            _shaderBridge.SetBuffer(_kernel.ID, "BlockSum", _blockSum);
            _shaderBridge.SetBuffer(_kernel.ID, "Result", _input);
            _shaderBridge.SetInt("InputSize", _size);
        }

        public void Dispatch()
        {
            _chunkPrefixSumDispatch.Execute();
        }

        public void Dispose()
        {
            _blockSum.Dispose();
        }
    }
}