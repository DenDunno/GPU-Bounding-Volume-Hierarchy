using Code.Utils.ShaderUtils;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class MortonCodeAssignment : ComputeShaderWrapper
    {
        private readonly float _sceneBoxSize = 10000;
        private readonly ComputeBuffer _boundingBoxes;
        private readonly ComputeBuffer _nodes;

        public MortonCodeAssignment(ComputeBuffer boundingBoxes, ComputeBuffer nodes) : base("MortonCodeAssignment")
        {
            _boundingBoxes = boundingBoxes;
            _nodes = nodes;
        }

        protected override void OnPreDispatch(CachedShaderBridge shaderBridge)
        {
            shaderBridge.SetVector("_Min", -Vector4.one * _sceneBoxSize);
            shaderBridge.SetVector("_Max",  Vector4.one * _sceneBoxSize);
            shaderBridge.SetBuffer("_BoundingBoxes", _boundingBoxes);
            shaderBridge.SetBuffer("_Nodes", _nodes);
        }
    }
}