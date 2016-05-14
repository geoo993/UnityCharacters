Shader ".ShaderExample/ColorGradient" {

	Properties {
		_Color1 ("Top Color", Color) = (1,1,1,1)
		_Color2 ("Bottom Color", Color) = (1,1,1,1)
		_Blend ("Blend", Range (0.0, 1.0) ) = 0.0 
	}

	SubShader {

	    Pass {

			Tags { "RenderType"="Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Lambert



	        fixed4 _Color1, _Color2;
	        float _Blend;


	        struct Input 
			{
				float2 uv_MainTex;
				float4 screenPos; 
			};

			void surf (Input IN, inout SurfaceOutput OUT) 
			{
				float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
				//fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * lerp(_Color2,_Color1,screenUV.y);

				//fixed4 c = lerp(_Color2,_Color1,screenUV.y);

				fixed4 mainOutput = _Color2.rgba * (1.0 - (_Color1.a * _Blend));
		      	fixed4 blendOutput = _Color1.rgba * _Color1.a * _Blend;  



				OUT.Albedo =  mainOutput.rgb + blendOutput.rgb;
				OUT.Alpha = mainOutput.a + blendOutput.a;
			}


	        ENDCG
	    }
	}
}