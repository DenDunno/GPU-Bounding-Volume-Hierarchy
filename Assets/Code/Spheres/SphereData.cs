using System;
using UnityEngine;

namespace Code.RenderFeature
{
    [Serializable]
    public struct SphereData
    {
        public Vector3 Position;
        public float Radius;
        public Color Color;
        public Color IntersectionColor;
        public float IntersectionPower;

        public SphereData(Vector3 position, float radius, Color color, Color intersectionColor, float intersectionPower)
        {
            Position = position;
            Radius = radius;
            Color = color;
            IntersectionColor = intersectionColor;
            IntersectionPower = intersectionPower;
        }

        public static int GetSize()
        {
            return 3 * 4 +
                   4 + 
                   4 * 4 +
                   4 * 4 + 
                   4;
        }
    }
}