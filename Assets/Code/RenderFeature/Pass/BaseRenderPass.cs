using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Code.RenderFeature.Pass
{
    public abstract class BaseRenderPass : ScriptableRenderPass
    {
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ResetTarget();
        }
        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (IsNotPreviewCamera(in renderingData))
            {
                CommandBuffer commandBuffer = CommandBufferPool.Get();
            
                using (new ProfilingScope(commandBuffer, profilingSampler))
                {
                    Draw(in renderingData, commandBuffer);
                }
 
                context.ExecuteCommandBuffer(commandBuffer);
                commandBuffer.Clear();
                CommandBufferPool.Release(commandBuffer);
            }
        }
        
        private bool IsNotPreviewCamera(in RenderingData renderingData)
        {
            return renderingData.cameraData.isPreviewCamera == false;
        }

        protected abstract void Draw(in RenderingData renderingData, CommandBuffer commandBuffer);
    }
}