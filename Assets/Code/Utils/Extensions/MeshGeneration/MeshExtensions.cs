using System.Linq;
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

        public static Mesh BuildUniformCube(float size)
        {
            MeshData data = new();
            Mesh side = BuildUniformQuad(size, size);

            AddSideToCube(data, side, Quaternion.Euler(90, 0, 0), new Vector3(0, size / 2, 0));
            AddSideToCube(data, side, Quaternion.Euler(-90, 0, 0), new Vector3(0, -size / 2, 0));
            AddSideToCube(data, side, Quaternion.Euler(0, 0, 0), new Vector3(0, 0, -size / 2));
            AddSideToCube(data, side, Quaternion.Euler(0, 180, 0), new Vector3(0, 0, size / 2));
            AddSideToCube(data, side, Quaternion.Euler(0, 90, 0), new Vector3(-size / 2, 0, 0));
            AddSideToCube(data, side, Quaternion.Euler(0, -90, 0), new Vector3(size / 2, 0, 0));

            return new Mesh()
            {
                vertices = data.Vertices.ToArray(),
                uv = data.UV.ToArray(),
                triangles = data.Triangles.ToArray(),
                normals = data.Normals.ToArray(),
            };
        }

        private static void AddSideToCube(MeshData cubeMesh, Mesh side, Quaternion rotation, Vector3 offset)
        {
            int verticesCount = cubeMesh.Vertices.Count;
            
            foreach (Vector3 sideVertex in side.vertices)
            {
                cubeMesh.Vertices.Add(rotation * sideVertex + offset);
            }
            
            cubeMesh.Normals.AddRange(Enumerable.Repeat(offset.normalized, side.vertexCount));
            cubeMesh.UV.AddRange(side.uv);
            
            foreach (int vertexIndex in side.triangles)
            {
                cubeMesh.Triangles.Add(vertexIndex + verticesCount);
            }
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