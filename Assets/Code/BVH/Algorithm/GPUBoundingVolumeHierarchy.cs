using System;
using System.Collections.Generic;
using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class GPUBoundingVolumeHierarchy : InEditorLifetime
    {
        [SerializeField] private BVHDebug _debug;
        [SerializeField] [Min(1)] private int _bufferSize;
        private BVHComponents _components;

        public List<AABB> BoundingBoxes => _components.BoundingBoxes;
        public BVHGPUBridge GPUBridge => _components.GPUBridge;
        public event Action Rebuilt;

        protected override void Reassemble()
        {
            _components = new BVHComponents(_bufferSize);
            _components.Initialize();
            Rebuild();
        }

        public void SendAndRebuild()
        {
            _components.GPUBridge.SendBoxesToGPU();
            Rebuild();
        }
        
        public void Rebuild()
        {
            _components.Algorithm.Execute(_components.BoundingBoxes.Count);
            Rebuilt?.Invoke();
        }

        private void OnDrawGizmos() => _debug?.Draw();
        protected override void Dispose() => _components?.Dispose();
    }
}