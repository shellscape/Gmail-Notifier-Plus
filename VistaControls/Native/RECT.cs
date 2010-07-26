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
	public struct RECT {
		public RECT(int left, int top, int right, int bottom) {
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public RECT(System.Drawing.Rectangle rect) {
			Left = rect.X;
			Top = rect.Y;
			Right = rect.Right;
			Bottom = rect.Bottom;
		}

		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		public int Width {
			get {
				return Right - Left;
			}
			set {
				Right = Left + value;
			}
		}
		public int Height {
			get {
				return Bottom - Top;
			}
			set {
				Bottom = Top + value;
			}
		}

		public System.Drawing.Rectangle ToRectangle() {
			return new System.Drawing.Rectangle(Left, Top, Right - Left, Bottom - Top);
		}

	}
}
