using UnityEngine;

namespace Code.Utils.ShaderUtils.Buffer
{
    public interface ISetDataOperation
    {
        void Execute(ComputeBuffer buffer);
    }
}