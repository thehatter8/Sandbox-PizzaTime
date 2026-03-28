using CrosshairMaker.Helpers;
using CrosshairMaker.Interfaces;
using Sandbox;
using CrosshairMaker.Abstract;
using Sandbox.Rendering;
using System;
using System.Reflection.Metadata.Ecma335;
using CrosshairMaker.Animator;
using System.Collections.Generic;

namespace CrosshairMaker
{
	[Icon( "align_horizontal_center" )]
	public sealed class AsymmetricalCrosshair : BaseCrosshair , IAsymmetricalCrosshair , IAnimatableCrosshair
	{
		private static Bar[] _defaultCrosshair()
		{
			Bar[] res = new Bar[4];
			res[0] = new Bar();
			res[1] = new Bar() { length = 3f, thickness = 10f };
			res[2] = new Bar();
			res[3] = new Bar();
			return res;
		}
		private readonly Bar[] _bars = _defaultCrosshair();

		public AsymmetricalCrosshair() { }

		[Property, Feature( "General" ), Group( "Bars" ) , Title("Top")]
		[InlineEditor]
		public Bar TopBar => _bars[0];
		[Space]
		[Property, Feature( "General" ), Group( "Bars" ) , Title("Bottom")]
		[InlineEditor]
		public Bar BottomBar => _bars[1];
		[Space]
		[Property, Feature( "General" ), Group( "Bars" ) , Title("Left")]
		[InlineEditor]
		public Bar LeftBar => _bars[2];
		[Space]
		[Property, Feature( "General" ), Group( "Bars" ) , Title("Right")]
		[InlineEditor]
		public Bar RightBar => _bars[3];

		//Top bar
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public float GetTopLength() => _bars[0].length;
		public void SetTopLength( float l ) => _bars[0].length = l;
		public float GetTopThickness() => _bars[0].thickness;
		public void SetTopThickness( float t ) => _bars[0].thickness = t;
		public float GetTopOffset() => _bars[0].offset;
		public void SetTopOffset( float o ) => _bars[0].offset = o;

		public float GetTopOutlineThickness() => _bars[0].outlineThickness;
		public void SetTopOutlineThickness( float t ) => _bars[0].outlineThickness = t;
		public Color GetTopOutlineColor() => _bars[0].outlineColor;
		public void SetTopOutlineColor( Color c ) => _bars[0].outlineColor = c;
		public Color GetTopColor() => _bars[0].barColor;
		public void SetTopColor(Color c) => _bars[0].barColor = c;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Top bar

		//Bottom bar
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public float GetBottomLength() => _bars[1].length;
		public float GetBottomThickness() => _bars[1].thickness;
		public float GetBottomOffset() => _bars[1].offset;
		public void SetBottomLength( float l ) => _bars[1].length = l;
		public void SetBottomThickness( float t )=> _bars[1].thickness = t;
		public void SetBottomOffset( float o ) => _bars[1].offset = o;

		public float GetBottomOutlineThickness() => _bars[1].thickness;
		public void SetBottomOutlineThickness( float t ) => _bars[1].outlineThickness = t;
		public Color GetBottomOutlineColor() => _bars[1].outlineColor;
		public void SetBottomOutlineColor( Color c ) => _bars[1].outlineColor = c;
		public Color GetBottomColor() => _bars[1].barColor;
		public void SetBottomColor( Color c ) => _bars[1].barColor = c;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Bottom bar

		//Left bar
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public float GetLeftLength() => _bars[2].length;
		public void SetLeftLength( float l ) => _bars[2].length = l;
		public float GetLeftThickness() => _bars[2].thickness;
		public void SetLeftThickness( float t ) => _bars[2].thickness = t;
		public float GetLeftOffset() => _bars[2].offset;
		public void SetLeftOffset( float o ) => _bars[2].offset = o;

		public float GetLeftOutlineThickness() => _bars[2].thickness;
		public void SetLeftOutlineThickness( float t ) => _bars[2].outlineThickness = t;
		public Color GetLeftOutlineColor() => _bars[2].outlineColor;
		public void SetLeftOutlineColor( Color c ) => _bars[2].outlineColor = c;
		public Color GetLeftColor() => _bars[2].barColor;
		public void SetLeftColor( Color c ) => _bars[2].barColor = c;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Left bar

		//Right bar
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public float GetRightLength() => _bars[3].length;
		public void SetRightLength( float l ) => _bars[3].length = l;
		public float GetRightThickness() => _bars[3].thickness;
		public void SetRightThickness( float t ) => _bars[3].thickness = t;
		public float GetRightOffset() => _bars[3].offset;
		public void SetRightOffset( float o ) => _bars[3].offset = o;

