using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace GmailNotifierPlus.Utilities {

	public enum TextStyle {
		Normal,
		Glowing
	}

	public static class GlassHelper {


		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

		[DllImport("gdi32.dll", ExactSpelling = true)]
		private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool DeleteObject(IntPtr hObject);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool DeleteDC(IntPtr hdc);

		[DllImport("gdi32.dll")]
		private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
		
		[DllImport("UxTheme.dll", CharSet = CharSet.Unicode)]
		private static extern int DrawThemeTextEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string text, int iCharCount, int dwFlags, ref RECT pRect, ref DTTOPTS pOptions);
		
		[DllImport("gdi32.dll")]
		private static extern IntPtr CreateDIBSection(IntPtr hdc, BITMAPINFO pbmi, uint iUsage, int ppvBits, IntPtr hSection, uint dwOffset);

		[StructLayout(LayoutKind.Sequential)]
		private struct DTTOPTS {
			public int dwSize;
			public int dwFlags;
			public int crText;
			public int crBorder;
			public int crShadow;
			public int iTextShadowType;
			public POINT ptShadowOffset;
			public int iBorderSize;
			public int iFontPropId;
			public int iColorPropId;
			public int iStateId;
			public bool fApplyOverlay;
			public int iGlowSize;
			public int pfnDrawTextCallback;
			public IntPtr lParam;
		}

		private const int DTT_COMPOSITED = 8192;
		private const int DTT_GLOWSIZE = 2048;
		private const int DTT_TEXTCOLOR = 1;

		[StructLayout(LayoutKind.Sequential)]
		private struct POINT {
			public POINT(int x, int y) {
				this.x = x;
				this.y = y;
			}

			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		private class BITMAPINFO {
			public int biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
			public int biCompression;
			public int biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public int biClrUsed;
			public int biClrImportant;
			public byte bmiColors_rgbBlue;
			public byte bmiColors_rgbGreen;
			public byte bmiColors_rgbRed;
			public byte bmiColors_rgbReserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public RECT(int left, int top, int right, int bottom) {
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}

			public RECT(Rectangle rectangle) {
				Left = rectangle.X;
				Top = rectangle.Y;
				Right = rectangle.Right;
				Bottom = rectangle.Bottom;
			}

			public Rectangle ToRectangle() {
				return new Rectangle(Left, Top, Right - Left, Bottom - Top);
			}

			public override string ToString() {
				return "Left: " + Left + ", " + "Top: " + Top + ", Right: " + Right + ", Bottom: " + Bottom;
			}
		}
	
		public static void DrawText(Graphics graphics, string text, Font font, Rectangle bounds, Color color, TextFormatFlags flags) {
			DrawText(graphics, text, font, bounds, color, flags, TextStyle.Normal);
		}

		public static void DrawText(Graphics graphics, string text, Font font, Rectangle bounds, Color color, TextFormatFlags flags, TextStyle textStyle) {

			if (!VisualStyleRenderer.IsSupported) {
				TextRenderer.DrawText(graphics, text, font, bounds, color, flags);
				return;
			}
			
			IntPtr primaryHdc = graphics.GetHdc();

			// Create a memory DC so we can work offscreen
			IntPtr memoryHdc = CreateCompatibleDC(primaryHdc);

			// Create a device-independent bitmap and select it into our DC
			BITMAPINFO info = new BITMAPINFO();
			info.biSize = Marshal.SizeOf(info);
			info.biWidth = bounds.Width;
			info.biHeight = -bounds.Height;
			info.biPlanes = 1;
			info.biBitCount = 32;
			info.biCompression = 0; // BI_RGB
			IntPtr dib = CreateDIBSection(primaryHdc, info, 0, 0, IntPtr.Zero, 0);
			SelectObject(memoryHdc, dib);

			// Create and select font
			IntPtr fontHandle = font.ToHfont();
			SelectObject(memoryHdc, fontHandle);

			// Draw glowing text
			VisualStyleRenderer renderer = new VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.Window.Caption.Active);
			DTTOPTS dttOpts = new DTTOPTS();
			dttOpts.dwSize = Marshal.SizeOf(typeof(DTTOPTS));

			if (textStyle == TextStyle.Glowing) {
				dttOpts.dwFlags = DTT_COMPOSITED | DTT_GLOWSIZE | DTT_TEXTCOLOR;
			}
			else {
				dttOpts.dwFlags = DTT_COMPOSITED | DTT_TEXTCOLOR;
			}
			dttOpts.crText = ColorTranslator.ToWin32(color);
			dttOpts.iGlowSize = 8; // This is about the size Microsoft Word 2007 uses
			RECT textBounds = new RECT(0, 0, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
			DrawThemeTextEx(renderer.Handle, memoryHdc, 0, 0, text, -1, (int)flags, ref textBounds, ref dttOpts);

			// Copy to foreground
			const int SRCCOPY = 0x00CC0020;
			BitBlt(primaryHdc, bounds.Left, bounds.Top, bounds.Width, bounds.Height, memoryHdc, 0, 0, SRCCOPY);

			// Clean up
			DeleteObject(fontHandle);
			DeleteObject(dib);
			DeleteDC(memoryHdc);

			graphics.ReleaseHdc(primaryHdc);
		}

	}
}