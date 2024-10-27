using Code.Utils.ShaderUtils;
using MyFolder.ComputeShaderNM;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class SetupStage : ComputeShaderPass 
    {
        private readonly float _sceneBoxSize = 10000;
        private readonly ComputeBuffer _boundingBoxes;
        private readonly ComputeBuffer _mortonCodes;
        private readonly ComputeBuffer _nodes;

        public SetupStage(ComputeShader shader, ComputeBuffer boundingBoxes, 
            ComputeBuffer nodes, ComputeBuffer mortonCodes) : base(shader, "Setup")
        {
            _boundingBoxes = boundingBoxes;
            _mortonCodes = mortonCodes;
            _nodes = nodes;
        }

        protected override void Setup(Kernel kernel, IShaderBridge<string> shaderBridge)
        {
            shaderBridge.SetVector("_Min", -Vector4.one * _sceneBoxSize);
            shaderBridge.SetVector("_Max",  Vector4.one * _sceneBoxSize);
            shaderBridge.SetBuffer(kernel.ID, "BoundingBoxes", _boundingBoxes);
            shaderBridge.SetBuffer(kernel.ID, "MortonCodes", _mortonCodes);
            shaderBridge.SetBuffer(kernel.ID, "Nodes", _nodes);
        }
    }
}