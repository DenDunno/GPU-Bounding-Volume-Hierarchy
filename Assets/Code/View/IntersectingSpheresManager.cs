using System.Collections.Generic;
using System.Linq;
using Code.RenderFeature;
using Code.RenderFeature.Data;
using UnityEngine;

namespace Code.View
{
    [ExecuteInEditMode]
    public class IntersectingSpheresManager : MonoBehaviour
    {
        [SerializeField] private IntersectingSpheresRendererFeature _rendererFeature;
        [SerializeField] private List<IntersectingSphere> _spheresView;

        private void OnValidate()
        {
            if (_spheresView != null)
            {
                foreach (IntersectingSphere sphere in _spheresView)
                {
                    sphere.InjectOnChangedCallback(this);
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

        [ContextMenu("Pass")]
        public void UpdateBuffer()
        {
            List<SphereData> data = _spheresView.Select(sphere => sphere.Data).ToList();
            _rendererFeature.PassData(data);
        }
    }
}