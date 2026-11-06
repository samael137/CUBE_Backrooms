Shader "Custom/URP_VHS_Retro"
{
    Properties
    {
        [HideInInspector] _BlitTexture ("Blit Texture", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0.0, 1.0)) = 0.35
        _ScanlineCount ("Scanline Count", Float) = 540.0
        _ColorShift ("RGB Color Shift (Bleed)", Range(0.0, 0.02)) = 0.004
        _GlitchIntensity ("Tape Glitch Intensity", Range(0.0, 0.03)) = 0.003
        _GlitchSpeed ("Glitch Speed", Float) = 8.0
        _NoiseIntensity ("Static Noise Intensity", Range(0.0, 0.5)) = 0.06
        _VignetteStrength ("VHS Border Vignette", Range(0.0, 1.0)) = 0.3
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline" 
        }
        LOD 100
        ZWrite Off 
        Cull Off

        Pass
        {
            Name "VHSRenderPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _ScanlineIntensity;
            float _ScanlineCount;
            float _ColorShift;
            float _GlitchIntensity;
            float _GlitchSpeed;
            float _NoiseIntensity;
            float _VignetteStrength;

            // Generador pseudo-aleatorio para ruido de estática analógica
            float hash12(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * 0.1031);
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }

            float4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;
                float time = _Time.y;

                // 1. Deformación / Jitter horizontal de cinta
                float glitchWave = sin(uv.y * 40.0 + time * _GlitchSpeed);
                float glitchZone = step(0.96, sin(time * 3.0 + uv.y * 6.0));
                uv.x += glitchWave * _GlitchIntensity * glitchZone;

                // Leve temblor horizontal en toda la pantalla
                float microJitter = (hash12(float2(time * 15.0, 0.0)) - 0.5) * 0.001;
                uv.x += microJitter;

                // 2. Desfase de color RGB (Aberración cromática analógica)
                float shift = _ColorShift;
                float r = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_PointClamp, uv + float2(shift, 0.0)).r;
                float g = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_PointClamp, uv).g;
                float b = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_PointClamp, uv - float2(shift, 0.0)).b;
                float3 color = float3(r, g, b);

                // 3. Estática / Ruido de cinta VHS
                float noise = (hash12(uv * 500.0 + frac(time * 20.0)) - 0.5) * _NoiseIntensity;
                color += noise;

                // 4. Scanlines horizontales
                float scan = sin(uv.y * _ScanlineCount * 3.14159265);
                scan = (scan + 1.0) * 0.5; // Mapear de [-1,1] a [0,1]
                color *= (1.0 - scan * _ScanlineIntensity);

                // 5. Viñeta en los bordes estilo lente / pantalla CRT
                float2 d = abs(uv - 0.5) * 2.0;
                float vig = saturate(1.0 - dot(d, d) * _VignetteStrength);
                color *= vig;

                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
}
