using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairMaker.Interfaces
{
	public interface IAsymmetricalCrosshair : IBarCrosshair , IOutlineableCrosshair
	{
		float IBarCrosshair.GetBarLength() => GetTopLength();
		void IBarCrosshair.SetBarLength(float l)
		{
			SetTopLength(l);
			SetBottomLength(l);
			SetLeftLength(l);
			SetRightLength(l);
		}
		float IBarCrosshair.GetBarThickness() => GetTopThickness();
		void IBarCrosshair.SetBarThickness(float t)
		{
			SetTopThickness(t);
			SetBottomThickness(t);
			SetLeftThickness(t);
			SetRightThickness(t);
		}
		float IBarCrosshair.GetBarOffsets() => GetTopOffset();
		void IBarCrosshair.SetBarOffsets(float o )
		{
			SetTopOffset(o);
			SetBottomOffset(o);
			SetLeftOffset(o);
			SetRightOffset(o);
		}
		public float GetTopLength();
		public void SetTopLength( float l );

		public float GetBottomLength();
		public void SetBottomLength( float l );

		public float GetLeftLength();
		public void SetLeftLength( float l );

		public float GetRightLength();
		public void SetRightLength( float l );

		public float GetTopThickness();
		public void SetTopThickness( float t );

		public float GetBottomThickness();
		public void SetBottomThickness( float t );

		public float GetLeftThickness();
		public void SetLeftThickness( float t );

		public float GetRightThickness();
		public void SetRightThickness( float t );

		public float GetTopOffset();
		public void SetTopOffset( float o );

		public float GetBottomOffset();
		public void SetBottomOffset( float o );

		public float GetLeftOffset();
		public void SetLeftOffset( float o );

		public float GetRightOffset();
		public void SetRightOffset( float o );

		public float GetTopOutlineThickness();
		public void SetTopOutlineThickness(float t);
		public float GetBottomOutlineThickness();
		public void SetBottomOutlineThickness(float t);
		public float GetLeftOutlineThickness();
		public void SetLeftOutlineThickness( float t );
		public float GetRightOutlineThickness();
		public void SetRightOutlineThickness( float t );
		public Color GetTopColor();
		public void SetTopColor(Color c);
		public Color GetBottomColor();
		public void SetBottomColor( Color c );
		public Color GetLeftColor();
		public void SetLeftColor( Color c );
		public Color GetRightColor();
		public void SetRightColor( Color c );
		public Color GetTopOutlineColor();
		public void SetTopOutlineColor(Color c);
		public Color GetBottomOutlineColor();
		public void SetBottomOutlineColor(Color c);
		public Color GetLeftOutlineColor();
		public void SetLeftOutlineColor(Color c);
		public Color GetRightOutlineColor();
		public void SetRightOutlineColor(Color c);

	}
}
