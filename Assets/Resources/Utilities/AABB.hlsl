
struct AABB
{
    float3 Max;
    float3 Min;

    static AABB Create(const float3 min, const float3 max)
    {
        AABB result;
        result.Min = min;
        result.Max = max;

        return result;
    }

    float3 Centroid() { return (Min + Max) * 0.5f; }
    float3 Size() { return Max - Min; }

    AABB Union(const AABB box)
    {
        return Create(min(Min, box.Min), max(Max, box.Max));
    }

    float3 GetRelativeCoordinates(const float3 pointInside)
    {
        return (pointInside - Min) / (Max - Min);
    }

    float ComputeSurfaceArea()
    {
        float3 size = Size();
        return 2 * (size.x * size.y + size.y * size.z + size.z * size.x);
    }
};
