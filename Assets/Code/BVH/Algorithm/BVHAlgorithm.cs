using System;
using DefaultNamespace;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHAlgorithm : IDisposable
    {
        private readonly GPURadixSort<MortonCode> _mortonCodesSorting;
        private readonly SetupStage _setupStage;
        private readonly BVHBuffers _buffers;

        public BVHAlgorithm(BVHBuffers buffers, ShadersPresenter shaders, int bufferSize)
        {
            _setupStage = new SetupStage(shaders.Setup, buffers);
            _mortonCodesSorting = new GPURadixSort<MortonCode>(shaders.Sorting, shaders.PrefixSum, bufferSize);
            _buffers = buffers;
        }

        public void Initialize()
        {
            _mortonCodesSorting.SetData(_buffers.MortonCodes);
            _setupStage.Prepare();
        }

        public void Execute(int count)
        {
            _setupStage.Dispatch(count);
            _mortonCodesSorting.Execute(count);
        }

        public void Dispose()
        {
            _mortonCodesSorting.Dispose();
            _buffers.Dispose();
        }
    }
}