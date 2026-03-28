using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairMaker.Interfaces
{
	public interface ICircleCrosshair
	{
		public float GetCircleRadius();
		public void SetCircleRadius(float r);
		public float GetCircleThickness();
		public void SetCircleThickness(float t);
	}
}
