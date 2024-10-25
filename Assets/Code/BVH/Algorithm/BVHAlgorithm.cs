namespace Code.Components.MortonCodeAssignment
{
    public class BVHAlgorithm
    {
        private readonly GPURadixSort _mortonCodesSorting;
        private readonly SetupStage _setupStage;

        public BVHAlgorithm(BVHBuffers buffers)
        {
            _setupStage = new SetupStage(buffers.Boxes, buffers.Nodes, buffers.Nodes);
        }

        public void Initialize()
        {
            _setupStage.Initialize();
        }

        public void Execute(int count)
        {
            _setupStage.Dispatch(count);
        }
    }
}