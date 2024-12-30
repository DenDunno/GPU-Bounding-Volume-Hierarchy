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
        ZWrite Off
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
                float2 sideSize : TEXCOORD1;
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
                float2 sideSize = GetSideSize(IN.sideIndex);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.scaledUV = IN.uv * sideSize;
                OUT.sideSize = sideSize;
                return OUT;
            }

            bool ComputeBorderByDimension(float uv, float size)
            {
                float maxBorderSize = (1 - _DashSize) / (2 * _DashCount);
                float borderSize = lerp(0, maxBorderSize, _BorderSize);

                return uv < borderSize || uv > size - borderSize;
            }

            int ComputeBorder(float2 uv, float2 sideSize)
            {
                return ComputeBorderByDimension(uv.x, sideSize.x) ||
                    ComputeBorderByDimension(uv.y, sideSize.y);
            }

            int IsCorner(float2 uv, float2 sideSize)
            {
                float offset = (1 + _DashSize) / (2 * _DashCount);

                int isBottomLeftCorner = uv.x < offset && uv.y < offset;
                int isBottomRightCorner = uv.x > sideSize.x - offset && uv.y < offset;
                int isTopLeftCorner = uv.x < offset && uv.y > sideSize.y - offset;
                int isTopRightCorner = uv.x > sideSize.x - offset && uv.y > sideSize.y - offset;
                int isCorner = isBottomLeftCorner || isBottomRightCorner || isTopLeftCorner || isTopRightCorner;

                return isCorner * ComputeBorder(uv, sideSize);
            }

            int ComputeDashByDimension(float dashedUV, float secondAxisUV, float secondAxisSize)
            {
                int liesOnTargetAxis = ComputeBorderByDimension(secondAxisUV, secondAxisSize);
                int isDashOnTargetAxis = frac(dashedUV * _DashCount) < _DashSize;
                return isDashOnTargetAxis && liesOnTargetAxis;
            }

            int ComputeDash(float2 scaledUV, float2 sideSize)
            {
                float2 dashedUV = scaledUV + (_DashSize + 1) / (2 * _DashCount);
                int xDash = ComputeDashByDimension(dashedUV.x, scaledUV.y, sideSize.y);
                int yDash = ComputeDashByDimension(dashedUV.y, scaledUV.x, sideSize.x);
                return xDash || yDash || IsCorner(scaledUV, sideSize);
            }

            half4 frag(Varyings input) : SV_Target
            {
                return _DashColor * ComputeDash(input.scaledUV, input.sideSize);
            }
            ENDHLSL
        }
    }
}