using System;
using Code.Data;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [ExecuteInEditMode]
    public class TopLevelAccelerationStructure : InEditorLifetime, IAABBProvider
    {
        public BottomLevelAccelerationStructure Cluster;
        private BoundingVolumeHierarchy _bvh;
        private BoundingVolumeHierarchy BVH => _bvh ??= FindFirstObjectByType<BoundingVolumeHierarchy>();
        private TransformSnapshot _transformSnapshot;
        
        private void Update()
        {
            if (transform.HasChanged(ref _transformSnapshot))
            {
                BVH.Bake();
            }
        }

        public AABB CalculateBox()
        {
            AABB bounds = Cluster.Bounds;
            Span<Vector3> corners = stackalloc Vector3[8];
            bounds.GetCorners(corners);

            transform.localToWorldMatrix.MultiplySpan(corners);
            return new AABB(corners.Min(), corners.Max());
        }
    }
}