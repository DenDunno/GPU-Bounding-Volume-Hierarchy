using Unity.Collections;
using UnityEngine;

namespace Code.Utils.ShaderUtils.Buffer
{
    public class SetNativeArrayOperation<TElement> : SetDataOperation<NativeArray<TElement>> where TElement : struct
    {
        public SetNativeArrayOperation(NativeArray<TElement> collection) : base(collection)
        {
        }

        public override void Execute(ComputeBuffer buffer)
        {
            buffer.SetData(Collection);
        }
    }
}