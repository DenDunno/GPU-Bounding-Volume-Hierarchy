using Code.Components.MortonCodeAssignment.Event;
using EditorWrapper;
using Sirenix.OdinInspector;

namespace Code.Components.MortonCodeAssignment
{
    public class StaticBVH : InputEditorLifeTime<BVHData>
    {
        private BVHComponents _components;
        private IDrawable _visualization;

        public BVHGPUBridge GPUBridge => _components.GPUBridge;
        public IEventClient RebuiltEvent => _components.RebuiltEvent;

        protected override void Reassemble()
        {
        }

        [Button]
        public void Bake()
        {
            _components?.Dispose();
            _components = new BVHComponents(Data);
            _components.Initialize();
            _components.Rebuild();
            Data.Nodes = _components.GPUBridge.FetchTree();
            Data.Root = _components.GPUBridge.FetchRoot();
            _visualization = new VisualizationFactory(Data).Create();
        }
        
        public void Rebuild() => _components.Rebuild();
        //private void OnDrawGizmos() => Data.BoxesInput.Value.Calculate().ForEach(x => x.Draw());
        private void OnDrawGizmos() => _visualization?.Draw();
        protected override void Dispose() => _components?.Dispose();
    }
}