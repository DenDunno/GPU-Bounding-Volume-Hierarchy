using UnityEngine;

namespace Code
{
    public struct BoundingBox
    {
        public readonly Vector3 Min;
        public readonly Vector3 Max;

        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public Vector3 Centre => (Min + Max) / 2;
        public Vector3 Size => Max - Min;
    }
}