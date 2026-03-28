using Sandbox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorInerp = CrosshairMaker.Animator.ColorInterpolationMode;

namespace CrosshairMaker.Animator
{
	
	public sealed class CrosshairAnimation : IEnumerable<IList>
	{
		private CrosshairAnimation() { }
		/// <summary>
		/// target float values for animation
		/// </summary>
		public List<float> Floats;
		/// <summary>
		/// target int values for animation
		/// </summary>
		public List<int> Ints;
		/// <summary>
		/// target colors, with interpolation mode
		/// </summary>
		public List<InterpolatableColor> Colors;
		/// <summary>
		/// Sizes of target properties
		/// </summary>
		public int[] Sizes => new int[] { Floats.Count(), Ints.Count(), Colors.Count() };
		/// <summary>
		/// Creates a copy of the animation
		/// </summary>
		public CrosshairAnimation Copy() => FromData( Floats, Ints, Colors );

		public override string ToString()
		{
			return "[CrosshairMaker.Animator.CrosshairAnimation]";
		}
		/// <summary>
		/// Prints data contained in animation
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public string ToString(string format )
		{
			StringBuilder ret = new();
			ret.Append( ToString() );
			format = format.ToLower();
			string[] formats = format.Split(',');
			bool printAll = formats.Contains( "a" ) || formats.Contains( "all" );
			if ( printAll || formats.Contains( "f" ) || format.Contains( "floats" ) )
			{
				ret.Append( " floats : " );
				ret.Append( string.Join( ",", Floats ) );
				ret.Append( ";" );
			}
			if ( printAll || formats.Contains( "i" ) || format.Contains( "ints" ) )
			{
				ret.Append( " ints : " );
				ret.Append( string.Join( ",", Ints ) );
				ret.Append( ";" );
			}
			if ( printAll || formats.Contains( "c" ) || format.Contains( "colors" ) )
			{
				ret.Append( " colors : " );
				ret.Append( string.Join( ",", Colors) );
				ret.Append( ";" );
			}

			return ret.ToString();

			
		}
		//Static
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public static bool SameSize( int[] one, int[] two )
		{
			if(one.Length != two.Length) return false;
			if(one.Length != 3) return false;
			for ( byte i = 0; i < 3; i++ )
				if ( one[i] != two[i] )
					return false;
			return true;
		}
		public static bool SameSize( CrosshairAnimation one, CrosshairAnimation two ) => SameSize(one.Sizes , two.Sizes);
		public static bool SameSize( int[] one ,CrosshairAnimation two ) => SameSize(one, two.Sizes);
		public static bool SameSize( CrosshairAnimation one, int[] two ) => SameSize(one.Sizes , two);
		/// <summary>
		/// Creates an empty animation
		/// </summary>
		public static CrosshairAnimation Empty
		{
			get
			{
				CrosshairAnimation a = new CrosshairAnimation();
				a.Floats = new();
				a.Ints = new();
				a.Colors = new();
				return a;
			}
		}
		/// <summary>
		/// Creates an CrosshairAnimation from lists of float, int , colors and possibly color interpolation mode
		/// </summary>
		/// <param name="floats">List of float in given animation</param>
		/// <param name="ints">List of ints in given animation</param>
		/// <param name="colors">List of Colors to</param>
		/// <param name="interp">[Optional] Interpolation mode</param>
		/// <returns></returns>
		public static CrosshairAnimation FromData( IEnumerable<float> floats, IEnumerable<int> ints, IEnumerable<Color> colors, ColorInerp interp = ColorInerp.HSV )
		{
			if ( interp == ColorInerp.HSV ) return FromData( floats, ints, colors,null );
			int colorCount = colors.Count();
			List<ColorInerp> lci = new List<ColorInerp>(colorCount);
			for ( int i = 0; i < colorCount; i++ ) lci.Add( interp );
			return FromData(floats, ints, colors, lci);
		}
		/// <summary>
		/// Creates an CrosshairAnimation from lists of float, int , colors and possibly color interpolation modes
		/// </summary>
		/// <param name="floats">List of float in given animation</param>
		/// <param name="ints">List of ints in given animation</param>
		/// <param name="colors">List of Colors in given animation</param>
		/// <param name="interps">[Optional] List of interpolation mode for colors, if this list is shorter than the colors parameter , the HSV interp mode is used by default</param>
		/// <returns></returns>
		public static CrosshairAnimation FromData( IEnumerable<float> floats, IEnumerable<int> ints, IEnumerable<Color> colors, IEnumerable<ColorInerp>? interps = null )
		{
			int i = 0;
			List<InterpolatableColor> selfColors = new();
			if ( interps != null )
			{
				int functionalColors = Math.Min( colors.Count(), interps.Count() );
				for ( ; i < functionalColors; i++ )
				{
					selfColors.Add( (colors.ElementAt( i ), interps.ElementAt( i )) );
				}
			}
			for ( ; i < colors.Count(); i++ )
			{
				selfColors.Add( (colors.ElementAt( i ), ColorInerp.HSV) );
			}
			return FromData( floats, ints, selfColors );
		}
		/// <summary>
		/// Creates an CrosshairAnimation from lists of float, int , colors and possibly color interpolation modes
		/// </summary>
		/// <param name="floats">List of float in given animation</param>
		/// <param name="ints">List of ints in given animation</param>
		/// <param name="colors">List of InterpolatableColor in the animation</param>
		/// <returns></returns>
		public static CrosshairAnimation FromData( IEnumerable<float> floats, IEnumerable<int> ints, IEnumerable<InterpolatableColor> colors )
		{
			CrosshairAnimation a = new();
			a.Floats = floats.ToList();
			a.Ints = ints.ToList();
			a.Colors = colors.ToList();
			return a;
		}
		/// <summary>
		/// Create CrosshairAnimation from the size of float properties, int properties and interpolableColor properties, default values can be specified
		/// </summary>
		/// <param name="floatCount">number of float properties</param>
		/// <param name="intCount">number of int properties</param>
		/// <param name="colorCount">number of interpolableColor properties</param>
		/// <param name="defaultF">[Optional] default value of float properties</param>
		/// <param name="defaultI">[Optional] default value of int properties</param>
		/// <param name="defaultC">[Optional] default value of interpolableColor properties</param>
		/// <returns></returns>
		public static CrosshairAnimation FromPropertyCount( int floatCount, int intCount, int colorCount, float defaultF = 0f, int defaultI = 0, InterpolatableColor? defaultC = null ) => FromPropertyCount( new int[]{floatCount, intCount, colorCount}, defaultF, defaultI, defaultC );
		/// <summary>
		/// Create CrosshairAnimation from the size of float properties, int properties and interpolableColor properties, default values can be specified
		/// </summary>
		/// <param name="propCount">Array containing the count of each properties like so : [float,int,InterpolatableColor]</param>
		/// <param name="defaultF">[Optional] default value of float properties</param>
		/// <param name="defaultI">[Optional] default value of int properties</param>
		/// <param name="defaultC">[Optional] default value of interpolableColor properties</param>
		/// <returns></returns>
		public static CrosshairAnimation FromPropertyCount( int[] propCount , float defaultF = 0f, int defaultI = 0 , InterpolatableColor? defaultC = null )
		{
			if ( propCount.Length != 3 ) throw new ArgumentException( "array length must be 3" );
			InterpolatableColor _defaultC = defaultC ?? new InterpolatableColor();

			CrosshairAnimation a = Empty;
			for(int i = 0; i < propCount[0]; i++ )
				a.Floats.Add( defaultF );
			for(int i = 0; i < propCount[1]; i++ )
				a.Ints.Add( defaultI );
			for(int i = 0; i < propCount[2]; i++ )
				a.Colors.Add( _defaultC);
			return a;
		}
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Static

		//IEnumerable
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//

		public IList this[int idx]
		{
			get
			{
				switch ( idx )
				{
					case 0: return Floats;
					case 1: return Ints;
					case 2: return Colors;
					default: throw new IndexOutOfRangeException();
				}
			}
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public IEnumerator<IList> GetEnumerator()
		{
			yield return Floats;
			yield return Ints;
			yield return Colors;
		}

		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//IEnumerable

	}
}
