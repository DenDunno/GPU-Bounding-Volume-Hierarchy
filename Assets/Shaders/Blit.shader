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
            #include "SphereData.hlsl"
            #include "Helpers.hlsl"
            #include "Raytracing.hlsl"
            #include "SpherePaint.hlsl"
            
            StructuredBuffer<SphereData> _Spheres;
            sampler2D _CameraDepthTexture;
            half4 _CameraParams;
            int _SpheresCount;
            float _Intensity;
            
            half4 IterateSpheres(Ray ray, const float depth)
            {
                half4 resultColor = half4(0, 0, 0, 0);

                for (int i = 0; i < _SpheresCount; ++i)
                {
                    const SphereData sphereData = _Spheres[i];
                    RaycastResult hitResult = HitSphere(ray, sphereData.position, sphereData.radius, depth);
                    
                    const half4 innerSphereColor = GetSphereColor(sphereData, ray.direction, hitResult.inner, depth);
                    const half4 outerSphereColor = GetSphereColor(sphereData, ray.direction, hitResult.outer, depth);

                    resultColor = saturate(resultColor + innerSphereColor + outerSphereColor);
                }

                return half4(resultColor.xyz, 1 - resultColor.a);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                const float sceneDepth = GetDepth(input.texcoord, _CameraDepthTexture);
                const Ray ray = GetInitialRay(input.texcoord, _CameraParams);

                const half4 spheresColor = IterateSpheres(ray, sceneDepth);
                const float4 sceneColor = FragBilinear(input);

                return BlendSrcOneMinusSrc(sceneColor, spheresColor);
            }
            ENDHLSL
        }
    }
}