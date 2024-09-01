using System;
using Code.Utils.Extensions;
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
        private readonly RadixSortBuffers _buffers;
        private readonly Vector3Int _threadGroups;
        private readonly int _sortedBitsPerPass;
        private readonly Kernel _globalScatter;
        private readonly Kernel _chunkSort;

        public GPURadixSort(GPURadixSortInput input)
        {
            _sortedBitsPerPass = input.SortedBitsPerPass;
            _chunkSort = new Kernel(input.SortShader, "ChunkSort");
            _globalScatter = new Kernel(input.SortShader, "GlobalScatter");
            _threadGroups = _chunkSort.ComputeThreadGroups(input.PayloadDispatch);
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
            _buffers.Bind(_chunkSort.ID, _shaderBridge);
            _buffers.Bind(_globalScatter.ID, _shaderBridge);
            _shaderBridge.SetInt("ThreadGroups", _threadGroups.x);
        }

        public void Execute(ref int[] output, int sortLength)
        {
            SetupBeforeDispatch(sortLength);

            for (int bitOffset = 0; bitOffset < 32; bitOffset += _sortedBitsPerPass)
            {
                SetBitOffset(bitOffset);
                _chunkSort.Dispatch(_threadGroups);
                _blockSumPrefixSum.Dispatch();
                _globalScatter.Dispatch(_threadGroups);
                
                //_buffers.Input.PrintInt($"Bit offset {bitOffset} = ");
            }
            
            output = new int[_buffers.Input.count];
            _buffers.Input.GetData(output);
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