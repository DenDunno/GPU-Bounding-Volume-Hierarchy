using System;
using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPUPrefixSum : IDisposable
    {
        private readonly IShaderBridge<string> _shaderBridge;
        private readonly ComputeBuffer _blockSum;
        private readonly ComputeBuffer _counter;
        private readonly ComputeBuffer _input;
        private readonly Kernel _kernel;
        private readonly int _size;

        public GPUPrefixSum(ComputeShader shader, ComputeBuffer input)
        {
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(shader));
            _kernel = new Kernel(shader, "PrefixSumSinglePass");
            _blockSum = new ComputeBuffer(1, sizeof(uint));
            _counter = new ComputeBuffer(1, sizeof(uint));
            _size = input.count;
            _input = input;
        }

        public void Initialize()
        {
            _shaderBridge.SetBuffer(_kernel.ID, "BlockCounter", _counter);
            _shaderBridge.SetBuffer(_kernel.ID, "BlockSum", _blockSum);
            _shaderBridge.SetBuffer(_kernel.ID, "Result", _input);
        }
        
        public void Dispatch()
        {
            Dispatch(_size);
        }
        
        public void Dispatch(int payload)
        {
            _blockSum.SetData(new uint[1]);
            _counter.SetData(new uint[1]);
            _kernel.DispatchPayload(payload);
        }

        public void Dispose()
        {
            _counter.Dispose();
            _blockSum.Dispose();
        }
    }
}