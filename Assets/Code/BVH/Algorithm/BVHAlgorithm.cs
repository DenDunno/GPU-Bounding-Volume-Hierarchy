using System;
using DefaultNamespace;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHAlgorithm : IDisposable
    {
        private readonly GPURadixSort<MortonCode> _mortonCodesSorting;
        private readonly HPLOC _bvhConstruction;
        private readonly SetupStage _setupStage;
        private readonly BVHBuffers _buffers;

        public BVHAlgorithm(BVHShaders bvhShaders, BVHBuffers buffers)
        {
            _buffers = buffers;
            _setupStage = new SetupStage(bvhShaders.Setup, _buffers);
            _bvhConstruction = new HPLOC(bvhShaders.BVHConstruction, _buffers);
            _mortonCodesSorting = new GPURadixSort<MortonCode>(bvhShaders.Sorting, bvhShaders.PrefixSum, buffers.Size);
        }

        public void Initialize()
        {
            _mortonCodesSorting.SetData(_buffers.MortonCodes);
            _bvhConstruction.Prepare();
            _setupStage.Prepare();
        }

        public void Execute(int count)
        {
            _setupStage.Execute(count);
            _mortonCodesSorting.Execute(count);
            _bvhConstruction.Execute(count);
        }

        public void Dispose()
        {
            _mortonCodesSorting.Dispose();
            _buffers.Dispose();
        }
    }
}