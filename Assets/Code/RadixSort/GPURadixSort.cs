using System;
using Code.Utils.ShaderUtils;
using Code.Utils.ShaderUtils.Buffer;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code
{
    public class GPURadixSort : IDisposable
    {
        private readonly IShaderBridge<string> _shaderBridge;
        private readonly Vector3Int _optimalDispatchSize;
        private readonly RadixSortBuffers _buffers;
        private readonly Kernel _kernel;
        private readonly int[] _output;

        public GPURadixSort(ComputeShader sortShader, int arraySize) : this(sortShader,
            new GPURadixSortInput(arraySize))
        {
        }

        private GPURadixSort(ComputeShader sortShader, GPURadixSortInput input)
        {
            _output = new int[input.ArraySize];
            _kernel = new Kernel(sortShader, "CSMain");
            _buffers = new RadixSortBuffers(input.ArraySize);
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(sortShader));
            _optimalDispatchSize = _kernel.ComputeOptimalDispatchSize(input.PayloadDispatch);
        }

        public void Initialize<TCollection>(SetDataOperation<TCollection> setDataOperation)
        {
            setDataOperation.Execute(_buffers.Input);
            _buffers.Bind(_kernel.ID, _shaderBridge);
            _shaderBridge.SetInt("ThreadGroups", _optimalDispatchSize.x);
        }

        public int[] Execute()
        {
            Execute(_output);
            return _output;
        }

        public void Execute(int[] output)
        {
            _kernel.Dispatch(_optimalDispatchSize);
            _buffers.LocalPrefixSum.GetData(output);
        }

        public void Dispose()
        {
            _buffers.Dispose();
        }
    }
}