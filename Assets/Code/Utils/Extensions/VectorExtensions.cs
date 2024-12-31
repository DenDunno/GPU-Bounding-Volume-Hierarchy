using System;
using UnityEngine;

namespace Code.Utils.Extensions
{
    public static class VectorExtensions
    {
        public static void MultiplySpan(this Matrix4x4 matrix, Span<Vector3> points)
        {
            for (int i = 0; i < points.Length; ++i)
            {
                points[i] = matrix.MultiplyPoint3x4(points[i]);
            }
        }

        public static Vector3 Min(this Span<Vector3> points)
        {
            Vector3 min = Vector3.one * float.MaxValue;

            foreach (Vector3 point in points)
            {
                min = Vector3.Min(min, point);
            }

            return min;
        }
        
        public static Vector3 Max(this Span<Vector3> points)
        {
            Vector3 max = Vector3.one * float.MinValue;

            foreach (Vector3 point in points)
            {
                max = Vector3.Max(max, point);
            }

            return max;
        }
    }
}