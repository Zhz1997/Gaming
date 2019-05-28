
Shader "Unlit/shader"
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
		Tags { "RenderType" = "Opaque"
		"LightMode" = "ForwardBase" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;

			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float height : TEXCOORD1;
				float3 normal : TEXCOORD2;
				float4 worldpos : TEXCOORD3;
				float4 vertex : SV_POSITION;

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D t1;
			float4 t1_ST;
			sampler2D t2;
			float4 t2_ST;
			sampler2D t3;
			float4 t3_ST;
			sampler2D t4;
			float4 t4_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.height = v.vertex.y;
				o.uv = TRANSFORM_TEX(v.uv, t1);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.worldpos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float3 normalDirection = normalize(i.normal);
				float3 viewDirection = normalize(_WorldSpaceCameraPos - i.worldpos.xyz);
				float3 vert2LightSource = _WorldSpaceLightPos0.xyz - i.worldpos.xyz;
				float oneOverDistance = 1.0 / length(vert2LightSource);
				float3 lightDirection = _WorldSpaceLightPos0.xyz - i.worldpos.xyz * _WorldSpaceLightPos0.w;


				float4 fragment_color = float4(0,0,0,0);
				float s = 10;
				float3 spec = float3(0.5, 0.5, 0.5);
				if (i.height >= 15) fragment_color = tex2D(t1,i.worldpos.xz/10).rgba; //snow
				else if (i.height > 3 && i.height < 15) fragment_color = tex2D(t2, i.worldpos.xz / 10).rgba; //trees
				else if (i.height > 1 && i.height <= 3) fragment_color = tex2D(t3, i.worldpos.xz / 10).rgba; //stone
				else fragment_color = tex2D(t4, i.worldpos.xz / 10).rgba; //water

				float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * fragment_color; //Ambient

				float3 diffuseReflection = _LightColor0.rgb * fragment_color * max(0.0, dot(normalDirection, lightDirection)); //Diffuse

				float3 specularReflection;
				if (dot(i.normal, lightDirection) < 0.0) //Light on other side
				{
					specularReflection = float3(0.0, 0.0, 0.0);
				}
				else
				{
					//Specular
					specularReflection = _LightColor0.rgb * spec * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), s);
				}

				float3 color = ambientLighting + diffuseReflection + specularReflection; //Texture is not applient on specularReflection
				return float4(color, 1.0);
			}
			ENDCG
		}
	}
}

