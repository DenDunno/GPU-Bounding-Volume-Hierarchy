using Code;
using EditorWrapper;
using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    public class SphereVisualization : IDrawable
    {
        private readonly SphereGenerationData _data;

        public SphereVisualization(SphereGenerationData data)
        {
            _data = data;
        }

        public void Draw()
        {
            GizmosUtils.SetColor(Color.red);
            Gizmos.DrawWireSphere(_data.Origin, _data.Radius);
            GizmosUtils.RestoreColor();
        }
    }
}