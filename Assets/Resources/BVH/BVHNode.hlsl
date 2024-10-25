#include "..//AABB.hlsl"
#include "Range.hlsl"

struct BVHNode
{
    AABB Box; 
    Range Range;

    static BVHNode Create(const AABB box, const uint threadId)
    {
        BVHNode node;
        node.Box = box;
        node.Range = Range::Create(threadId);

        return node;
    }
};
