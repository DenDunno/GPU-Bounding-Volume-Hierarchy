using UnityEditor;
using UnityEngine;

public class DissolveDirection : MonoBehaviour
{
    [SerializeField] private float _length;
    [SerializeField] private float _thickness = 3;
    [SerializeField] private Color _color = Color.red;
    [SerializeField] private float _sphereRadius = 0.05f;
    public Vector3 Direction => transform.up;

    private void OnDrawGizmos()
    {
        Vector3 start = transform.TransformPoint(Direction * _length / 2);
        Vector3 end = transform.TransformPoint(-Direction * _length / 2);

        Handles.DrawBezier(start, end, start, end, _color, null, _thickness);
        
        Color oldColor = Gizmos.color;
        Gizmos.color = _color;
        Gizmos.DrawSphere(end, _sphereRadius);
        Gizmos.color = oldColor;
    }
}