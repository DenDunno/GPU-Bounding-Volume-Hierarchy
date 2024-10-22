#include "AABB.hlsl"

// 32 bytes layout
struct BVHNode
{
    AABB Box; 
    uint Index; 
    uint TrianglesCount; 

    bool IsLeaf() { return TrianglesCount > 0; }
    uint TriangleIndex() { return Index; }
    uint LeftNode() { return Index; }
    uint RightNode() { return Index + 1; }

    static BVHNode Create(const AABB box)
    {
        BVHNode node;
        node.Box = box;
        node.Index = 0;
        node.TrianglesCount = 0;

        return node;
    }
};
