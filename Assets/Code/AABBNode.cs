using System;
using Code.RenderFeature.Data;

namespace DefaultNamespace
{
    [Serializable]
    public struct AABBNode
    {
        public uint MortonCode;
        public AABB AABB;

        public AABBNode(uint mortonCode, AABB aabb)
        {
            MortonCode = mortonCode;
            AABB = aabb;
        }
    }
}