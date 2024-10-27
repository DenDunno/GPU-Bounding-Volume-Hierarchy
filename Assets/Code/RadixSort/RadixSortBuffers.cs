using System;
using Code.Utils.ShaderUtils;
using UnityEngine;

namespace Code
{
    public class RadixSortBuffers : IDisposable
    {
        public readonly ComputeBuffer LocalPrefixSum;
        public readonly ComputeBuffer LocalShuffle;
        public readonly ComputeBuffer BlockSum;
        public readonly ComputeBuffer Input;

        public RadixSortBuffers(int inputSize, int blocks, int threadGroupsX)
        {
            BlockSum = new ComputeBuffer(threadGroupsX * blocks, sizeof(int));
            LocalPrefixSum = new ComputeBuffer(inputSize, sizeof(int));
            LocalShuffle = new ComputeBuffer(inputSize, sizeof(int));
            Input = new ComputeBuffer(inputSize, sizeof(int));
        }

        public void Bind(int kernelId, ComputeBuffer input, IShaderBridge<string> shaderBridge)
        {
            shaderBridge.SetBuffer(kernelId, "LocalPrefixSum", LocalPrefixSum);
            shaderBridge.SetBuffer(kernelId, "LocalShuffle", LocalShuffle);
            shaderBridge.SetBuffer(kernelId, "BlockSum", BlockSum);
            shaderBridge.SetBuffer(kernelId, "Input", input);
        }

        public void Dispose()
        {
            LocalPrefixSum.Dispose();
            LocalShuffle.Dispose();
            BlockSum.Dispose();
            Input.Dispose();
        }
    }
}