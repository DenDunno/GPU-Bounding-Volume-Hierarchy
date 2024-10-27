using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils.ShaderUtils.Buffer
{
    public readonly struct SetListOperation<TElement> : ISetDataOperation where TElement : struct
    {
        private readonly List<TElement> _collection;

        public SetListOperation(List<TElement> collection)
        {
            _collection = collection;
        }

        public void Execute(ComputeBuffer buffer)
        {
            buffer.SetData(_collection);
        }
    }
}