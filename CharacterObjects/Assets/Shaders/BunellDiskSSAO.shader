Shader ".ShaderExample/Bunell Disk SSAO" {
//	 Properties {
//	_FXDepthTexture ("Depth Texture (FXDepthTexture)", 2D) = "" {}
//	_FXWorldNormalTexture ("World Normal Texture (FXWorldNormalTexture)", 2D) = "" {}
//	_noiseTex ("NoiseTex", 2D) = ""{}
//	}
//	SubShader {
//	//Blend One Zero
//	Blend Zero SrcColor
//	Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Opaque"}
//	Lighting Off
//	Cull Off
//	Fog { Mode Off }
//	ZWrite Off
//	 
//	Pass {
//	CGPROGRAM
//	 
//	#pragma vertex vert
//	#pragma fragment frag 
//	#pragma target 3.0
//	#pragma glsl
//	 
//	#include "UnityCG.cginc"
//	 
//	#define FORCE_TEX2DLOD
//	#include "../FXLab.cginc"
//	 
//	float _Bias;
//	sampler2D noiseTex;
//	float4 _ProjInfo;
//	float4x4 _InvProj;
//	 
//	struct appdata {
//	float4 vertex : POSITION;
//	float4 texcoord : TEXCOORD0;
//	float3 normal : NORMAL;
//	};
//	 
//	struct v2f {
//	float4 pos : SV_POSITION;
//	float2 uv : TEXCOORD0;
//	 
//	};
//	 
//	v2f vert (appdata v) {
//	v2f o;
//	o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
//	o.uv = v.texcoord.xy;
//	 
//	return o;
//	}
//	 
//	float3 posFromDepth(float2 coord)
//	{
//	float Depth=sampleDepth(coord); //sampleDepth return the depth value (0...1 range)
//	float4 Pos=float4((coord.x-0.5)*2,(0.5-coord.y)*2,1,1);
//	float4 Ray=mul(Pos,_InvProjMatrix);
//	return Ray.xyz*Depth;
//	}
//	 
//	 
//	float aoFF(in half3 ddiff, in half3 cnorm, in float c1, in float c2, in float2 uv)
//	{
//	half3 vv = normalize(ddiff);
//	float rd = length(ddiff);
//	return (1.0-clamp(dot(sampleWorldNormal(uv+half2(c1,c2)),-vv),0.0,1.0)) *
//	            clamp(dot( cnorm,vv ),0.0,1.0)* 
//	             (1.0 - 1.0/sqrt(1.0/(rd*rd) + 1.0));
//	}
//	 
//	 
//	fixed4 frag( v2f o ) : COLOR
//	{
//	float dep = sampleFloat(_FXDepthTexture, o.uv);
//	half3 n = sampleWorldNormal(o.uv);
//	half3 p = posFromDepth(o.uv);
//	 
//	half2 fres = half2(_ScreenParams.x/64.0*5,_ScreenParams.y/64.0*5);
//	half3 random = tex2D(noiseTex,o.uv*fres.xy);
//	random = random*2.0-half3(1.0);
//	 
//	float ao = 0.0;
//	float incx = 1/_ScreenParams.x*0.1;
//	float incy = 1.0/_ScreenParams.y*0.1;
//	float pw = incx;
//	float ph = incy;
//	float cdepth = sampleDepth(o.uv);
//	for(float i=0.0; i<3.0; ++i) 
//	    {
//	        float npw = (pw+0.0007)/cdepth;
//	        float nph = (ph+0.0007)/cdepth;
//	 
//	        half3 ddiff = posFromDepth(o.uv+half2(npw,nph))-p;
//	        half3 ddiff2 = posFromDepth(o.uv+half2(npw,-nph))-p;
//	        half3 ddiff3 = posFromDepth(o.uv+half2(-npw,nph))-p;
//	        half3 ddiff4 = posFromDepth(o.uv+half2(-npw,-nph))-p;
//	        half3 ddiff5 = posFromDepth(o.uv+half2(0,nph))-p;
//	        half3 ddiff6 = posFromDepth(o.uv+half2(0,-nph))-p;
//	        half3 ddiff7 = posFromDepth(o.uv+half2(npw,0))-p;
//	        half3 ddiff8 = posFromDepth(o.uv+half2(-npw,0))-p;
//	 
//	        ao+=  aoFF(ddiff,n,npw,nph,o.uv);
//	        ao+=  aoFF(ddiff2,n,npw,-nph,o.uv);
//	        ao+=  aoFF(ddiff3,n,-npw,nph,o.uv);
//	        ao+=  aoFF(ddiff4,n,-npw,-nph,o.uv);
//	        ao+=  aoFF(ddiff5,n,0,nph,o.uv);
//	        ao+=  aoFF(ddiff6,n,0,-nph,o.uv);
//	        ao+=  aoFF(ddiff7,n,npw,0,o.uv);
//	        ao+=  aoFF(ddiff8,n,-npw,0,o.uv);
//	 
//	        //increase sampling area:
//	        pw += incx;  
//	        ph += incy;    
//	    } 
//	    ao/=24.0;
//	 
//	fixed3 color = saturate(1- ao);
//	 
//	return fixed4(color, 1);
//	}
//	ENDCG
//	}
//	}
//	 
//	    Fallback off
//	CustomEditor "FXMaterialEditor"
}