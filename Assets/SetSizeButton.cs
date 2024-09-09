using System;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace DefaultNamespace
{
    public class SetSizeButton : MonoBehaviour
    {
        [SerializeField] private UpdateSortTest _test;
        [SerializeField] private InputField _inputField;
        
        private void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(SetSize);
        }

        private void SetSize()
        {
            int size = Convert.ToInt32(_inputField.text);
            _test.Setup(size);
        }
    }
}