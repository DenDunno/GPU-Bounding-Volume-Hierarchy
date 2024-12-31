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
            return new Mesh
            {
                vertices = new[]
                {
                    new Vector3(0 - width / 2, -height / 2, 0),
                    new Vector3(width - width / 2, -height / 2, 0),
                    new Vector3(0 - width / 2, height - height / 2, 0),
                    new Vector3(width - width / 2, height - height / 2, 0)
                },
                triangles = new[]
                {
                    0, 2, 1,
                    2, 3, 1
                },
                normals = new[]
                {
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward,
                    -Vector3.forward
                },
                uv = new[]
                {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1)
                }
            };
        }
        
        public static Mesh BuildUniformQuad(float width, float height, bool flipUV)
        {
            float start = flipUV ? 1 : 0;
            float end = flipUV ? 0 : 1;

            return new Mesh
            {
                vertices = new Vector3[]
                {
                    new(0, 0, 0),
                    new(-width / 2, -height / 2, 0f),
                    new(width / 2f, -height / 2, 0f),
                    new(width / 2, -height / 2, 0f),
                    new(width / 2f, height / 2, 0f),
                    new(width / 2f, height / 2, 0f),
                    new(-width / 2, height / 2, 0f),
                    new(-width / 2f, height / 2, 0f),
                    new(-width / 2, -height / 2, 0f),
                },
                uv = new Vector2[]
                {
                    new(0, 1),
                    new(start, 0),
                    new(end, 0),
                    new(start, 0),
                    new(end, 0),
                    new(start, 0),
                    new(end, 0),
                    new(start, 0),
                    new(end, 0)
                },
                triangles = new[]
                {
                    2, 1, 0,
                    4, 3, 0,
                    6, 5, 0,
                    8, 7, 0,
                },
            };
        }

        public static Mesh CreateIndexedCube()
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Mesh cubeMesh = cube.GetComponent<MeshFilter>().mesh;
            Mesh mesh = new();
            return mesh;
        }
        
        public static Mesh BuildUniformCube(float size)
        {
            MeshData data = new();

            AddSideToCube(size, flipUV: false, data, Quaternion.Euler(90, 0, 0), new Vector3(0, size / 2, 0));
            AddSideToCube(size, flipUV: false, data, Quaternion.Euler(-90, 0, 0), new Vector3(0, -size / 2, 0));

            AddSideToCube(size, flipUV: false, data, Quaternion.Euler(0, 0, 0), new Vector3(0, 0, -size / 2));
            AddSideToCube(size, flipUV: false, data, Quaternion.Euler(0, 180, 0), new Vector3(0, 0, size / 2));

            AddSideToCube(size, flipUV: false, data, Quaternion.Euler(0, 90, 0), new Vector3(-size / 2, 0, 0));
            AddSideToCube(size, flipUV: false, data, Quaternion.Euler(0, -90, 0), new Vector3(size / 2, 0, 0));

            return new Mesh()
            {
                vertices = data.Vertices.ToArray(),
                uv = data.UV.ToArray(),
                triangles = data.Triangles.ToArray(),
                normals = data.Normals.ToArray(),
            };
        }

        private static void AddSideToCube(float size, bool flipUV, MeshData cubeMesh, Quaternion rotation, Vector3 offset)
        {
            Mesh side = BuildUniformQuad(size, size, flipUV);
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
    }
}