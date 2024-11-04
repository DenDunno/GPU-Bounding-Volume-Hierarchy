using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils.ShaderUtils
{
    public class CachedShaderBridge : IShaderBridge<string>
    {
        private readonly Dictionary<string, int> _cachedProperties = new();
        private readonly IShaderBridge<int> _bridge;

        public CachedShaderBridge(IShaderBridge<int> bridge)
        {
            _bridge = bridge;
        }

        public void SetInt(string name, int value) => _bridge.SetInt(GetId(name), value);
        public void SetFloat(string name, float value) => _bridge.SetFloat(GetId(name), value);
        public void SetVector(string name, Vector4 value) => _bridge.SetVector(GetId(name), value);
        public void SetColor(string key, Color value) => _bridge.SetColor(GetId(key), value);
        public void SetBuffer(int kernelId, string name, ComputeBuffer value) => 
            _bridge.SetBuffer(kernelId, GetId(name), value);

        private int GetId(string name)
        {
            if (_cachedProperties.TryGetValue(name, out int id) == false)
            {
                id = Shader.PropertyToID(name);
                _cachedProperties.Add(name, id);
            }
            
            return id;
        }
    }
}