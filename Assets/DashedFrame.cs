using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform), typeof(Image))]
public class DashedFrame : MonoBehaviour
{
    [SerializeField] [HideInInspector] private Image _image;
    [SerializeField] [Range(0, 1)] private float _radius = 1f;
    [SerializeField] [Range(0, 1)] private float _thickness = 0.2f;
    private Vector2 _rectSize;
    
    private void OnValidate()
    {
        _image ??= GetComponent<Image>();
        //_image.material = new Material(Shader.Find("Unlit/RoundedBox"));
        //_image.material.SetFloat("_Radius", Mathf.Max(_radius, 0.0001f));
        //_image.material.SetFloat("_Thickness", _thickness);
    }

    private void Update()
    {
        Rect rect = ((RectTransform)transform).rect;
        _image.material.SetVector("_RectangleSize", rect.size);
    }
}