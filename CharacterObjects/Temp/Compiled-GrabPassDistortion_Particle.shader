// Compiled shader for PC, Mac & Linux Standalone, uncompressed size: 1.4KB

// Skipping shader variants that would not be included into build of current scene.

Shader "Custom/GrabPass Distortion (Particle)" {
Properties {
 _MainTex ("Texture (R,G=X,Y Distortion; B=Mask; A=Unused)", 2D) = "white" { }
 _IntensityAndScrolling ("Intensity (XY); Scrolling (ZW)", Vector) = (0.100000,0.100000,1.000000,1.000000)
 _DistanceFade ("Distance Fade (X=Near, Y=Far, ZW=Unused)", Vector) = (20.000000,50.000000,0.000000,0.000000)
[Toggle(MASK)]  _MASK ("Texture Blue channel is Mask", Float) = 0.000000
[Toggle(MIRROR_EDGE)]  _MIRROR_EDGE ("Mirror screen borders", Float) = 0.000000
[Toggle(DEBUGUV)]  _DEBUGUV ("Debug Texture Coordinates", Float) = 0.000000
[Toggle(DEBUGDISTANCEFADE)]  _DEBUGDISTANCEFADE ("Debug Distance Fade", Float) = 0.000000
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 GrabPass {
  "_GrabTexture"
 }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  GpuProgramID 31438
Program "vp" {
// Platform opengl had shader errors
//   <no keywords>
//   Keywords { "DEBUGUV" }
// Platform metal had shader errors
//   <no keywords>
//   Keywords { "DEBUGUV" }
// Platform glcore had shader errors
//   <no keywords>
//   Keywords { "DEBUGUV" }
}
Program "fp" {
// Platform opengl skipped due to earlier errors
// Platform metal had shader errors
//   <no keywords>
//   Keywords { "DEBUGUV" }
// Platform glcore skipped due to earlier errors
}
 }
}
}