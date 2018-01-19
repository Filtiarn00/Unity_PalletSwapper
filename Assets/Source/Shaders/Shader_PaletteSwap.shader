Shader "Custome/PaletteSwapArray"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			half4 _Out[256];
			
			struct v2f
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (v2f v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			half4 frag (v2f i) : SV_Target
			{
			   half4 c = tex2D(_MainTex, i.uv);
			   if (c.r == c.b && c.r == c.g && c.a != 0)
			       return _Out[ c.r * 300];	
			return c;
			}
			ENDCG
		}
	}
}