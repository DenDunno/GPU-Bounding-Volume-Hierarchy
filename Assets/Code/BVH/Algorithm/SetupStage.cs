using Code.Data;
using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class SetupStage : ComputeShaderPass 
    {
        private readonly BVHBuffers _buffers;
        private readonly AABB _sceneSize;

        public SetupStage(ComputeShader shader, BVHBuffers buffers, AABB sceneSize) : base(shader, "Setup")
        {
            _sceneSize = sceneSize;
            _buffers = buffers;
        }

        protected override void Setup(Kernel kernel, IShaderBridge<string> shaderBridge)
        {
            shaderBridge.SetVector("_Min", _sceneSize.Min);
            shaderBridge.SetVector("_Max", _sceneSize.Max);
            shaderBridge.SetBuffer(kernel.ID, "BlockCounter", _buffers.BlockCounter);
            shaderBridge.SetBuffer(kernel.ID, "BlockOffset", _buffers.BlockOffset);
            shaderBridge.SetBuffer(kernel.ID, "MortonCodes", _buffers.MortonCodes);
            shaderBridge.SetBuffer(kernel.ID, "BoundingBoxes", _buffers.Boxes);
            shaderBridge.SetBuffer(kernel.ID, "ParentIds", _buffers.ParentIds);
            shaderBridge.SetBuffer(kernel.ID, "Nodes", _buffers.Nodes);
            shaderBridge.SetBuffer(kernel.ID, "Tree", _buffers.Tree);
        }

        protected override void OnPreDispatch(IShaderBridge<string> shaderBridge, Vector3Int payload)
        {
            shaderBridge.SetInt("LeavesCount", payload.x);
        }
    }
}