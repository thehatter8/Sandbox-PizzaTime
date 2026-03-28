using Sandbox;
using Sandbox.Rendering;

namespace CrosshairMaker.Interfaces
{
	public interface IHudPaintable
	{
		public Vector2 GetOrigin();
		public bool GetIsVisible();
		public void SetIsVisible(bool v);
		public void SetOrigin(Vector2 v);
		public float GetRotation();
		public void SetRotation(float r);
		public Color GetCrosshairColor();
		public void SetCrosshairColor( Color o );
		public void Paint( HudPainter hud );
		public static Vector2 GetOriginPx(IHudPaintable self) => PercentageToScreen( self as Component,self.GetOrigin() );
		public static Vector2 PercentageToScreen( Component self, Vector2 pos )
		{
			pos /= 100;
			Vector2 screenSize = self.Scene.Camera.ScreenRect.Size;
			return new Vector2( pos.x * screenSize.x, pos.y * screenSize.y );
		}
		protected delegate void HudAction( ref HudPainter hud );
		
	}
}
