using UnityEngine;

namespace Code.Utils.ShaderUtils
{
    public class MaterialBridge : IShaderBridge<int>, IShaderBridge<string>
    {
        private readonly Material _material;

        public MaterialBridge(Material material) => _material = material;

        public void SetInt(int key, int value) => _material.SetInt(key, value);
        public void SetInt(string key, int value) => _material.SetInt(key, value);
        public void SetFloat(int key, float value) => _material.SetFloat(key, value);
        public void SetFloat(string key, float value) => _material.SetFloat(key, value);
        public void SetVector(int key, Vector4 value) => _material.SetVector(key, value);
        public void SetVector(string key, Vector4 value) => _material.SetVector(key, value);
        public void SetBuffer(int kernelId, int key, ComputeBuffer value) => _material.SetBuffer(key, value);
        public void SetBuffer(int kernelId, string key, ComputeBuffer value) => _material.SetBuffer(key, value);
    }
}