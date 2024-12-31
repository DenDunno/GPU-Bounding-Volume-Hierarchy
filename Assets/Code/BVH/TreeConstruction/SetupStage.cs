using Code.Data;
using Code.Utils.ShaderUtils;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class SetupStage : ComputeShaderPass 
    {
        private readonly TreeConstructionBuffers _buffers;
        private readonly AABB _sceneSize;

        public SetupStage(ComputeShader shader, TreeConstructionBuffers buffers, AABB sceneSize) : base(shader, "Setup")
        {
            _sceneSize = sceneSize;
            _buffers = buffers;
        }

        protected override void Setup(IShaderBridge<string> shaderBridge, int kernelId)
        {
            shaderBridge.SetVector("_Min", _sceneSize.Min);
            shaderBridge.SetVector("_Max", _sceneSize.Max);
            shaderBridge.SetBuffer(kernelId, "MortonCodes", _buffers.MortonCodes);
            shaderBridge.SetBuffer(kernelId, "BoundingBoxes", _buffers.Boxes);
            shaderBridge.SetBuffer(kernelId, "ParentIds", _buffers.ParentIds);
            shaderBridge.SetBuffer(kernelId, "Nodes", _buffers.Nodes);
        }

        protected override void OnPreDispatch(IShaderBridge<string> shaderBridge, Vector3Int payload)
        {
            shaderBridge.SetInt("LeavesCount", payload.x);
        }
    }
}