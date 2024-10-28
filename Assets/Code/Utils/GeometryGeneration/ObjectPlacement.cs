using DefaultNamespace.Code.GeometryGeneration;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField] private Vector3Int _dimensions = Vector3Int.one;
    [SerializeField] private int _counts = 100;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private Vector3 _origin;
    [SerializeField] private GameObject _prefab;
    
    [Button] private void GenerateInSphere() => Generate(new PointInSphereGeneration(_origin, _radius));
    
    [Button] private void GenerateInCube() => Generate(new CubePointGeneration(_dimensions, _radius));

    private void Generate(IPointGeneration pointGeneration)
    {
        DestroyChildren();
        GenerateChildren(pointGeneration);
    }

    private void GenerateChildren(IPointGeneration pointGeneration)
    {
        for (int i = 0; i < _counts; ++i)
        {
            GameObject instance = Instantiate(_prefab, transform, true);
            instance.transform.position = pointGeneration.Evaluate();
            instance.name = $"Index = {i + 1}";
        }
    }

    private void DestroyChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}