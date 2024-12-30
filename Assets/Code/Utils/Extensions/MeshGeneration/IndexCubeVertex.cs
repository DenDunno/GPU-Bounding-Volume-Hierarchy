using UnityEngine;

namespace TerraformingTerrain2d
{
    public readonly struct IndexCubeVertex
    {
        public readonly Vector3 Position;
        public readonly Vector3 Normal;
        public readonly Vector2 UV;
        public readonly int SideIndex;

        public IndexCubeVertex(Vector3 position, Vector3 normal, Vector2 uv, int sideIndex)
        {
            SideIndex = sideIndex;
            Position = position;
            Normal = normal;
            UV = uv;
        }
    }
}