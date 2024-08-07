using UnityEngine;

namespace Code.Utils.ShaderUtils.Buffer
{
    public class SetArrayOperation<TElement> : SetDataOperation<TElement[]> where TElement : struct
    {
        public SetArrayOperation(TElement[] collection) : base(collection)
        {
        }

        public override void Execute(ComputeBuffer buffer)
        {
            buffer.SetData(Collection);
        }
    }
}