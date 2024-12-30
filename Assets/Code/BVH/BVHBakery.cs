using Code.Editor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components.MortonCodeAssignment
{
    public class BVHBakery : MonoBehaviour
    {
        [SerializeField] private BVHData _data;

        [Button]
        public void Bake()
        {
            BVHFacade facade = new(_data, BVHShaders.Load());
            facade.Initialize();
            facade.Rebuild();
            CreateTreeConfig(facade);
            facade.Dispose();
        }

        private void CreateTreeConfig(BVHFacade facade)
        {
            BVHNode[] tree = facade.Components.GPUBridge.FetchTree();
            EditorSaveUtilities.Save("BVH cluster", new BLASFactory(tree));
        }
    }
}