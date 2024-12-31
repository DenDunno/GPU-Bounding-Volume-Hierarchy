using System.Collections.Generic;
using System.Linq;
using EditorWrapper;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BoundingVolumeHierarchy : InputEditorLifeTime<BVHData>
    {
        [SerializeField] private List<TopLevelAccelerationStructure> _topLevelStructures;
        private ComputeBuffer _bottomLevelsBuffer;
        private IDrawable _visualization;
        
        public void Add(TopLevelAccelerationStructure topLevelStructure)
        {
            _topLevelStructures.Add(topLevelStructure);
        }

        public void Remove(TopLevelAccelerationStructure topLevelAccelerationStructure)
        {
            _topLevelStructures.Remove(topLevelAccelerationStructure);
        }
        
        protected override void Reassemble()
        {
            Bake();
        }

        public void Bake()
        {
            _bottomLevelsBuffer = CreateBottomLevelBuffer();
            IBoundingBoxesInput input = new ManualBoundingBoxesInput(_topLevelStructures);
            BVHFacade facade = new(Data, input, BVHShaders.Load());
            facade.Initialize();
            facade.Rebuild();
            
            _visualization = new DrawableIfTrue(
                new VisualizationFactory(Data).Create(facade.FetchTree(), 0),
                Data.Visualization.Show);
            
            facade.Dispose();
        }

        private ComputeBuffer CreateBottomLevelBuffer()
        {
            int bufferSize = _topLevelStructures.Sum(x => x.Cluster.Tree.Length);
            ComputeBuffer bottomLevelsBuffer = new(bufferSize, BVHNode.GetSize());

            int offset = 0;
            foreach (TopLevelAccelerationStructure structure in _topLevelStructures)
            {
                int treeSize = structure.Cluster.Tree.Length;
                bottomLevelsBuffer.SetData(structure.Cluster.Tree, 0, offset, treeSize);
                offset += treeSize;
            }

            return bottomLevelsBuffer;
        }

        private void OnDrawGizmos()
        {
            _visualization?.Draw();
        }

        protected override void Dispose()
        {
            if (_bottomLevelsBuffer != null && _bottomLevelsBuffer.IsValid())
                _bottomLevelsBuffer.Dispose();
        }
    }
}