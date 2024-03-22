Shader "Unlit/RoundedBox"
{
    Properties
    {
        _MainTex("Main texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma enable_d3d11_debug_symbols
            #include "UnityCG.cginc"

            #define VERTICAL_SIDE 0
            #define CORNER 1
            #define HORIZONTAL_SIDE 2

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 samplePosition : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            half4 _Color;
            int _Frequency;
            float _Thickness;
            float _Offset;
            float _Radius;
            float _RotationSpeed;
            float _DashSize;
            float _Feather;
            float2 _RectangleSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.samplePosition = (2 * v.uv - 1) * _RectangleSize;
                return o;
            }

            float GetRoundedBoxSdf(float2 position, float2 size, float radius)
            {
                float2 boxPosition = abs(position) - size + radius;
                return min(max(boxPosition.x, boxPosition.y), 0.0) + length(max(boxPosition, 0.0)) - radius;
            }
            
            float EvaluateLinearFeatheredEdge(float x, float start, float end, float feather)
            {
                float increase = (x - start) / feather;
                float decrease = (end - x) / feather;

                increase = clamp(increase, 0.0, 1.0);
                decrease = clamp(decrease, 0.0, 1.0);
                
                float value = x < start + feather ? increase : 1.0; 
                value = x > end - feather ? decrease : value; 

                return value;
            }

            float EvaluateOutline(float maxRadius, float radius, float2 samplePosition)
            {
                const float thickness = lerp(0, maxRadius, _Thickness);
                const float sdf = GetRoundedBoxSdf(samplePosition, _RectangleSize, radius);

                return EvaluateLinearFeatheredEdge(sdf, -thickness, 0, _Feather);
            }

            float GetRoundedRectanglePerimeter(float2 rect, float radius)
            {
                const float firstSide = rect.x - 2 * radius;
                const float secondSide = rect.y - 2 * radius;
                const float perimeter = 2.0 * (firstSide + secondSide + UNITY_PI * radius);

                return perimeter;
            }

            int EvaluateQuadrant(float2 v)
            {
                v = sign(v);
                return -v.y + (-v.x * v.y + 3) / 2;
            }

            float2 GetQuadrantCoordinates(float2 v)
            {
                return float2(abs(v.x), abs(v.y));
            }

            int GetSectorInsideQuadrant(float2 arcRelativeCoordinates)
            {
                if (arcRelativeCoordinates.x > 0 && arcRelativeCoordinates.y > 0)
                    return 1;

                return arcRelativeCoordinates.x - arcRelativeCoordinates.y > 0 ? 0 : 2;
            }

            float2 InverseSecondAndFourthQuadrant(float2 samplePosition)
            {
                float2 flippedPosition = float2(samplePosition.y, samplePosition.x);
                const int isSecondOrFourthQuadrant = samplePosition.x * samplePosition.y < 0;

                return isSecondOrFourthQuadrant ? flippedPosition : samplePosition;
            }

            float EvaluatePerimeter(float2 radius)
            {
                const float2 innerRectangle = 2 * (_RectangleSize - radius);

                return 2 * (innerRectangle.x + innerRectangle.y + UNITY_PI * radius);
            }

            float EvaluateCornerLength(float2 arcRelativeCoordinates, float radius, float2 quadrantSize)
            {
                const float angle = atan2(arcRelativeCoordinates.y, arcRelativeCoordinates.x);
                const float arcLength = angle * radius;

                return arcLength + (quadrantSize.y - radius);
            }

            float EvaluateQuadrantSegmentLength(int quadrant, float radius, float segmentPerimeter,float2 samplePosition)
            {
                const float2 inversedSamplePosition = InverseSecondAndFourthQuadrant(samplePosition);
                const float2 quadrantSize = quadrant % 2 == 1 ? _RectangleSize.yx : _RectangleSize.xy;
                const float2 quadrantCoordinates = GetQuadrantCoordinates(inversedSamplePosition);

                const float2 arcPosition = quadrantSize - radius;
                const float2 arcRelativeCoordinates = quadrantCoordinates - arcPosition;
                const int sector = GetSectorInsideQuadrant(arcRelativeCoordinates);

                switch (sector)
                {
                case VERTICAL_SIDE: return quadrantCoordinates.y;
                case CORNER: return EvaluateCornerLength(arcRelativeCoordinates, radius, quadrantSize);
                case HORIZONTAL_SIDE: return segmentPerimeter - quadrantCoordinates.x;
                default: return -1;
                }
            }

            float EvaluateBorderCoordinate(float radius, float perimeter, float2 samplePosition)
            {
                const int quadrant = EvaluateQuadrant(samplePosition);
                const float segmentPerimeter = perimeter / 4;
                const float previousSegmentsSum = quadrant * segmentPerimeter;
                const float quadrantSegmentLength = EvaluateQuadrantSegmentLength(quadrant, radius, segmentPerimeter, samplePosition);

                return previousSegmentsSum + quadrantSegmentLength;
            }

            float EvaluateDash(float radius, float2 samplePosition)
            {
                const float perimeter = EvaluatePerimeter(radius);
                const float borderCoordinate = EvaluateBorderCoordinate(radius, perimeter, samplePosition);
                const float dashSize = perimeter / _Frequency;
                const float offset = _Time.z * _RotationSpeed % perimeter;
                const float visibleDashSize = lerp(0, dashSize, _DashSize);
                const float dashValue = (borderCoordinate + offset) % dashSize;
                
                return EvaluateLinearFeatheredEdge(dashValue, 0, visibleDashSize, _Feather);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float maxRadius = min(_RectangleSize.x, _RectangleSize.y);
                const float radius = lerp(0, maxRadius, _Radius);
                const float outline = EvaluateOutline(maxRadius, radius, i.samplePosition);
                const float dash = EvaluateDash(radius, i.samplePosition);

                return fixed4(_Color.xyz, _Color.a * outline *  dash);
            }
            ENDCG
        }
    }
}