using Code.Components.MortonCodeAssignment.Event;
using EditorWrapper;
using Sirenix.OdinInspector;

namespace Code.Components.MortonCodeAssignment
{
    public class StaticBVH : InputEditorLifeTime<BVHData>
    {
        private AssetFacade _facade;
        private IDrawable _visualization;

        public IEventClient RebuiltEvent => _facade.Components.RebuiltEvent;
        public BVHGPUBridge GPUBridge => _facade.Components.GPUBridge;

        protected override void Reassemble()
        {
        }

        [Button]
        public void Bake()
        {
            _facade?.Dispose();
            _facade = new AssetFacade(Data, BVHShaders.Load());
            _facade.Initialize();
            _facade.Rebuild();
            Data.Nodes = GPUBridge.FetchTree();
            Data.Root = GPUBridge.FetchRoot();
            _visualization = new VisualizationFactory(Data).Create();
        }
        
        public void Rebuild() => _facade.Rebuild();
        //private void OnDrawGizmos() => Data.BoxesInput.Value.Calculate().ForEach(x => x.Draw());
        private void OnDrawGizmos() => _visualization?.Draw();
        protected override void Dispose() => _facade?.Dispose();
    }
}