Shader "Custom/BlendedSkybox"
{
    Properties
    {
        _DayCube   ("Day Cubemap",   Cube) = "white" {}
        _NightCube ("Night Cubemap", Cube) = "black" {}
        _Blend     ("Blend",         Range(0,1)) = 0
        _Exposure  ("Exposure",      Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _DayCube;
            samplerCUBE _NightCube;
            float _Blend;
            float _Exposure;

            struct appdata { float4 vertex : POSITION; };
            struct v2f
            {
                float4 pos    : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 day   = texCUBE(_DayCube,   i.texcoord);
                fixed4 night = texCUBE(_NightCube,  i.texcoord);
                fixed4 col   = lerp(day, night, _Blend);
                return col * _Exposure;
            }
            ENDCG
        }
    }
}