using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    public class LinePointGeneration : IPointGeneration
    {
        private readonly LineGenerationData _data;
        private int _index;

        public LinePointGeneration(LineGenerationData data)
        {
            _data = data;
        }

        public Vector3 Evaluate()
        {
            return _data.Distance * _data.Direction * _index++;
        }
    }
}