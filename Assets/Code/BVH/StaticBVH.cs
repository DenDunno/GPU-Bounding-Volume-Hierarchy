using Code.Components.MortonCodeAssignment.Event;
using Code.Utils.Extensions;
using Code.Utils.GPUShaderEmulator;
using EditorWrapper;
using Sirenix.OdinInspector;
using Unity.Collections;
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
            NativeArray<int> buffer = new(8, Allocator.TempJob);
            GPUShaderEmulator<BatchTest> test = new(2, 4, new BatchTest(buffer));
            test.Execute();
            buffer.ToArray().Print();
            buffer.Dispose();
            // _facade?.Dispose();
            // _facade = new ParallelBVHFacade(Data, BVHShaders.Load());
            // _facade.Initialize();
            // _facade.Rebuild();
            // GPUBridge.FetchTree().Print("", x => $"\n{x}");
        }
        
        public void Rebuild() => _facade.Rebuild();
        private void OnDrawGizmos() => _visualization?.Draw();
        protected override void Dispose() => _facade?.Dispose();
    }
}