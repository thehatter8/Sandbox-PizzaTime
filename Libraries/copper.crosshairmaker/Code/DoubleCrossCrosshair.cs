using CrosshairMaker;
using CrosshairMaker.Abstract;
using CrosshairMaker.Animator;
using CrosshairMaker.Interfaces;
using Sandbox;
using Sandbox.Rendering;
using System;
using System.Collections.Generic;

namespace CrosshairMaker
{
	public class DoubleCrossCrosshair : BarCrosshair , IAnimatableCrosshair
	{
		[Property, Feature( "General" ), Group( "Bars" ), Title( "Second length" )]
		[Range( 0, 64, 0.1f )]
		public float Length2 = 10f;
		[Property, Feature( "General" ), Group( "Bars" ), Title( "Second thickness" )]
		[Range( 0, 64, 0.1f )]
		public float Thickness2 = 3f;
		[Property, Feature( "General" ), Group( "Bars" ), Title( "Second offset" )]
		[Range( 0, 64, 0.1f )]
		public float Offset2 = 5f;

		public override void Paint( HudPainter hud )
		{
			if ( Length == 0 || Thickness == 0 ) return;
			Vector2 originPx = IHudPaintable.GetOriginPx( this );
			IHudPaintable.HudAction toPaint;
			toPaint = new( (ref HudPainter hp ) =>
			{
				IBarCrosshair.RenderBar( this, hp, originPx );
				Rect r2;
				{
					Vector2 ro = new Vector2( Thickness2 / -2, Offset2) + originPx;
					Vector2 size = new Vector2( Thickness2, Length2 );
					r2 = new Rect(ro,size);
				}
				hp.DrawRect( r2, GetCrosshairColor() );
			} );
			
			PaintRepeating( hud, originPx, toPaint, 4, GetRotation() );
		}

		private static readonly AnimationValueSetter _animationSetters = _animFactory();
		private static AnimationValueSetter _animFactory()
		{
			List<Action<IAnimatableCrosshair, float>> fs = new()
			{
				(s,v) => (s as DoubleCrossCrosshair).SetRotation(v),
				(s,v) => (s as DoubleCrossCrosshair).SetOrigin(new(v,s.GetOrigin().y)),
				(s,v) => (s as DoubleCrossCrosshair).SetOrigin(new(s.GetOrigin().x,v)),
				(s,v) => (s as DoubleCrossCrosshair).SetBarLength(v),
				(s,v) => (s as DoubleCrossCrosshair).SetBarThickness(v),
				(s,v) => (s as DoubleCrossCrosshair).SetBarOffsets(v),
				(s,v) => (s as DoubleCrossCrosshair).Length2 = v,
				(s,v) => (s as DoubleCrossCrosshair).Thickness2 = v,
				(s,v) => (s as DoubleCrossCrosshair).Offset2 = v
			};
			List<Action<IAnimatableCrosshair, Color>> cs = new()
			{
				(s,v) => (s as DoubleCrossCrosshair).SetCrosshairColor(v),
				(s,v) => (s as DoubleCrossCrosshair).SetOutlineColor(v)
			};

			List<Func<IAnimatableCrosshair, float>> fg = new()
			{
				(s) => (s as DoubleCrossCrosshair).GetRotation(),
				(s) => (s as DoubleCrossCrosshair).GetOrigin().x,
				(s) => (s as DoubleCrossCrosshair).GetOrigin().y,
				(s) => (s as DoubleCrossCrosshair).GetBarLength(),
				(s) => (s as DoubleCrossCrosshair).GetBarThickness(),
				(s) => (s as DoubleCrossCrosshair).GetBarOffsets(),
				(s) => (s as DoubleCrossCrosshair).Length2,
				(s) => (s as DoubleCrossCrosshair).Thickness2,
				(s) => (s as DoubleCrossCrosshair).Offset2
			};
			List<Func<IAnimatableCrosshair, Color>> cg = new()
			{
				(s) => (s as DoubleCrossCrosshair).GetCrosshairColor(),
				(s) => (s as DoubleCrossCrosshair).GetOutlineColor(),
			};
			AnimationValueSetter avs = AnimationValueSetter.FromActions( floatSetters: fs, colorSetters: cs, floatGetters: fg, colorGetters: cg );
			return avs;
		}
		private static AnimationDictionary DoubleCrossAnimations
		{
			get
			{
				AnimationDictionary ap = new AnimationDictionary();
				ap.Init( _animationSetters );
				int k = ap.AddNewAnimation();
				ap[k].Floats[5] = 10;
				ap[k].Colors[0] = new InterpolatableColor( Color.White );
				ap[k].Colors[1] = new InterpolatableColor( Color.Black );
				ap.FloatsPropertyNames = new string[]
				{
					"Extra Rotation (deg)",
					"Extra X offset (%)",
					"Extra Y offset (%)",
					"Extra bar length",
					"Extra bar thickness",
					"Extra bar offset",
					"Extra bar length 2",
					"Extra bar thickness 2",
					"Extra bar length 2",
				};
				ap.ColorsPropertyNames = new string[]
				{
					"Target crosshair color",
					"Target outline color"
				};
				return ap;
			}
		}
		public AnimationDictionary GetAnimationDictionary() => DoubleCrossAnimations;
	}
}
