
half4 GetFresnelColor(float3 normal, float3 viewDirection, half4 color)
{
    const float absDot = abs(dot(normal, viewDirection));
    const float fresnelValue = (1 - pow(absDot, 1)) * 1;

    return saturate(color * fresnelValue);
}

half4 GetIntersectionColor(float sceneDepth, float fragmentEyeDepth, float power, float4 color)
{
    float difference = saturate(sceneDepth - fragmentEyeDepth);
    difference = 1 - saturate(difference * power);

    return difference * color;
}

half4 GetSphereColor(SphereData sphereData, float3 viewDirection, HitResult hitResult, float sceneDepth)
{
    const half4 fresnelColor = GetFresnelColor(hitResult.normal, viewDirection, sphereData.color);
    const half4 intersectionColor = GetIntersectionColor(sceneDepth, hitResult.fragmentViewDepth, sphereData.intersectionPower, sphereData.intersectionColor);
    
    return (fresnelColor + intersectionColor) * hitResult.success;
}