Shader ".ShaderExample/SSAO2" {
//    Properties {
//        _FXDepthTexture ("Depth Texture (FXDepthTexture)", 2D) = "" {}
////      _FXWorldNormalTexture ("WorldNormal Texture (FXWorldNormalTexture)",2D) = "" {}
//        _SampleRange ("Sample Range", Range(0, 10)) = 3
//        _Area ("Area", Range(0, 20)) = 8
//        _Falloff ("Falloff", Float) = 0.87
//        _Strength ("Strength", Range(0, 10)) = 1
//        _Bias ("Bias", Range(0, 1)) = 0.0
//        _NoiseTex ("Noise Texture", 2D) = "gray" {}
//        _NoiseScale ("NoiseScale",Range (0,10)) = 3
//        _Near ("Min Z",Float) = 0.1
//        _Far ("Max Z",Float) = 1000
//    }
//    SubShader {
//        //Blend One Zero
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
//            float _SampleRange;
//            float _Area;
//            float _Falloff;
//            float _Strength;
//            float _NoiseScale;
//            float _Bias;
//            sampler2D _NoiseTex;
//            float _Near;
//            float _Far;
// 
//            struct appdata {
//                float4 vertex : POSITION;
//                float4 texcoord : TEXCOORD0;
//            };
// 
//            struct v2f {
//                float4 pos : SV_POSITION;
//                float2 uv : TEXCOORD0;
//                float2 uvr : TEXCOORD1;
//            };
//                                   
//            v2f vert (appdata v) {
//                v2f o;
//                o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
//                o.uv = v.texcoord.xy;
//                o.uvr = v.texcoord.xy * _NoiseScale;
//                return o;
//            }
//           
//           
//            fixed3 calculateNormal(float depth, float2 texcoords)
//            {
//                depth *= _ProjectionParams.z;
// 
//                float2 offset1 = float2(0.0,_FXDepthTexture_TexelSize.y);
//                float2 offset2 = float2(_FXDepthTexture_TexelSize.x,0.0);
// 
//                float depth1 = sampleFloat(_FXDepthTexture, texcoords + offset1) * _ProjectionParams.z;
//                float depth2 = sampleFloat(_FXDepthTexture, texcoords + offset2) * _ProjectionParams.z;
// 
//                float3 p1 = float3(offset1, depth1 - depth);
//                float3 p2 = float3(offset2, depth2 - depth);
// 
//                float3 normal = cross(p1, p2);
//                normal.z = -normal.z;
// 
//                return normalize(normal);
//            }
//           
//                   
//        float4 frag (v2f o) : COLOR
//        {
//        const float3 RAND_SAMPLES[26] = {
//        float3(0.2196607,0.9032637,0.2254677),
//        float3(0.05916681,0.2201506,-0.1430302),
//        float3(-0.4152246,0.1320857,0.7036734),
//        float3(-0.3790807,0.1454145,0.100605),
//        float3(0.3149606,-0.1294581,0.7044517),
//        float3(-0.1108412,0.2162839,0.1336278),
//        float3(0.658012,-0.4395972,-0.2919373),
//        float3(0.5377914,0.3112189,0.426864),
//        float3(-0.2752537,0.07625949,-0.1273409),
//        float3(-0.1915639,-0.4973421,-0.3129629),
//        float3(-0.2634767,0.5277923,-0.1107446),
//        float3(0.8242752,0.02434147,0.06049098),
//        float3(0.06262707,-0.2128643,-0.03671562),
//        float3(-0.1795662,-0.3543862,0.07924347),
//        float3(0.06039629,0.24629,0.4501176),
//        float3(-0.7786345,-0.3814852,-0.2391262),
//        float3(0.2792919,0.2487278,-0.05185341),
//        float3(0.1841383,0.1696993,-0.8936281),
//        float3(-0.3479781,0.4725766,-0.719685),
//        float3(-0.1365018,-0.2513416,0.470937),
//        float3(0.1280388,-0.563242,0.3419276),
//        float3(-0.4800232,-0.1899473,0.2398808),
//        float3(0.6389147,0.1191014,-0.5271206),
//        float3(0.1932822,-0.3692099,-0.6060588),
//        float3(-0.3465451,-0.1654651,-0.6746758),
//        float3(0.2448421,-0.1610962,0.1289366),
//        };
//        float4 _Params = float4(_SampleRange,_Near,_Falloff,_Strength); // x=radius, y=minz, z=attenuation power, w=SSAO power
//               
//        // read random normal from noise texture
//        half3 randN = tex2D (_NoiseTex, o.uvr).xyz * 2.0 - 1.0;    
//   
//        // read scene depth/normal
//        float4 depthnormal = tex2D(_FXWorldNormalTexture, o.uv);
//        float depth = sampleFloat(_FXDepthTexture, o.uv);
//        float3 viewNorm = calculateNormal(depth, o.uv);
//        depth *= _ProjectionParams.z;
//        float scale = _Params.x / depth;
//   
//        // accumulated occlusion factor
//        float occ = 0.0;
//        for (int s = 0; s < 26; ++s)
//        {
//        // Reflect sample direction around a random vector
//        half3 randomDir = reflect(RAND_SAMPLES[s], randN);
//       
//        // Make it point to the upper hemisphere
//        half flip = (dot(viewNorm,randomDir)<0) ? 1.0 : -1.0;
//        randomDir *= -flip;
//        // Add a bit of normal to reduce self shadowing
//        randomDir += viewNorm * 0.4;
//       
//        float2 offset = randomDir.xy * scale;
//        float sD = depth - (randomDir.z * _Params.x);
// 
//        // Sample depth at offset location
////        float4 sampleND = tex2D (_FXDepthTexture, o.uv + offset);
//        float sampleD = sampleFloat(_FXDepthTexture, o.uv + offset);
//        float3 sampleN = calculateNormal(sampleD, o.uv + offset);
////      DecodeDepthNormal (sampleND, sampleD, sampleN);
//        sampleD *= _ProjectionParams.z;
//        float zd = saturate(sD-sampleD);
//        if (zd > _Params.y&zd < _Far) {
//            // This sample occludes, contribute to occlusion
//            occ += pow(1-zd,_Params.z); // sc2
//            //occ += 1.0-saturate(pow(1.0 - zd, 11.0) + zd); // nullsq
//            //occ += 1.0/(1.0+zd*zd*10); // iq
//        }        
//        }
//        occ /= 26;         
//        fixed3 color = saturate(1-occ);
//        color = pow (color, _Params.w);  
//        return (depth > _Far) ? 1f : fixed4(color, 1); 
//        }
//            ENDCG
//        }
//        }
//       
//    Fallback off
//    CustomEditor "FXMaterialEditor"

}