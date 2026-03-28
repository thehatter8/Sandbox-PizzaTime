using CrosshairMaker.Interfaces;
using Sandbox;
using Sandbox.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrosshairMaker.Abstract
{
	public abstract class BaseCrosshair : Component, IHudPaintable
	{
		//Properties
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		[Button( "Add center dot", "filter_center_focus" ), Feature( "General" ), Group( "Appearance" )]
		[HideIf(nameof(_hasCenterdot),true)]
		protected void AddCenterDotAction()
		{
			if ( !_hasCenterdot ) GameObject.AddComponent<CenterDot>().SetOrigin(GetOrigin());
		}
		protected bool _hasCenterdot
		{
			get
			{
				IEnumerable<CenterDot> dots = GameObject.GetComponents<CenterDot>();
				if ( dots == null ) return false;
				return dots.Any();
			}
		}

		[Property, Feature( "General" ), Group( "Appearance" ), Title( "Crosshair color" )]
		[ColorUsage]
		public Color CrosshairColor = Color.White;
		[Property, Feature( "General" ), Group( "Appearance" )]
		public bool IsVisible
		{
			get => _isVisible; 
			set => _isVisible = value;
		}
		[Property, Feature( "General" ), Group( "Position" ), Title( "Crosshair position" )]
		public Vector2 CrosshairPosition
		{
			get => _origin;
			set => _origin = value ;
		}
		[Property, Feature( "General" ), Group( "Position" ), Title( "Crosshair rotation" )]
		[Range(0,360,1)]
		public float Rotation
		{
			get => _rotation;
			set => SetRotation( value );
		}
		[Button( "Center crosshair", "filter_center_focus" ), Feature( "General" ), Group( "Position" )]
		protected void CenterAction() => SetOrigin( new Vector2( 50, 50 ) );

		protected Vector2 _origin = new Vector2( 50f, 50f );
		protected float _rotation = 0f;
		protected bool _isVisible = true;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Properties


		//Methods
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		protected override void OnUpdate()
		{
			base.OnUpdate();
			if ( Scene.Camera == null ) return;
			if ( IsVisible ) Paint( Scene.Camera.Hud );
		}
		protected static void PaintRepeating( HudPainter hud, Vector2 origin, IHudPaintable.HudAction toPaint, byte repeats, float axialOffset )
		{
			if ( repeats == 0 ) return;
			if ( repeats == 1 )
			{
				toPaint.Invoke( ref hud );
				return;
			}
			float angleDelta;
			if ( repeats == 4 ) angleDelta = 90f; // most crosshairs repeat 4 times
			else angleDelta = 360f / repeats;

			for ( short i = 0; i < repeats; i++ )
			{
				float angle = angleDelta * i + axialOffset;
				Matrix rot = Matrix.CreateRotationZ( angle, origin );
				hud.SetMatrix( rot );
				toPaint.Invoke( ref hud );
			}
			hud.SetMatrix( Matrix.Identity );
			
		}
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Methods

		//Interface impl
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public bool GetIsVisible() => _isVisible;
		public void SetIsVisible(bool v) => _isVisible = v;
		public abstract void Paint( HudPainter hud );
		public Vector2 GetOrigin() => _origin;
		public void SetOrigin( Vector2 v ) => _origin = v;
		public float GetRotation() => _rotation;

		public void SetRotation( float r )
		{
			if ( r < 0f || r > 360f )
			{
				r %= 360f;
				while ( r < -0f ) r += 360f;
			}
			_rotation = r;
		}
		public Color GetCrosshairColor() => CrosshairColor;
		public void SetCrosshairColor( Color o ) => CrosshairColor = o;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Interface impl
	}
}
