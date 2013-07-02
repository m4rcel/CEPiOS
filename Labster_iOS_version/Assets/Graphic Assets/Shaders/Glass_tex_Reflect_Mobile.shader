Shader "ReaShader/Transparent/GlassTexturedMobile"
{
Properties 
	{
        _Color ("Main Color", Color) = (0.5, 0.5, 0.5, 0)
        _RefColor ("Reflection Color", Color) = (0.5, 0.5, 0.5, 0)
		_Ref ("Reflecion strenght", Range (0.01, 10)) = 0.7
		_MainTex ("Base Texture", 2D) = "black" {}
        _EnvTex ("EnvMap", 2D) = "black" {}
    }

    SubShader 
    {
//	      Tags { "Queue"="Background" "IgnoreProjector" = "True" "RenderType"="Background"}
		Tags{        
		"Queue"="Transparent" "IgnoreProjector" = "True" "RenderType"="Transparent"
		} 
	      
//	    Pass{
//	    Colormask A
//	    }
        
        Pass{   
//         	Alphatest Greater 0
	        Cull Back
            ZWrite ON

//        	Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending
	        Blend One One                       // Additive
//	        Blend One OneMinusDstColor          // Soft Additive
//          Blend DstColor Zero                 // Multiplicative
//          Blend DstColor SrcColor             // 2x Multiplicative
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"           

            //Declare Properties
            sampler2D _EnvTex;
            sampler2D _MainTex;
            fixed4 _Color,_RefColor;
            fixed _Ref;
            //------------------            

            //Declare structures
            struct v2f
            {
                float4 pos : SV_POSITION;
                half4 color : COLOR0;
                half2 uvSphere : TEXCOORD0;
                half2 uv_MainTex : TEXCOORD1;
            };
            struct appdata_custom 
            {
                float4 vertex : POSITION;
                //float4 tangent : TANGENT;
                half3 normal : NORMAL;
                half4 texcoord : TEXCOORD0;
                half4 color : COLOR;
            };
            //--------------------
        
            half4 _EnvTex_ST;;
            half4 _MainTex_ST;
            
            //vertex shader
            v2f vert (appdata_custom v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                
                //Sphere mapping ported from http://www.ozone3d.net/tutorials/glsl_texturing_p04.php
                half3 u = normalize( mul(UNITY_MATRIX_MV, v.vertex));
                half3 n = mul(UNITY_MATRIX_IT_MV, float4(v.normal.xyz,0));
                half3 r = reflect( u, n);
                
                half m = 2.0 * sqrt( r.x*r.x + r.y*r.y + (r.z+1.0)*(r.z+1.0) );
                o.uvSphere = half2(r.x/m+0.5, r.y/m+0.5);
                //---------------
                                
                o.uv_MainTex = TRANSFORM_TEX (v.texcoord, _MainTex);;
                o.color = v.color;
                return o;
            }
            
            //fragment shader
            fixed4 frag (v2f i) : COLOR 
            {
                fixed4 tex = tex2D(_EnvTex, i.uvSphere)*_RefColor;
                tex *= _RefColor;
                fixed4 MainTex = tex2D(_MainTex, i.uv_MainTex);
                tex.rgb *=_Ref;   //double the color values, but leave alpha untouched
                fixed4 col = MainTex * _Color ;
                col.a = _Color.a + tex;
                //col +=tex;   
                return col + (MainTex * (tex + tex));
            }
            ENDCG        
        }
    } 
}