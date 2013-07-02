
Shader "ReaShader/Shadow_multiply" {
	Properties {
		_MainTex ("Base", 2D) = "white" {}
		_Bri("Brightness",Range(0,1)) = 0.0
	}
	
	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float _Bri;
						
		struct v2f {
			half4 pos : SV_POSITION;
			half2 uv : TEXCOORD0;
		};

		v2f vert(appdata_full v) {
			v2f o;
			
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);	
			o.uv.xy = v.texcoord.xy;
					
			return o; 
		}
		
		fixed4 frag( v2f i ) : COLOR {	
			float4 tex = tex2D (_MainTex, i.uv.xy);
			float4 sha = tex + _Bri;
			return saturate(sha);
		}
	
	ENDCG
	
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend Zero SrcColor
		
	Pass {
	
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		ENDCG
		 
		}
				
	} 
	FallBack Off
}
