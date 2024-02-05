using UnityEngine;

namespace Code.Utils.SubFrustums
{
    public readonly struct FrustumPlane
    {
        public readonly Vector3 Point;
        public readonly Vector3 Normal;

        public FrustumPlane(Vector3 normal, Vector3 point)
        {
            Point = point;
            Normal = normal.normalized;
        }
    
        public bool IsOutOfBounds(Vector3 spherePosition, float radius)
        {
            Vector3 difference = Point - spherePosition;
            float distance = Vector3.Dot(difference, Normal);

            // Vector3 endPoint = spherePosition + Normal * distance;
            // Color oldColor = Gizmos.color;
            // Gizmos.color = distance > radius ? Color.green : Color.red;
            // Gizmos.DrawLine(spherePosition, endPoint);
            // Gizmos.DrawSphere(endPoint, 0.1f);
            // Gizmos.color = oldColor;

            return distance < -radius;
        }

        public static int GetSize()
        {
            return 3 * 4 + 3 * 4;
        }
    }
}