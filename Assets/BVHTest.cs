using System.Collections.Generic;
using Code.RenderFeature.Data;
using Code.Utils;
using DefaultNamespace;
using UnityEngine;

public class BVHTest : MonoBehaviour
{
    [SerializeField] [HideInInspector] private List<AABBNode> _sortedLeafs;
    [SerializeField] [HideInInspector] private List<AABB> _treeInternalNodes;
    private readonly LeafGenerator _leafGenerator = new();

    public void Rebuild(List<AABB> leafs)
    {
        _sortedLeafs = _leafGenerator.GetNodes(leafs, new AABB(Vector3.one * -125, Vector3.one * 125));
        _sortedLeafs.Sort((first, second) => first.MortonCode.CompareTo(second.MortonCode));
        
        _treeInternalNodes.Clear();
        BuildTree(leafs, _sortedLeafs, 0, leafs.Count - 1);
    }

    private AABB Merge(List<AABB> leafs, int start, int end)
    {
        AABB mergedBox = leafs[start];
        
        for (int i = start + 1; i <= end; ++i)
        {
            mergedBox = mergedBox.Union(leafs[i]);
        }

        return mergedBox;
    }

    private void BuildTree(List<AABB> leafs, List<AABBNode> sortedLeafs, int start, int end)
    {
        if (start == end)
            return;

        _treeInternalNodes.Add(Merge(leafs, start, end));
        
        int splitIndex = FindSplit(sortedLeafs, start, end);
        BuildTree(leafs, sortedLeafs, start, splitIndex);
        BuildTree(leafs, sortedLeafs, splitIndex + 1, end);
    }

    private int FindSplit(List<AABBNode> sortedLeafs, int start, int end)
    {
        uint firstCode = sortedLeafs[start].MortonCode;
        uint lastCode = sortedLeafs[end].MortonCode;

        if (firstCode == lastCode)
            return (start + end) >> 1;
        
        uint commonPrefix = NumberOfLeadingZerosLong(firstCode ^ lastCode);
        int split = start; 
        int step = end - start;

        do
        {
            step = (step + 1) >> 1; 
            int newSplit = split + step; 

            if (newSplit < end)
            {
                uint splitCode = sortedLeafs[newSplit].MortonCode;
                uint splitPrefix = NumberOfLeadingZerosLong(firstCode ^ splitCode);
                if (splitPrefix > commonPrefix)
                    split = newSplit; 
            }
        }
        while (step > 1);

        return split;
    }
    
    private static uint NumberOfLeadingZerosLong(ulong x)
    {
        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 16;
        x |= x >> 32;
        
        x -= x >> 1 & 0x5555555555555555;
        x = (x >> 2 & 0x3333333333333333) + (x & 0x3333333333333333);
        x = (x >> 4) + x & 0x0f0f0f0f0f0f0f0f;
        x += x >> 8;
        x += x >> 16;
        x += x >> 32;

        const int numLongBits = sizeof(long) * 8; // compile time constant
        return numLongBits - (uint)(x & 0x0000007f); // subtract # of 1s from 64
    }

    private void OnDrawGizmos()
    {
        Random.InitState(0);
        
        foreach (AABB internalNode in _treeInternalNodes)
        {
            Color randomColor = ColorExtensions.GetRandom();
            internalNode.Draw(randomColor);
        }
    }
}