		public float GetRightOutlineThickness() => _bars[3].thickness;
		public void SetRightOutlineThickness( float t ) => _bars[3].thickness = t;
		public Color GetRightOutlineColor() => _bars[3].outlineColor;
		public void SetRightOutlineColor( Color c ) => _bars[3].outlineColor = c;
		public Color GetRightColor() => _bars[3].barColor;
		public void SetRightColor( Color c ) => _bars[3].barColor = c;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Right bar


		public override void Paint(HudPainter hud)
		{
			for(byte i = 0; i < 4; i++ )
			{
				Bar b = _bars[i];
				if ( b.length == 0 || b.thickness == 0 ) continue;
				Draw( b, i ,hud);
			}
			void Draw(Bar b , byte idx , HudPainter hp)
			{
				Vector2 origin = IHudPaintable.GetOriginPx(this);
				Vector2 size;
				float halfThickness = b.thickness / 2;
				switch(idx)
				{
					default: return;
					case 0:
						origin += new Vector2( -halfThickness, (-b.offset - b.length) );
						size = new Vector2(b.thickness, b.length);
						break;
					case 1:
						origin += new Vector2( -halfThickness, b.offset );
						size = new Vector2( b.thickness, b.length );
						break;
					case 2:
						origin += new Vector2( (-b.offset - b.length), -halfThickness );
						size = new Vector2( b.length, b.thickness);
						break;
					case 3:
						origin += new Vector2( b.offset, -halfThickness );
						size = new Vector2( b.length, b.thickness );
						break;
				}
				Rect br = new Rect(origin, size);
				if(b.BarOutline) hp.DrawRect( br, b.barColor, new Vector4( 0 ), new Vector4( b.outlineThickness ), b.outlineColor );
				else hp.DrawRect( br, b.barColor);
			}
		}
		
		public bool GetOutline()
		{
			for(byte i = 0; i< 4; i++ )
				if ( _bars[i].BarOutline ) return true;
			return false;
		}

		public void SetOutline( bool o )
		{
			for ( byte i = 0; i < 4; i++ ) _bars[i].BarOutline = o;
		}

		public  float GetOutlineThickness() => GetTopOutlineThickness();

		public void SetOutlineThickness( float t )
		{
			for ( byte i = 0; i < 4; i++ ) _bars[i].thickness = t;
		}

		public Color GetOutlineColor() => GetTopOutlineColor();

		public void SetOutlineColor( Color o )
		{
			for ( byte i = 0; i < 4; i++ ) _bars[i].outlineColor = o;
		}

