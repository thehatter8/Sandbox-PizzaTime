using CrosshairMaker.Animator;
using CrosshairMaker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairMaker.Animator
{
	public sealed class AnimationValueSetter
	{
		//Properties
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		/// <summary>
		/// with CrosshairAnimation, links the floats values with the corresponding setter
		/// </summary>
		public readonly List<Action<IAnimatableCrosshair,float>> FloatSetters ;
		/// <summary>
		/// with CrosshairAnimation, links the ints values with the corresponding setter
		/// </summary>
		public readonly List<Action<IAnimatableCrosshair, int>> IntSetters ;
		/// <summary>
		/// with CrosshairAnimation, links the color values with the corresponding setter
		/// </summary>
		public readonly List<Action<IAnimatableCrosshair, Color>> ColorSetters;

		public readonly List<Func<IAnimatableCrosshair, float>> FloatGetters;
		public readonly List<Func<IAnimatableCrosshair,int>> IntGetters;
		public readonly List<Func<IAnimatableCrosshair,Color>> ColorGetters;
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Properties
		private AnimationValueSetter(
			IEnumerable<Action<IAnimatableCrosshair, float>>? fas = null ,
			IEnumerable<Action<IAnimatableCrosshair, int>>? ias = null ,
			IEnumerable<Action<IAnimatableCrosshair, Color>>? cas = null,
			List<Func<IAnimatableCrosshair, float>>? fgf = null ,
			List<Func<IAnimatableCrosshair,int>> igf = null ,
			List<Func<IAnimatableCrosshair,Color>> cgf = null
			)
		{
			FloatSetters = fas?.ToList() ?? new(0);
			IntSetters = ias?.ToList() ?? new( 0 );
			ColorSetters = cas?.ToList() ?? new( 0 );
			FloatGetters = fgf?.ToList() ?? new( 0 );
			IntGetters = igf?.ToList() ?? new( 0 );
			ColorGetters = cgf?.ToList() ?? new( 0 );
		}
		public static AnimationValueSetter Empty => new();
		public static AnimationValueSetter FromActions ( 
			IEnumerable<Action<IAnimatableCrosshair, float>>? floatSetters = null,
			IEnumerable<Action<IAnimatableCrosshair, int>>? intSetters = null,
			IEnumerable<Action<IAnimatableCrosshair, Color>>? colorSetters = null ,
			List<Func<IAnimatableCrosshair, float>>? floatGetters = null,
			List<Func<IAnimatableCrosshair,int>> intGetters = null,
			List<Func<IAnimatableCrosshair,Color>> colorGetters = null 
			)
			=> new(floatSetters,intSetters,colorSetters,floatGetters,intGetters,colorGetters);
		public int[] Sizes => new int[] { FloatSetters.Count , IntSetters.Count , ColorSetters.Count };

		public override string ToString()
		{
			return $"[AnimationValueSetter] [{(FloatSetters.Count, IntSetters.Count, ColorSetters.Count)}][{(FloatGetters.Count, IntGetters.Count, ColorGetters.Count)}]";
		}
	}
}
