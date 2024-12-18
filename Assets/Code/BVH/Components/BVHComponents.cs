using Code.Components.MortonCodeAssignment.Event;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHComponents 
    {
        public readonly IBoundingBoxesInput BoxesInput;
        public readonly EventWrapper RebuiltEvent;
        public readonly BVHGPUBridge GPUBridge;
        public readonly BVHAlgorithm Algorithm;
        public readonly BVHBuffers Buffers;

        public BVHComponents(BVHData data, BVHShaders shaders)
        {
            RebuiltEvent = new EventWrapper();
            BoxesInput = data.BoxesInput.Value;
            Buffers = new BVHBuffers(data.BoxesInput.Count);
            GPUBridge = new BVHGPUBridge(Buffers, data.BoxesInput.Count);
            IBVHConstructionAlgorithm construction = new BVHConstructionFactory(Buffers, shaders, data.Algorithm, data.IsStupidSearch).Create();
            Algorithm = new BVHAlgorithm(shaders, Buffers, RebuiltEvent, construction, data.SceneSize.Box);
        }
    }
}