#include "AABB.hlsl"

struct AABBNode
{
    AABB box;
    int mortonCode;

    static AABBNode Create(const AABB box, const int code)
    {
        AABBNode node;
        node.box = box;
        node.mortonCode = code;

        return node;
    }
};
