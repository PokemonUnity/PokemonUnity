Shader "Custom/FadeOut" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MinDistance ("Minimum Distance", float) = 2
        _MaxDistance ("Maximum Distance", float) = 3
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
 
    Pass {
        ZWrite On
        ColorMask 0
    }
   
        CGPROGRAM
        #pragma surface surf Lambert alpha:fade
        #pragma target 3.0
        sampler2D _MainTex;
        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };
        float _MinDistance;
        float _MaxDistance;
        fixed4 _Color;
        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            
            // help: https://developer.download.nvidia.com/cg/index_stdlib.html
            float distanceFromCamera = distance(IN.worldPos, _WorldSpaceCameraPos);
            float fade = saturate((distanceFromCamera - _MinDistance) / _MaxDistance);
            
            o.Alpha = c.a * fade;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
