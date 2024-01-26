Shader "DenDunno/IntersectingSpheres"
{
    Properties
    {
        [HDR] _FresnelColor("Fresnel color", Color) = (0, 0.61, 1, 1)
        _FresnelPower("Fresnel power", float) = 1
        _FresnelStrength("Fresnel strength", float) = 1
        _IntersectionPower("Intersection power", Float) = 25
        [HDR] _IntersectionColor("Intersection color", Color) = (0, 0.94, 1, 1)
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 0
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
        }

        Pass
        {
            Cull [_Cull]
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            LOD 100

            Stencil
            {
                Ref 0
                Comp Equal
                Pass IncrSat
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature Fresnel
            #pragma enable_d3d11_debug_symbols
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 viewPosition : TEXCOORD1;
                float3 viewDirection : TEXCOORD2;
                float4 screenPosition : TEXCOORD4;
                float4 localVertex : TEXCOORD5;
            };

            half4 _FresnelColor;
            float _FresnelPower;
            float _FresnelStrength;
            float _IntersectionPower;
            half4 _IntersectionColor;
            sampler2D _CameraDepthTexture;

            v2f vert(appdata v)
            {
                float3 worldPosition = mul(unity_ObjectToWorld, v.vertex);

                v2f o;
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDirection = normalize(UnityWorldSpaceViewDir(worldPosition));
                o.screenPosition = ComputeScreenPos(o.vertex);
                o.viewPosition = mul(UNITY_MATRIX_V, float4(worldPosition, 1.0)).xyz;
                o.localVertex = v.vertex;

                return o;
            }

            float invLerp(float from, float to, float value)
            {
                return (value - from) / (to - from);
            }

            half4 evaluateFresnelColor(float3 normal, float3 viewDirection, float power)
            {
                const float absDot = abs(dot(normal, viewDirection));
                const float fresnelValue = (1 - pow(absDot, power)) * _FresnelStrength;

                return saturate(_FresnelColor * fresnelValue);
            }

            half4 getIntersectionColor(float4 screenPosition, float3 viewPositionZ)
            {
                const float2 screenUv = screenPosition.xy / screenPosition.w;
                const float fragmentEyeDepth = -viewPositionZ;
                const float depthTextureValue = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenUv);
                const float sceneDepth = LinearEyeDepth(depthTextureValue);

                float difference = saturate(sceneDepth - fragmentEyeDepth);
                difference = 1 - saturate(difference * _IntersectionPower);

                return difference * _IntersectionColor;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const half4 fresnelEffect = evaluateFresnelColor(i.normal, i.viewDirection, _FresnelPower);
                const half4 intersectionColor = getIntersectionColor(i.screenPosition, i.viewPosition.z);

                return intersectionColor + fresnelEffect;
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}