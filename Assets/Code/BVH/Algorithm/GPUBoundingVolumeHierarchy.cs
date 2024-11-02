using System;
using System.Linq;
using Code.Data;
using Code.Utils.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class GPUBoundingVolumeHierarchy : InEditorLifetime
    {
        [SerializeField] private Sphere[] _spheres;
        [SerializeField] private BVHDebug _debug;
        private BVHAlgorithm _algorithm;

        public event Action Rebuilt;
        
        protected override void Reassemble()
        {
            if (_spheres.Length <= 0) return;

            BVHShaders bvhShaders = BVHShaders.Load();
            _algorithm = new BVHAlgorithm(bvhShaders, _spheres.Length);
            
            AABB[] boxes = _spheres.Select(sphere => sphere.Provide()).ToArray();
            _algorithm.Buffers.Boxes.SetData(boxes);
            _algorithm.Initialize();
            _algorithm.Execute(_spheres.Length);
            _debug.Initialize(_spheres.Length - 1, _algorithm.Buffers);
            Rebuilt?.Invoke();
        }

        public BVHNode[] FetchInnerNodes()
        {
            return _algorithm.Buffers.Nodes.FetchData<BVHNode>(_spheres.Length - 1);
        }

        [Button]
        private void Dispatch()
        {
            OnValidate();
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                Dispatch();

            _debug?.Draw();
        }

        protected override void Dispose()
        {
            _algorithm?.Dispose();
        }
    }
}