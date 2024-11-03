using System.Collections.Generic;
using Code.Components.MortonCodeAssignment.Event;
using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class GPUBoundingVolumeHierarchy : InEditorLifetime
    {
        [SerializeField] [Min(1)] private int _bufferSize;
        private BVHComponents _components;

        public List<AABB> BoundingBoxes => _components.Content.BoundingBoxes;
        public IEventClient RebuiltEvent => _components.RebuiltEvent;
        public BVHGPUBridge GPUBridge => _components.GPUBridge;

        protected override void Reassemble()
        {
            _components = new BVHComponents(_bufferSize);
            _components.Initialize();
        }

        public void Rebuild() => _components.Rebuild();
        public void SendAndRebuild() => _components.SendAndRebuild();
        protected override void Dispose() => _components?.Dispose();
    }
}