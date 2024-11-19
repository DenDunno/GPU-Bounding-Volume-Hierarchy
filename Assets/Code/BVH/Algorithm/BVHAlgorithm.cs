using System;
using Code.Components.MortonCodeAssignment.Event;
using Code.Data;
using DefaultNamespace;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHAlgorithm : IDisposable
    {
        private readonly GPURadixSort<MortonCode> _mortonCodesSorting;
        private readonly IBVHConstructionAlgorithm _bvhConstruction;
        private readonly IEventPublisher _rebuiltEvent;
        private readonly SetupStage _setupStage;
        private readonly BVHBuffers _buffers;

        public BVHAlgorithm(BVHShaders bvhShaders, BVHBuffers buffers, IEventPublisher rebuiltEvent,
            IBVHConstructionAlgorithm bvhConstruction, AABB sceneSize)
        {
            _buffers = buffers;
            _rebuiltEvent = rebuiltEvent;
            _bvhConstruction = bvhConstruction;
            _setupStage = new SetupStage(bvhShaders.Setup, _buffers, sceneSize);
            _mortonCodesSorting = new GPURadixSort<MortonCode>(bvhShaders.Sorting, bvhShaders.PrefixSum, buffers.Size);
        }

        public void Initialize()
        {
            _mortonCodesSorting.SetData(_buffers.MortonCodes);
            _bvhConstruction.Prepare();
            _setupStage.Prepare();
        }

        public void Execute(int leavesCount)
        {
            _setupStage.Execute(leavesCount);
            _mortonCodesSorting.Execute(leavesCount);
            _bvhConstruction.Execute(leavesCount);
            _rebuiltEvent.Raise();
        }

        public void Dispose()
        {
            _mortonCodesSorting.Dispose();
            _buffers.Dispose();
        }
    }
}