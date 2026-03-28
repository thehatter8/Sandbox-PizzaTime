using CrosshairMaker.Abstract;
using CrosshairMaker.Animator;
using CrosshairMaker.Interfaces;
using Sandbox.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairMaker
{
	[Icon( "horizontal_distribute" )]
	public class SideBracketsCrosshair : TexturableCrosshair , IAnimatableCrosshair
	{
		private static readonly AnimationValueSetter _animationSetters = _animFactory();
		private static AnimationValueSetter _animFactory()
		{
			List<Action<IAnimatableCrosshair, float>> fs = new()
			{
				(s,v) => (s as SideBracketsCrosshair).SetRotation(v),
				(s,v) => (s as SideBracketsCrosshair).SetOrigin(new(v,s.GetOrigin().y)),
				(s,v) => (s as SideBracketsCrosshair).SetOrigin(new(s.GetOrigin().x,v)),
				(s,v) => (s as SideBracketsCrosshair).SetBarLength(v),
				(s,v) => (s as SideBracketsCrosshair).SetBarThickness(v),
				(s,v) => (s as SideBracketsCrosshair).SetBarOffsets(v)
			};
			List<Action<IAnimatableCrosshair, Color>> cs = new()
			{
				(s,v) => (s as SideBracketsCrosshair).SetCrosshairColor(v),
				(s,v) => (s as SideBracketsCrosshair).SetOutlineColor(v)
			};

			List<Func<IAnimatableCrosshair, float>> fg = new()
			{
				(s) => (s as SideBracketsCrosshair).GetRotation(),
				(s) => (s as SideBracketsCrosshair).GetOrigin().x,
				(s) => (s as SideBracketsCrosshair).GetOrigin().y,
				(s) => (s as SideBracketsCrosshair).GetBarLength(),
				(s) => (s as SideBracketsCrosshair).GetBarThickness(),
				(s) => (s as SideBracketsCrosshair).GetBarOffsets()
			};
			List<Func<IAnimatableCrosshair, Color>> cg = new()
			{
				(s) => (s as SideBracketsCrosshair).GetCrosshairColor(),
				(s) => (s as SideBracketsCrosshair).GetOutlineColor(),
			};
			AnimationValueSetter avs = AnimationValueSetter.FromActions( floatSetters: fs, colorSetters: cs, floatGetters: fg, colorGetters: cg );
			return avs;
		}
		private static AnimationDictionary SideBracketsAnimations
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
		public AnimationDictionary GetAnimationDictionary() => SideBracketsAnimations;

		public override void Paint( HudPainter hud )
		{
			if ( Length == 0 || Thickness == 0 ) return;
			Vector2 originPx = IHudPaintable.GetOriginPx( this );
			IHudPaintable.HudAction toPaint;
			if ( TextureFeature )
			{
				toPaint = new( ( ref HudPainter hp ) => ITexturable.RenderTextured( this, hp, originPx ) );
			}
			else
			{
				toPaint = new( ( ref HudPainter hp ) => IBarCrosshair.RenderBar( this, hp, originPx ) );
			}
			PaintRepeating( hud, originPx, toPaint, 2, (GetRotation() + 90) );
		}
	}
}
