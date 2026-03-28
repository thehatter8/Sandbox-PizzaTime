using Sandbox;
using System;
using System.Collections.Generic;

namespace CrosshairMaker.Animator
{
	public sealed partial class CrosshairAnimator
	{
		//floats
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		// Float 1
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float1 ), float.NaN )]
		public string Float1Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 0 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float1 ), float.NaN )]
		public float float1
		{
			get => _ExtractFloat( 0 );
			set => _InjectFloat( 0, value );
		}

		// Float 2
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float2 ), float.NaN )]
		public string Float2Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 1 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float2 ), float.NaN )]
		public float float2
		{
			get => _ExtractFloat( 1 );
			set => _InjectFloat( 1, value );
		}

		// Float 3
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float3 ), float.NaN )]
		public string Float3Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 2 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float3 ), float.NaN )]
		public float float3
		{
			get => _ExtractFloat( 2 );
			set => _InjectFloat( 2, value );
		}

		// Float 4
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float4 ), float.NaN )]
		public string Float4Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 3 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float4 ), float.NaN )]
		public float float4
		{
			get => _ExtractFloat( 3 );
			set => _InjectFloat( 3, value );
		}

		// Float 5
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float5 ), float.NaN )]
		public string Float5Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 4 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float5 ), float.NaN )]
		public float float5
		{
			get => _ExtractFloat( 4 );
			set => _InjectFloat( 4, value );
		}

		// Float 6
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float6 ), float.NaN )]
		public string Float6Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 5 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float6 ), float.NaN )]
		public float float6
		{
			get => _ExtractFloat( 5 );
			set => _InjectFloat( 5, value );
		}

		// Float 7
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float7 ), float.NaN )]
		public string Float7Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 6 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float7 ), float.NaN )]
		public float float7
		{
			get => _ExtractFloat( 6 );
			set => _InjectFloat( 6, value );
		}

		// Float 8
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float8 ), float.NaN )]
		public string Float8Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 7 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float8 ), float.NaN )]
		public float float8
		{
			get => _ExtractFloat( 7 );
			set => _InjectFloat( 7, value );
		}

		// Float 9
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float9 ), float.NaN )]
		public string Float9Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 8 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float9 ), float.NaN )]
		public float float9
		{
			get => _ExtractFloat( 8 );
			set => _InjectFloat( 8, value );
		}

		// Float 10
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float10 ), float.NaN )]
		public string Float10Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 9 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float10 ), float.NaN )]
		public float float10
		{
			get => _ExtractFloat( 9 );
			set => _InjectFloat( 9, value );
		}

		// Float 11
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float11 ), float.NaN )]
		public string Float11Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 10 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float11 ), float.NaN )]
		public float float11
		{
			get => _ExtractFloat( 10 );
			set => _InjectFloat( 10, value );
		}

		// Float 12
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float12 ), float.NaN )]
		public string Float12Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 11 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float12 ), float.NaN )]
		public float float12
		{
			get => _ExtractFloat( 11 );
			set => _InjectFloat( 11, value );
		}

		// Float 13
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float13 ), float.NaN )]
		public string Float13Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 12 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float13 ), float.NaN )]
		public float float13
		{
			get => _ExtractFloat( 12 );
			set => _InjectFloat( 12, value );
		}

		// Float 14
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float14 ), float.NaN )]
		public string Float14Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 13 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float14 ), float.NaN )]
		public float float14
		{
			get => _ExtractFloat( 13 );
			set => _InjectFloat( 13, value );
		}

		// Float 15
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float15 ), float.NaN )]
		public string Float15Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 14 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float15 ), float.NaN )]
		public float float15
		{
			get => _ExtractFloat( 14 );
			set => _InjectFloat( 14, value );
		}

		// Float 16
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( float16 ), float.NaN )]
		public string Float16Property => _GetStringProperty( AnimationData.FloatsPropertyNames, 15 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( float16 ), float.NaN )]
		public float float16
		{
			get => _ExtractFloat( 15 );
			set => _InjectFloat( 15, value );
		}

		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//floats

		//ints
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int1 ), int.MinValue )]
		public string int1Property => _GetStringProperty( AnimationData.IntsPropertyNames, 0 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int1 ), int.MinValue )]
		public int int1
		{
			get => _ExtractInt( 0 );
			set => _InjectInt( 0, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int2 ), int.MinValue )]
		public string int2Property => _GetStringProperty( AnimationData.IntsPropertyNames, 1 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int2 ), int.MinValue )]
		public int int2
		{
			get => _ExtractInt( 1 );
			set => _InjectInt( 1, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int3 ), int.MinValue )]
		public string int3Property => _GetStringProperty( AnimationData.IntsPropertyNames, 2 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int3 ), int.MinValue )]
		public int int3
		{
			get => _ExtractInt( 2 );
			set => _InjectInt( 2, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int4 ), int.MinValue )]
		public string int4Property => _GetStringProperty( AnimationData.IntsPropertyNames, 3 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int4 ), int.MinValue )]
		public int int4
		{
			get => _ExtractInt( 3 );
			set => _InjectInt( 3, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int5 ), int.MinValue )]
		public string int5Property => _GetStringProperty( AnimationData.IntsPropertyNames, 4 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int5 ), int.MinValue )]
		public int int5
		{
			get => _ExtractInt( 4 );
			set => _InjectInt( 4, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int6 ), int.MinValue )]
		public string int6Property => _GetStringProperty( AnimationData.IntsPropertyNames, 5 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int6 ), int.MinValue )]
		public int int6
		{
			get => _ExtractInt( 5 );
			set => _InjectInt( 5, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int7 ), int.MinValue )]
		public string int7Property => _GetStringProperty( AnimationData.IntsPropertyNames, 6 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int7 ), int.MinValue )]
		public int int7
		{
			get => _ExtractInt( 6 );
			set => _InjectInt( 6, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int8 ), int.MinValue )]
		public string int8Property => _GetStringProperty( AnimationData.IntsPropertyNames, 7 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int8 ), int.MinValue )]
		public int int8
		{
			get => _ExtractInt( 7 );
			set => _InjectInt( 7, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int9 ), int.MinValue )]
		public string int9Property => _GetStringProperty( AnimationData.IntsPropertyNames, 8 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int9 ), int.MinValue )]
		public int int9
		{
			get => _ExtractInt( 8 );
			set => _InjectInt( 8, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int10 ), int.MinValue )]
		public string int10Property => _GetStringProperty( AnimationData.IntsPropertyNames, 9 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int10 ), int.MinValue )]
		public int int10
		{
			get => _ExtractInt( 9 );
			set => _InjectInt( 9, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int11 ), int.MinValue )]
		public string int11Property => _GetStringProperty( AnimationData.IntsPropertyNames, 10 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int11 ), int.MinValue )]
		public int int11
		{
			get => _ExtractInt( 10 );
			set => _InjectInt( 10, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int12 ), int.MinValue )]
		public string int12Property => _GetStringProperty( AnimationData.IntsPropertyNames, 11 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int12 ), int.MinValue )]
		public int int12
		{
			get => _ExtractInt( 11 );
			set => _InjectInt( 11, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int13 ), int.MinValue )]
		public string int13Property => _GetStringProperty( AnimationData.IntsPropertyNames, 12 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int13 ), int.MinValue )]
		public int int13
		{
			get => _ExtractInt( 12 );
			set => _InjectInt( 12, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int14 ), int.MinValue )]
		public string int14Property => _GetStringProperty( AnimationData.IntsPropertyNames, 13 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int14 ), int.MinValue )]
		public int int14
		{
			get => _ExtractInt( 13 );
			set => _InjectInt( 13, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int15 ), int.MinValue )]
		public string int15Property => _GetStringProperty( AnimationData.IntsPropertyNames, 14 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int15 ), int.MinValue )]
		public int int15
		{
			get => _ExtractInt( 14 );
			set => _InjectInt( 14, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( int16 ), int.MinValue )]
		public string int16Property => _GetStringProperty( AnimationData.IntsPropertyNames, 15 );

		[Property, Group( GroupProperties )]
		[Range( -64, 64 )]
		[HideIf( nameof( int16 ), int.MinValue )]
		public int int16
		{
			get => _ExtractInt( 15 );
			set => _InjectInt( 15, value );
		}

		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//ints

		//colors
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		private static readonly InterpolatableColor _ErrColor = new InterpolatableColor( Color.Black, ColorInterpolationMode.RGB );
		private bool c1Null => _ExtractColor( 0 ) == null;
		private bool c2Null => _ExtractColor( 1 ) == null;
		private bool c3Null => _ExtractColor( 2 ) == null;
		private bool c4Null => _ExtractColor( 3 ) == null;
		private bool c5Null => _ExtractColor( 4 ) == null;
		private bool c6Null => _ExtractColor( 5 ) == null;
		private bool c7Null => _ExtractColor( 6 ) == null;
		private bool c8Null => _ExtractColor( 7 ) == null;
		private bool c9Null => _ExtractColor( 8 ) == null;
		private bool c10Null => _ExtractColor( 9 ) == null;
		private bool c11Null => _ExtractColor( 10 ) == null;
		private bool c12Null => _ExtractColor( 11 ) == null;
		private bool c13Null => _ExtractColor( 12 ) == null;
		private bool c14Null => _ExtractColor( 13 ) == null;
		private bool c15Null => _ExtractColor( 14 ) == null;
		private bool c16Null => _ExtractColor( 15 ) == null;
		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c1Null ), true )]
		public string color1Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 0 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c1Null ), true )]
		public InterpolatableColor color1
		{
			get => _ExtractColor( 0 ) ?? _ErrColor;
			set => _InjectColor( 0, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c2Null ), true )]
		public string color2Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 1 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c2Null ), true )]
		public InterpolatableColor color2
		{
			get => _ExtractColor( 1 ) ?? _ErrColor;
			set => _InjectColor( 1, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c3Null ), true )]
		public string color3Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 2 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c3Null ), true )]
		public InterpolatableColor color3
		{
			get => _ExtractColor( 2 ) ?? _ErrColor;
			set => _InjectColor( 2, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c4Null ), true )]
		public string color4Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 3 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c4Null ), true )]
		public InterpolatableColor color4
		{
			get => _ExtractColor( 3 ) ?? _ErrColor;
			set => _InjectColor( 3, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c5Null ), true )]
		public string color5Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 4 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c5Null ), true )]
		public InterpolatableColor color5
		{
			get => _ExtractColor( 4 ) ?? _ErrColor;
			set => _InjectColor( 4, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c6Null ), true )]
		public string color6Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 5 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c6Null ), true )]
		public InterpolatableColor color6
		{
			get => _ExtractColor( 5 ) ?? _ErrColor;
			set => _InjectColor( 5, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c7Null ), true )]
		public string color7Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 6 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c7Null ), true )]
		public InterpolatableColor color7
		{
			get => _ExtractColor( 6 ) ?? _ErrColor;
			set => _InjectColor( 6, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c8Null ), true )]
		public string color8Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 7 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c8Null ), true )]
		public InterpolatableColor color8
		{
			get => _ExtractColor( 7 ) ?? _ErrColor;
			set => _InjectColor( 7, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c9Null ), true )]
		public string color9Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 8 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c9Null ), true )]
		public InterpolatableColor color9
		{
			get => _ExtractColor( 8 ) ?? _ErrColor;
			set => _InjectColor( 8, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c10Null ), true )]
		public string color10Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 9 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c10Null ), true )]
		public InterpolatableColor color10
		{
			get => _ExtractColor( 9 ) ?? _ErrColor;
			set => _InjectColor( 9, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c11Null ), true )]
		public string color11Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 10 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c11Null ), true )]
		public InterpolatableColor color11
		{
			get => _ExtractColor( 10 ) ?? _ErrColor;
			set => _InjectColor( 10, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c12Null ), true )]
		public string color12Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 11 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c12Null ), true )]
		public InterpolatableColor color12
		{
			get => _ExtractColor( 11 ) ?? _ErrColor;
			set => _InjectColor( 11, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c13Null ), true )]
		public string color13Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 12 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c13Null ), true )]
		public InterpolatableColor color13
		{
			get => _ExtractColor( 12 ) ?? _ErrColor;
			set => _InjectColor( 12, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c14Null ), true )]
		public string color14Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 13 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c14Null ), true )]
		public InterpolatableColor color14
		{
			get => _ExtractColor( 13 ) ?? _ErrColor;
			set => _InjectColor( 13, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c15Null ), true )]
		public string color15Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 14 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c15Null ), true )]
		public InterpolatableColor color15
		{
			get => _ExtractColor( 14 ) ?? _ErrColor;
			set => _InjectColor( 14, value );
		}

		[Property, Group( GroupProperties )]
		[ReadOnly]
		[HideIf( nameof( c16Null ), true )]
		public string color16Property => _GetStringProperty( AnimationData.ColorsPropertyNames, 15 );
		[Property, Group( GroupProperties )]
		[HideIf( nameof( c16Null ), true )]
		public InterpolatableColor color16
		{
			get => _ExtractColor( 15 ) ?? _ErrColor;
			set => _InjectColor( 15, value );
		}

		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//colors

		//Properties Injector/Extractor
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		private void _InjectFloat( int idx, float value )
		{
			if ( AnimationData == null ) return;
			if ( !AnimationData.TryGetValue( AnimationIndex, out CrosshairAnimation data ) ) return;
			if ( data.Floats.Count <= idx ) return;
			data.Floats[idx] = value;
		}
		private float _ExtractFloat( int idx )
		{
			if ( AnimationData == null ) return float.NaN;
			if ( !AnimationData.TryGetValue( AnimationIndex, out CrosshairAnimation data ) ) return float.NaN;
			if ( data.Floats.Count <= idx ) return float.NaN;
			return data.Floats[idx];

		}
		private void _InjectInt( int idx, int value )
		{
			if ( AnimationData == null ) return;
			if ( !AnimationData.TryGetValue( AnimationIndex, out CrosshairAnimation data ) ) return;
			if ( data.Ints.Count <= idx ) return;
			data.Ints[idx] = value;
		}
		private int _ExtractInt( int idx )
		{
			if ( AnimationData == null ) return int.MinValue;
			if ( !AnimationData.TryGetValue( AnimationIndex, out CrosshairAnimation data ) ) return int.MinValue;
			if ( data.Ints.Count <= idx ) return int.MinValue;
			return data.Ints[idx];
		}
		private void _InjectColor( int idx, InterpolatableColor value )
		{
			if ( AnimationData == null ) return;
			if ( !AnimationData.TryGetValue( AnimationIndex, out CrosshairAnimation data ) ) return;
			if ( data.Colors.Count <= idx ) return;
			data.Colors[idx] = value;
		}
		private InterpolatableColor? _ExtractColor( int idx )
		{
			if ( AnimationData == null ) return null;
			if ( !AnimationData.TryGetValue( AnimationIndex, out CrosshairAnimation data ) ) return null;
			if ( data.Colors.Count <= idx ) return null;
			return data.Colors[idx];
		}
#nullable enable
		private string _GetStringProperty( string[]? arr, int index )
		{
			if ( arr == null ) return "";
			if ( arr.Length <= index ) return "";
			return arr[index] ?? "";
		}
#nullable disable
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Properties Injector/Extractor
	}
}
