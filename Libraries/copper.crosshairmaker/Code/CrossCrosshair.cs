using CrosshairMaker.Abstract;
using CrosshairMaker.Animator;
using CrosshairMaker.Interfaces;
using Sandbox;
using Sandbox.Rendering;
using System;
using System.Collections.Generic;

namespace CrosshairMaker
{
	public sealed class CrossCrosshair : TexturableCrosshair, IBarCrosshair, IOutlineableCrosshair, IAnimatableCrosshair
	{

		private static readonly AnimationValueSetter _animationSetters = _animFactory();
		private static AnimationValueSetter _animFactory()
		{
			List<Action<IAnimatableCrosshair, float>> fs = new()
			{
				(s,v) => (s as CrossCrosshair).SetRotation(v),
				(s,v) => (s as CrossCrosshair).SetOrigin(new(v,s.GetOrigin().y)),
				(s,v) => (s as CrossCrosshair).SetOrigin(new(s.GetOrigin().x,v)),
				(s,v) => (s as CrossCrosshair).SetBarLength(v),
				(s,v) => (s as CrossCrosshair).SetBarThickness(v),
				(s,v) => (s as CrossCrosshair).SetBarOffsets(v)
			};
			List<Action<IAnimatableCrosshair, Color>> cs = new()
			{
				(s,v) => (s as CrossCrosshair).SetCrosshairColor(v),
				(s,v) => (s as CrossCrosshair).SetOutlineColor(v)
			};

			List<Func<IAnimatableCrosshair, float>> fg = new()
			{
				(s) => (s as CrossCrosshair).GetRotation(),
				(s) => (s as CrossCrosshair).GetOrigin().x,
				(s) => (s as CrossCrosshair).GetOrigin().y,
				(s) => (s as CrossCrosshair).GetBarLength(),
				(s) => (s as CrossCrosshair).GetBarThickness(),
				(s) => (s as CrossCrosshair).GetBarOffsets()
			};
			List<Func<IAnimatableCrosshair, Color>> cg = new()
			{
				(s) => (s as CrossCrosshair).GetCrosshairColor(),
				(s) => (s as CrossCrosshair).GetOutlineColor(),
			};
			AnimationValueSetter avs = AnimationValueSetter.FromActions( floatSetters:fs, colorSetters:cs,floatGetters:fg,colorGetters:cg );
			return avs;
		}
		private static AnimationDictionary CrossAnimations
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
					"Extra bar offset"
				};
				ap.ColorsPropertyNames = new string[]
				{
					"Target crosshair color",
					"Target outline color"
				};
				return ap;
			}
		}
		public AnimationDictionary GetAnimationDictionary() => CrossAnimations;
		public override void Paint( HudPainter hud )
		{
			if ( Length == 0 || Thickness == 0 ) return;
			Vector2 originPx = IHudPaintable.GetOriginPx(this);
			IHudPaintable.HudAction toPaint;
			if ( TextureFeature )
			{
				toPaint = new((ref HudPainter hp) => ITexturable.RenderTextured(this, hp , originPx));
			}
			else
			{
				toPaint = new((ref HudPainter hp) => IBarCrosshair.RenderBar(this, hp,originPx ));
			}
			PaintRepeating( hud, originPx, toPaint, 4, GetRotation());
		}

		
	}
}
