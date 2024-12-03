using DefaultNamespace.Code.GeometryGeneration;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField, Nested] private ObjectPlacementData _data;

    [Button]
    private void Generate()
    {
        DestroyChildren();
        GenerateChildren(_data.CreateGenerationAlgorithm(), _data.Prefab, _data.Count);
    }

    private void GenerateChildren(IPointGeneration pointGeneration, GameObject prefab, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject instance = Instantiate(prefab, transform, true);
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