Shader "Custom/TransparentWaterProperOcclusion"
{
    Properties
    {
        _Color ("Base Color", Color) = (0.0, 0.5, 1.0, 0.5)
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _ScrollSpeed ("Normal Map Scroll Speed", Vector) = (0.1, 0.1, 0, 0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            ZWrite On // Enable depth writing
            ZTest LEqual // Render only if the fragment is closer or equal to the camera
            Blend SrcAlpha OneMinusSrcAlpha // Alpha blending for transparency
            Cull Back // Cull back faces for performance

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _NormalMap;
            float4 _Color;
            float4 _ScrollSpeed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Apply scrolling UVs for dynamic water surface
                o.uv = v.uv + _Time.yz * _ScrollSpeed.xy;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the normal map for surface detail
                fixed4 normalDetail = tex2D(_NormalMap, i.uv);

                // Apply the base color and modulate alpha
                fixed4 color = _Color;
                color.rgb *= normalDetail.r; // Modulate RGB with normal map
                color.a *= normalDetail.r;  // Modulate alpha with normal map

                return color;
            }
            ENDCG
        }
    }
}
