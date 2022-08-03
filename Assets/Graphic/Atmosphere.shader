Shader "Planet/Atmosphere"
{
    Properties
    {
        _CenterColor ("Center color", Color) = (0, 0, 0, 0)
        _BackColor ("Back color", Color) = (0, 0, 0, 0)
        _StartFrom ("Start Atmosphere Radius", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float4 _CenterColor;
            float4 _BackColor;
            float _StartFrom;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                _StartFrom = 1. / abs(1. - _StartFrom);
                fixed4 col = lerp(_CenterColor, _BackColor, clamp(distance(float2(0.5, 0.5), i.uv)*2.*_StartFrom-_StartFrom+1., 0., 1.));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
