using System;
using System.Collections.Generic;
using System.Linq;
using Code.RenderFeature.Data;
using DefaultNamespace.Code.GeometryGeneration;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteAlways]
public class CubeGenerator : MonoBehaviour
{
    [SerializeField] private Vector3Int _dimensions = Vector3Int.one;
    [SerializeField] private int _counts = 100;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private Vector3 _origin;
    
    [Button] private void GenerateInSphere() => Generate(new PointInSphereGeneration(_origin, _radius));
    
    [Button] private void GenerateInCube() => Generate(new CubePointGeneration(_dimensions, _radius));

    private void Generate(IPointGeneration pointGeneration)
    {
        DestroyChildren();
        GenerateChildren(pointGeneration);
    }

    private void Update()
    {
        List<AABB> boxes = GetBounds();
        GetComponent<BVHTest>().Rebuild(boxes);
    }

    private List<AABB> GetBounds()
    {
        List<AABB> boxes = GetComponentsInChildren<MeshRenderer>()
            .Select(meshRenderer => new AABB(meshRenderer.bounds))
            .ToList();
        return boxes;
    }

    private void GenerateChildren(IPointGeneration pointGeneration)
    {
        for (int i = 0; i < _counts; ++i)
        {
            Vector3 point = pointGeneration.Evaluate();
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);

            child.name = $"Object {i}";
            child.transform.parent = transform;
            child.transform.position = point;
            DestroyImmediate(child.GetComponent<BoxCollider>());
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