using System.Collections.Generic;
using Code.Utils.Factory;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHConstructionFactory : IFactory<IBVHConstructionAlgorithm>
    {
        private readonly Dictionary<BVHConstructionId, IBVHConstructionAlgorithm> _algorithms;
        private readonly BVHConstructionId _targetAlgorithm;

        public BVHConstructionFactory(BVHBuffers buffers, BVHShaders shaders, BVHConstructionId target, bool isStupidSearch)
        {
            _targetAlgorithm = target;
            _algorithms = new Dictionary<BVHConstructionId, IBVHConstructionAlgorithm>()
            {
                [BVHConstructionId.CPU] = new CPUConstruction(buffers.Nodes, isStupidSearch),
                [BVHConstructionId.HPLOC] = new HPLOC(shaders.HPLOCShader, buffers),
                [BVHConstructionId.PLOCPlusPlus] = new PLOCPlusPLus(shaders.PlocPlusPLusShader, buffers),
            };
        }

        public IBVHConstructionAlgorithm Create()
        {
            return _algorithms[_targetAlgorithm];
        }
    }
}