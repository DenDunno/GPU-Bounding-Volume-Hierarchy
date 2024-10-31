using System.Linq;
using Code.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class GPUBoundingVolumeHierarchy : InEditorLifetime
    {
        [SerializeField] private Sphere[] _spheres;
        [SerializeField] private BVHDebug _debug;
        private BVHAlgorithm _algorithm;

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
            _debug.Initialize(_spheres.Length - 1, _algorithm.Buffers.Nodes);
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