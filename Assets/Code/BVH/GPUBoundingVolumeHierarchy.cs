using Code.Components.MortonCodeAssignment.Event;

namespace Code.Components.MortonCodeAssignment
{
    public class GPUBoundingVolumeHierarchy : InputEditorLifeTime<BVHData>
    {
        private readonly EventWrapper _rebuiltEvent = new();
        private BVHComponents _components;

        public BVHContent Content => _components.Content;
        public BVHGPUBridge GPUBridge => _components.GPUBridge;
        public IEventClient RebuiltEvent => _rebuiltEvent;

        protected override void Reassemble()
        {
            _components = new BVHComponents(Data, _rebuiltEvent);
            _components.Algorithm.Initialize();
        }

        public void Rebuild() => _components.Rebuild();
        public void SendAndRebuild() => _components.SendAndRebuild();
        protected override void Dispose() => _components?.Dispose();
    }
}