using UnityEngine;

namespace Code.Utils.ShaderUtils
{
    public abstract class ComputeShaderWrapper
    {
        private readonly ComputeShader _shader;
        private readonly CachedShaderBridge _bridge;

        protected ComputeShaderWrapper(string name)
        {
            _shader = Resources.Load<ComputeShader>(name);
            _bridge = new CachedShaderBridge(new ComputeShaderBridge(_shader));
        }

        public void Initialize()
        {
            OnInitialize(_bridge);
        }

        public void Dispatch(int targetDispatch)
        {
            OnPreDispatch(_bridge);

            int dispatchX = Mathf.CeilToInt(targetDispatch / 8f);
            _shader.Dispatch(0, dispatchX, 1, 1);
        }

        protected virtual void OnInitialize(CachedShaderBridge shaderBridge) { }
        protected virtual void OnPreDispatch(CachedShaderBridge shaderBridge) { }
    }
}