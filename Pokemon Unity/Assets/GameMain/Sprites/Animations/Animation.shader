Shader "Custom/Animation" {
	Properties {
        _Color ("Main Color", Color) = (1,1,1,0)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }

    SubShader {
        Pass {
	        Lighting Off
            Cull Off
            ZTest Always
            Material {
                Diffuse [_Color]
            }
            
            Alphatest Greater [_Cutoff]
        	AlphaToMask True
        	ColorMask RGB

            SetTexture [_MainTex] {
            	constantColor [_Color]
            	Combine constant * texture
        	}
    	}
	}
}