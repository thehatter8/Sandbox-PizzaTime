using CrosshairMaker.Abstract;
using CrosshairMaker.Animator;
using CrosshairMaker.Interfaces;
using Sandbox;
using Sandbox.Rendering;
using System.Collections.Generic;
using System;
namespace CrosshairMaker
{
	[Icon( "title" )]
	public class YCrosshair : TexturableCrosshair, IAnimatableCrosshair
	{
		[Property,Feature("General"),Group("Bars"),Title("Side bars angle")]
		[Range(0,180,1)]
		public float BarAngle
		{
			get => _barAngle;
			set
			{
				if ( value > 180f || value < 0f ) 
				{
					value %= 180;
					if ( value < 0f ) value += 180f;
				}
				_barAngle = value;
			}
		}
		private float _barAngle = 120f;

		public override void Paint( HudPainter hud )
		{
			Vector2 origin = IHudPaintable.GetOriginPx( this );

			IHudPaintable.HudAction toPaint;
			if(TextureFeature)
				toPaint = new( ( ref HudPainter h ) => ITexturable.RenderTextured( this, h, origin ) );
			else
				toPaint = new( ( ref HudPainter h ) => IBarCrosshair.RenderBar( this, h ) );

			//Paint bottom bar
			Matrix rot = Matrix.CreateRotationZ( GetRotation(), origin );
			hud.SetMatrix( rot );
			toPaint.Invoke( ref hud );

			//Paint bottom left bar
			rot = Matrix.CreateRotationZ( GetRotation() + BarAngle, origin );
			hud.SetMatrix( rot );
			toPaint.Invoke( ref hud );
			//Paint bottom right bar
			
			rot = Matrix.CreateRotationZ( GetRotation() - BarAngle, origin );
			hud.SetMatrix( rot );
			toPaint.Invoke( ref hud );
			
			hud.SetMatrix(Matrix.Identity);
		}
		private static readonly AnimationValueSetter _animationSetters = _animFactory();
		private static AnimationValueSetter _animFactory()
		{
			List<Action<IAnimatableCrosshair, float>> fs = new()
			{
				(s,v) => (s as YCrosshair).SetRotation(v),
				(s,v) => (s as YCrosshair).SetOrigin(new(v,s.GetOrigin().y)),
				(s,v) => (s as YCrosshair).SetOrigin(new(s.GetOrigin().x,v)),
				(s,v) => (s as YCrosshair).SetBarLength(v),
				(s,v) => (s as YCrosshair).SetBarThickness(v),
				(s,v) => (s as YCrosshair).SetBarOffsets(v),
				(s,v) => (s as YCrosshair).BarAngle = v
			};
			List<Action<IAnimatableCrosshair, Color>> cs = new()
			{
				(s,v) => (s as YCrosshair).SetCrosshairColor(v),
				(s,v) => (s as YCrosshair).SetOutlineColor(v)
			};

			List<Func<IAnimatableCrosshair, float>> fg = new()
			{
				(s) => (s as YCrosshair).GetRotation(),
				(s) => (s as YCrosshair).GetOrigin().x,
				(s) => (s as YCrosshair).GetOrigin().y,
				(s) => (s as YCrosshair).GetBarLength(),
				(s) => (s as YCrosshair).GetBarThickness(),
				(s) => (s as YCrosshair).GetBarOffsets(),
				(s) => (s as YCrosshair).BarAngle
			};
			List<Func<IAnimatableCrosshair, Color>> cg = new()
			{
				(s) => (s as YCrosshair).GetCrosshairColor(),
				(s) => (s as YCrosshair).GetOutlineColor(),
			};
			AnimationValueSetter avs = AnimationValueSetter.FromActions( floatSetters: fs, colorSetters: cs, floatGetters: fg, colorGetters: cg );
			return avs;
		}
		private static AnimationDictionary YAnimations
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
					"Extra side bar angle"
				};
				ap.ColorsPropertyNames = new string[]
				{
					"Target crosshair color",
					"Target outline color"
				};
				return ap;
			}
		}
		public AnimationDictionary GetAnimationDictionary() => YAnimations;
	}
}
