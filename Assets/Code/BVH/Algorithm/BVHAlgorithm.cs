using System;
using Code.Utils.Extensions;
using DefaultNamespace;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHAlgorithm : IDisposable
    {
        private readonly GPURadixSort<MortonCode> _mortonCodesSorting;
        private readonly HPLOC _bvhConstruction;
        private readonly SetupStage _setupStage;
        public readonly BVHBuffers Buffers;

        public BVHAlgorithm(BVHShaders bvhShaders, int bufferSize)
        {
            Buffers = new BVHBuffers(bufferSize);
            _setupStage = new SetupStage(bvhShaders.Setup, Buffers);
            _bvhConstruction = new HPLOC(bvhShaders.BVHConstruction, Buffers);
            _mortonCodesSorting = new GPURadixSort<MortonCode>(bvhShaders.Sorting, bvhShaders.PrefixSum, bufferSize);
        }

        public void Initialize()
        {
            _mortonCodesSorting.SetData(Buffers.MortonCodes);
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
            Buffers.Dispose();
            _mortonCodesSorting.Dispose();
        }
    }
}