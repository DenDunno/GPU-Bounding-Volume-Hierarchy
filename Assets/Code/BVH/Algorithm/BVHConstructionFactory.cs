using System.Collections.Generic;
using Code.Utils.Factory;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHConstructionFactory : IFactory<IBVHConstructionAlgorithm>
    {
        private readonly Dictionary<BVHConstructionId, IBVHConstructionAlgorithm> _algorithms;
        private readonly BVHConstructionId _targetAlgorithm;

        public BVHConstructionFactory(BVHBuffers buffers, BVHShaders shaders, BVHConstructionId target)
        {
            _targetAlgorithm = target;
            _algorithms = new Dictionary<BVHConstructionId, IBVHConstructionAlgorithm>()
            {
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