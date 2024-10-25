using UnityEngine;

namespace Code.Data
{
    public readonly struct AABB
    {
        public readonly Vector3 Min;
        public readonly Vector3 Max;

        public AABB(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public Vector3 Centroid => (Min + Max) / 2f;

        public void Draw()
        {
            Draw(Color.green);
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
        
        public void Draw(Color gizmoColor)
        {
            Color color = Gizmos.color;
            Gizmos.color = gizmoColor;

            Gizmos.DrawWireCube(Centroid, Max - Min);

            Gizmos.color = color;
        }
        
        public static int GetSize() // 24
        {
            return 3 * 4 + 
                   3 * 4; 
        }
    }
}