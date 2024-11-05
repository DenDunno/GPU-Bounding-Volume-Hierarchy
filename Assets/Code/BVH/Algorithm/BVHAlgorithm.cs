using System;
using Code.Components.MortonCodeAssignment.Event;
using Code.Data;
using DefaultNamespace;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHAlgorithm : IDisposable
    {
        private readonly GPURadixSort<MortonCode> _mortonCodesSorting;
        private readonly IEventPublisher _rebuiltEvent;
        private readonly HPLOC _bvhConstruction;
        private readonly SetupStage _setupStage;
        private readonly BVHBuffers _buffers;

        public BVHAlgorithm(BVHShaders bvhShaders, BVHBuffers buffers, IEventPublisher rebuiltEvent, AABB sceneSize)
        {
            _buffers = buffers;
            _rebuiltEvent = rebuiltEvent;
            _bvhConstruction = new HPLOC(bvhShaders.BVHConstruction, _buffers);
            _setupStage = new SetupStage(bvhShaders.Setup, _buffers, sceneSize);
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
            _rebuiltEvent.Raise();
        }

        public void Dispose()
        {
            _mortonCodesSorting.Dispose();
            _buffers.Dispose();
        }
    }
}