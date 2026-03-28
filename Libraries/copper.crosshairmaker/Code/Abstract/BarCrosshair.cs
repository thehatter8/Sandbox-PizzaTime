using Sandbox;
using CrosshairMaker.Interfaces;
using System;
using System.Collections.Generic;

namespace CrosshairMaker.Abstract
{
	[Icon("add")]
	public abstract class BarCrosshair : BaseCrosshair, IBarCrosshair , IOutlineableCrosshair
	{
		[Property, Feature( "General" ), Group( "Bars" ), Title( "Length" )]
		[Range( 0, 64, 0.1f )]
		public float Length = 10f;
		[Property, Feature( "General" ), Group( "Bars" ), Title( "Thickness" )]
		[Range( 0, 64, 0.1f )]
		public float Thickness = 3f;
		[Property, Feature( "General" ), Group( "Bars" ), Title( "Offset" )]
		[Range( 0, 64, 0.1f )]
		public float Offset = 5f;

		[Property, FeatureEnabled( "Outline" )]
		public bool Outline { get; set; }

		[Property, Feature( "Outline" ), Title( "Outline Color" )]
		[ColorUsage]
		public Color OutlineColor = Color.Black;
		[Property, Feature( "Outline" ), Title( "Outline Thickness" )]
		[Range( 0, 16, 0.1f )]
		public float OutlineThickness;

		/*
		[Property, FeatureEnabled( "CrosshairAnimation" )]
		public bool AnimationFeature { get; set; } = false;
		[Property, Feature( "CrosshairAnimation" ), Group( "Editor tools" )]
		[InlineEditor]
		public IAnimatableCrosshair.EditorAnimationPlayer _editAnimPlayer = null;

		[Property, Feature( "CrosshairAnimation" ), Group( "Editor tools" ), Title( "CrosshairAnimation player" )]
		[ReadOnly]
		[HideIf(nameof(_eapActive),true)]
		public string _label1 => "Animations can only be previewed while game is running ";
		protected bool _eapActive => _editAnimPlayer != null;
		 */

		protected override void OnStart()
		{
			base.OnStart();
		}

		public virtual float GetBarLength() => Length;
		public virtual void SetBarLength( float l ) => Length = l;
		public virtual float GetBarThickness() => Thickness;
		public virtual void SetBarThickness( float t ) => Thickness = t;
		public virtual float GetBarOffsets() => Offset;
		public virtual void SetBarOffsets( float o ) => Offset = o;

		public virtual bool GetOutline() => Outline;
		public virtual void SetOutline(bool o) => Outline = o;
		public virtual Color GetOutlineColor() => OutlineColor;
		public virtual void SetOutlineColor(Color c) => OutlineColor = c;
		public virtual float  GetOutlineThickness() => OutlineThickness;
		public virtual void SetOutlineThickness(float t) => OutlineThickness = t;

		public IEnumerable<string> GetAnimationNames()
		{
			yield return "shoot";
			yield return "focus";
		}

	}
}
