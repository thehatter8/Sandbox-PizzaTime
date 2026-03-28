using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Helpers
{
	public static class TupleI4Extentions
	{
		//Converters
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public static (int, int,int,int) ToInt( this (byte, byte,byte,byte) a ) => (a.Item1, a.Item2,a.Item3 ,a.Item4);
		public static (int, int,int,int) ToInt( this (sbyte, sbyte,sbyte,sbyte) a ) => (a.Item1, a.Item2,a.Item3 ,a.Item4);
		public static (int, int,int,int) ToInt( this (short, short, short, short) a ) => (a.Item1, a.Item2,a.Item3 ,a.Item4);
		public static (int, int,int,int) ToInt( this (ushort, ushort, ushort, ushort) a ) => (a.Item1, a.Item2,a.Item3 ,a.Item4);
		public static (int, int,int,int) ToInt( this (float, float, float, float) a ) => ((int)a.Item1, (int)a.Item2, (int)a.Item3 , (int)a.Item4);

		public static int[] ToArray(this (int, int, int, int) self) => new int[] { self.Item1, self.Item2, self.Item3, self.Item4 };
		private static (int, int, int, int) ToTuple4( this int[] i4 ) => (i4[0], i4[1], i4[2], i4[3]);
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Converters

		//Vector4Int
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		public static (int, int, int, int) Substract( this (int, int, int, int) left, (int, int, int, int) right ) =>
			(left.Item1 - right.Item1, left.Item2 - right.Item2, left.Item3 - right.Item3, left.Item4 - right.Item4);
		public static (int, int, int, int) Add( this (int, int, int, int) left, (int, int, int, int) right ) =>
			(left.Item1 + right.Item1, left.Item2 + right.Item2, left.Item3 + right.Item3, left.Item4 + right.Item4);
		public static (int, int, int, int) Multiply( this (int, int, int, int) self, int multiplier )
		{
			int[] res = self.ToArray();
			for ( int i = 0; i < 4; i++ ) res[i] *= multiplier;
			return res.ToTuple4();
		}
		public static (int, int, int, int) Divide( this (int, int, int, int) self, int divider )
		{
			int[] res = self.ToArray();
			for ( int i = 0; i < 4; i++ ) res[i] /= divider;
			return res.ToTuple4();
		}
		//-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-//
		//Vector4Int
	}
}
