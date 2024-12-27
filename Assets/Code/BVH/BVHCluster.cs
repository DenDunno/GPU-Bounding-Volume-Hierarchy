using Code.Components.MortonCodeAssignment.Event;
using EditorWrapper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHCluster : InputEditorLifeTime<BVHData>
    {
        [SerializeField, HideInInspector] private BVHNode[] _tree;
        [SerializeField] private bool _enableNaming;
        private IDrawable _visualization;
        private BVHFacade _facade;

        public IEventClient RebuiltEvent => _facade.Components.RebuiltEvent;
        public BVHGPUBridge GPUBridge => _facade.Components.GPUBridge;

        protected override void Reassemble()
        {
            _visualization = new DrawableIfTrue(
                new VisualizationFactory(Data).Create(_tree, 0),
                Data.Visualization.Show);
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
            _facade = new BVHFacade(Data, BVHShaders.Load());
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