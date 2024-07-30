using System;

namespace Code.Data
{
    [Serializable]
    public struct AABBNode
    {
        public AABB AABB;
        public uint MortonCode;

        public AABBNode(uint mortonCode, AABB aabb)
        {
            MortonCode = mortonCode;
            AABB = aabb;
        }

        public static int GetSize() // 26
        {
            return AABB.GetSize() + 4;
        }
    }
}