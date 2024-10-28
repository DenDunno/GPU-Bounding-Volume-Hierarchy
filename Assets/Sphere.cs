using Code.Data;
using UnityEngine;

public class Sphere : MonoBehaviour, IAABBProvider
{
    [SerializeField] private float _radius = 0.5f;
    
    public AABB Provide()
    {
        return new AABB(
            transform.position - Vector3.one * _radius,
            transform.position + Vector3.one * _radius);
    }
}