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

        public BVHAlgorithm(BVHBuffers buffers, BVHShaders bvhShaders, int bufferSize)
        {
            _mortonCodesSorting = new GPURadixSort<MortonCode>(bvhShaders.Sorting, bvhShaders.PrefixSum, bufferSize);
            _bvhConstruction = new HPLOC(bvhShaders.BVHConstruction, buffers);
            _setupStage = new SetupStage(bvhShaders.Setup, buffers);
            _buffers = buffers;
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
            
            _buffers.MortonCodes.SetData(new MortonCode[]
            {
                new(0,2),
                new(1,3),
                new(2,4),
                new(3,5),
                new(4,8),
                new(5,12),
                new(6,13),
                new(7,15),
            });
            
            _bvhConstruction.Execute(count);
        }

        public void Dispose()
        {
            _mortonCodesSorting.Dispose();
            _buffers.Dispose();
        }
    }
}