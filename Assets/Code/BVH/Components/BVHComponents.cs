using Code.Components.MortonCodeAssignment.Event;
using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHComponents 
    {
        public readonly IBoundingBoxesInput BoxesInput;
        public readonly TreeConstructionBuffers Buffers;
        public readonly EventWrapper RebuiltEvent;
        public readonly BVHGPUBridge GPUBridge;
        public readonly BVHAlgorithm Algorithm;

        public BVHComponents(BVHConstructionAlgorithmId algorithm, BVHShaders shaders, IBoundingBoxesInput boxesInput, AABB bounds)
        {
            BoxesInput = boxesInput;
            RebuiltEvent = new EventWrapper();
            Buffers = new TreeConstructionBuffers(BoxesInput.Count);
            GPUBridge = new BVHGPUBridge(Buffers, BoxesInput.Count);
            IBVHConstructionAlgorithm construction = new BVHConstructionFactory(Buffers, shaders, algorithm).Create();
            Algorithm = new BVHAlgorithm(shaders, Buffers, RebuiltEvent, construction, bounds);
        }
    }
}