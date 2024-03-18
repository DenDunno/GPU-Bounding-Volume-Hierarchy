using UnityEngine;

namespace Code.Utils.ShaderUtils
{
    public class MaterialBridge : IShaderBridge
    {
        private readonly Material _material;

        public MaterialBridge(Material material) => _material = material;

        public void SetInt(int id, int value) => _material.SetInt(id, value);
        public void SetFloat(int id, float value) => _material.SetFloat(id, value);
        public void SetVector(int id, Vector4 value) => _material.SetVector(id, value);
        public void SetBuffer(int id, ComputeBuffer value) => _material.SetBuffer(id, value);
    }
}