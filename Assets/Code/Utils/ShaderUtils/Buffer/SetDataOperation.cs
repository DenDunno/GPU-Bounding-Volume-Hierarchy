using UnityEngine;

namespace Code.Utils.ShaderUtils.Buffer
{
    public abstract class SetDataOperation<TCollection>
    {
        protected readonly TCollection Collection;

        protected SetDataOperation(TCollection collection)
        {
            Collection = collection;
        }

        public abstract void Execute(ComputeBuffer buffer);
    }
}