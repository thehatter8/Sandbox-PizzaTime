using CrosshairMaker.Abstract;
using CrosshairMaker.Interfaces;
using Sandbox.Rendering;
using Sandbox;
using CrosshairMaker.Animator;
using System.Collections.Generic;
using System;


namespace CrosshairMaker
{
	[Icon( "adjust" )]
	public sealed class CenterDot : BaseCrosshair , IOutlineableCrosshair , IAnimatableCrosshair
	{
		public enum DotShape 
		{
			None,
			Square,
			Circle,
		}
		private bool _showOutlineProp => Outline && Shape != DotShape.None;

		[Property, Feature( "General" ), Group( "Appearance" ), Title( "Dot Shape" )]
		public DotShape Shape { get; set; } = DotShape.Circle;
		[Property,Feature("General"),Group("Appearance"),Title("Dot Size")]
		[HideIf(nameof(Shape),DotShape.None)]
		[Range(0,32)]
		public float DotSize = 5f;

		[Property, Feature( "General" ), Group( "Outline" ), Title( "Outline" )]
		[HideIf(nameof(Shape),DotShape.None)]
		public bool Outline = false;
		[Property, Feature( "General" ), Group( "Outline" ), Title( "Outline Thickness" )]
		[ShowIf(nameof(_showOutlineProp),true)]
		[Range(0,16)]
		public float OutlineThickness = 2f;
		[Property, Feature( "General" ), Group( "Outline" ), Title( "Outline Color" )]
		[ShowIf(nameof(_showOutlineProp),true)]
		[ColorUsage]
		public Color OutlineColor = Color.Black;

		public override void Paint( HudPainter hud )
		{
			if ( Shape == DotShape.None ) return;
			float rad = (Shape == DotShape.Square)? 0 : DotSize / 2;
			Vector2 origin = IHudPaintable.GetOriginPx(this) - new Vector2( DotSize / 2);
			Rect dot = new Rect( origin, new Vector2( DotSize ) );
			if (! Outline )
			{
				hud.DrawRect( dot, CrosshairColor,new Vector4(rad));
			}
			else
			{
				hud.DrawRect( dot, CrosshairColor, new Vector4( rad ), new Vector4( OutlineThickness ), OutlineColor );
			}
		}

		public bool GetOutline() => Outline;
		public void SetOutline( bool o ) => Outline = o;
		public float GetOutlineThickness() => OutlineThickness;
		public void SetOutlineThickness( float t ) => OutlineThickness = t;
		public Color GetOutlineColor() => OutlineColor;
		public void SetOutlineColor( Color o ) => OutlineColor = o;

		private static readonly AnimationValueSetter _animationSetters = _animFactory();
		private static AnimationValueSetter _animFactory()
		{
			List<Action<IAnimatableCrosshair, float>> fs = new()
			{
				(s,v) => (s as CenterDot).SetRotation(v),
				(s,v) => (s as CenterDot).SetOrigin(new(v,s.GetOrigin().y)),
				(s,v) => (s as CenterDot).SetOrigin(new(s.GetOrigin().x,v)),
				(s,v) => (s as CenterDot).DotSize = v,
			};
			List<Action<IAnimatableCrosshair, Color>> cs = new()
			{
				(s,v) => (s as CenterDot).SetCrosshairColor(v),
				(s,v) => (s as CenterDot).SetOutlineColor(v)
			};

			List<Func<IAnimatableCrosshair, float>> fg = new()
			{
				(s) => (s as CenterDot).GetRotation(),
				(s) => (s as CenterDot).GetOrigin().x,
				(s) => (s as CenterDot).GetOrigin().y,
				(s) => (s as CenterDot).DotSize,

			};
			List<Func<IAnimatableCrosshair, Color>> cg = new()
			{
				(s) => (s as CenterDot).GetCrosshairColor(),
				(s) => (s as CenterDot).GetOutlineColor()
			};
			AnimationValueSetter avs = AnimationValueSetter.FromActions( floatSetters: fs, colorSetters: cs, floatGetters: fg, colorGetters: cg );
			return avs;
		}
		private static AnimationDictionary DotAnimation
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
					"Extra dot size"
				};
				ap.ColorsPropertyNames = new string[]
				{
					"Target Color",
					"Target outline Color"
				};
				return ap;
			}
		}
		public AnimationDictionary GetAnimationDictionary() => DotAnimation;
	}
}
