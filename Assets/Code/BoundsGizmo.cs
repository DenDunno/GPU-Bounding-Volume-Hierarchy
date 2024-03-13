using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BoundsGizmo : MonoBehaviour
{
    [SerializeField] [HideInInspector] private Renderer _renderer;

    private void OnDrawGizmos()
    {
        _renderer = GetComponent<Renderer>();
        Bounds bounds = _renderer.bounds;
        
        Gizmos.DrawWireCube(bounds.center, bounds.max - bounds.min);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(bounds.min, 0.1f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bounds.max, 0.1f);
    }
}