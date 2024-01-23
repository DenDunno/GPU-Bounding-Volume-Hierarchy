struct Ray
{
    float3 origin;
    float3 direction;
};

struct HitResult
{
    int success;
    float distance;
    float3 normal;
    float3 hitPoint;
    float fragmentViewDepth;
};

struct RaycastResult
{
    HitResult innerHitResult;
    HitResult outerHitResult;
};

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

HitResult CreateHitResult()
{
    HitResult hitResult;
    hitResult.success = 0;
    hitResult.distance = -1;
    hitResult.normal = float3(0, 0, 0);
    hitResult.fragmentViewDepth = 0;
    hitResult.hitPoint = float3(0, 0, 0);
    
    return hitResult;
}

HitResult BuildHitResult(float distance, float3 sphereCentre, Ray ray, float discriminant, float sceneDepth)
{
    const float3 hitPoint = ray.origin + distance * ray.direction;
    const float3 cameraViewPoint = mul(UNITY_MATRIX_V, float4(hitPoint, 1)).xyz;

    HitResult hitResult;
    hitResult.fragmentViewDepth = -cameraViewPoint.z;
    hitResult.success = discriminant >= 0 && hitResult.fragmentViewDepth < sceneDepth;
    hitResult.normal = normalize(hitPoint - sphereCentre);
    hitResult.distance = distance;
    hitResult.hitPoint = hitPoint;
    
    return hitResult;
}

RaycastResult HitSphere(const Ray ray, const float3 position, const float radius, const float sceneDepth)
{
    RaycastResult result;

    if (dot(position - ray.origin, ray.direction) < 0)
    {
        result.innerHitResult = CreateHitResult();
        result.outerHitResult = CreateHitResult();
        return result;
    }

    const float a = 1; // a = dot(direction, direction) = length(direction) = 1, direction is already normalized
    const float b = 2 * dot(ray.direction, ray.origin - position);
    const float c = dot(ray.origin - position, ray.origin - position) - radius * radius;
    const float discriminant = b * b - 4 * a * c;

    const float x1 = (-b + sqrt(discriminant)) / (2 * a);
    const float x2 = (-b - sqrt(discriminant)) / (2 * a);

    result.innerHitResult = BuildHitResult(max(x1, x2), position, ray, discriminant, sceneDepth);
    result.outerHitResult = BuildHitResult(min(x1, x2), position, ray, discriminant, sceneDepth);

    return result;
}
