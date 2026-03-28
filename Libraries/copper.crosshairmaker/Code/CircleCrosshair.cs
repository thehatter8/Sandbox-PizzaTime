using CrosshairMaker.Interfaces;
using CrosshairMaker.Abstract;
using Sandbox.Rendering;
using System;
using static Sandbox.TextRendering;
using CrosshairMaker.Animator;
using System.Collections.Generic;

namespace Sandbox
{
	[Icon( "adjust" )]
	public sealed class CircleCrosshair : BaseCrosshair , IOutlineableCrosshair , IAnimatableCrosshair , ICircleCrosshair
	{
		[Property, Feature( "General" ), Group( "Circle" ), Title( "Circle Radius" )]
		[Range( 0, 64, 0.1f )]
		public float Radius
		{
			get => _radius;
			set => _radius = value;
		}
		private float _radius = 15f;
		[Property, Feature( "General" ), Group( "Circle" ), Title( "Circle Thickness" )]
		[Range( 0, 64, 0.1f )]
		public float Thickness
		{
			get => _thickness;
			set => _thickness = value;
		}
		private float _thickness = 3;

		[Property, FeatureEnabled( "Outline" )]
		public bool Outline { get; set; }

		[Property, Feature( "Outline" ), Title( "Outline Color" )]
		[ColorUsage]
		public Color OutlineColor = Color.Black;
		[Property, Feature( "Outline" ), Title( "Outline Thickness" )]
		[Range( 0, 16, 0.1f )]
		public float OutlineThickness;
		public bool GetOutline() => Outline;
		public void SetOutline( bool o ) => Outline = o;
		public Color GetOutlineColor() => OutlineColor;
		public void SetOutlineColor( Color c ) => OutlineColor = c;
		public float GetOutlineThickness() => Thickness;
		public void SetOutlineThickness( float t ) => Thickness = t;

		public override void Paint( HudPainter hud )
		{
			if ( Thickness < 0 ) return;
			Vector2 origin = IHudPaintable.GetOriginPx(this);
			origin -= new Vector2( Radius );
			Vector2 bounds = new Vector2(Radius * 2);
			Rect circle = new Rect( origin, bounds );
			if ( !Outline || OutlineThickness < 0.001f )
			{
				hud.DrawRect(circle , Color.Transparent , new Vector4(Radius),new Vector4(Math.Min(Thickness,Radius - 0.001f)) ,CrosshairColor);
			}
			else
			{
				hud.DrawRect(circle , Color.Transparent , new Vector4(Radius),new Vector4(Math.Min(Thickness,Radius - 0.001f)) ,OutlineColor);
				float doubleThick = OutlineThickness * 2;
				Vector2 inOrigin = origin + new Vector2( OutlineThickness ); // offsets origin for outline
				Vector2 inBounds = bounds - new Vector2( doubleThick ); // shrinks bounds
				Rect inCircle = new Rect( inOrigin,  inBounds);

				hud.DrawRect(inCircle , Color.Transparent , new Vector4(Radius),new Vector4(Math.Min(Thickness - doubleThick ,Radius - 0.001f - doubleThick ) ) ,CrosshairColor);
			}
		}
		public float GetCircleRadius() => Radius;
		public void SetCircleRadius( float r ) => Radius = r;
		public float GetCircleThickness() => Thickness;
		public void SetCircleThickness( float t ) => Thickness = t;

		private static readonly AnimationValueSetter _animationSetters = _animFactory();
		private static AnimationValueSetter _animFactory()
		{
			List<Action<IAnimatableCrosshair, float>> fs = new()
			{
				(s,v) => (s as CircleCrosshair).SetRotation(v),
				(s,v) => (s as CircleCrosshair).SetOrigin(new(v,s.GetOrigin().y)),
				(s,v) => (s as CircleCrosshair).SetOrigin(new(s.GetOrigin().x,v)),
				(s,v) => (s as CircleCrosshair).SetCircleRadius(v),
				(s,v) => (s as CircleCrosshair).SetCircleThickness(v),
			};
			List<Action<IAnimatableCrosshair, Color>> cs = new()
			{
				(s,v) => (s as CircleCrosshair).SetCrosshairColor(v),
				(s,v) => (s as CircleCrosshair).SetOutlineColor(v)
			};

			List<Func<IAnimatableCrosshair, float>> fg = new()
			{
				(s) => (s as CircleCrosshair).GetRotation(),
				(s) => (s as CircleCrosshair).GetOrigin().x,
				(s) => (s as CircleCrosshair).GetOrigin().y,
				(s) => (s as CircleCrosshair).GetCircleRadius(),
				(s) => (s as CircleCrosshair).GetCircleThickness()
			};
			List<Func<IAnimatableCrosshair, Color>> cg = new()
			{
				(s) => (s as CircleCrosshair).GetCrosshairColor(),
				(s) => (s as CircleCrosshair).GetOutlineColor()
			};
			AnimationValueSetter avs = AnimationValueSetter.FromActions( floatSetters: fs, colorSetters: cs, floatGetters: fg, colorGetters: cg );
			return avs;
		}
		private static AnimationDictionary CircleAnimation
		{
			get
			{
				AnimationDictionary ap = new AnimationDictionary();
				ap.Init( _animationSetters );
				int k = ap.AddNewAnimation();
				CrosshairAnimation anim = ap[k];

				anim.Colors[0] = new( Color.White );
				anim.Colors[1] = new( Color.Black );

				ap.FloatsPropertyNames = new string[]
				{
					"Extra Rotation (deg)",
					"Extra X offset (%)",
					"Extra Y offset (%)",
					"Extra radius",
					"Extra thickness"
				};
				ap.ColorsPropertyNames = new string[]
				{
					"Target Color",
					"Target outline Color"
				};
				return ap;
			}
		}
		public AnimationDictionary GetAnimationDictionary() => CircleAnimation;

		
	}
}
