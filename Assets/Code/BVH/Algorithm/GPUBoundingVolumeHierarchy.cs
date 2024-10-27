using System.Linq;
using Code.Data;
using Sirenix.Utilities;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class GPUBoundingVolumeHierarchy : InEditorLifetime
    {
        [SerializeField] private Sphere[] _spheres;
        private BVHAlgorithm _algorithm;
        private BVHBuffers _buffers;

        protected override void Reassemble()
        {
            _buffers = new BVHBuffers(_spheres.Length);
            
            AABB[] boxes = _spheres.Select(sphere => sphere.Provide()).ToArray();
            _buffers.Boxes.SetData(boxes);

            ShadersPresenter shaders = new ShadersPresenter().Load();
            _algorithm = new BVHAlgorithm(_buffers, shaders, _spheres.Length);
            _algorithm.Initialize();
        }

        private void Update()
        {
            _spheres.ForEach(x => x.Provide().Draw());
            _algorithm.Execute(_spheres.Length);
        }

        protected override void Dispose()
        {            
            _buffers.Dispose();
        }
    }
}