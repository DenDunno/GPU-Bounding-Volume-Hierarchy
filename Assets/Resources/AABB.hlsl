
struct AABB
{
    float3 minPoint;
    float3 maxPoint;

    static AABB Create(const float3 min, const float3 max)
    {
        AABB result;
        result.minPoint = min;
        result.maxPoint = max;

        return result;
    }

    float3 Centroid() { return (minPoint + maxPoint) * 0.5f; }

    AABB Union(AABB box)
    {
        return Create(min(minPoint, box.minPoint), max(maxPoint, box.maxPoint));
    }

    float3 GetRelativeCoordinates(const float3 pointInside) 
    {
        return (pointInside - minPoint) / (maxPoint - minPoint);
    }
};
