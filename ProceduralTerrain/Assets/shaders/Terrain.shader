Shader "Custom/NewSurfaceShader"
{
	Properties
	{
		t1("Texture",2D) = "white"{}
		t2("Texture",2D) = "white"{}
		t3("Texture",2D) = "white"{}
		t4("Texture",2D) = "white"{}
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D t1;
		sampler2D t2;
		sampler2D t3;
		sampler2D t4;

        struct Input
        {
			float3 worldPos;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		float inverseLerp(float a, float b, float value) {
			return saturate((value - a) / (b - a));
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			//float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
			if (IN.worldPos.y >= 15) { //snow
				//o.Albedo = float3(1, 1, 1);
				o.Albedo = tex2D(t1, IN.worldPos.xz / 10);
			}
			else if (IN.worldPos.y <= 2) { //water
				//o.Albedo = float3(0, 0, 1);
				o.Albedo = tex2D(t4, IN.worldPos.xz / 10);
			}
			else if (IN.worldPos.y > 3 && IN.worldPos.y < 15) { //trees
				//o.Albedo = float3(0, 1, 0);
				o.Albedo = tex2D(t2, IN.worldPos.xz / 10);
			}
			else { // stone
				//o.Albedo = float3(1, 0, 0);
				o.Albedo = tex2D(t3, IN.worldPos.xz / 10);
			}
        }
        ENDCG
    }
    FallBack "Specular"
}
