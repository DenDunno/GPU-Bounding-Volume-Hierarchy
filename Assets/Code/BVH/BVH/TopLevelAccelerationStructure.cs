using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [ExecuteInEditMode]
    public class TopLevelAccelerationStructure : InEditorLifetime, IAABBProvider
    {
        public BottomLevelAccelerationStructure Cluster;

        public AABB CalculateBox()
        {
            AABB worldSpaceBounds = transform.localToWorldMatrix * Cluster.Bounds;
            
            return worldSpaceBounds;
        }
    }
}