using UnityEngine;

namespace Code.Utils.ShaderUtils.Buffer
{
    public readonly struct SetVoidOperation : ISetDataOperation
    {
        public void Execute(ComputeBuffer buffer)
        {
        }
    }
}