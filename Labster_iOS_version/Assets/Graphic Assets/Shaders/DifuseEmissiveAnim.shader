Shader "ReaShader/Special/DiffuseEmissive_animated" 
{
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Emissive ("Emission ", 2D) = "white" {}
	_Trans ("Transparency", Float) = 0
	_Emission ("Emission Strength", Float) = 0
	_speedX ("ScrollTimeX", Float) = 0
	_speedY ("ScrollTimeY", Float) = 0
	
}

SubShader 
	{
	Tags 
	{
	"Queue"="Opaque" "IgnoreProjector" = "True" "RenderType"="Geometry"
	}
	LOD 200
//	Blend SrcAlpha OneMinusSrcAlpha
//	Blend One One

	CGPROGRAM
	#pragma surface surf Lambert
	
	
	sampler2D _MainTex,_Emissive;
	fixed4 _Color;
	//fixed4 _Time;
	fixed _Trans,_Emission,_speedX,_speedY;
	
	struct Input 
	{
		float2 uv_MainTex;
		float2 uv_Emissive;
	};
	
	void surf (Input IN, inout SurfaceOutput o) 
	{
		fixed scrollX = _Time * _speedX;
		fixed scrollY = _Time * _speedY;
		
		fixed2 UVpan = fixed2(IN.uv_MainTex.x + scrollX,IN.uv_MainTex.y + scrollY);
		fixed4 c = tex2D(_MainTex,UVpan.xy) * _Color;
		fixed4 Em = tex2D(_Emissive,UVpan.xy) * _Color;
		o.Albedo = c.rgb;
//		o.Alpha = c.a * _Trans;
		o.Emission = Em.rgb * _Emission;
	}
	ENDCG
	}

Fallback "Transparent/VertexLit"
}
