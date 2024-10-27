namespace Code.Components.MortonCodeAssignment
{
    public class BVHAlgorithm
    {
        private readonly GPURadixSort _mortonCodesSorting;
        private readonly SetupStage _setupStage;
        private readonly BVHBuffers _buffers;

        public BVHAlgorithm(BVHBuffers buffers, ShadersPresenter shaders, int bufferSize)
        {
            _setupStage = new SetupStage(shaders.Setup, buffers.Boxes, buffers.Nodes, buffers.MortonCodes);
            _mortonCodesSorting = new GPURadixSort(shaders.Sorting, shaders.PrefixSum, bufferSize);
            _buffers = buffers;
        }

        public void Initialize()
        {
            _mortonCodesSorting.SetData(_buffers.MortonCodes);
            _setupStage.Prepare();
        }

        public void Execute(int count)
        {
            _setupStage.Dispatch(count);
            _mortonCodesSorting.Execute(count);
        }
    }
}