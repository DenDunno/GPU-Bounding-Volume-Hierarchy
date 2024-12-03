using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    public class CubePointGeneration : IPointGeneration
    {
        private readonly CubeGenerationData _data;
        private int _index;

        public CubePointGeneration(CubeGenerationData data)
        {
            _data = data;
        }

        public Vector3 Evaluate()
        {
            int z = _index % _data.Dimensions.z;
            int y = _index / _data.Dimensions.z % _data.Dimensions.y;
            int x = _index / (_data.Dimensions.y * _data.Dimensions.z);
            
            _index++;
            
            return new Vector3(x, y, z) * _data.Distance;
        }
    }
}