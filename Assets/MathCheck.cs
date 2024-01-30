using System;
using UnityEngine;

[ExecuteInEditMode]
public class MathCheck : MonoBehaviour
{
    [SerializeField] private GameObject _sphere;
    [SerializeField] private GameObject _plane;

    private void OnDrawGizmos()
    {
        if (_sphere == null || _plane == null)
            return;
        
        Vector3 planeNormal = _plane.transform.up;
        Vector3 difference = _plane.transform.position - _sphere.transform.position;
        float distance = Vector3.Dot(difference, planeNormal);

        Vector3 endPoint = _sphere.transform.position + planeNormal * distance;
        
        Gizmos.color = distance - 0.5f <= 0 ? Color.green : Color.red;
        Gizmos.DrawLine(_sphere.transform.position, endPoint);
        Gizmos.DrawSphere(endPoint, 0.1f);
    }
}