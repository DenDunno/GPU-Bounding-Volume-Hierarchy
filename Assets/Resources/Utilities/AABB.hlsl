
#ifndef AABB_HLSL
#define AABB_HLSL
#define INT_MAX 2147483647
#define INT_MIN -2147483648
#define FLOAT_MAX 3.402823466e+38

struct AABB
{
    float3 Min;
    float3 Max;

    static AABB CreateMaxBox()
    {
        return Create(float3(-FLOAT_MAX, -FLOAT_MAX, -FLOAT_MAX), float3(FLOAT_MAX, FLOAT_MAX, FLOAT_MAX));
    }
    
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
#endif