using System.Linq;
using Code.Components.MortonCodeAssignment.TestTree;
using Code.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class GPUBoundingVolumeHierarchy : InEditorLifetime
    {
        [SerializeField] private int _test;
        [SerializeField] private Sphere[] _spheres;
        [SerializeField] private float _depthFactor;
        private BVHAlgorithm _algorithm;
        private TreeVisualization _tree;
        private BVHBuffers _buffers;
        private TreeNode _root;

        protected override void Reassemble()
        {
            _buffers = new BVHBuffers(_spheres.Length);
            
            AABB[] boxes = _spheres.Select(sphere => sphere.Provide()).ToArray();
            _buffers.Boxes.SetData(boxes);

            BVHShaders bvhShaders = BVHShaders.Load();
            _algorithm = new BVHAlgorithm(_buffers, bvhShaders, _spheres.Length);
            _algorithm.Initialize();
        }

        [Button]
        private void Dispatch()
        {
            _algorithm.Execute(_spheres.Length);
            _root = new TreeCalculator().Compute(_buffers.Nodes, _spheres.Length);
            _tree = new TreeVisualization(_spheres.Length);
            _tree.Initialize(_root);
        }

        private void Update()
        {
            _tree?.Draw(_root, _depthFactor);
        }

        protected override void Dispose()
        {
            _algorithm?.Dispose();
        }
    }
}