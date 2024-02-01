Shader "ClusterDebug"
{
    Properties
    {
        _GapSize("GapSize", float) = 0.01
        _Color("Color", Color) = (1,1,1,1)
        _MaxTilesColor("Max tiles color", Color) = (0,1,1,1)
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

            StructuredBuffer<int> _ActiveTiles;
            half4 _MaxTilesColor;
            int _TilesCountX;
            int _TilesCountY;
            int _SpheresCount;
            float _GapSize;
            half4 _Color;

            float InvLerp(float from, float to, float value)
            {
                return (value - from) / (to - from);
            }

            float2 GetLocalUv(float xTile, float yTile)
            {
                return float2(InvLerp(floor(xTile), ceil(xTile), xTile), InvLerp(floor(yTile), ceil(yTile), yTile));
            }

            int GetRectangleShape(float2 uv)
            {
                const float2 width = float2(_GapSize / 2, _GapSize);
                const float2 bottomLeft = step(width, uv);
                const float2 topRight = step(width, 1.0 - uv);
                const int rectangle = bottomLeft.x * bottomLeft.y * topRight.x * topRight.y;

                return rectangle;
            }

            uint GetHash(uint s)
            {
                s ^= 2747636419u;
                s *= 2654435769u;
                s ^= s >> 16;
                s *= 2654435769u;
                s ^= s >> 16;
                s *= 2654435769u;
                return s;
            }

            float Random(uint seed)
            {
                return float(GetHash(seed)) / 4294967295.0; // 2^32-1
            }

            half4 GetRandomColor(int seed)
            {
                return saturate(1 - half4(Random(seed + 1), Random(seed + 2), Random(seed + 3), 1)) / 2;
            }

            half4 GetDebugColor(float2 uv)
            {
                const float xTile = _TilesCountX * uv.x;
                const float yTile = _TilesCountY * uv.y;
                const int tileIndex = (int)yTile * _TilesCountX + (int)xTile;
                const int sphereCountInTile = _ActiveTiles[tileIndex];
                const float2 localUv = GetLocalUv(xTile, yTile);
                const half4 color = GetRandomColor(sphereCountInTile + 1);
                const int hasSpheresInTile = sphereCountInTile > 0;

                return half4(localUv.x, localUv.y, 0, 1) * hasSpheresInTile / 3;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                const float4 sceneColor = FragBilinear(input);
                const half4 debugColor = GetDebugColor(input.texcoord);

                return sceneColor + debugColor;
            }
            ENDHLSL
        }
    }
}