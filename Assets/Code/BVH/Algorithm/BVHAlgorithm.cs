using System;
using Code.Components.MortonCodeAssignment.Event;
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
        private readonly BVHContent _content;

        public BVHAlgorithm(BVHShaders bvhShaders, BVHBuffers buffers, IEventPublisher rebuiltEvent, BVHContent content)
        {
            _content = content;
            _buffers = buffers;
            _rebuiltEvent = rebuiltEvent;
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

        public void Execute()
        {
            _setupStage.Execute(_content.Count);
            _mortonCodesSorting.Execute(_content.Count);
            _bvhConstruction.Execute(_content.Count);
            _rebuiltEvent.Raise();
        }

        public void Dispose()
        {
            _mortonCodesSorting.Dispose();
            _buffers.Dispose();
        }
    }
}