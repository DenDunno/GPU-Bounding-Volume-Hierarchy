using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class DashedCube : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    
    private void OnValidate()
    {
        _meshRenderer ??= GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        _meshRenderer.material.SetVector("_Size", transform.lossyScale);
    }
}