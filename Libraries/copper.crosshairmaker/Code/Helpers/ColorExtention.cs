using Sandbox;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
namespace CrosshairMaker.Helpers
{
	public static class ColorExtention
	{
		public static byte GetR( this Color c ) => (byte)( c.RgbInt >> 16 );
		public static byte GetG( this Color c ) => (byte)( c.RgbInt >> 8 );
		public static byte GetB( this Color c ) => (byte)c.RgbInt;
		public static byte GetA( this Color c ) => (byte)c.RgbaInt;
		
		public static Vector3 ToRgbVector( this Color c ) => new(c.r, c.g, c.b);
		public static Vector4 ToRgbaVector( this Color c ) => new(c.r, c.g, c.b,c.a);
	}

}
