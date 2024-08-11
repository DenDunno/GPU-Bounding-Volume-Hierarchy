using System;
using Code.Utils.ShaderUtils;
using UnityEngine;

namespace Code
{
    public class RadixSortBuffers : IDisposable
    {
        public readonly ComputeBuffer LocalPrefixSum;
        public readonly ComputeBuffer BlockSum;
        public readonly ComputeBuffer Input;

        public RadixSortBuffers(int inputSize)
        {
            LocalPrefixSum = new ComputeBuffer(inputSize, sizeof(int));
            BlockSum = new ComputeBuffer(inputSize, sizeof(int));
            Input = new ComputeBuffer(inputSize, sizeof(int));
        }

        public void Bind(int kernelId, IShaderBridge<string> shaderBridge)
        {
            shaderBridge.SetBuffer(kernelId, "LocalPrefixSum", LocalPrefixSum);
            shaderBridge.SetBuffer(kernelId, "BlockSum", BlockSum);
            shaderBridge.SetBuffer(kernelId, "Input", Input);
        }

        public void Dispose()
        {
            LocalPrefixSum.Dispose();
            BlockSum.Dispose();
            Input.Dispose();
        }
    }
}