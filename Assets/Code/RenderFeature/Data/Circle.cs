using UnityEngine;

namespace Code.RenderFeature.Data
{
    public struct Circle
    {
        public Vector2 Position;
        public float Radius;

        public static int GetSize()
        {
            return 4 * 2 + 4;
        }

        public override string ToString()
        {
            return $"Position = {Position} Radius = {Radius}";
        }
    }
}