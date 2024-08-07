using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils.ShaderUtils.Buffer
{
    public class SetListOperation<TElement> : SetDataOperation<List<TElement>> where TElement : struct
    {
        public SetListOperation(List<TElement> collection) : base(collection)
        {
        }

        public override void Execute(ComputeBuffer buffer)
        {
            buffer.SetData(Collection);
        }
    }
}