Shader "ColorBlit"
{
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "ColorBlitPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma enable_d3d11_debug_symbols
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "HLSLSupport.cginc"

            struct SphereData
            {
                float3 position;
                float radius;
                float4 color;
                float4 intersectionColor;
                float intersectionPower;
            };

            StructuredBuffer<SphereData> _Spheres;
            sampler2D _CameraDepthTexture;
            half4 _CameraParams;
            int _SpheresCount;
            float _Intensity;

            #include "Helpers.hlsl"
            #include "Raytracing.hlsl"
            #include "SpherePaint.hlsl"

            half4 IterateSpheres(Ray ray, const float sceneDepth)
            {
                half4 resultColor = half4(0, 0, 0, 0);

                for (int i = 0; i < _SpheresCount; ++i)
                {
                    const SphereData sphereData = _Spheres[i];
                    const RaycastResult hitResult = HitSphere(ray, sphereData.position, sphereData.radius, sceneDepth);
                    
                    const half4 innerSphereColor = GetSphereColor(sphereData, ray.direction, hitResult.innerHitResult, sceneDepth);
                    const half4 outerSphereColor = GetSphereColor(sphereData, ray.direction, hitResult.outerHitResult, sceneDepth);

                    resultColor = saturate(resultColor + innerSphereColor + outerSphereColor);
                }

                return half4(resultColor.xyz, 1 - resultColor.a);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                const float sceneDepth = GetDepth(input.texcoord);
                const Ray ray = GetInitialRay(input.texcoord, _CameraParams);

                const half4 spheresColor = IterateSpheres(ray, sceneDepth);
                const float4 sceneColor = FragBilinear(input);

                return BlendSrcOneMinusSrc(sceneColor, spheresColor);
            }
            ENDHLSL
        }
    }
}