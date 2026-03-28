using CrosshairMaker.Abstract;
using CrosshairMaker.Animator;
using CrosshairMaker.Interfaces;
using Sandbox;
using Sandbox.Rendering;
using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairMaker
{
	[Icon( "my_location" )]
	public sealed class TargetCrosshair : BaseCrosshair, IBarCrosshair, ICircleCrosshair , IAnimatableCrosshair
	{

		//Cross
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		[Property,Feature("General"),Group("Cross"),Title("Bar Length")]
		[Range(0,64,0.1f)]
		public float BarLength { get; set; } = 10f;
		[Property,Feature("General"),Group("Cross"),Title("Bar Thickness")]
		[Range(0,64,0.1f)]
		public float BarThickness { get; set; } = 3f;
		[Property,Feature("General"),Group("Cross"),Title("Bar Offset")]
		[Range(0,64,0.1f)]
		public float BarOffset { get; set; } = 5f;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Cross

		//Circle
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		[Property,Feature("General"),Group("Circle"),Title("Circle Radius")]
		[Range(0,64,0.1f)]
		public float CircleRadius{ get; set; } = 12f;
		[Property,Feature("General"),Group("Circle"),Title("Circle Thickness")]
		[Range(0,32,0.1f)]
		public float CircleThickness{ get; set; } = 3f;

		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Circle

		public float GetBarLength()	=> BarLength;
		public void SetBarLength( float l ) => BarLength = l;
		public float GetBarOffsets() => BarOffset;
		public void SetBarOffsets( float o ) => BarOffset = o;
		public float GetBarThickness() => BarThickness;
		public void SetBarThickness( float t ) => BarThickness = t;

		public override void Paint( HudPainter hud )
		{
			Vector2 origin = IHudPaintable.GetOriginPx(this);
			_DrawCircle(hud, origin);
			_DrawCross(hud, origin);
		}
		private void _DrawCross( HudPainter hud , Vector2 origin)
		{
			if ( BarLength == 0 || BarThickness == 0 ) return;
			IHudPaintable.HudAction toPaint = new( ( ref HudPainter hp ) => IBarCrosshair.RenderBar(this,hp,origin));
			PaintRepeating(hud, origin,toPaint,4,GetRotation());
		}
		private void _DrawCircle( HudPainter hud ,Vector2 origin)
		{
			if ( CircleThickness == 0 ) return;
			if(CircleRadius == 0) return;
			Vector2 cOrigin = origin - new Vector2(CircleRadius);
			Vector2 size = new Vector2(CircleRadius * 2);
			Rect circle = new Rect(cOrigin, size);
			hud.DrawRect( circle, Color.Transparent, new Vector4( CircleRadius ), new Vector4( Math.Min( CircleThickness, CircleRadius - 0.001f ) ), CrosshairColor );

		}

		public void PlayAnimation( string animationName ) { }

		public float GetCircleRadius() => CircleRadius;

		public void SetCircleRadius( float r ) => CircleRadius = r;

		public float GetCircleThickness() => CircleThickness;

		public void SetCircleThickness( float t ) => CircleThickness = t;

		private static readonly AnimationValueSetter _animationSetters = _animFactory();
		private static AnimationValueSetter _animFactory()
		{
			List<Action<IAnimatableCrosshair, float>> fs = new()
			{
				(s,v) => (s as TargetCrosshair).SetRotation(v),
				(s,v) => (s as TargetCrosshair).SetOrigin(new(v,s.GetOrigin().y)),
				(s,v) => (s as TargetCrosshair).SetOrigin(new(s.GetOrigin().x,v)),
				(s,v) => (s as TargetCrosshair).SetBarLength(v),
				(s,v) => (s as TargetCrosshair).SetBarThickness(v),
				(s,v) => (s as TargetCrosshair).SetBarOffsets(v),
				(s,v) => (s as TargetCrosshair).SetCircleRadius(v),
				(s,v) => (s as TargetCrosshair).SetCircleThickness(v),
			};
			List<Action<IAnimatableCrosshair, Color>> cs = new()
			{
				(s,v) => (s as TargetCrosshair).SetCrosshairColor(v),
			};

			List<Func<IAnimatableCrosshair, float>> fg = new()
			{
				(s) => (s as TargetCrosshair).GetRotation(),
				(s) => (s as TargetCrosshair).GetOrigin().x,
				(s) => (s as TargetCrosshair).GetOrigin().y,
				(s) => (s as TargetCrosshair).GetBarLength(),
				(s) => (s as TargetCrosshair).GetBarThickness(),
				(s) => (s as TargetCrosshair).GetBarOffsets(),
				(s) => (s as TargetCrosshair).GetCircleRadius(),
				(s) => (s as TargetCrosshair).GetCircleThickness()
			};
			List<Func<IAnimatableCrosshair, Color>> cg = new()
			{
				(s) => (s as TargetCrosshair).GetCrosshairColor()
			};
			AnimationValueSetter avs = AnimationValueSetter.FromActions( floatSetters: fs, colorSetters: cs, floatGetters: fg, colorGetters: cg );
			return avs;
		}
		private static AnimationDictionary TargetAnimation
		{
			get
			{
				AnimationDictionary ap = new AnimationDictionary();
				ap.Init( _animationSetters );
				int k = ap.AddNewAnimation();
				CrosshairAnimation anim = ap[k];

				anim.Colors[0] = new( Color.White );

				ap.FloatsPropertyNames = new string[]
				{
					"Extra Rotation (deg)",
					"Extra X offset (%)",
					"Extra Y offset (%)",
					"Extra bar length",
					"Extra bar thickness",
					"Extra bar offset",
					"Extra radius",
					"Extra thickness"
				};
				ap.ColorsPropertyNames = new string[]
				{
					"Target Color",
				};
				return ap;
			}
		}
		public AnimationDictionary GetAnimationDictionary() => TargetAnimation;
	}
}
