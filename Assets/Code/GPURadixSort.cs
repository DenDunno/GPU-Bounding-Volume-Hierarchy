using System;
using Code.Utils.ShaderUtils;
using Code.Utils.ShaderUtils.Buffer;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPURadixSort : IDisposable
    {
        private readonly CachedShaderBridge _shaderBridge;
        private readonly ComputeBuffer _blockSum;
        private readonly ComputeBuffer _input;
        private readonly ComputeBuffer _outputBuffer;
        private readonly Kernel _kernel;
        private readonly int[] _output;

        public GPURadixSort(ComputeShader sortShader, int arraySize) : this(sortShader,
            new GPURadixSortInput(arraySize))
        {
        }

        public GPURadixSort(ComputeShader sortShader, GPURadixSortInput input)
        {
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(sortShader));
            _kernel = new Kernel(sortShader, "CSMain", input.PayloadDispatch);
            _input = new ComputeBuffer(input.ArraySize, sizeof(int));
            _outputBuffer = new ComputeBuffer(input.ArraySize, sizeof(int));
            _output = new int[input.ArraySize];
        }

        public void Initialize<TCollection>(SetDataOperation<TCollection> setDataOperation)
        {
            setDataOperation.Execute(_input);
            _shaderBridge.SetBuffer(_kernel.ID, "Input", _input);
            _shaderBridge.SetBuffer(_kernel.ID, "Output", _outputBuffer);
        }

        public int[] Execute()
        {
            Execute(_output);
            return _output;
        }

        public void Execute(int[] output)
        {
            _shaderBridge.SetInt("ThreadGroups", _kernel.DispatchSize.x);
            _kernel.Dispatch();
            _outputBuffer.GetData(output);
        }

        public void Dispose()
        {
            _input.Dispose();
        }
    }
}