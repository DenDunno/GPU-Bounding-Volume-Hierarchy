using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public abstract class ComputeShaderPass
    {
        private readonly IShaderBridge<string> _shaderBridge;
        private readonly Kernel _kernel;

        protected ComputeShaderPass(ComputeShader shader, string kernelName)
        {
            _shaderBridge = new CachedShaderBridge(new ComputeShaderBridge(shader));
            _kernel = new Kernel(shader, kernelName);
        }

        public void Prepare()
        {
            Setup(_kernel, _shaderBridge);
        }

        public void Dispatch(int payloadX = 1, int payloadY = 1, int payloadZ = 1)
        {
            OnPreDispatch(_shaderBridge);
            _kernel.DispatchPayload(payloadX, payloadY, payloadZ);
        }

        protected virtual void Setup(Kernel kernel, IShaderBridge<string> shaderBridge) {}
        protected virtual void OnPreDispatch(IShaderBridge<string> shaderBridge) {}
    }
}