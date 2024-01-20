Shader "Unlit/Sphere"
{
    Properties
    {
        [HDR] _FresnelColorStart("Fresnel color start", Color) = (1,1,1,1)
        [HDR] _FresnelColorEnd("Fresnel color end", Color) = (1,1,1,1)
        _Feather("Feather", Range(0, 1)) = 0
        [Toggle(Fresnel)] Fresnel("Fresnel", Float) = 0
        _FresnelPower("Fresnel power", float) = 1
        _FresnelStrength("Fresnel strength", float) = 1
        _Dissolve("Dissolve", Range(0, 1)) = 1
        _DissolveVector("Dissolve vector", Vector) = (0, 1, 0, 0)
        _DissolveNoiseTexture("Dissolve noise texture", 2D) = "white" {}
        _DissolveStrength("Dissolve strength", float) = 1
        _OutlineSize("Outline size", Range(0, 1)) = 1
        [HDR] _OutlineColor("Outline color", Color) = (1,1,1,1)
        [Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", Int) = 0
        _IntersectionPower("Intersection power", Float) = 1
        [HDR] _IntersectionColor("Intersection color", Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
        }
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull [_CullMode]
            LOD 100
            
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

            half4 _FresnelColorStart;
            half4 _FresnelColorEnd;
            float _FresnelPower;
            float _FresnelStrength;
            float _Dissolve;
            float4 _DissolveVector;
            float _DissolveStrength;
            sampler2D _DissolveNoiseTexture;
            sampler2D _CameraDepthTexture;
            float _OutlineSize;
            half4 _OutlineColor;
            float _IntersectionPower;
            float _Feather;
            half4 _IntersectionColor;

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
                
                return saturate(_FresnelColorEnd * fresnelValue);
            }

            float getLerpValueAlongAxis(float3 localVertex, float2 uv)
            {
                const float3 dissolveDirection = normalize(_DissolveVector);
                const float lineProjection = dot(localVertex, dissolveDirection) + 0.5f; // [0 1]
                const float noise = tex2D(_DissolveNoiseTexture, uv).x * _DissolveStrength;

                return lineProjection;
            }

            half4 getIntersectionColor(float4 screenPosition, float3 viewPositionZ)
            {
                const float2 screenUv = screenPosition.xy / screenPosition.w;
                const float fragmentEyeDepth = -viewPositionZ;
                const float depthTextureValue = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenUv);
                const float sceneDepth = LinearEyeDepth(depthTextureValue);

                float difference = saturate(sceneDepth - fragmentEyeDepth);
                difference = 1 - saturate(difference * _IntersectionPower);

                float d = 1 - Linear01Depth(depthTextureValue);
                //return half4(d,d,d, 1);
                return difference * _IntersectionColor;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const half4 fresnelEffect = evaluateFresnelColor(i.normal, i.viewDirection, _FresnelPower);
                const half4 intersectionColor = getIntersectionColor(i.screenPosition, i.viewPosition.z);
                const float lerpAlongAxis = getLerpValueAlongAxis(i.localVertex, i.uv);
                
                //return intersectionColor;
                return intersectionColor + fresnelEffect;
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}