#include "..//AABB.hlsl"
#include "Range.hlsl"

struct BVHNode
{
    AABB Box; 
    Range Range;
    uint LeftChild;
    uint RightChild;
    
    static BVHNode Create(const AABB box, const uint threadId)
    {
        BVHNode node;
        node.Box = box;
        node.Range = Range::Create(threadId);
        node.LeftChild = 0;
        node.RightChild = 0;
        
        return node;
    }
};