using Code;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSizeButton : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    [SerializeField] private UpdateTest _test;
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        _test.Setup(int.Parse(_inputField.text));
    }
}