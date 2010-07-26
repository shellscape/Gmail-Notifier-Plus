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
	internal static class GDI {

		#region DC and Blitting

		[DllImport("gdi32.dll", ExactSpelling = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteDC(IntPtr hdc);

		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, BitBltOp dwRop);

		public enum BitBltOp : uint {
			SRCCOPY = 0x00CC0020,		/* dest = source                   */
			SRCPAINT = 0x00EE0086,		/* dest = source OR dest           */
			SRCAND = 0x008800C6,		/* dest = source AND dest          */
			SRCINVERT = 0x00660046,		/* dest = source XOR dest          */
			SRCERASE = 0x00440328,		/* dest = source AND (NOT dest )   */
			NOTSRCCOPY = 0x00330008,	/* dest = (NOT source)             */
			NOTSRCERASE = 0x001100A6,	/* dest = (NOT src) AND (NOT dest) */
			MERGECOPY = 0x00C000CA,		/* dest = (source AND pattern)     */
			MERGEPAINT = 0x00BB0226,	/* dest = (NOT source) OR dest     */
			PATCOPY = 0x00F00021,		/* dest = pattern                  */
			PATPAINT = 0x00FB0A09,		/* dest = DPSnoo                   */
			PATINVERT = 0x005A0049,		/* dest = pattern XOR dest         */
			DSTINVERT = 0x00550009,		/* dest = (NOT dest)               */
			BLACKNESS = 0x00000042,		/* dest = BLACK                    */
			WHITENESS = 0x00FF0062,		/* dest = WHITE                    */

			NOMIRRORBITMAP = 0x80000000,/* Do not Mirror the bitmap in this call */
			CAPTUREBLT = 0x40000000		/* Include layered windows */
		}

		[DllImport("user32.dll")]
		public static extern int FillRect(IntPtr hDC, [In] ref Native.RECT lprc, IntPtr hbr);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateSolidBrush(int crColor);

		#endregion

		#region GDI Object handling

		[DllImport("gdi32.dll", ExactSpelling = true)]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		[DllImport("gdi32.dll", ExactSpelling = true)]
		public static extern bool DeleteObject(IntPtr hObject);

		#endregion

	}
}
