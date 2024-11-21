using System;
using EditorWrapper;
using UnityEngine;

namespace Code.Data
{
    [Serializable]
    public readonly struct AABB : IDrawable
    {
        public readonly Vector3 Min;
        public readonly Vector3 Max;

        public AABB(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public Vector3 Size => Max - Min;
        public Vector3 Centroid => (Min + Max) / 2f;
        
        public void Draw()
        {
            Gizmos.DrawCube(Centroid, Max - Min);
            Gizmos.DrawWireCube(Centroid, Max - Min);
        }

        public Vector3 GetRelativeCoordinates(Vector3 point)
        {
            point.x -= Min.x;
            point.y -= Min.y;
            point.z -= Min.z;
            point.x /= Max.x - Min.x;
            point.y /= Max.y - Min.y;
            point.z /= Max.z - Min.z;
            
            return point;
        }
        
        public static int GetSize() // 24
        {
            return 3 * 4 + 
                   3 * 4; 
        }

        public override string ToString()
        {
            return $"Max = {Max} Min = {Min}";
        }

        public AABB Union(AABB box)
        {
            return new AABB(Vector3.Min(Min, box.Min), Vector3.Max(Max, box.Max));
        }

        public float ComputeSurfaceArea()
        {
            Vector3 size = Size;
            return 2 * (size.x * size.y + size.y * size.z + size.z * size.x);
        }
    }
}