using System;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    [Serializable]
    public abstract class DebugElement
    {
        [SerializeField] private bool _show;

        public void TryDraw()
        {
            if (_show)
            {
                OnDraw();
            }
        }

        protected abstract void OnDraw();
    }
}