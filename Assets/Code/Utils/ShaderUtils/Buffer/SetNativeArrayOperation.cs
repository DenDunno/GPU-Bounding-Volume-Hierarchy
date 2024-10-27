using Unity.Collections;
using UnityEngine;

namespace Code.Utils.ShaderUtils.Buffer
{
    public readonly struct SetNativeArrayOperation<TElement> : ISetDataOperation where TElement : struct
    {
        private readonly NativeArray<TElement> _collection;

        public SetNativeArrayOperation(NativeArray<TElement> collection)
        {
            _collection = collection;
        }

        public void Execute(ComputeBuffer buffer)
        {
            buffer.SetData(_collection);
        }
    }
}