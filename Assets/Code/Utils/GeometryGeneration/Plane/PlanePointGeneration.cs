using UnityEngine;

namespace DefaultNamespace.Code.GeometryGeneration.Plane
{
    public class PlanePointGeneration : IPointGeneration
    {
        private readonly PlaneGenerationData _data;

        public PlanePointGeneration(PlaneGenerationData data)
        {
            _data = data;
        }

        public Vector3 Evaluate()
        {
            Vector3 arbitrary = Vector3.up;
            
            if (Mathf.Abs(Vector3.Dot(_data.Normal, arbitrary)) > 0.9f)
            {
                arbitrary = Vector3.right;
            }
                
            Vector3 tangent = Vector3.Cross(_data.Normal, arbitrary).normalized;
            Vector3 bitangent = Vector3.Cross(_data.Normal, tangent).normalized;
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float r = Mathf.Sqrt(Random.Range(0f, 1f)) * _data.Radius;
            return _data.Point + tangent * (r * Mathf.Cos(angle)) + bitangent * (r * Mathf.Sin(angle));
        }
    }
}