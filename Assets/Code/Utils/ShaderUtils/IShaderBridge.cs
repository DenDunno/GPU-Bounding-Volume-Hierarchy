using UnityEngine;

namespace Code.Utils.ShaderUtils
{
    public interface IShaderBridge
    {
        void SetInt(int id, int value);
        void SetFloat(int id, float value);
        void SetVector(int id, Vector4 value);
        void SetBuffer(int id, ComputeBuffer value);
    }
}