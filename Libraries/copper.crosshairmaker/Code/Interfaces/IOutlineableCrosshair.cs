using CrosshairMaker.Interfaces;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairMaker.Interfaces
{
	public interface IOutlineableCrosshair : IHudPaintable
	{
		public bool GetOutline();
		public void SetOutline( bool o );
		public float GetOutlineThickness();
		public void SetOutlineThickness(float t);
		public Color GetOutlineColor();
		public void SetOutlineColor( Color o );
	}
}
