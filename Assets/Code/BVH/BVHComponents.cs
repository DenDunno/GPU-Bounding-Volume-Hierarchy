using System;
using Code.Components.MortonCodeAssignment.Event;
using EditorWrapper;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHComponents : IDisposable
    {
        public IDrawable Visualization;
        public readonly BVHGPUBridge GPUBridge;
        public readonly BVHAlgorithm Algorithm;
        public readonly EventWrapper RebuiltEvent;
        private readonly BVHVisualizationFactory _bvhVisualizationFactory;

        public BVHComponents(BVHData data)
        {
            RebuiltEvent = new EventWrapper();
            BVHBuffers buffers = new(data.BufferSize);
            GPUBridge = new BVHGPUBridge(buffers, data.Content);
            Algorithm = new BVHAlgorithm(BVHShaders.Load(), buffers, RebuiltEvent, data.Content);
            _bvhVisualizationFactory = new BVHVisualizationFactory(GPUBridge, data.Content, data.VisualizationData);
        }
        
        public void SendAndRebuild()
        {
            GPUBridge.SendBoxesToGPU();
            Rebuild();
        }
        
        public void Rebuild()
        {
            Algorithm.Execute();
            Visualization = _bvhVisualizationFactory.Create();
        }

        public void Dispose()
        {
            Algorithm.Dispose();
        }
    }
}