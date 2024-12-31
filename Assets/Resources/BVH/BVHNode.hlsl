#include "..//Utilities/AABB.hlsl"

#ifndef BVH_NODE_HLSL
#define BVH_NODE_HLSL

struct BVHNode
{
    uint Left;
    uint Right;
    AABB Box;

    static BVHNode Create(const uint leftChild, const uint rightChild, const AABB box)
    {
        BVHNode node;
        node.Box = box;
        node.Left = leftChild;
        node.Right = rightChild;

        return node;
    }
};
#endif
