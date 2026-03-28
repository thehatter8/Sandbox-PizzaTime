using Sandbox;
using System;

namespace CrosshairMaker.Helpers
{
	public class Bar
	{
		[KeyProperty]
		[Range( 0, 64, 0.1f )]
		public float length { get; set; } = 10f;
		[KeyProperty]
		[Range( 0, 64, 0.1f )]
		public float thickness { get; set; } = 3;
		[KeyProperty]
		[Range(0,64,0.1f)]
		public float offset { get; set; } = 5;
		public Bar() { }
		public Bar(float l, float t, float o) { length = l; thickness = t; offset = o; }
		public Rect ToRect(bool vertical = true) => vertical? new Rect( 0 , 0 , thickness, length) : new Rect( 0 , 0 , length, thickness );
		[KeyProperty,Title("Outline")]
		public bool BarOutline { get; set; } = false;
		[KeyProperty]
		[ColorUsage]
		public Color barColor { get; set; } = Color.White;
		[KeyProperty]
		[ColorUsage]
		[ShowIf(nameof( BarOutline ),true)]
		public Color outlineColor { get; set; } = Color.Black;
		[KeyProperty]
		[Range( 0, 32, 0.1f )]
		[ShowIf( nameof( BarOutline ), true )]
		public float outlineThickness = 2f;

	}
}
