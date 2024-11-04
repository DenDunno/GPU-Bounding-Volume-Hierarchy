using System;
using Code.Components.MortonCodeAssignment.Event;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHComponents : IDisposable
    {
        public readonly BVHGPUBridge GPUBridge;
        public readonly BVHAlgorithm Algorithm;
        public readonly BVHContent Content;

        public BVHComponents(BVHData data, EventWrapper rebuiltEvent)
        {
            Content = new BVHContent();
            BVHBuffers buffers = new(data.BufferSize);
            GPUBridge = new BVHGPUBridge(buffers, Content);
            Algorithm = new BVHAlgorithm(BVHShaders.Load(), buffers, rebuiltEvent, Content);
        }

        public void SendAndRebuild()
        {
            GPUBridge.SendBoxesToGPU();
            Rebuild();
        }
        
        public void Rebuild()
        {
            Algorithm.Execute();
        }

        public void Dispose()
        {
            Algorithm.Dispose();
        }
    }
}