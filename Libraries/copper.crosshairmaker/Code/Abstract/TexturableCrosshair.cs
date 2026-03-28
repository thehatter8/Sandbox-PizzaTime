using CrosshairMaker.Interfaces;
using Sandbox;
using Sandbox.Rendering;

namespace CrosshairMaker.Abstract
{
	public abstract class TexturableCrosshair : BarCrosshair , ITexturable
	{
		[Property, FeatureEnabled( "Texture" )]
		public bool TextureFeature = false;
		

		[Property, Feature( "Texture" ), Group( "Image" ), Title( "Texture" )]
		
		protected Texture ImgTexture = null;

		[Button( "Fit Texture", "fullscreen" ), Feature("Texture"),Group("Image")]
		[HideIf(nameof( ImgTexture ),null)]
		protected void FitTexture()
		{
			if ( ImgTexture == null ) 
			{
				Log.Warning( "[Fit Texture] ERROR : Texture is null" );
				return;
			}
			if ( !ImgTexture.IsValid() )
			{
				Log.Warning( "[Fit Texture] ERROR : Texture is invalid" );
				return;
			}
			SetBarThickness( ImgTexture.Width );
			SetBarLength( ImgTexture.Height );
		}
		
		public void RenderElem(HudPainter hud , Vector2? origin = null, Rect? rect = null)
		{
			origin ??=	IHudPaintable.GetOriginPx(this);
			rect ??= IBarCrosshair._MakeRect(this,origin.Value );
			
			if ( TextureFeature ) ITexturable.RenderTextured(this, hud ,origin,rect);
			
			else IBarCrosshair.RenderBar(this, hud, origin, rect);
		}

		public Texture GetTexture() => ImgTexture;

		public void SetTexture( Texture texture ) => ImgTexture = texture;
		public void SetTexture( string path ) => ImgTexture = Texture.Load(path);

		Texture ITexturable.GetTexture() => GetTexture();
		void ITexturable.SetTexture( Texture texture ) => SetTexture(texture);
		void ITexturable.SetTexture( string path ) => SetTexture(path);
	}
}
