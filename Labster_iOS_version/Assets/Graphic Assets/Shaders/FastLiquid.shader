Shader "ReaShader/Special/FastLiquidUnlit-WorldSpace"
{
Properties 
	{
        _Color ("Main Color", Color) = (0.5, 0.5, 0.5, 0)
		_Em ("Emission Strenght", Range (0, 5)) = 0.7
        _MainTex ("Main Texture", 2D) = "black" {}
        _Size ("Scale", float) = 1.0
        _Spd ("Speed", float) = 1.0
    }

    SubShader 
    {
	      Tags 
	      {        
	       "Queue"="Transparent+1" "IgnoreProjector" = "True" "RenderType"="Transparent"
	      } 
	    Pass{
	    Zwrite ON
	    Colormask A
	    }
        Pass
        {   
//         	Alphatest Greater 0
	        Cull Back
            ZWrite Off
            ZTest LEqual

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
            sampler2D _MainTex;
            half4 _Color;
            half _Em,_Size,_Spd;
            //------------------            

            //Declare structures
            struct v2f
            {
                half4 pos : SV_POSITION;
                half4 color : COLOR0;
                half3 projNormal : TEXCOORD1;
                half3 wpos : TEXCOORD2;
                half3 wnorm : TEXCOORD3;
                

            };
            struct appdata_custom 
            {
                half4 vertex : POSITION;
                //half4 tangent : TANGENT;
                half3 normal : NORMAL;
                half4 texcoord : TEXCOORD0;
                half4 color : COLOR;
            };
            //--------------------
        
            half4 _MainTex_ST;
            //vertex shader
            v2f vert (appdata_custom v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
//				o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
                half3 worldPos = mul(_Object2World, v.vertex).xyz;
				half3 worldNormal = mul(_Object2World, half4(v.normal,0)).xyz;
				o.projNormal = saturate(pow(worldNormal * 5,8));
				 
				o.wpos = worldPos;
				o.wnorm = worldNormal;
				
                o.color = v.color;
                return o;
            }
            
            //fragment shader
            half4 frag (v2f i) : COLOR 
            {
				half2 s = half2(_Size,-_Size);
				half2 panner = _Time * _Spd;
				half2 panZY = (i.wpos.zy + panner);
				half2 panZX = (i.wpos.zx + panner);
				half2 panXY = (i.wpos.xy + panner);
				
                half4 x = tex2D(_MainTex, (panZY * s)) ;
                half4 y = tex2D(_MainTex, (panZX * s)) ;
				half4 z = tex2D(_MainTex, (panXY * s)) ;
				half4 tex = lerp(y,x,i.projNormal.z);
				tex = lerp(tex,z,i.projNormal.x);

                half4 col = _Color;
				col.a = _Color.a + tex.b;
                col.rgb *=tex.rgb;
                return col * _Em + col;
            }
            ENDCG        
        }
    } 
}