using Code.Components.MortonCodeAssignment.Event;
using Code.Utils.Extensions;
using EditorWrapper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class StaticBVH : InputEditorLifeTime<BVHData>
    {
        private ParallelBVHFacade _facade;
        private IDrawable _visualization;

        public IEventClient RebuiltEvent => _facade.Components.RebuiltEvent;
        public BVHGPUBridge GPUBridge => _facade.Components.GPUBridge;

        protected override void Reassemble() {}

        private void Update()
        {
            if (Application.isPlaying)
                Bake();
        }

        [Button]
        public void Bake()
        {
            _facade?.Dispose();
            _facade = new ParallelBVHFacade(Data, BVHShaders.Load());
            _facade.Initialize();
            _facade.Rebuild();
            _facade.Components.Buffers.Nodes.Print<BVHNode>("", x => $"{x}\n");
        }
        
        public void Rebuild() => _facade.Rebuild();
        private void OnDrawGizmos() => _visualization?.Draw();
        protected override void Dispose() => _facade?.Dispose();
    }
}