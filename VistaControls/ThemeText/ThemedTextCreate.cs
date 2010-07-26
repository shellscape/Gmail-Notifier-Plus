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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VistaControls.ThemeText.Options;
using System.Windows.Forms.VisualStyles;

namespace VistaControls.ThemeText {
	
	public partial class ThemedText : IDisposable {

		IntPtr _TextHDC;
		Rectangle _TextHDCBounds;

		internal ThemedText(IntPtr textHdc, Rectangle bounds) {
			_TextHDC = textHdc;
			_TextHDCBounds = bounds;
		}


		public static ThemedText CreateWithGlow(Graphics g, string text, Font font, Padding internalBounds,
			Rectangle bounds, Color color, TextFormatFlags formatFlags, int glowSize) {

			return Create(g, text, font, internalBounds, bounds, color, formatFlags,
				new IThemeTextOption[] { new GlowOption(glowSize) }
			);
		}

		public static ThemedText Create(Graphics g, string text, Font font, Padding internalBounds,
			Rectangle bounds, Color color, TextFormatFlags formatFlags, IThemeTextOption[] options) {

			//Get HDC and create a compatible HDC
			IntPtr outputHdc = g.GetHdc();

			IntPtr DIB;
			IntPtr compatHdc = CreateNewHDC(outputHdc, bounds, out DIB);

			//Draw
			DrawOnHDC(compatHdc, text, font, internalBounds, bounds, color, formatFlags, options);

			//Clean up
			CleanUpHDC(DIB);
			g.ReleaseHdc(outputHdc);

			//Return the wrapped HDC
			return new ThemedText(compatHdc, bounds);
		}

		public void Update(Graphics g, string text, Font font, Padding internalBounds,
			Rectangle bounds, Color color, TextFormatFlags formatFlags, IThemeTextOption[] options) {

			IntPtr compatHdc = this._TextHDC;

			if (bounds.Equals(_TextHDCBounds)) {
				//Same bounds, reuse HDC and Clear it
				IntPtr hClearBrush = Native.GDI.CreateSolidBrush(ColorTranslator.ToWin32(Color.Black));
				Native.RECT cleanRect = new Native.RECT(bounds);
				Native.GDI.FillRect(compatHdc, ref cleanRect, hClearBrush);
				Native.GDI.DeleteObject(hClearBrush);
			}
			else {
				//Other bounds, create new HDC
				IntPtr outputHdc = g.GetHdc();
				IntPtr DIB;
				compatHdc = CreateNewHDC(outputHdc, bounds, out DIB);

				//Store new data
				_TextHDC = compatHdc;
				_TextHDCBounds = bounds;

				//Clear up
				CleanUpHDC(DIB);
				g.ReleaseHdc(outputHdc);
			}

			DrawOnHDC(compatHdc, text, font, internalBounds, bounds, color, formatFlags, options);
		}

		private static IntPtr CreateNewHDC(IntPtr outputHdc, Rectangle bounds, out IntPtr DIB) {
			IntPtr compatHdc = Native.GDI.CreateCompatibleDC(outputHdc);

			//Create a DIB-Bitmap on which to draw (DIB use top-down ref system, thus we set negative height)
			Native.Bitmap.BITMAPINFO info = new Native.Bitmap.BITMAPINFO();
			info.biSize = Marshal.SizeOf(info);
			info.biWidth = bounds.Width;
			info.biHeight = -bounds.Height;
			info.biPlanes = 1;
			info.biBitCount = 32;
			info.biCompression = 0;
			DIB = Native.Bitmap.CreateDIBSection(outputHdc, ref info, 0, 0, IntPtr.Zero, 0);
			Native.GDI.SelectObject(compatHdc, DIB);

			return compatHdc;
		}

		private static void DrawOnHDC(IntPtr compatHdc, string text, Font font, Padding internalBounds,
			Rectangle bounds, Color color, TextFormatFlags formatFlags, IThemeTextOption[] options) {

			//Create the Font to use
			IntPtr hFont = font.ToHfont();
			Native.GDI.SelectObject(compatHdc, hFont);

			//Get theme renderer
			VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);

			//Prepare options
			NativeMethods.DTTOPTS dttOpts = new NativeMethods.DTTOPTS();
			dttOpts.dwSize = Marshal.SizeOf(dttOpts);
			dttOpts.dwFlags = NativeMethods.DTTOPSFlags.DTT_COMPOSITED | NativeMethods.DTTOPSFlags.DTT_TEXTCOLOR;
			dttOpts.crText = ColorTranslator.ToWin32(color);
			foreach (IThemeTextOption op in options)
				op.Apply(ref dttOpts);

			//Set full bounds with padding
			Native.RECT RECT = new Native.RECT(internalBounds.Left, internalBounds.Top,
				bounds.Width - internalBounds.Right, bounds.Height - internalBounds.Bottom);

			//Draw
			int ret = NativeMethods.DrawThemeTextEx(renderer.Handle, compatHdc, 0, 0, text, -1, (int)formatFlags, ref RECT, ref dttOpts);
			if (ret != 0)
				Marshal.ThrowExceptionForHR(ret);

			//Clean up
			Native.GDI.DeleteObject(hFont);
		}

		private static void CleanUpHDC(IntPtr DIB) {
			Native.GDI.DeleteObject(DIB);
		}

		#region IDisposable Members

		public void Dispose() {
			Native.GDI.DeleteDC(_TextHDC);
		}

		#endregion

	}

}
