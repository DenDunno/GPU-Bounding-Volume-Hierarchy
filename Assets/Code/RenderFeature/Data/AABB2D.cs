using UnityEngine;

namespace Code.RenderFeature.Data
{
    public struct AABB2D
    {
        public Vector2 Min;
        public Vector2 Max;
        
        public static int GetSize()
        {
            return 4 * 2 + 4 * 2;
        }
    }
}