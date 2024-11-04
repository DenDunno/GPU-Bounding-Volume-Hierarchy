using Code.Components.MortonCodeAssignment.Event;

namespace Code.Components.MortonCodeAssignment
{
    public class GPUBoundingVolumeHierarchy : InEditorLifetime<BVHData>
    {
        private BVHComponents _components;
        
        public BVHContent Content => Data.Content;
        public BVHGPUBridge GPUBridge => _components.GPUBridge;
        public IEventClient RebuiltEvent => _components.RebuiltEvent;

        protected override void Reassemble(BVHData data)
        {
            _components = new BVHComponents(data);
            _components.Algorithm.Initialize();
            SendAndRebuild();
        }

        public void Rebuild() => _components.Rebuild();
        public void SendAndRebuild() => _components.SendAndRebuild();
        
        private void OnDrawGizmos() => _components?.Visualization?.Draw();
        protected override void Dispose() => _components?.Dispose();
    }
}