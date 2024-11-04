using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Utils.ShaderUtils
{
    public class ComputeShaderBridge : IShaderBridge<int>, IShaderBridge<string>
    {
        private readonly ComputeShader _shader;

        public ComputeShaderBridge(ComputeShader shader) => _shader = shader;
        
        public void SetInt(int key, int value) => _shader.SetInt(key, value);
        public void SetInt(string key, int value) => _shader.SetInt(key, value);
        public void SetFloat(int key, float value) => _shader.SetFloat(key, value);
        public void SetFloat(string key, float value) => _shader.SetFloat(key, value);
        public void SetVector(int key, Vector4 value) => _shader.SetVector(key, value);
        public void SetVector(string key, Vector4 value) => _shader.SetVector(key, value);
        public void SetColor(int key, Color value) => SetVector(key, value.ToVector4());
        public void SetColor(string key, Color value) => SetVector(key, value.ToVector4());
        public void SetBuffer(int kernelId, int key, ComputeBuffer value) => _shader.SetBuffer(kernelId, key, value);
        public void SetBuffer(int kernelId, string key, ComputeBuffer value) => _shader.SetBuffer(kernelId, key, value);
    }
}