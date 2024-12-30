using System.Collections.Generic;
using Code.Utils.Factory;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHConstructionFactory : IFactory<IBVHConstructionAlgorithm>
    {
        private readonly Dictionary<BVHConstructionAlgorithmId, IBVHConstructionAlgorithm> _algorithms;
        private readonly BVHConstructionAlgorithmId _targetAlgorithm;

        public BVHConstructionFactory(BVHBuffers buffers, BVHShaders shaders, BVHConstructionAlgorithmId target)
        {
            _targetAlgorithm = target;
            _algorithms = new Dictionary<BVHConstructionAlgorithmId, IBVHConstructionAlgorithm>()
            {
                [BVHConstructionAlgorithmId.CPU] = new CPUConstruction(buffers.Nodes),
                [BVHConstructionAlgorithmId.HPLOC] = new HPLOC(shaders.HPLOCShader, buffers),
                [BVHConstructionAlgorithmId.PLOCPlusPlus] = new PLOCPlusPLus(shaders.PlocPlusPLusShader, buffers),
            };
        }

        public IBVHConstructionAlgorithm Create()
        {
            return _algorithms[_targetAlgorithm];
        }
    }
}