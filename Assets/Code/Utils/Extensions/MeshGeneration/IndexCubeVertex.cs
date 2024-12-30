using UnityEngine;

namespace TerraformingTerrain2d
{
    public readonly struct IndexCubeVertex
    {
        public readonly Vector3 Position;
        public readonly Vector3 Normal;
        public readonly Vector2 UV;
        public readonly Vector2Int SideIndex;

        public IndexCubeVertex(Vector3 position, Vector3 normal, Vector2 uv, int sideIndex)
        {
            SideIndex = new Vector2Int(sideIndex, 0);
            Position = position;
            Normal = normal;
            UV = uv;
        }
    }
}