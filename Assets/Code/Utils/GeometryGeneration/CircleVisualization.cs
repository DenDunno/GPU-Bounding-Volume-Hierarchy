using EditorWrapper;
using TerraformingTerrain2d;
using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    public class CircleVisualization 
    {
        private readonly Material _material;
        private readonly Mesh _mesh;
        
        public CircleVisualization()
        {
            _material = Resources.Load<Material>("Materials/Circle");
            _mesh = MeshExtensions.BuildQuad(1f);
        }

        public void Draw(float radius, Vector3 point, Vector3 normal)
        {
            _material.SetPass(0);
            Graphics.DrawMeshNow(_mesh, 
                Matrix4x4.TRS(point, Quaternion.LookRotation(normal), radius * Vector3.one * 2f));
        }
    }
}