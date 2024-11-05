using Code.Data;
using UnityEngine;

public class Sphere : MonoBehaviour, IAABBProvider
{
    [SerializeField] private float _radius = 0.5f;

    private void OnValidate()
    {
        transform.localScale = _radius * Vector3.one * 2f;
    }

    public AABB CalculateBox()
    {
        return new AABB(
            transform.position - Vector3.one * _radius,
            transform.position + Vector3.one * _radius);
    }
}