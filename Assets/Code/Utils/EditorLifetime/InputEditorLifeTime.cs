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
            if (_data != null)
            {
                _data.OnValidate();

                if (_data.IsValid())
                {
                    base.Regenerate();   
                }
            }
        }
    }
}