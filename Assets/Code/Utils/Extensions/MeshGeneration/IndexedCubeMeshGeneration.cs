using System.Collections.Generic;
using TerraformingTerrain2d;
using UnityEngine;
using UnityEngine.Rendering;

public class IndexedCubeMeshGeneration 
{
    private readonly float _size;

    public IndexedCubeMeshGeneration(float size)
    {
        _size = size / 2f;
    }
    
    public Mesh Build()
    {
        Mesh mesh = new();
        int vertexCount = 24;
        int indicesCount = 36;
        SetParams(mesh, vertexCount, indicesCount);
        SetVertexBuffer(mesh, vertexCount);
        SetIndexBuffer(mesh, indicesCount);
        SetSubMesh(mesh, indicesCount);

        return mesh;
    }

    private static void SetParams(Mesh mesh, int vertexCount, int indicesCount)
    {
        mesh.SetIndexBufferParams(indicesCount, IndexFormat.UInt32);
        mesh.SetVertexBufferParams(vertexCount, new VertexAttributeDescriptor[]
        {
            new(VertexAttribute.Position),
            new(VertexAttribute.Normal),
            new(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2),
            new(VertexAttribute.TexCoord1, VertexAttributeFormat.UInt32, 2),
        });
    }

    private void SetVertexBuffer(Mesh mesh, int vertexCount)
    {
        mesh.SetVertexBufferData(new List<IndexCubeVertex>()
        {
            // front
            new(new Vector3(_size, _size, -_size), -Vector3.forward, new Vector2(1, 1), 0),
            new(new Vector3(_size, -_size, -_size), -Vector3.forward, new Vector2(1, 0), 0),
            new(new Vector3(-_size, -_size, -_size), -Vector3.forward, new Vector2(0, 0), 0),
            new(new Vector3(-_size, _size, -_size), -Vector3.forward, new Vector2(0, 1), 0),

            //back
            new(new Vector3(_size, _size, _size), Vector3.forward, new Vector2(1, 1), 0),
            new(new Vector3(_size, -_size, _size), Vector3.forward, new Vector2(1, 0), 0),
            new(new Vector3(-_size, -_size, _size), Vector3.forward, new Vector2(0, 0), 0),
            new(new Vector3(-_size, _size, _size), Vector3.forward, new Vector2(0, 1), 0),

            // left
            new(new Vector3(-_size, _size, -_size), -Vector3.right, new Vector2(0, 1), 1),
            new(new Vector3(-_size, -_size, -_size), -Vector3.right, new Vector2(0, 0), 1),
            new(new Vector3(-_size, -_size,  _size), -Vector3.right, new Vector2(1, 0), 1),
            new(new Vector3(-_size,  _size,  _size), -Vector3.right, new Vector2(1, 1), 1),

            // right
            new(new Vector3(_size, _size, -_size), Vector3.right, new Vector2(0, 1), 1),
            new(new Vector3(_size, -_size, -_size), Vector3.right, new Vector2(0, 0), 1),
            new(new Vector3(_size, -_size,  _size), Vector3.right, new Vector2(1, 0), 1),
            new(new Vector3(_size,  _size,  _size), Vector3.right, new Vector2(1, 1), 1),

            //up
            new(new Vector3(_size, _size, _size), Vector3.up, new Vector2(1, 1), 2),
            new(new Vector3(_size, _size, -_size), Vector3.up, new Vector2(1, 0), 2),
            new(new Vector3(-_size, _size, -_size), Vector3.up, new Vector2(0, 0), 2),
            new(new Vector3(-_size, _size, _size), Vector3.up, new Vector2(0, 1), 2),

            //down
            new(new Vector3(_size, -_size, _size), -Vector3.up, new Vector2(1, 1), 2),
            new(new Vector3(_size, -_size, -_size), -Vector3.up, new Vector2(1, 0), 2),
            new(new Vector3(-_size, -_size, -_size), -Vector3.up, new Vector2(0, 0), 2),
            new(new Vector3(-_size, -_size, _size), -Vector3.up, new Vector2(0, 1), 2),
        }, 0, 0, vertexCount);
    }

    private void SetIndexBuffer(Mesh mesh, int indicesCount)
    {
        mesh.SetIndexBufferData(new List<int>()
        {
            0, 1, 3,
            1, 2, 3,

            4, 7, 5,
            5, 7, 6,

            8, 9, 11,
            9, 10, 11,

            12, 15, 13,
            13, 15, 14,

            16, 17, 19,
            17, 18, 19,

            20, 23, 21,
            21, 23, 22
        }, 0, 0, indicesCount);
    }

    private void SetSubMesh(Mesh mesh, int indicesCount)
    {
        mesh.subMeshCount = 1;
        mesh.SetSubMesh(0, new SubMeshDescriptor(0, indicesCount));
        mesh.bounds = new Bounds(Vector3.zero, new Vector3(_size, _size, _size) * 2);
    }
}