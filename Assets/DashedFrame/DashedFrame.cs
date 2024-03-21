using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform), typeof(Image))]
public class DashedFrame : MonoBehaviour
{
    [SerializeField] [HideInInspector] private Image _image;
    [SerializeField] [Range(0, 1)] private float _radius = 0.5f;
    [SerializeField] [Range(0, 1)] private float _thickness = 0.2f;
    [SerializeField] [Range(0, 1)] private float _dashSize = 0.5f;
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] private int _frequency = 20;
    [SerializeField] private Color _color = Color.white;

    private void OnValidate()
    {
        _image ??= GetComponent<Image>();
        _image.material = new Material(Shader.Find("Unlit/RoundedBox"));

        _frequency = Math.Max(4, _frequency);
        _image.material.SetFloat("_Radius", _radius);
        _image.material.SetFloat("_Thickness", _thickness);
        _image.material.SetFloat("_RotationSpeed", _rotationSpeed);
        _image.material.SetFloat("_DashSize", _dashSize);
        _image.material.SetInt("_Frequency", _frequency);
        _image.material.SetColor("_Color", _color);
    }
    
    private void Update()
    {
        Vector2 size = ((RectTransform)transform).rect.size;
        _image.material.SetVector("_RectangleSize", size);
    }
}