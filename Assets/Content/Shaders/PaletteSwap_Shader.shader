Shader "Custome/PaletteSwap"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}



	SubShader
	{
		ZWrite off
        Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

			sampler2D _MainTex;
			half4 _Out[256];
	
			UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(int, _Offset)
            UNITY_INSTANCING_BUFFER_END(Props)

			struct v2f
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			v2f vert (v2f v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			half4 frag (v2f i) : SV_Target
			{
			   UNITY_SETUP_INSTANCE_ID(i);

			   half4 c = tex2D(_MainTex, i.uv);
			   if (c.r == c.b && c.r == c.g && c.a != 0)
			   {
				   int offset = UNITY_ACCESS_INSTANCED_PROP(Props, _Offset);
			       return _Out[c.rgb.r * 255 + offset];	
			   }
			
			return c;
			}
			ENDCG
		}
	}
}