Shader "Custom/SimpleToon"
{
    Properties
    {
        _MainTex ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _ShadowStep ("Shadow Threshold", Range(0, 1)) = 0.5
        _ShadowSmoothness ("Shadow Smoothness", Range(0, 0.5)) = 0.05
        _EmissionMap ("Emission Map", 2D) = "black" {}
        _EmissionColor ("Emission Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float3 normalOS     : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _EmissionMap;

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _BaseColor;
                float _ShadowStep;
                float _ShadowSmoothness;
                float4 _EmissionColor;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                // Texture sampling
                float4 baseColor = tex2D(_MainTex, input.uv) * _BaseColor;
                float4 emission = tex2D(_EmissionMap, input.uv) * _EmissionColor;

                // Lighting calculation
                Light mainLight = GetMainLight();
                float3 normal = normalize(input.normalWS);
                float NdotL = dot(normal, mainLight.direction);

                // Toon Shading (Step function)
                float lightIntensity = smoothstep(_ShadowStep - _ShadowSmoothness, _ShadowStep + _ShadowSmoothness, NdotL);
                
                float3 diffuse = baseColor.rgb * (lightIntensity * mainLight.color + (1 - lightIntensity) * 0.4f); // 0.4f is ambient shadow color
                
                return float4(diffuse + emission.rgb, baseColor.a);
            }
            ENDHLSL
        }
    }
}
