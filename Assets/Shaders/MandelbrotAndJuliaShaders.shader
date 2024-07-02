Shader "Unlit/MandelbrotAndJuliaShaders"
{
	Properties
	{
		_Area ("Centre of Area", Vector) = (0., 0., 0., 0.) // the complex number that centreted in centre of screen
	
		_Zoom ("Zoom", float) = 1. // zoom of complex plane
	
		_Iter ("Iterations", float) = 255. // how many iteretaion
	
		_RGBA ("RGBA Channels", Vector) = (1., 1., 1., 1.) // rgba channels for coloring fractals
	
		_Aspect ("Aspect Ratio", float) = 1. // aspect ratio for mapping the uv to screen
	
		[Toggle] _Enable ("Enable ?", Float) = 1. // enable/disable = mandelbrot/julia
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		
		Pass
	        {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _ENABLE_ON
	
			#include "UnityCG.cginc"
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			
			float _Zoom, _Iter, _Aspect;
			float4 _Area, _RGBA;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float2 square(float2 z) // square of complex number
			{
				return float2(z.x*z.x-z.y*z.y, 2*z.x*z.y);
			}
			
			float mandelbrot_iter(float2 _c) // mandelbrot iteretaion of the complex number _c
			{
				float2 z = 0.;
				int i = 0;
				for (i = 0; i < _Iter; i++)
				{
					z = _c + square(z);
					if (length(z) > 2)	break;
				}
			
				return (float)i / _Iter;
			}
			
			float julia_iter(float2 _z, float2 _c) // julia iteretaion of the complex number _z and the _c is the constant complex number in the equation
			{
				int i = 0;
				for (i = 0; i < _Iter; i++)
				{
					_z = _c + square(_z);
					if (length(_z) > 2)	break;
				}
			
				return (float)i / _Iter;
			}
			
			
			float4 frag (v2f i) : SV_Target
			{
				float2 uv = (i.uv -.5) *_Zoom * float2(_Aspect, 1.) + _Area.xy;
		        	
				//col is the value of how many itteration divided by max iterations
				#if _ENABLE_ON // the material that has this toggle enabled will draw mandelbrot fractal
					float col = mandelbrot_iter(uv);
				#else // the material that has this toggle disabled will draw julia fractal
					float col = julia_iter(uv, _Area.xy);
				#endif
		        	
		        	return float4(sin(_RGBA.x*col)*.5+.5, sin(_RGBA.y*col)*.5+.5, sin(_RGBA.z*col)*.5+.5, 1.);
			}
			ENDCG
		}
	}
}
