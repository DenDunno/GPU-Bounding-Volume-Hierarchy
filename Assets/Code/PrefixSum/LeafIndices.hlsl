#include "..//Common.hlsl"

struct LeafIndices
{
    uint Left;
    uint Right;

    static LeafIndices Create(uint threadId, uint step)
    {
        LeafIndices indices;
        indices.Right = THREAD_LAST_INDEX - threadId * (step * 2);
        indices.Left = indices.Right - step;
        return indices;
    }
};