using Code.Utils.Factory;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BottomLevelAccelerationStructureFactory : IFactory<BottomLevelAccelerationStructure>
    {
        private readonly BVHNode[] _tree;

        public BottomLevelAccelerationStructureFactory(BVHNode[] tree)
        {
            _tree = tree;
        }

        public BottomLevelAccelerationStructure Create()
        {
            BottomLevelAccelerationStructure asset = ScriptableObject.CreateInstance<BottomLevelAccelerationStructure>();
            asset.Initialize(_tree);

            return asset;
        }
    }
}