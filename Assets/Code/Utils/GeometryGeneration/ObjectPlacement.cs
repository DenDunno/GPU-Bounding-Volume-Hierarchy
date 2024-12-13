using System;
using System.Collections.Generic;
using Code;
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
        IPointGeneration algorithm = _data.CreateGenerationAlgorithm();
        List<GameObject> objects = Generate(algorithm, _data.Prefab, _data.Count);
        NotifyListeners(objects);
    }

    private void DestroyChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private List<GameObject> Generate(IPointGeneration pointGeneration, GameObject prefab, int count)
    {
        List<GameObject> objects = new();
        RandomUtils.InitState(Environment.TickCount);
        
        for (int i = 0; i < count; ++i)
        {
            GameObject instance = Instantiate(prefab, transform, true);
            instance.transform.position = pointGeneration.Evaluate();
            instance.name = $"Index = {i}";
            instance.SetActive(true);
            objects.Add(instance);
        }

        RandomUtils.RestoreState();
        
        return objects;
    }

    private void NotifyListeners(List<GameObject> objects)
    {
        IObjectPlacementListener[] listeners = GetComponents<IObjectPlacementListener>();
        
        foreach (IObjectPlacementListener listener in listeners)
        {
            listener.Accept(objects);
        }
    }
}