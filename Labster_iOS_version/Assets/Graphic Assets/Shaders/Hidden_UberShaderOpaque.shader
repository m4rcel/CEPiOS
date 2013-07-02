Shader "Hidden/Highlighted/ReaShader/UberShader_Opaque" {
	Properties{
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_EmStr ("Emission Intensity", Float) = 0
	_EColor ("Emission Color", Color) = (1,1,1,1)
	_Mask ("SplatMask(R)emission(G)Reflection", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
	_RefStr ("Reflection strenght", Float) = 0
	_Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
	_EmissionLM ("Emission (Lightmapper)", Float) = 0
	
	_Outline ("Outline", Color) = (0,0,0,1)
	_InnerTint ("Inner Tint Value", Range (0, 1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque"  "RenderEffect"="Highlighted"}
		LOD 400
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma exclude_renderers d3d11_9x
		#pragma target 3.0
//		#pragma debug
		
			sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _Mask;
			samplerCUBE _Cube;
			float4 _Color;
			float4 _ReflectColor;
			float4 _EColor;
			float _EmStr;
			float _RefStr;
			
			struct Input {
				float2 uv_MainTex;
				float2 uv_Mask;
				float2 uv_BumpMap;
				float3 worldRefl;
				INTERNAL_DATA	
			};
			
			void surf (Input IN, inout SurfaceOutput o) {
				float4 tex = tex2D(_MainTex, IN.uv_MainTex);
				float4 m = tex2D(_Mask, IN.uv_Mask);
				float3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				
				o.Albedo = tex.rgb * _Color;
				o.Normal = n;
				
				float3 worldRefl = WorldReflectionVector (IN, o.Normal);
				float3 reflcol = texCUBE (_Cube, worldRefl);
					reflcol = reflcol * _ReflectColor * _RefStr * m.g;
			
				o.Emission = (tex.rgb * _EmStr * _EColor * m.r) + reflcol;

			}
			ENDCG
		}
	FallBack "Self-Illumin/Specular"
}
