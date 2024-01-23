using System;
using Code.RenderFeature;
using UnityEngine;

[ExecuteAlways]
public class IntersectingSphere : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private float _radius;
    [SerializeField] private Color _intersectionColor;
    [SerializeField] private float _intersectionPower;
    private Action _onChangedCallback;
    
    public SphereData Data => new(transform.position, _radius, _color, _intersectionColor, _intersectionPower);
    
    public void InjectOnChangedCallback(Action onChangedCallback)
    {
        _onChangedCallback = onChangedCallback;
    }
    
    private void OnValidate()
    {
        _onChangedCallback?.Invoke();
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            _onChangedCallback?.Invoke();
        }
    }
}