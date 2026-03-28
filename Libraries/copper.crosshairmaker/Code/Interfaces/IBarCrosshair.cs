using Sandbox.Rendering;
using Sandbox;

namespace CrosshairMaker.Interfaces
{
	public interface IBarCrosshair : IHudPaintable
	{
		public float GetBarLength();
		public void SetBarLength(float l);
		public float GetBarThickness();
		public void SetBarThickness(float t);
		public float GetBarOffsets();
		public void SetBarOffsets(float o);
		protected static void RenderBar( IBarCrosshair self, HudPainter hud, Vector2? origin = null, Rect? rect = null )
		{
			origin ??= GetOriginPx(self);
			rect ??= _MakeRect( self, origin.Value );
			if ( self is IOutlineableCrosshair ioc )
			{
				if ( ioc.GetOutline() )
				{
					hud.DrawRect( rect.Value, ioc.GetCrosshairColor(), Vector4.Zero , new Vector4( ioc.GetOutlineThickness() ), ioc.GetOutlineColor() );
					return;
				}
			}
			hud.DrawRect( rect.Value, self.GetCrosshairColor() );
		}
		protected static Rect _MakeRect( IBarCrosshair self, Vector2 origin )
		{
			float halfThick = self.GetBarThickness() / 2;
			Vector2 start = new Vector2( -halfThick, self.GetBarOffsets() );
			Vector2 size = new Vector2( self.GetBarThickness(), self.GetBarLength() );
			start += origin;
			return new Rect( start, size );
		}
	}
}
