using Code.Data;
using UnityEngine;

namespace Code.Core
{
    [ExecuteAlways]
    public class Sphere : MonoBehaviour
    {
        [SerializeField] private float _radius;
        private ISphereDataValidateCallback _validateCallback;
        private int _index;

        public SphereData Data => new(transform.position, _radius);

        public void InjectOnChangedCallback(ISphereDataValidateCallback validateCallback, int index)
        {
            _validateCallback = validateCallback;
            _index = index;
        }

        private void OnValidate()
        {
            transform.localScale = Vector3.one * _radius * 2;
            _validateCallback?.UpdateByIndex(_index);
        }
    }
}