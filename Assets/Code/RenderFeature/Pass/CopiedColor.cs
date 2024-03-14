using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Code.RenderFeature.Pass
{
    public class CopiedColor : IDisposable
    {
        private readonly string _passName;
        private RTHandle _rtHandle;

        public CopiedColor(string passName)
        {
            _passName = passName;
        }

        public RTHandle RTHandle => _rtHandle;

        public void ReAllocateIfNeeded(RenderTextureDescriptor textureDescriptor)
        {
            textureDescriptor.depthBufferBits = (int)DepthBits.None;
            RenderingUtils.ReAllocateIfNeeded(ref _rtHandle, textureDescriptor, name:_passName);
        }

        public void Dispose()
        {
            _rtHandle?.Release();
        }
    }
}