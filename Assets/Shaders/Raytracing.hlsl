#include <UnityShaderVariables.cginc>

struct Ray
{
    float3 origin;
    float3 direction;
};
            
struct HitResult
{
    int success;
    float3 hitPoint;
    float distance;
    float3 normal;
};
            
struct Sphere
{
    float radius;
    float3 position;
};

HitResult CreateHitResult()
{
    HitResult result;
    result.success = 0;
    result.hitPoint = float3(0,0,0);
    result.normal = float3(0,0,0);
    result.distance = 1000000000;
                
    return result;
}
            
Ray CreateRay(float3 origin, float3 direction)
{
    Ray ray;
    ray.direction = direction;
    ray.origin = origin;
                
    return ray;
}

Ray GetInitialRay(float2 uv, half4 cameraInput)
{
    const float3 localViewPoint = float3(uv - 0.5, 1) * cameraInput;
    const float3 worldViewPoint = mul(unity_CameraToWorld, float4(localViewPoint, 1));
    const float3 cameraPosition = _WorldSpaceCameraPos;

    return CreateRay(cameraPosition, normalize(worldViewPoint - cameraPosition));
}

HitResult HitSphere(const Ray ray, const Sphere sphere)
{
    HitResult result = CreateHitResult();
                
    if (dot(sphere.position - ray.origin, ray.direction) < 0)
    {
        return result;
    }

    const float a = 1; // a = dot(direction, direction) = length(direction) = 1, direction is already normalized
    const float b = 2 * dot(ray.direction, ray.origin - sphere.position);
    const float c = dot(ray.origin - sphere.position, ray.origin - sphere.position) - sphere.radius * sphere.radius;
    const float discriminant = b * b - 4 * a * c;
    result.success = discriminant >= 0;
                 
    if (result.success)
    {
        const float x1 = (-b + sqrt(discriminant)) / (2 * a);
        const float x2 = (-b - sqrt(discriminant)) / (2 * a);
        const float distance = min(x1, x2);

        if (distance < result.distance)
        {
            result.hitPoint = ray.origin + distance * ray.direction;
            result.normal = normalize(result.hitPoint - sphere.position);
            result.distance = distance;
        }
    }

    return result;
}