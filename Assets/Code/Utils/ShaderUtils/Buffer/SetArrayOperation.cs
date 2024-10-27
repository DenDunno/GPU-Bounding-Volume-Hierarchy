using UnityEngine;

namespace Code.Utils.ShaderUtils.Buffer
{
    public readonly struct SetArrayOperation<TElement> : ISetDataOperation where TElement : struct
    {
        private readonly TElement[] _collection;

        public SetArrayOperation(TElement[] collection)
        {
            _collection = collection;
        }

        public void Execute(ComputeBuffer buffer)
        {
            buffer.SetData(_collection);
        }
    }
}