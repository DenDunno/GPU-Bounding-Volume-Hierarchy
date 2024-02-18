using System;
using UnityEngine;

namespace Code.RenderFeature.Data
{
    [Serializable]
    public struct AABB
    {
        public Vector3 Min;
        public Vector3 Max;

        public AABB(Bounds bounds) : this(bounds.min, bounds.max)
        {
        }

        public AABB(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public AABB Union(AABB box)
        {
            return new AABB(Vector3.Min(Min, box.Min), Vector3.Max(Max, box.Max));
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

        public Vector3 Centroid => (Min + Max) / 2f;

        public void Draw()
        {
            Draw(Color.green);
        }
        
        public void Draw(Color gizmoColor)
        {
            Color color = Gizmos.color;
            Gizmos.color = gizmoColor;
            
            Gizmos.DrawWireCube(Centroid, Max - Min);

            Gizmos.color = color;
        }
    }
}