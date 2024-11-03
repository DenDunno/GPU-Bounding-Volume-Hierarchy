using System.Linq;
using Code.Data;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHTestPayload : MonoBehaviour
    {
        [SerializeField] private GPUBoundingVolumeHierarchy _bvh;
        [SerializeField] private Sphere[] _spheres;

        [Button]
        public void Rebuild()
        {
            _bvh.BoundingBoxes.Clear();
            AABB[] aabbs = _spheres.Select(sphere => sphere.Provide()).ToArray();
            aabbs.ForEach(aabb => _bvh.BoundingBoxes.Add(aabb));
            _bvh.SendAndRebuild();
        }

        private void Update()
        {
            Rebuild();
        }
    }
}