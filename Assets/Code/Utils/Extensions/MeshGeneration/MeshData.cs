using System.Collections.Generic;
using UnityEngine;

namespace TerraformingTerrain2d
{
    public class MeshData
    {
        public readonly List<Vector3> Vertices = new();
        public readonly List<Vector3> Normals = new();
        public readonly List<int> Triangles = new();
        public readonly List<Vector2> UV = new();
    }
}