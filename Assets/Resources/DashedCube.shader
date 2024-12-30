Shader "Unlit/DashedCube"
{
    Properties
    {
        _DashColor("Dash color", Color) = (1,1,1,1)
        _FaceColor("Face color", Color) = (1,1,1,1)
        _DashSize("Dash size", Range(0, 1)) = 0.1
        _DashCount("Dash count", Integer) = 1
        _BorderSize("Border size", Range(0, 1)) = 0.1
        _Offset("_Offset", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        //ZWrite Off
        Cull Off
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float3 _Size;
            float2 _Offset;
            int _DashCount;
            float _DashSize;
            half4 _DashColor;
            half4 _FaceColor;
            float _BorderSize;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                int sideIndex : TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 scaledUV : TEXCOORD0;
            };

            float2 GetSideSize(int sideIndex)
            {
                switch (sideIndex)
                {
                case 0: return float2(_Size.x, _Size.y);
                case 1: return float2(_Size.z, _Size.y);
                case 2: return float2(_Size.x, _Size.z);
                default: return float2(0, 0);
                }
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.scaledUV = IN.uv * GetSideSize(IN.sideIndex);
                return OUT;
            }

            bool ComputeBorderByDimension(float uv)
            {
                float maxBorderSize = (1 - _DashSize) / (2 * _DashCount);
                float borderSize = lerp(0, maxBorderSize, _BorderSize);

                return uv < borderSize || uv > 1 - borderSize;
            }

            int ComputeBorder(float2 uv)
            {
                return ComputeBorderByDimension(uv.x) || ComputeBorderByDimension(uv.y);
            }

            int IsCorner(float2 uv)
            {
                float offset = (1 - _DashSize) / (2 * _DashCount);

                int isBottomLeftCorner = uv.x < offset && uv.y < offset;
                int isBottomRightCorner = uv.x > 1 - offset && uv.y < offset;
                int isTopLeftCorner = uv.x < offset && uv.y > 1 - offset;
                int isTopRightCorner = uv.x > 1 - offset && uv.y > 1 - offset;

                return isBottomLeftCorner || isBottomRightCorner || isTopLeftCorner || isTopRightCorner;
            }

            int ComputeDashByDimension(float dashedUV)
            {
                return frac(dashedUV * _DashCount) < _DashSize;
            }

            int ComputeDash(float2 uv)
            {
                float2 dashedUV = uv + (_DashSize + 1) / (2 * _DashCount);
                return ComputeDashByDimension(dashedUV.x) || ComputeDashByDimension(dashedUV.y) || IsCorner(uv);
            }

            half4 frag(Varyings input) : SV_Target
            {
                return half4(frac(input.scaledUV), 0, 1);
                int isBorder = ComputeBorder(input.scaledUV);
                int isFace = isBorder == 0;
                half4 dash = _DashColor * isBorder * ComputeDash(input.scaledUV);
                half4 face = isFace * _FaceColor;
                return face + dash;
            }
            ENDHLSL
        }
    }
}