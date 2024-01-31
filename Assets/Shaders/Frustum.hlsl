
struct FrustumPlane
{
    float3 Point;
    float3 Normal;
    
    bool IsOutOfBounds(float3 spherePosition, float radius)
    {
        const float3 difference = Point - spherePosition;
        const float distance = dot(difference, Normal);
        
        return distance < -radius;
    }
};

struct Frustum
{
    FrustumPlane Top;
    FrustumPlane Bottom;
    FrustumPlane Right;
    FrustumPlane Left;
    FrustumPlane Far;
    FrustumPlane Near;

    int IsOutside(float3 position, float radius)
    {
        return Top.IsOutOfBounds(position, radius) ||
               Bottom.IsOutOfBounds(position, radius) ||
               Right.IsOutOfBounds(position, radius) ||
               Left.IsOutOfBounds(position, radius) ||
               Far.IsOutOfBounds(position, radius) ||
               Near.IsOutOfBounds(position, radius);
    }
};