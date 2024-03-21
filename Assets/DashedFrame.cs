using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform), typeof(Image))]
public class DashedFrame : MonoBehaviour
{
    [SerializeField] [HideInInspector] private Image _image;
    [SerializeField] [Range(0, 1)] private float _radius = 1f;
    [SerializeField] [Range(0, 1)] private float _thickness = 0.2f;
    [SerializeField] private Color _color = Color.white;
    private readonly string _shaderName = "Unlit/RoundedBox";
    private Vector2 _rectSize;

    private void OnValidate()
    {
        _image ??= GetComponent<Image>();

        if (_image.material.shader.name != _shaderName)
        {
            _image.material = new Material(Shader.Find(_shaderName));
        }
        
        _image.material.SetFloat("_Radius", Mathf.Max(_radius, 0.0001f));
        _image.material.SetFloat("_Thickness", _thickness);
        _image.material.SetColor("_Color", _color);
    }

    private void Update()
    {
        Rect rect = ((RectTransform)transform).rect;
        _image.material.SetVector("_RectangleSize", rect.size);
    }
}