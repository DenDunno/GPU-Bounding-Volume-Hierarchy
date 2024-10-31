using System.Linq;
using Code.Components.MortonCodeAssignment.TestTree;
using Code.Data;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class GPUBoundingVolumeHierarchy : InEditorLifetime
    {
        [SerializeField] private int _test;
        [SerializeField] private Sphere[] _spheres;
        [SerializeField] private float _depthFactor;
        [SerializeField] private float _lineLength;
        [SerializeField] private bool _showNodes;
        [SerializeField] private bool _showBounds;
        private BVHAlgorithm _algorithm;
        private TreeVisualization _tree;
        private TreeNode _root;
        
        private int InnerNodesCount => _spheres.Length - 1;
        
        protected override void Reassemble()
        {
            if (_spheres.Length <= 0) return;

            BVHShaders bvhShaders = BVHShaders.Load();
            _algorithm = new BVHAlgorithm(bvhShaders, _spheres.Length);
            
            AABB[] boxes = _spheres.Select(sphere => sphere.Provide()).ToArray();
            _algorithm.Buffers.Boxes.SetData(boxes);
            _algorithm.Initialize();
        }

        [Button]
        private void Dispatch()
        {
            Dispose();
            Reassemble();
            _algorithm.Execute(_spheres.Length);
            _root = new TreeCalculator().Compute(_algorithm.Buffers, InnerNodesCount, _showNodes);
            _tree = new TreeVisualization(InnerNodesCount);
            _tree.Initialize(_root);
        }

        private void Update()
        {
            if (Application.isPlaying)
                Dispatch();
            
            _tree?.Draw(_root, _depthFactor, _lineLength);
        }

        protected override void Dispose()
        {
            _algorithm?.Dispose();
        }

        private void OnDrawGizmos()
        {
            if (_showBounds)
            {
                _spheres.ForEach(x => x.Provide().Draw());   
            }
        }
    }
}