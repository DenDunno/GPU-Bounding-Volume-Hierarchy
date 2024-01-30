using UnityEngine;

namespace Code.Utils
{
    public struct RectPoints
    {
        public readonly Vector3 TopLeft;
        public readonly Vector3 TopRight;
        public readonly Vector3 BottomRight;
        public readonly Vector3 BottomLeft;

        public RectPoints(Vector3 topLeft, Vector3 topRight, Vector3 bottomRight, Vector3 bottomLeft)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;
        }

        public Vector3 this[int i] => i switch
        {
            0 => TopLeft,
            1 => TopRight,
            2 => BottomRight,
            3 => BottomLeft,
            _ => Vector3.zero
        };
    }
}