using System;
using System.Linq;
using Code.Components.MortonCodeAssignment.TestTree;
using Code.Data;
using Code.Utils.Extensions;
using DefaultNamespace;
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
        [SerializeField] private float _depthFactor2;
        private BVHAlgorithm _algorithm;
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
        }

        private void Update()
        {
            new TreeVisualization(_depthFactor, _depthFactor2).Draw(_root);
        }

        protected override void Dispose()
        {
            _algorithm?.Dispose();
        }
    }
}