		private static readonly AnimationValueSetter _animationSetters = _animFactory();
		private static AnimationValueSetter _animFactory()
		{
			List<Action<IAnimatableCrosshair, float>> fs = new()
			{
				(s,v) => (s as AsymmetricalCrosshair).SetRotation(v),
				(s,v) => (s as AsymmetricalCrosshair).SetOrigin(new(v,s.GetOrigin().y)),
				(s,v) => (s as AsymmetricalCrosshair).SetOrigin(new(s.GetOrigin().x,v)),
				(s,v) => (s as AsymmetricalCrosshair).SetTopLength(v),
				(s,v) => (s as AsymmetricalCrosshair).SetTopThickness(v),
				(s,v) => (s as AsymmetricalCrosshair).SetTopOffset(v),
				(s,v) => (s as AsymmetricalCrosshair).SetBottomLength(v),
				(s,v) => (s as AsymmetricalCrosshair).SetBottomThickness(v),
				(s,v) => (s as AsymmetricalCrosshair).SetBottomOffset(v),
				(s,v) => (s as AsymmetricalCrosshair).SetLeftLength(v),
				(s,v) => (s as AsymmetricalCrosshair).SetLeftThickness(v),
				(s,v) => (s as AsymmetricalCrosshair).SetLeftOffset(v),
				(s,v) => (s as AsymmetricalCrosshair).SetRightLength(v),
				(s,v) => (s as AsymmetricalCrosshair).SetRightThickness(v),
				(s,v) => (s as AsymmetricalCrosshair).SetRightOffset(v)
			};
			List<Action<IAnimatableCrosshair, Color>> cs = new()
			{
				(s,v) => (s as AsymmetricalCrosshair).SetTopColor(v),
				(s,v) => (s as AsymmetricalCrosshair).SetBottomColor(v),
				(s,v) => (s as AsymmetricalCrosshair).SetLeftColor(v),
				(s,v) => (s as AsymmetricalCrosshair).SetRightColor(v),
				(s,v) => (s as AsymmetricalCrosshair).SetTopOutlineColor(v),
				(s,v) => (s as AsymmetricalCrosshair).SetBottomOutlineColor(v),
				(s,v) => (s as AsymmetricalCrosshair).SetLeftOutlineColor(v),
				(s,v) => (s as AsymmetricalCrosshair).SetRightOutlineColor(v)
			};

			List<Func<IAnimatableCrosshair, float>> fg = new()
			{
				(s) => (s as AsymmetricalCrosshair).GetRotation(),
				(s) => (s as AsymmetricalCrosshair).GetOrigin().x,
				(s) => (s as AsymmetricalCrosshair).GetOrigin().y,
				(s) => (s as AsymmetricalCrosshair).GetTopLength(),
				(s) => (s as AsymmetricalCrosshair).GetTopThickness(),
				(s) => (s as AsymmetricalCrosshair).GetTopOffset(),
				(s) => (s as AsymmetricalCrosshair).GetBottomLength(),
				(s) => (s as AsymmetricalCrosshair).GetBottomThickness(),
				(s) => (s as AsymmetricalCrosshair).GetBottomOffset(),
				(s) => (s as AsymmetricalCrosshair).GetLeftLength(),
				(s) => (s as AsymmetricalCrosshair).GetLeftThickness(),
				(s) => (s as AsymmetricalCrosshair).GetLeftOffset(),
				(s) => (s as AsymmetricalCrosshair).GetRightLength(),
				(s) => (s as AsymmetricalCrosshair).GetRightThickness(),
				(s) => (s as AsymmetricalCrosshair).GetRightOffset(),

			};
			List<Func<IAnimatableCrosshair, Color>> cg = new()
			{
				(s) => (s as AsymmetricalCrosshair).GetTopColor(),
				(s) => (s as AsymmetricalCrosshair).GetBottomColor(),
				(s) => (s as AsymmetricalCrosshair).GetLeftColor(),
				(s) => (s as AsymmetricalCrosshair).GetRightColor(),
				(s) => (s as AsymmetricalCrosshair).GetTopOutlineColor(),
				(s) => (s as AsymmetricalCrosshair).GetBottomOutlineColor(),
				(s) => (s as AsymmetricalCrosshair).GetLeftOutlineColor(),
				(s) => (s as AsymmetricalCrosshair).GetRightOutlineColor()
			};
			AnimationValueSetter avs = AnimationValueSetter.FromActions( floatSetters: fs, colorSetters: cs, floatGetters: fg, colorGetters: cg );
			return avs;
		}
		private static AnimationDictionary AsymmetricAnimation
		{
			get
			{
				AnimationDictionary ap = new AnimationDictionary();
				ap.Init(_animationSetters);
				int k = ap.AddNewAnimation();
				CrosshairAnimation anim = ap[k];
				anim.Floats[5] = 8; //top offset
				anim.Floats[8] = 8; //Bottom offset
				anim.Floats[11] = 16; //Left offset
				anim.Floats[14] = 16; //Right offset

				anim.Floats[7] = 16; //Bottom thickness

				//Colors
				anim.Colors[0] = new(Color.White);
				anim.Colors[1] = new(Color.White);
				anim.Colors[2] = new(Color.White);
				anim.Colors[3] = new(Color.White);
				//Outlines
				anim.Colors[4] = new( Color.Black );
				anim.Colors[5] = new( Color.Black );
				anim.Colors[6] = new( Color.Black );
				anim.Colors[7] = new( Color.Black );

				ap.FloatsPropertyNames = new string[15]
				{
					"Extra Rotation (deg)",
					"Extra X offset (%)",
					"Extra Y offset (%)",
					"Extra top length",
					"Extra top thickness",
					"Extra top offset",
					"Extra bottom length",
					"Extra bottom thickness",
					"Extra bottom offset",
					"Extra left length",
					"Extra left thickness",
					"Extra left offset",
					"Extra right length",
					"Extra right thickness",
					"Extra right offset"
				};
				ap.ColorsPropertyNames = new string[8]
				{
					"Top bar color",
					"Bottom bar color",
					"Left bar color",
					"Right bar color",
					"Top outline color",
					"Bottom outline color",
					"Left outline color",
					"Right outline color"
				};
				return ap;
			}
		}
		public AnimationDictionary GetAnimationDictionary() => AsymmetricAnimation;
	}
}
