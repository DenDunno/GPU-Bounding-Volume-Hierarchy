using System;
using Code.Components.MortonCodeAssignment.Event;
using Code.Data;
using EditorWrapper;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHComponents : IDisposable
    {
        public readonly EventWrapper RebuiltEvent;
        public readonly BVHGPUBridge GPUBridge;
        private readonly IBoundingBoxesInput _boxesInput;
        private readonly BVHAlgorithm _algorithm;
        private readonly BVHBuffers _buffers;

        public BVHComponents(BVHData data)
        {
            RebuiltEvent = new EventWrapper();
            _boxesInput = data.BoxesInput.Value;
            _buffers = new BVHBuffers(data.BoxesInput.Count);
            GPUBridge = new BVHGPUBridge(_buffers, data.BoxesInput.Count);
            _algorithm = new BVHAlgorithm(BVHShaders.Load(), _buffers, RebuiltEvent, data.SceneSize.Box);
        }

        public void Initialize()
        {
            _algorithm.Initialize();
            _buffers.Boxes.SetData(_boxesInput.Calculate());
        }

        public void Rebuild()
        {
            _algorithm.Execute(_buffers.Size);
        }

        public void Dispose()
        {
            _algorithm.Dispose();
        }
    }
}