Shader "Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _Color;
samplerCUBE ambientColor;
float ambIntensity;

struct Input {
	float2 uv_MainTex;
	float3 worldNormal;
	INTERNAL_DATA
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
	o.Normal = half3(0,0,1);
	o.Emission = o.Albedo * texCUBE(ambientColor, WorldNormalVector (IN, o.Normal)) * ambIntensity;
}
ENDCG
}

Fallback "VertexLit"
}
