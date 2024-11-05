using Code.Components.MortonCodeAssignment;
using UnityEngine;

namespace Code
{
    public abstract class InputEditorLifeTime<TData> : InEditorLifetime where TData : IValidatedData
    {
        [SerializeField] private TData _data;

        public TData Data => _data;

        protected override void Regenerate()
        {
            if (_data != null && _data.IsValid())
            {
                _data.OnValidate();
                base.Regenerate();
            }
        }
    }
}