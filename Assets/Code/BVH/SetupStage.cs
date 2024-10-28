using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class SetupStage : ComputeShaderPass 
    {
        private readonly float _sceneBoxSize = 10000;
        private readonly BVHBuffers _buffers;

        public SetupStage(ComputeShader shader, BVHBuffers buffers) : base(shader, "Setup")
        {
            _buffers = buffers;
        }

        protected override void OnPreDispatch(IShaderBridge<string> shaderBridge, Vector3Int payload)
        {
            shaderBridge.SetInt("LeavesCount", payload.x);
        }

        protected override void Setup(Kernel kernel, IShaderBridge<string> shaderBridge)
        {
            shaderBridge.SetVector("_Min", -Vector4.one * _sceneBoxSize);
            shaderBridge.SetVector("_Max",  Vector4.one * _sceneBoxSize);
            shaderBridge.SetBuffer(kernel.ID, "MortonCodes", _buffers.MortonCodes);
            shaderBridge.SetBuffer(kernel.ID, "BoundingBoxes", _buffers.Boxes);
            shaderBridge.SetBuffer(kernel.ID, "ParentIds", _buffers.ParentIds);
            shaderBridge.SetBuffer(kernel.ID, "Nodes", _buffers.Nodes);
        }
    }
}