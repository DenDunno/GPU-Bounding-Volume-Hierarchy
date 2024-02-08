using Code.RenderFeature.Data;
using UnityEngine;

namespace Code.View
{
    [ExecuteAlways]
    public class IntersectingSphere : MonoBehaviour
    {
        public Color Color = Color.white;
        public float Radius = 1;
        public Color IntersectionColor = Color.green;
        public float IntersectionPower = 7;
        public float FresnelPower = 1; 
        private IntersectingSpheresManager _manager;
    
        public SphereData Data => new(transform.position, Radius, Color, IntersectionColor, IntersectionPower, FresnelPower);
    
        public void InjectOnChangedCallback(IntersectingSpheresManager manager)
        {
            _manager = manager;
        }
    
        private void OnValidate()
        {
            _manager?.UpdateBuffer();
        }

        public void SubmitChanges()
        {
            _manager?.UpdateBuffer();
        }

        private void OnDestroy()
        {
            _manager?.Remove(this);
        }
    }
}