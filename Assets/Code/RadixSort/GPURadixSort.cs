using System;
using System.Collections.Generic;
using Code.Utils.ShaderUtils;
using Code.Utils.ShaderUtils.Buffer;
using MyFolder.ComputeShaderNM;
using Unity.Collections;
using UnityEngine;

namespace Code
{
    public class GPURadixSort<T> : IDisposable where T : struct
    {
        private readonly IShaderBridge<string> _shaderBridge;
        private readonly GPUPrefixSum _blockSumPrefixSum;
        private readonly RadixSortBuffers<T> _buffers;
        private readonly Vector3Int _threadGroups;
        private readonly int _sortedBitsPerPass;
        private readonly Kernel _globalScatter;
        private readonly Kernel _chunkSort;

        public GPURadixSort(ComputeShader sort, ComputeShader prefixSum, int size)
        {
            GPURadixSortInput input = new(sort, prefixSum, size);
            _sortedBitsPerPass = input.SortedBitsPerPass;
            _chunkSort = new Kernel(input.SortShader, "ChunkSort");
            _globalScatter = new Kernel(input.SortShader, "GlobalScatter");
            _threadGroups = _chunkSort.ComputeThreadGroups(input.PayloadDispatch);
            _buffers = new RadixSortBuffers<T>(input.ArraySize, input.Blocks, _threadGroups.x);
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(input.SortShader));
            _blockSumPrefixSum = new GPUPrefixSum(input.PrefixSumShader, _buffers.BlockSum);
        }

        public void GetData(Array array) => _buffers.Input.GetData(array);

        public void SetData(T[] input) => 
            Setup(_buffers.Input, new SetArrayOperation<T>(input));

        public void SetData(List<T> input) => 
            Setup(_buffers.Input, new SetListOperation<T>(input));

        public void SetData(NativeArray<T> input) =>
            Setup(_buffers.Input, new SetNativeArrayOperation<T>(input));

        public void SetData(ComputeBuffer input) => 
            Setup(input, new SetVoidOperation());

        private void Setup<TSetData>(ComputeBuffer input, TSetData setDataOperation) where TSetData : ISetDataOperation
        {
            setDataOperation.Execute(input);
            _buffers.Bind(_chunkSort.ID, input, _shaderBridge);
            _buffers.Bind(_globalScatter.ID, input, _shaderBridge);
            _shaderBridge.SetInt("ThreadGroups", _threadGroups.x);
            _blockSumPrefixSum.Initialize();
        }

        public void Execute(int sortLength)
        {
            SetupBeforeDispatch(sortLength);
            
            for (int bitOffset = 0; bitOffset < 32; bitOffset += _sortedBitsPerPass)
            {
                SetBitOffset(bitOffset);
                _chunkSort.Dispatch(_threadGroups);
                _blockSumPrefixSum.Dispatch();
                _globalScatter.Dispatch(_threadGroups);
            }
        }

        private void SetBitOffset(int bitOffset)
        {
            _shaderBridge.SetInt("BitOffset", bitOffset);
        }

        private void SetupBeforeDispatch(int sortLength)
        {
            _shaderBridge.SetInt("SortLength", sortLength);
        }

        public void Dispose()
        {
            _buffers.Dispose();
            _blockSumPrefixSum.Dispose();
        }
    }
}