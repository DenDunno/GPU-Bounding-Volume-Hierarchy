using Code.Data;
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
            BVHFacade facade = CreateBVHFacade();
            facade.Initialize();
            facade.Rebuild();
            TryBake(facade);
            facade.Dispose();
        }

        private BVHFacade CreateBVHFacade()
        {
            AABB bounds = new BoundsCalculator(_data.BoxesInput.Value).Compute();
            BVHFacade facade = new(_data.Algorithm, _data.BoxesInput.Value, BVHShaders.Load(), bounds);
            return facade;
        }

        private void TryBake(BVHFacade facade)
        {
            if (EditorSaveUtilities.TryGetFilePathFromSavePanel("BVH cluster", out string path))
            {
                BottomLevelAccelerationStructure asset = CreateBottomLevelStructure(facade, path);
                CreateTopLevelStructure(asset);
            }
        }

        private static BottomLevelAccelerationStructure CreateBottomLevelStructure(BVHFacade facade, string path)
        {
            BVHNode[] tree = facade.FetchTree();
            BottomLevelAccelerationStructure asset = ScriptableObject.CreateInstance<BottomLevelAccelerationStructure>();
            asset.Initialize(tree);
            EditorSaveUtilities.Save(path, asset);
            return asset;
        }

        private void CreateTopLevelStructure(BottomLevelAccelerationStructure cluster)
        {
            TopLevelAccelerationStructure topLevelStructure = gameObject.AddComponent<TopLevelAccelerationStructure>();
            topLevelStructure.Cluster = cluster;
            DestroyImmediate(this);
            TryAddTopLevelStructureToBVH(topLevelStructure);
        }

        private void TryAddTopLevelStructureToBVH(TopLevelAccelerationStructure topLevelStructure)
        {
            BoundingVolumeHierarchy bvh = FindFirstObjectByType<BoundingVolumeHierarchy>(FindObjectsInactive.Include);

            if (bvh)
            {
                bvh.Add(topLevelStructure);
            }
            else
            {
                Debug.LogWarning($"There is no \'BoundingVolumeHierarchy\' GameObject on the scene. " +
                                 $"You should add top level acceleration structure {gameObject.name} " +
                                 $"to the tree manually.");
            }
        }
    }
}