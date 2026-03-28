using CrosshairMaker.Abstract;
using CrosshairMaker.Animator;
using CrosshairMaker.Interfaces;
using Sandbox;
using Sandbox.Rendering;
using System;
using System.Collections.Generic;


namespace CrosshairMaker
{
	[Icon( "emergency" )]
	public class StarCrosshair : TexturableCrosshair, IAnimatableCrosshair
	{
		private static readonly AnimationValueSetter _animationSetters = _animFactory();
		private static AnimationValueSetter _animFactory()
		{
			List<Action<IAnimatableCrosshair, float>> fs = new()
			{
				(s,v) => (s as StarCrosshair).SetRotation(v),
				(s,v) => (s as StarCrosshair).SetOrigin(new(v,s.GetOrigin().y)),
				(s,v) => (s as StarCrosshair).SetOrigin(new(s.GetOrigin().x,v)),
				(s,v) => (s as StarCrosshair).SetBarLength(v),
				(s,v) => (s as StarCrosshair).SetBarThickness(v),
				(s,v) => (s as StarCrosshair).SetBarOffsets(v)
			};
			List<Action<IAnimatableCrosshair, int>> iss = new()
			{
				(s,v) => (s as StarCrosshair).BranchCount = v
			};
			List<Action<IAnimatableCrosshair, Color>> cs = new()
			{
				(s,v) => (s as StarCrosshair).SetCrosshairColor(v),
				(s,v) => (s as StarCrosshair).SetOutlineColor(v)
			};

			List<Func<IAnimatableCrosshair, float>> fg = new()
			{
				(s) => (s as StarCrosshair).GetRotation(),
				(s) => (s as StarCrosshair).GetOrigin().x,
				(s) => (s as StarCrosshair).GetOrigin().y,
				(s) => (s as StarCrosshair).GetBarLength(),
				(s) => (s as StarCrosshair).GetBarThickness(),
				(s) => (s as StarCrosshair).GetBarOffsets()
			};
			List<Func<IAnimatableCrosshair, int>> ig = new()
			{
				(s) => (s as StarCrosshair).BranchCount
			};
			List<Func<IAnimatableCrosshair, Color>> cg = new()
			{
				(s) => (s as StarCrosshair).GetCrosshairColor(),
				(s) => (s as StarCrosshair).GetOutlineColor(),
			};
			AnimationValueSetter avs = AnimationValueSetter.FromActions( fs, iss,cs , fg, ig,cg );
			return avs;
		}
		private static AnimationDictionary StarAnimations
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
				ap.IntsPropertyNames = new string[]
				{
					"Extra branches"
				};
				ap.ColorsPropertyNames = new string[]
				{
					"Target crosshair color",
					"Target outline color"
				};
				return ap;
			}
		}
		public AnimationDictionary GetAnimationDictionary() => StarAnimations;

		public override void Paint( HudPainter hud )
		{
			Vector2 o = IHudPaintable.GetOriginPx( this );

			IHudPaintable.HudAction toPaint;
			if(TextureFeature)
				toPaint = new((ref HudPainter h ) => ITexturable.RenderTextured( this, h,o ));
			else
				toPaint = new( ( ref HudPainter h ) => IBarCrosshair.RenderBar( this, h, o ) );
			PaintRepeating( hud, o, toPaint, _branchCount , GetRotation());
		}

		[Property, Feature( "General" ), Group( "Bars" ), Title( "Branch count" )]
		[Range( 1, 16,1 )]
		public int BranchCount
		{
			get => _branchCount;
			set => _branchCount = (byte)value;
		}
		private byte _branchCount = 5;
	}
}
