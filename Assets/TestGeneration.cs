using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TestGeneration : MonoBehaviour
{
    [Button]
    public void Generate()
    {
        GetComponent<MeshFilter>().mesh = new CubeMeshGeneration(1f).Build();
    }
}