using System.Collections.Generic;
using EditorWrapper;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BoundingVolumeHierarchy : InputEditorLifeTime<BVHData>
    {
        [SerializeField] private List<TopLevelAccelerationStructure> _topLevelStructures;
        private IDrawable _visualization;
        private BVHFacade _facade;

        public void Add(TopLevelAccelerationStructure topLevelStructure)
        {
            _topLevelStructures.Add(topLevelStructure);
        }
        
        protected override void Reassemble()
        {
            BVHBuffers buffers = new BVHBuffersFactory(_topLevelStructures).Create();
            IBoundingBoxesInput input = new ManualBoundingBoxesInput(_topLevelStructures);
            _facade = new BVHFacade(Data.Algorithm, input, BVHShaders.Load(), Data.SceneBounds);
            _facade.Initialize();
            buffers.Dispose();
            Bake();
        }

        public void Bake()
        {
            _facade.Rebuild();
            
            _visualization = new DrawableIfTrue(
                new VisualizationFactory(Data).Create(_facade.FetchTree(), 0),
                Data.Visualization.Show);
        }

        protected override void Dispose()
        {
            _facade?.Dispose();
        }

        private void OnDrawGizmos()
        {
            _visualization?.Draw();
        }
    }
}