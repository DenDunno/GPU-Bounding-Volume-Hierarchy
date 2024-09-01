using System;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code
{
    public class PrefixSumTest : IDisposable
    {
        private readonly IGPUPrefixSum _prefixSum;
        private readonly int[] _expectedPrefixSum;
        private readonly int[] _outputPrefixSum;
        private readonly ComputeBuffer _buffer;
        private readonly int[] _input;

        public PrefixSumTest(int size, int seed, ComputeShader prefixSumShader)
        {
            _outputPrefixSum = new int[size];
            _buffer = new ComputeBuffer(size, sizeof(int));
            _input = new RandomCollectionGeneration(seed, size, 0, 10).Create();
            _prefixSum = new GPUPrefixSumFactory(prefixSumShader, _buffer).Create();
            _expectedPrefixSum = new PrefixSumGeneration().Generate(_input);
        }
        
        public CollectionComparisonResult<int> Run()
        {
            _buffer.SetData(_input);
            _prefixSum.Dispatch();
            _buffer.GetData(_outputPrefixSum);

            return _expectedPrefixSum.IsSame(_outputPrefixSum);
        }

        public void Dispose()
        {
            _buffer.Dispose();
            _prefixSum.Dispose();
        }
    }
}