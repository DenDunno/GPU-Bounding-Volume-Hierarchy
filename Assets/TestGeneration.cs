using Sirenix.OdinInspector;
using TerraformingTerrain2d;
using UnityEngine;

public class TestGeneration : MonoBehaviour
{
    [Button]
    public void Generate()
    {
        GetComponent<MeshFilter>().mesh = MeshExtensions.BuildUniformQuad(1f, 1f);
    }
}