using Code.Data;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class InputFromMesh : IBoundingBoxesInput
    {
        private readonly Transform _transform;
        private readonly Mesh _mesh;

        public InputFromMesh(Mesh mesh, Transform transform)
        {
            _transform = transform;
            _mesh = mesh;
        }

        public AABB[] Calculate()
        {
            AABB[] output = new AABB[_mesh.triangles.Length / 3];
            Vector3[] vertices = _mesh.vertices;
            int[] triangles = _mesh.triangles;

            for (int i = 0; i < output.Length; ++i)
            {
                Triangle triangle = new(
                    vertices[triangles[i * 3 + 0]],
                    vertices[triangles[i * 3 + 1]],
                    vertices[triangles[i * 3 + 2]]);
                
                output[i] = (_transform.localToWorldMatrix * triangle).CalculateBox();
            }

            return output;
        }
    }
}