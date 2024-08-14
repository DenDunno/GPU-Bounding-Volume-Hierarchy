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
        private readonly GPUPrefixSum _prefixSum;
        private readonly Kernel _chunkSortKernel;
        private readonly int[] _output;

        public GPURadixSort(GPURadixSortInput input)
        {
            _output = new int[input.ArraySize];
            _buffers = new RadixSortBuffers(input.ArraySize);
            _chunkSortKernel = new Kernel(input.SortShader, "CSMain");
            _prefixSum = new GPUPrefixSum(_buffers.Input, input.PrefixSumShader);
            _optimalDispatchSize = _chunkSortKernel.ComputeOptimalDispatchSize(input.PayloadDispatch);
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(input.SortShader));
        }

        public void Initialize<TCollection>(SetDataOperation<TCollection> setDataOperation)
        {
            _prefixSum.Initialize();
            setDataOperation.Execute(_buffers.Input);
            _buffers.Bind(_chunkSortKernel.ID, _shaderBridge);
            _shaderBridge.SetInt("ThreadGroups", _optimalDispatchSize.x);
        }

        public int[] Execute()
        {
            Execute(_output);
            return _output;
        }

        public void Execute(int[] output)
        {
            _chunkSortKernel.Dispatch(_optimalDispatchSize);
            _prefixSum.Dispatch();
            _buffers.Input.GetData(output);
        }

        public void Dispose()
        {
            _buffers.Dispose();
        }
    }
}