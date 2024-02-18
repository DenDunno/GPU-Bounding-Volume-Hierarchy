using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration
{
    public class CubePointGeneration : IPointGeneration
    {
        private readonly Vector3Int _dimensions;
        private readonly float _distance;
        private int _index;
        
        public CubePointGeneration(Vector3Int dimensions, float distance)
        {
            _dimensions = dimensions;
            _distance = distance;
        }
        
        public Vector3 Evaluate()
        {
            int z = _index % _dimensions.z;
            int y = _index / _dimensions.z % _dimensions.y;
            int x = _index / (_dimensions.y * _dimensions.z);
            
            _index++;
            
            return new Vector3(x, y, z) * _distance;
        }
    }
}