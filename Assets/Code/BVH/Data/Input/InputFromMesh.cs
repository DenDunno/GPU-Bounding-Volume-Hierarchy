using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class InputFromMesh : IBoundingBoxesInput
    {
        private readonly Mesh _mesh;

        public InputFromMesh(Mesh mesh)
        {
            _mesh = mesh;
        }

        public int Count => _mesh.triangles.Length / 3;

        public AABB[] Calculate()
        {
            AABB[] output = new AABB[Count];
            Vector3[] vertices = _mesh.vertices;
            int[] triangles = _mesh.triangles;

            for (int i = 0; i < output.Length; ++i)
            {
                Triangle triangle = new(
                    vertices[triangles[i * 3 + 0]],
                    vertices[triangles[i * 3 + 1]],
                    vertices[triangles[i * 3 + 2]]);
                
                output[i] = triangle.CalculateBox();
            }

            return output;
        }
    }
}