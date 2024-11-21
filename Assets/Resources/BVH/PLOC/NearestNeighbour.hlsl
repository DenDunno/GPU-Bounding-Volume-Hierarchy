#include "..//..//Utilities/BitManipulation.hlsl"
#include "..//NodesInput.hlsl"

#ifndef THREADS
#define THREADS 256
#endif
#define UINT_MAX_VALUE -1
#define RADIUS_SHIFT 0
#define RADIUS (1 << RADIUS_SHIFT)
#define PLOC_RANGE_SIZE (THREADS + 4 * RADIUS)
#define ENCODE_MASK ~(1 << (RADIUS_SHIFT + 1) - 1)
groupshared uint Neighbours[THREADS + 4 * RADIUS];
//groupshared AABB Boxes[THREADS + 4 * RADIUS];

// To store range from [-8, 8] we need 4 bits.
// 3 bits for the (value - 1). Last bit for the sign.
// Can't store 0 with this layout but for offset purpose it is fine
// decode(ABCD) = (toDecimal(ABC) + 1) * sign(D)
// decode(1110) = (7 + 1) *  1 =  8
// decode(1111) = (7 + 1) * -1 = -8
// decode(0000) = (0 + 1) *  1 =  1
// decode(0001) = (0 + 1) * -1 = -1
uint EncodeOffsetIntoLowerBits(const uint id, const uint neighbor)
{
    const int signedOffset = neighbor - id;
    const uint signLastLowerBit = signedOffset >> 31;
    const uint valueUpperBits = (abs(signedOffset) - 1) << 1;
    return valueUpperBits | signLastLowerBit;
}

int DecodeOffsetFromLowerBits(const uint encodedValue)
{
    int offset = ExtractBits(encodedValue, 1, RADIUS_SHIFT) + 1;
    uint sign = ExtractLowestBit(encodedValue);

    return sign ? -offset : offset;
}

void InitializeNeighbours(int threadId, int blockOffset)
{
    for (int id = threadId; id < PLOC_RANGE_SIZE; id += THREADS)
    {
        int globalId = id - 2 * RADIUS + blockOffset;
        Neighbours[id] = UINT_MAX_VALUE;

        // if (IsInBounds(globalId))
        // {
        //     Boxes[id] = Nodes[ComputeLeafIndex(globalId)].Box;
        // }
        // else
        // {
        //     Boxes[id] = AABB::CreateMaxBox();
        // }
    }

    GroupMemoryBarrierWithGroupSync();
}

void UpdateNeighbourFromTheLeft(uint selfId, uint i, uint distanceUpperBits)
{
    uint neighbourEncodedDistance = distanceUpperBits | EncodeOffsetIntoLowerBits(selfId + i, selfId);
    InterlockedMin(Neighbours[selfId + i], neighbourEncodedDistance);
}

void UpdateSelfBasedOnRightNeighbours(uint id, uint minDistanceIndex)
{
    InterlockedMin(Neighbours[id], minDistanceIndex);
}

uint GetDistanceToNeighbourUpperBits(uint neighbourId, AABB box, uint encodeMask)
{
    float distance = box.Union(Nodes[neighbourId].Box).ComputeSurfaceArea();
    uint positiveDistanceInteger = asuint(distance) << 1;
    return positiveDistanceInteger & encodeMask;
}

void UpdateMinDistanceIndex(uint id, uint i, uint distanceUpperBits, inout uint minDistanceIndex)
{
    uint selfEncodedDistance = distanceUpperBits | EncodeOffsetIntoLowerBits(id, id + i);
    minDistanceIndex = min(minDistanceIndex, selfEncodedDistance);
}

void RunSearch(uint threadId, int blockOffset)
{
    uint minDistanceIndex = UINT_MAX_VALUE;
    uint encodeMask = ENCODE_MASK;

    for (uint id = threadId; id < THREADS + 3 * RADIUS; id += THREADS)
    {
        AABB box = Nodes[id + blockOffset].Box;

        [unroll(RADIUS)]
        for (uint i = 1; i <= RADIUS; i++)
        {
            if (IsInBounds(id))
            {
                uint distanceUpperBits = GetDistanceToNeighbourUpperBits(id + blockOffset, box, encodeMask);
                UpdateMinDistanceIndex(distanceUpperBits, id, i, minDistanceIndex);
                UpdateNeighbourFromTheLeft(id, i, distanceUpperBits);
            }
        }

        UpdateSelfBasedOnRightNeighbours(threadId, minDistanceIndex);
    }
}

int RunStupidSearch(int globalId)
{
    int minDistanceIndex = INT_MAX;
    float minDistance = FLOAT_MAX;

    for (int globalNeighbourId = globalId - RADIUS; globalNeighbourId <= globalId + RADIUS; ++globalNeighbourId)
    {
        if (IsInBounds(globalNeighbourId) && globalNeighbourId != globalId)
        {
            float distance = Nodes[globalId].Box.Union(Nodes[globalNeighbourId].Box).ComputeSurfaceArea();

            if (distance < minDistance)
            {
                minDistanceIndex = globalNeighbourId;
                minDistance = distance;
            }   
        }
    }

    return minDistanceIndex;
}
