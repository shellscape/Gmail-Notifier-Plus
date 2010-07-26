/*****************************************************
 *            Vista Controls for .NET 2.0
 * 
 * http://www.codeplex.com/vistacontrols
 * 
 * @author: Lorenz Cuno Klopfenstein
 * Licensed under Microsoft Community License (Ms-CL)
 * 
 *****************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace VistaControls.Native {

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT {
		public POINT(int x, int y) {
			X = x;
			Y = y;
		}
		public POINT(System.Drawing.Point p) {
			X = p.X;
			Y = p.Y;
		}
		public POINT(System.Drawing.PointF p) {
			X = (int)p.X;
			Y = (int)p.Y;
		}

		public int X;
		public int Y;
	}

}
