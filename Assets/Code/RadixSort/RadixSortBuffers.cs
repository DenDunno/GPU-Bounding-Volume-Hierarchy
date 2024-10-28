using System;
using System.Runtime.InteropServices;
using Code.Utils.ShaderUtils;
using UnityEngine;

namespace Code
{
    public class RadixSortBuffers<T> : IDisposable where T : struct
    {
        private readonly ComputeBuffer _localPrefixSum;
        private readonly ComputeBuffer _localShuffle;
        public readonly ComputeBuffer BlockSum;
        public readonly ComputeBuffer Input;

        public RadixSortBuffers(int inputSize, int blocks, int threadGroupsX)
        {
            int stride = Marshal.SizeOf(default(T));
            BlockSum = new ComputeBuffer(threadGroupsX * blocks, stride);
            _localPrefixSum = new ComputeBuffer(inputSize, stride);
            _localShuffle = new ComputeBuffer(inputSize, stride);
            Input = new ComputeBuffer(inputSize, stride);
        }

        public void Bind(int kernelId, ComputeBuffer input, IShaderBridge<string> shaderBridge)
        {
            shaderBridge.SetBuffer(kernelId, "LocalPrefixSum", _localPrefixSum);
            shaderBridge.SetBuffer(kernelId, "LocalShuffle", _localShuffle);
            shaderBridge.SetBuffer(kernelId, "BlockSum", BlockSum);
            shaderBridge.SetBuffer(kernelId, "Input", input);
        }

        public void Dispose()
        {
            _localPrefixSum.Dispose();
            _localShuffle.Dispose();
            BlockSum.Dispose();
            Input.Dispose();
        }
    }
}