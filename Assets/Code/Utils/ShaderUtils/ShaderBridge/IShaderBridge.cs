using UnityEngine;

namespace Code.Utils.ShaderUtils
{
    public interface IShaderBridge<in TKey>
    {
        void SetInt(TKey key, int value);
        void SetFloat(TKey key, float value);
        void SetVector(TKey key, Vector4 value);
        void SetColor(TKey key, Color value);
        void SetBuffer(int kernelId, TKey key, ComputeBuffer value);
    }
}