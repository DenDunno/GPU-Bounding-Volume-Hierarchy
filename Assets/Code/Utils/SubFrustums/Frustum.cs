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

        public bool IsOutside(Vector3 position, float radius, bool show)
        {
            return Top.IsOutOfBounds(position, radius, show) ||
                   Bottom.IsOutOfBounds(position, radius, show) ||
                   Right.IsOutOfBounds(position, radius, show) ||
                   Left.IsOutOfBounds(position, radius, show) ||
                   Far.IsOutOfBounds(position, radius, show) ||
                   Near.IsOutOfBounds(position, radius, show);
        }

        public static int GetSize()
        {
            return FrustumPlane.GetSize() * 6;
        }
    };
}