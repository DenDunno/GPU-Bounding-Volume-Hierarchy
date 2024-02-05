using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Code.RenderFeature
{
    internal class IntersectingSpheresRendererFeature : ScriptableRendererFeature
    {
        [SerializeField] private List<SphereData> _sphereData;
        [SerializeField] private Material _passMaterial;
        [SerializeField] private bool _debug;
        [SerializeField] private Material _debugMaterial;
        [SerializeField] private InjectionPoint _injectionPoint = InjectionPoint.AfterRenderingPostProcessing;
        [SerializeField] private int _maxSpheres = 500;
        [SerializeField] private int _tileSizeX = 10;
        [SerializeField] private int _tileSizeY = 10;
        [SerializeField] private ComputeShader _cullingShader;
        private IntersectingSpheresBlitPass _renderPass;
 
        public override void Create()
        {
            _renderPass?.Dispose();
            _renderPass = new IntersectingSpheresBlitPass
            {
                renderPassEvent = (RenderPassEvent)_injectionPoint
            };

            _renderPass.ConfigureInput(ScriptableRenderPassInput.Color);
        }
 
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (_passMaterial != null && _cullingShader != null)    
            {
                _renderPass.Dispose();
                _renderPass.Setup(_passMaterial, ref renderingData, _tileSizeX, _tileSizeY, _maxSpheres, _cullingShader, _debugMaterial, _sphereData, _debug);
                renderer.EnqueuePass(_renderPass);
            }
        }
    
        protected override void Dispose(bool disposing)
        {
            _renderPass.Dispose();
        }

        public void PassData(List<SphereData> data)
        {
            _sphereData.Clear();

            foreach (SphereData sphereData in data)
            {
                _sphereData.Add(sphereData);
            }
        }
    }
}