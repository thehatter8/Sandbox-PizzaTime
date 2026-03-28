using Sandbox;
using Sandbox.Rendering;
using static Sandbox.CameraComponent;
using System;
using System.Text;

namespace CrosshairMaker.Interfaces
{
	public interface ITexturable : IBarCrosshair
	{
		public Texture GetTexture();
		public void SetTexture(Texture texture);
		public void SetTexture(string path);
		protected static void RenderTextured( ITexturable self, HudPainter hud, Vector2? origin = null, Rect? rect = null )
		{
			origin ??= GetOriginPx(self);
			rect ??= _MakeRect( self, origin.Value );

			
			hud.DrawTexture( self.GetTexture(), rect.Value );

			
		}
	}
}
