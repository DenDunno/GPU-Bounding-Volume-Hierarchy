using System.Linq;
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

        [Button]
        private void Dispatch()
        {
            _algorithm.Execute(_spheres.Length);
            
            MortonCode[] mortonCodes = new MortonCode[_spheres.Length];
            _buffers.MortonCodes.GetData(mortonCodes);
            mortonCodes.Print();
        }

        protected override void Dispose()
        {
            _algorithm?.Dispose();
        }

        Vector3 GetSpherePosition(MortonCode[] codes, int mortonCodeId)
        {
            uint sphereId = codes[mortonCodeId].ObjectId;
            return _spheres[sphereId].transform.position;
        }
        
        private void OnDrawGizmos()
        {
            Reassemble();
            _spheres.ForEach(x => x.Provide().Draw());

            _algorithm.Execute(_spheres.Length);
            MortonCode[] mortonCodes = new MortonCode[_spheres.Length];
            _buffers.MortonCodes.GetData(mortonCodes);
            
            for (int i = 0; i < mortonCodes.Length - 1; ++i)
            {
                Gizmos.DrawLine(
                    GetSpherePosition(mortonCodes, i),
                    GetSpherePosition(mortonCodes, i + 1));
            }
        }
    }
}