using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class DashedCube : MonoBehaviour
{
    [SerializeField, HideInInspector] private MeshRenderer _meshRenderer;
    private static readonly int _sizePropertyId = Shader.PropertyToID("_Size");
    private MaterialPropertyBlock _materialPropertyBlock;

    private bool IsInitialized => _meshRenderer;

    private void Update()
    {
        if (IsInitialized)
        {
            PassBoxSize();
        }
        else
        {
            Initialize();
        }
    }

    private void PassBoxSize()
    {
        _materialPropertyBlock ??= new MaterialPropertyBlock(); 
        _materialPropertyBlock.SetVector(_sizePropertyId, transform.lossyScale);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    private void Initialize()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = new IndexedCubeMeshGeneration(1f).Build();
        _meshRenderer.material = Resources.Load<Material>("DashedCube");
    }
}