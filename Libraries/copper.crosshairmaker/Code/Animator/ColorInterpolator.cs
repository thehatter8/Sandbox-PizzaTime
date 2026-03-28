using CrosshairMaker.Helpers;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairMaker.Animator
{
	public enum ColorInterpolationMode : byte
	{
		RGB = 0,
		RGBA = 1,
		HSV = 2,
		HSVA = 3,
	}
	public static class ColorInterpolator
	{
		public static bool HasAlpha(ColorInterpolationMode cim) => (cim & (ColorInterpolationMode)1) != 0;
		public static bool HsvMode( ColorInterpolationMode cim) => (cim & (ColorInterpolationMode)2) != 0;
		
		public static Color Interpolate( Color Start , Color End, float time , ColorInterpolationMode cim = ColorInterpolationMode.HSV )
		{
			if(Start.Equals(End)) return Start;
			Color result = default;
			if ( HasAlpha( cim ) )
			{
				float deltaA = End.a - Start.a;
				deltaA = deltaA * time;
				result.a = deltaA + Start.a;
			}
			else result.a = 1;

			if ( HsvMode(cim) )
			{
				ColorHsv startH = Start.ToHsv(), endH = End.ToHsv();
				Vector3 delta = new Vector3(
					endH.Hue - startH.Hue,
					endH.Saturation - startH.Saturation,
					endH.Value - startH.Value
					);
				delta.x = ((delta.x + 180f) % 360f) - 180f; // normalizes angle
				delta.x *= time;

				delta.y *= time;
				delta.z *= time;

				delta.x += startH.Hue;
				delta.y += startH.Saturation;
				delta.z += startH.Value;

				delta.x = (delta.x + 360) % 360;
				delta.y = Math.Clamp( delta.y, 0f, 1f );
				delta.z = Math.Clamp( delta.z, 0f, 1f );

				ColorHsv hsvRes = new ColorHsv( delta.x, delta.y, delta.z );
				float alpha = result.a; //stores previously calculated alpha
				result = hsvRes.ToColor(); // override result with HSV color
				result.a = alpha; // re sets alpha value
			}
			else
			{
				Vector3 delta = End.ToRgbVector() - Start.ToRgbVector();
				delta *= time;
				delta += Start;
				result.r = delta.x;
				result.g = delta.y;
				result.b = delta.z;
			}
			return result;
		}
	}
	public struct InterpolatableColor
	{
		public InterpolatableColor()
		{
			Color = Color.Black;
			Interp = ColorInterpolationMode.HSV;
		}
		public InterpolatableColor(Color c,ColorInterpolationMode i = ColorInterpolationMode.HSV )
		{
			Color = c;
			Interp = i;
		}
		[KeyProperty]
		[ColorUsage]
		public readonly Color Color;
		[KeyProperty]
		public readonly ColorInterpolationMode Interp;

		public static explicit operator(Color,ColorInterpolationMode) (InterpolatableColor self ) => (self.Color,self.Interp);
		public static implicit operator InterpolatableColor ((Color c, ColorInterpolationMode i) t) => new( t.c,t.i);
		
	}
}
