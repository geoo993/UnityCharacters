Shader ".ShaderExample/PostProcessing/Bloom" {
//    Properties {
//        _Contrast ("Contrast", Float) = 1
//        _Brightness ("Brightness", Float) = 1
//        _Bias ("Bias", Float) = 0
//        _BlurRange ("Blur Range", Float) = 0.2
//    }
//    SubShader {
//
//    	//Blend One Zero
//        Blend Zero SrcColor
//        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Opaque"}
//        Lighting Off
//        Cull Off
//        Fog { Mode Off }
//        ZWrite Off
//                       
//        Pass {
//            CGPROGRAM
// 
//            #pragma vertex vert
//            #pragma fragment frag
//            #pragma target 3.0
//            #pragma glsl
//           
//            #include "UnityCG.cginc"
//           
//            #define FORCE_TEX2DLOD
//            #include "../FXLab.cginc"
//
////        Blend One One
////        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Opaque" "FXTextures"="Depth, ScreenBuffer"}
////        Lighting Off
////        Cull Off
////        Fog { Mode Off }
////        ZWrite Off
////                       
////        Pass {
////            CGPROGRAM
////            #pragma vertex vert
////            #pragma fragment frag
////            #pragma target 3.0
////            #include "UnityCG.cginc"
////           
////            //#define SCREENGRAB_BLUR_STEPS 8
////           // #include "../FXLab.cginc"
//           
//            float _Contrast;
//            float _Brightness;
//            float _Bias;
//            float _BlurRange;
// 
// 
//            struct appdata {
//                float4 vertex : POSITION;
//                float4 texcoord : TEXCOORD0;
//            };
// 
// 
//            struct v2f {
//                float4 pos : SV_POSITION;
//                float2 uv : TEXCOORD0;
//                float4 screenPos : TEXCOORD2;
//            };
//                                   
//            v2f vert (appdata v) {
//                v2f o;
//                o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
//                o.uv = v.texcoord.xy;
//                o.screenPos = o.pos;
//                return o;
//            }
//           
//            #define ITERATIONS 3
//                   
//            fixed4 frag( v2f o ) : COLOR
//            {
//                SCREEN_UV(o.screenPos);
//               
//                fixed3 color = 0;
//                for (int i = 0; i < ITERATIONS; ++i)
//                {
//                    color += SAMPLE_BLURRED_SCREEN(0, _BlurRange / ITERATIONS * (i + 1));
//                }
//                color /= ITERATIONS;
//               
//                color = saturate(color - _Bias);
//                color = saturate(color * _Brightness);
//                color = saturate(((color * 0.5 - 0.5) * _Contrast) * 2 + 1);
//               
//                return fixed4(color, 1);
//            }
//            ENDCG
//        }
//    }
//    Fallback off
}
 