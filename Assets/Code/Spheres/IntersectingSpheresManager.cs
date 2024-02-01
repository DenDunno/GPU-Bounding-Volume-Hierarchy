using System.Collections.Generic;
using UnityEngine;

namespace Code.RenderFeature
{
    [ExecuteInEditMode]
    public class IntersectingSpheresManager : MonoBehaviour
    {
        [SerializeField] private List<IntersectingSphere> _spheresView;

        private void OnValidate()
        {
            if (_spheresView != null)
            {
                foreach (IntersectingSphere sphere in _spheresView)
                {
                    sphere?.InjectOnChangedCallback(this);
                }
            }
        }

        public void Add(IntersectingSphere sphere)
        {
            _spheresView.Add(sphere);
        }
        
        public void Remove(IntersectingSphere sphere)
        {
            _spheresView.Remove(sphere);
        }

        public void UpdateBuffer()
        {
        }
    }
}