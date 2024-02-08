using UnityEngine.Rendering;

namespace Code.RenderFeature.Data
{
    public struct BlitArguments
    {
        public readonly CommandBuffer CommandBuffer;
        public readonly RTHandle Destination;
        public readonly RTHandle Source;

        public BlitArguments(CommandBuffer commandBuffer, RTHandle source, RTHandle destination)
        {
            CommandBuffer = commandBuffer;
            Destination = destination;
            Source = source;
        }
    }
}