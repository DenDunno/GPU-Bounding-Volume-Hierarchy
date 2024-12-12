using Code.Components.MortonCodeAssignment.Event;
using EditorWrapper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class StaticBVH : InputEditorLifeTime<BVHData>
    {
        [SerializeField, HideInInspector] private BVHNode[] _tree;
        [SerializeField] private bool _enableNaming;
        private ParallelBVHFacade _facade;
        private IDrawable _visualization;

        public IEventClient RebuiltEvent => _facade.Components.RebuiltEvent;
        public BVHGPUBridge GPUBridge => _facade.Components.GPUBridge;

        protected override void Reassemble()
        {
            if (_tree.Length != 0)
                _visualization = new VisualizationFactory(Data).Create(_tree, _tree.Length - 1);
        }

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
            _tree = GPUBridge.FetchTree();
            Reassemble();
        }

        public void Rebuild() => _facade.Rebuild();
        private void OnDrawGizmos() => _visualization?.Draw();
        protected override void Dispose() => _facade?.Dispose();
    }
}