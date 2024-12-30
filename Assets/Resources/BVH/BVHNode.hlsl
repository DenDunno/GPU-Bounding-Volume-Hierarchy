#include "..//Utilities/AABB.hlsl"
#include "..//Utilities/BitManipulation.hlsl"

// 32 bytes layout
// __Data.x [1b - isInner] [31b - isInner ? LefChildIndex : TriangleIndex]
// __Data.y [1b - isInner] [31b - isInner ? RightChildIndex : TriangleCount]
#ifndef BVH_NODE_HLSL
#define BVH_NODE_HLSL
struct BVHNode
{
    uint2 __Data;
    AABB Box;

    static BVHNode Create(const uint leftChild, const uint rightChild, const AABB box)
    {
        BVHNode node;
        node.Box = box;
        node.__Data = uint2(leftChild, rightChild);

        return node;
    }

    bool IsLeaf() { return BitUtils::ExtractTopBit(__Data.x); }
    uint LeftChild() { return BitUtils::ExtractLower31Bits(__Data.x); }
    uint RightChild() { return BitUtils::ExtractLower31Bits(__Data.y); }
    uint TriangleIndex() { return LeftChild(); }
    uint TriangleCount() { return RightChild(); }

    void MarkAsLeaf() { __SetIsLeaf(1); }
    void MarkAsInternalNode() { __SetIsLeaf(0); }
    void __SetIsLeaf(const uint value) { __Data.x = BitUtils::SetTopBit(__Data.x, value); }
    void SetLeftChild(const uint value) { __Data.x = value; }
    void SetRightChild(const uint value) { __Data.y = value; }
};
#endif