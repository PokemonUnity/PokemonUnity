﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Tinted" {
 Properties {
     _Color ("Color", Color) = (0.5,0.5,0.5,1)
     _MainTex ("Particle Texture", 2D) = "white" {}
     [HideInInspector]_MidColor("MidColor", Color) = (0.5,0.5,0.5,0.5)
 }
 
 Category {
     Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane"	"CanUseSpriteAtlas"="True"}
     Blend SrcAlpha OneMinusSrcAlpha
     AlphaTest Greater .01
     ColorMask RGB
     Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
     BindChannels {
         Bind "Color", color
         Bind "Vertex", vertex
         Bind "TexCoord", texcoord
     }
     
     // ---- Fragment program cards
     SubShader {
         Pass {
         
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #pragma fragmentoption ARB_precision_hint_fastest
             #pragma multi_compile_particles
             

             #include "UnityCG.cginc"
 
             sampler2D _MainTex;
             fixed4 _Color;
             fixed4 _MidColor;
             
             struct appdata_t {
                 float4 vertex : POSITION;
                 fixed4 color : COLOR;
                 float2 texcoord : TEXCOORD0;
             };
 
             struct v2f {
                 float4 vertex : POSITION;
                 fixed4 color : COLOR;
                 float2 texcoord : TEXCOORD0;
                 #ifdef SOFTPARTICLES_ON
                 float4 projPos : TEXCOORD1;
                 #endif
             };
             
             float4 _MainTex_ST;
 
             v2f vert (appdata_t v)
             {
                 v2f o;
                 o.vertex = UnityObjectToClipPos(v.vertex);
                 #ifdef SOFTPARTICLES_ON
                 o.projPos = ComputeScreenPos (o.vertex);
                 COMPUTE_EYEDEPTH(o.projPos.z);
                 #endif
                 o.color = v.color;
                 o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                 return o;
             }
 
             sampler2D _CameraDepthTexture;
             float _InvFade;
             
             fixed4 frag (v2f i) : COLOR
             {
                 #ifdef SOFTPARTICLES_ON
                 float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
                 float partZ = i.projPos.z;
                 float fade = saturate (_InvFade * (sceneZ-partZ));
                 i.color.a *= fade;
                 #endif
                 
             //object's color    
             //    return ((i.color * tex2D(_MainTex, i.texcoord).a)*i.color.a);
             
             
                 return tex2D(_MainTex, i.texcoord) + (( ((i.color * tex2D(_MainTex, i.texcoord).a)) - _MidColor)*2);
             
             //original
             //    return 2.0f * i.color * _Color * tex2D(_MainTex, i.texcoord) ;
             }
             ENDCG 
         }
     }     
     
    
 }
 }