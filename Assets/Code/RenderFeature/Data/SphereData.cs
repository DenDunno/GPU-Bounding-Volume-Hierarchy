using System;
using UnityEngine;

namespace Code.RenderFeature.Data
{
    [Serializable]
    public struct SphereData
    {
        public Vector3 Position;
        public float Radius;

        public SphereData(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public static int GetSize() // 16
        {
            return 3 * 4 + 4;
        }
    }
}