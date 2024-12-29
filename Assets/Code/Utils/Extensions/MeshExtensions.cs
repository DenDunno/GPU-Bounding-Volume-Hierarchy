using Code.Components.MortonCodeAssignment;
using UnityEngine;

namespace TerraformingTerrain2d
{
    public static class MeshExtensions
    {
        public static Mesh BuildQuad(float size)
        {
            return BuildQuad(size, size);
        }

        public static Mesh BuildQuad(float width, float height)
        {
            Mesh mesh = new();

            Vector3[] vertices = new Vector3[4]
            {
                new(0 - width / 2, -height / 2, 0),
                new(width - width / 2, -height / 2, 0),
                new(0 - width / 2, height - height / 2, 0),
                new(width - width / 2, height - height / 2, 0)
            };
            mesh.vertices = vertices;

            int[] tris = new int[6]
            {
                0, 2, 1,
                2, 3, 1
            };
            mesh.triangles = tris;

            Vector3[] normals = new Vector3[4]
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward
            };
            mesh.normals = normals;

            Vector2[] uv = new Vector2[4]
            {
                new(0, 0),
                new(1, 0),
                new(0, 1),
                new(1, 1)
            };
            mesh.uv = uv;

            return mesh;
        }

        public static Mesh BuildUniformQuad(float width, float height)
        {
            return new Mesh
            {
                vertices = new Vector3[]
                {
                    new(0, 0, 0),
                    new(-width / 2, -height / 2, 0f),
                    new(width / 2f, -height / 2, 0f),
                    new(width / 2,  -height / 2, 0f),
                    new(width / 2f, height / 2, 0f),
                    new(width / 2f, height / 2, 0f),
                    new(-width / 2, height / 2, 0f),
                    new(-width / 2f, height / 2, 0f),
                    new(-width / 2, -height / 2, 0f),
                },
                uv = new Vector2[]
                {
                    new(0, 1),
                    new(0, 0),
                    new(1, 0),
                    new(0, 0),
                    new(1, 0),
                    new(0, 0),
                    new(1, 0),
                    new(0, 0),
                    new(1, 0),
                },
                triangles = new[]
                {
                    2, 1, 0,
                    4, 3, 0,
                    6, 5, 0,
                    8, 7, 0,
                },
            };;
        }
    }
}