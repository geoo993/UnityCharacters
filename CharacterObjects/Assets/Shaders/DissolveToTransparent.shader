// clips materials, using an image as guidance.
// use clouds or random noise as the slice guide for best results.
 Shader ".ShaderExample/DissolveToTransparent" 
 {

  		Properties {

	     	_MainTex ("Texture (RGB)", 2D) = "white" {}
	     	_DissolveTex ("Dissolve Text (RGB)", 2D) = "white" {}
	      	_DissolveAmount ("Dissolve Amount", Range(0.0, 1.0)) = 0.5

	      	_BurnSize ("Burn Size", Range(0.0, 1.0)) = 0.15
 			_BurnRamp ("Burn Ramp (RGB)", 2D) = "white" {}
    	}
    	SubShader {
	     	Tags { "RenderType" = "Opaque" }
	      	Cull Off
	      	CGPROGRAM
	      	//if you're not planning on using shadows, remove "addshadow" for better performance
	     	 #pragma surface surf Lambert addshadow

	     	struct Input {
	          	float2 uv_MainTex;
	          	float2 uv_DissolveTex;
	          	float _DissolveAmount;
	     	};

	     	sampler2D _MainTex, _DissolveTex, _BurnRamp;
	      	float _DissolveAmount, _Shininess, _BurnSize;
	      	float4 _Color;

		     void surf (Input IN, inout SurfaceOutput o) {
		         clip(tex2D (_DissolveTex, IN.uv_DissolveTex).rgb - _DissolveAmount);
		         o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
			 
				 half test = tex2D (_DissolveTex, IN.uv_MainTex).rgb - _DissolveAmount;

				 if(test < _BurnSize && _DissolveAmount > 0 && _DissolveAmount < 1)
				 {
				    o.Emission = tex2D(_BurnRamp, float2(test *(1/_BurnSize), 0));
					o.Albedo *= o.Emission;
				 }
			}
//	      	void surf (Input IN, inout SurfaceOutput o) 
//	      	{
//
//			    half4 tex = tex2D(_MainTex, IN.uv_MainTex);
//			    half4 texd = tex2D(_DissolveTex, IN.uv_DissolveTex);
//			    o.Albedo = tex.rgb * _Color.rgb;
//			    o.Gloss = tex.a;
//			    o.Alpha = _Color.a - texd.r;
//
//			    if ((o.Alpha < 0.0) (o.Alpha > - 0.1) (_Color.a > 0.0))
//			    {
//			        o.Alpha = 1;
//			        o.Albedo = float3(1,0,0);
//			    }
//
//
//			    o.Specular = _Shininess;
//			}

	      ENDCG
	} 
	Fallback "Diffuse"
 }