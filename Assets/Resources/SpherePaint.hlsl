
half4 GetFresnelColor(const float3 normal, const float3 viewDirection, const half4 color, const float fresnelPower)
{
    const float absDot = abs(dot(normal, viewDirection));
    const float fresnelValue = (1 - pow(absDot, fresnelPower)) * 1;

    return saturate(color * fresnelValue);
}

half4 GetIntersectionColor(const float sceneDepth, const float fragmentEyeDepth, const float power, const float4 color)
{
    float difference = saturate(sceneDepth - fragmentEyeDepth);
    difference = 1 - saturate(difference * power);

    return difference * color;
}

half4 GetSphereColor(const SphereData sphereData, const float3 viewDirection, const HitResult hitResult, const float sceneDepth)
{
    const half4 fresnelColor = GetFresnelColor(hitResult.normal, viewDirection, sphereData.color, sphereData.fresnelPower);
    const half4 intersectionColor = GetIntersectionColor(sceneDepth, hitResult.fragmentViewDepth, sphereData.intersectionPower, sphereData.intersectionColor);
    
    return (fresnelColor + intersectionColor) * hitResult.success;
}