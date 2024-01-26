Shader "Unlit/ZTestOnly"
{
    Properties {}
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
        }

        Pass
        {
            ColorMask 0
        }
    }
}