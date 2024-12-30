using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BottomLevelAccelerationStructure : ScriptableObject
    {
        [field: SerializeField, HideInInspector] public BVHNode[] Tree { get; private set; }
        [field: SerializeField, HideInInspector] public AABB Bounds { get; private set; }

        public void Initialize(BVHNode[] tree)
        {
            Bounds = tree[0].Box;
            Tree = tree;
        }
    }
}