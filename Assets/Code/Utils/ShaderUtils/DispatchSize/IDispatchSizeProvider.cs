using Code.Utils.Provider;
using UnityEngine;

namespace Code.Utils.ShaderUtils.DispatchSize
{
    public interface IDispatchSizeProvider : IProvider<Vector3Int>
    {
        void Reset() {}
    }
}