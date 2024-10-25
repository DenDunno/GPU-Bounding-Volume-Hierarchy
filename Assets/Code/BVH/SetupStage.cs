using Code.Utils.ShaderUtils;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class SetupStage : ComputeShaderPass 
    {
        private readonly float _sceneBoxSize = 10000;
        private readonly ComputeBuffer _boundingBoxes;
        private readonly ComputeBuffer _mortonCodes;
        private readonly ComputeBuffer _nodes;

        public SetupStage(ComputeBuffer boundingBoxes, ComputeBuffer nodes,
            ComputeBuffer mortonCodes) : base("MortonCode/Setup", "Setup")
        {
            _boundingBoxes = boundingBoxes;
            _mortonCodes = mortonCodes;
            _nodes = nodes;
        }

        protected override void Setup(IShaderBridge<string> shaderBridge)
        {
            shaderBridge.SetVector("_Min", -Vector4.one * _sceneBoxSize);
            shaderBridge.SetVector("_Max",  Vector4.one * _sceneBoxSize);
            shaderBridge.SetBuffer(0, "BoundingBoxes", _boundingBoxes);
            shaderBridge.SetBuffer(0, "MortonCodes", _mortonCodes);
            shaderBridge.SetBuffer(0, "Nodes", _nodes);
        }
    }
}