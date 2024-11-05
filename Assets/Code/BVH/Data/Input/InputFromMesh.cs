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

        public AABB[] Calculate()
        {
            AABB[] output = new AABB[_mesh.triangles.Length / 3];
            Vector3[] vertices = _mesh.vertices;
            int[] triangles = _mesh.triangles;

            for (int i = 0; i < output.Length; ++i)
            {
                output[i] = new Triangle(
                    vertices[triangles[i * 3 + 0]],
                    vertices[triangles[i * 3 + 1]],
                    vertices[triangles[i * 3 + 2]]).CalculateBox();
            }

            return output;
        }
    }
}