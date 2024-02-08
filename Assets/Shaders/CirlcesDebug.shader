Shader "CirlcesDebug.shader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
    }

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
            Name "ClusterDebug"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma enable_d3d11_debug_symbols
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "AABB2D.hlsl"

            StructuredBuffer<Circle2D> _Circles;
            int _VisibleCirclesCount;
            half2 _CameraParams;
            half4 _Color;

            half4 GetDebugColor(float2 uv)
            {
                for (int i = 0; i < _VisibleCirclesCount; ++i)
                {
                    Circle2D circle = _Circles[i];
                    circle.position *= _CameraParams;
                    
                    if (IsInside(circle, uv))
                    {
                        return _Color;
                    }
                }
                
                return half4(0, 0, 0, 0);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                const float4 sceneColor = FragBilinear(input);
                const half4 debugColor = GetDebugColor(input.texcoord * _CameraParams);

                return sceneColor + debugColor;
            }
            ENDHLSL
        }
    }
}