using System;
using Code.Components.MortonCodeAssignment.Event;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHComponents : IDisposable
    {
        public readonly BVHContent Content;
        public readonly BVHGPUBridge GPUBridge;
        public readonly EventWrapper RebuiltEvent;
        private readonly BVHAlgorithm _algorithm;

        public BVHComponents(int bufferSize)
        {
            Content = new BVHContent();
            RebuiltEvent = new EventWrapper();
            BVHBuffers buffers = new(bufferSize);
            GPUBridge = new BVHGPUBridge(buffers, Content);
            _algorithm = new BVHAlgorithm(BVHShaders.Load(), buffers, RebuiltEvent);
        }

        public void Initialize()
        {
            _algorithm.Initialize();
        }

        public void SendAndRebuild()
        {
            GPUBridge.SendBoxesToGPU();
            Rebuild();
        }
        
        public void Rebuild()
        {
            _algorithm.Execute(Content.BoundingBoxes.Count);
        }

        public void Dispose()
        {
            _algorithm.Dispose();
            Content.Dispose();
        }
    }
}