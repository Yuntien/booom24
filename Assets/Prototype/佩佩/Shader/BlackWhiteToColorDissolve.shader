Shader "Custom/BlackWhiteToColorDissolve"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _ColorTransition ("Color Transition", Range(0, 1)) = 0
        _PixelSize ("Pixel Size", Range(1, 128)) = 19
        _DissolveSeed ("Dissolve Seed", Range(-8, 8)) = 0.5
        _SpriteFade ("Sprite Fade", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float _ColorTransition;
            float _PixelSize;
            float _DissolveSeed;
            float _SpriteFade;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                float4 col = tex2D(_MainTex, i.uv);

                // Convert to grayscale
                float grayscale = dot(col.rgb, float3(0.3, 0.59, 0.11));

                // Pixelate UVs
                float2 pixelUv = floor(i.uv * _PixelSize) / _PixelSize;

                // Random dissolve pattern based on pixelated UVs
                float noise = frac(sin(dot(pixelUv, float2(12.9898,78.233))) * 43758.5453);

                // Use the noise to determine if the pixel should be grayscale or colored
                float transition = smoothstep(_ColorTransition, _ColorTransition + 0.1, noise + _DissolveSeed);

                // Interpolate between grayscale and original color based on transition
                float3 finalColor = lerp(grayscale.xxx, col.rgb, transition);

                // Apply sprite fade
                finalColor *= i.color.rgb;
                float finalAlpha = col.a * _SpriteFade * i.color.a;

                return float4(finalColor, finalAlpha);
            }
            ENDCG
        }
    }

    Fallback "Sprites/Default"
}
