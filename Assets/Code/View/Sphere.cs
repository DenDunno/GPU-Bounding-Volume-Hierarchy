using Code.RenderFeature.Data;
using UnityEngine;

namespace Code.View
{
    [ExecuteAlways]
    public class Sphere : MonoBehaviour
    {
        [SerializeField] private float _radius;
        private SpheresManager _manager;
        private int _index;
        
        public SphereData Data => new(transform.position, _radius);
    
        public void InjectOnChangedCallback(SpheresManager manager, int index)
        {
            _manager = manager;
            _index = index;
        }
    
        private void OnValidate()
        {
        }
    }
}