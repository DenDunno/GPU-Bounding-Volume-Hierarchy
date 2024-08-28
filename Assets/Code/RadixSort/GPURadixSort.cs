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
        private readonly IGPUPrefixSum _blockSumPrefixSum;
        private readonly Vector3Int _threadGroups;
        private readonly RadixSortBuffers _buffers;
        private readonly Kernel _chunkSortKernel;

        public GPURadixSort(GPURadixSortInput input)
        {
            _chunkSortKernel = new Kernel(input.SortShader, "CSMain");
            _threadGroups = _chunkSortKernel.ComputeThreadGroups(input.PayloadDispatch);
            _buffers = new RadixSortBuffers(input.ArraySize, input.Blocks, _threadGroups.x);
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(input.SortShader));
            _blockSumPrefixSum = new GPUPrefixSumFactory(input.PrefixSumShader, _buffers.BlockSum).Create();
        }

        public void SetData(int[] input)
        {
            SetData(new SetArrayOperation<int>(input));
        }

        private void SetData<TCollection>(SetDataOperation<TCollection> setDataOperation)
        {
            setDataOperation.Execute(_buffers.Input);
            _buffers.Bind(_chunkSortKernel.ID, _shaderBridge);
            _shaderBridge.SetInt("ThreadGroups", _threadGroups.x);
        }

        public void Execute(ref int[] output, int sortLength)
        {
            _shaderBridge.SetInt("SortLength", sortLength);
            _chunkSortKernel.Dispatch(_threadGroups);
            _blockSumPrefixSum.Dispatch();
            output = new int[_buffers.BlockSum.count];
            _buffers.BlockSum.GetData(output);
        }

        public void Dispose()
        {
            _buffers.Dispose();
            _blockSumPrefixSum.Dispose();
        }
    }
}