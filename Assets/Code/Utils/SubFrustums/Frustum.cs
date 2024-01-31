using UnityEngine;

namespace Code.Utils.SubFrustums
{
    public struct Frustum
    {
        public FrustumPlane Top;
        public FrustumPlane Bottom;

        public FrustumPlane Right;
        public FrustumPlane Left;

        public FrustumPlane Far;
        public FrustumPlane Near;

        public bool IsOutside(Vector3 position, float radius)
        {
            return Top.IsOutOfBounds(position, radius) ||
                   Bottom.IsOutOfBounds(position, radius) ||
                   Right.IsOutOfBounds(position, radius) ||
                   Left.IsOutOfBounds(position, radius) ||
                   Far.IsOutOfBounds(position, radius) ||
                   Near.IsOutOfBounds(position, radius);
        }

        public static int GetSize()
        {
            return FrustumPlane.GetSize() * 6;
        }
    };
}