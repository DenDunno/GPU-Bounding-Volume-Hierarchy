using System.Collections.Generic;
using Code.Data;
using Code.RenderFeature.Pass;
using UnityEngine.Rendering.Universal;

namespace Code.RenderFeature
{
    internal class IntersectingSpheresRendererFeature : ScriptableRendererFeature
    {
        //[SerializeField] private IntersectingSpheresPassData _data;
        private IntersectingSpheresPass _renderPass;
        private List<SphereData> _sphereData = new();

        public override void Create()
        {
            // _renderPass?.Dispose();
            // _renderPass = new IntersectingSpheresPass(_data);
            // _renderPass.ConfigureInput(ScriptableRenderPassInput.Color);
        }
 
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            // if (_data.IsValid && _sphereData?.Count != 0)    
            // {
            //     _renderPass.Setup(ref renderingData.cameraData, _sphereData);
            //     renderer.EnqueuePass(_renderPass);
            // }
        }
    
        protected override void Dispose(bool disposing)
        {
            //_renderPass.Dispose();
        }

        public void PassData(List<SphereData> data)
        {
            _sphereData = data;
        }
    }
}