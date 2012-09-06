using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GmailNotifierPlus.Utilities {

	public static class ImageHelper {

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		extern static bool DestroyIcon(IntPtr handle);

		private static Dictionary<int, Icon> _iconCache;

		static ImageHelper() {
			_iconCache = new Dictionary<int, Icon>();

			// prepare the cache. 1 - 10 is going to be pretty common. might as well create these now.
			for(var i = 1; i < 11; i++) {
				GetNumberIcon(i);
			}
		}

		// we'll have to do this with a method since static classes cant have destructors.
		public static void Cleanup() {
			foreach(var kvp in _iconCache) {
				kvp.Value.Dispose();
			}
			_iconCache.Clear();
		}

		public static Icon GetNumberIcon(int number) {

			try {

				if(_iconCache.ContainsKey(number)) {
					return _iconCache[number];
				}
				
				using(Bitmap numbers = ImageHelper.GetNumberImage(number)) {

					Icon numberIcon = null;
					
					if(numbers != null) {
						numberIcon = ImageHelper.IconFromBitmap(numbers);
					}

					if(numberIcon != null) {
						_iconCache.Add(number, numberIcon);
					}

					return numberIcon;
				}
			}
			catch(System.ComponentModel.Win32Exception ex) {
				// trying to solve the "The operation completed successfully" exception on Icon ctor.
				// though with the addition of the icon cache, I doubt this will be a problem any more.
				if(ex.NativeErrorCode != 0) {
					return null;
				}
				else {
					throw;
				}
			}

		}

		public static Icon IconFromBitmap(Bitmap bitmap) {

			IntPtr hIcon = bitmap.GetHicon();
			using(Icon icon = Icon.FromHandle(hIcon)) { // we can use Icon's dispose method because it calls DestroyIcon for us.
				return icon.Clone() as Icon;
			}
		}

		public static Bitmap GetNumberImage(int number) {

			// overlay icons MUST be 16x16. Stupid limitation by microsoft.
			Bitmap bitmap = new Bitmap(16, 16);

			using(Graphics graphics = Graphics.FromImage(bitmap)) {
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

				using(Bitmap numbers = DrawNumbers(number)) {

					// yeah, most of these numbers are arbitrary, based on the font we're using 
					// and the numbers used to calc width and height.

					int width = numbers.Width;
					int height = numbers.Height;
					double percent = width > height ?
						19.0 / width :
						19.0 / height;

					width = (int)(width * percent);
					height = (int)(height * percent);

					int x = number.ToString().Length > 1 ?
						Math.Max(-3, bitmap.Width - numbers.Width) :
						1;
					int y = Math.Max(0, (bitmap.Height - numbers.Height) / 2);

					graphics.DrawImage(numbers, x, -2, width, height);
				}
			}

			return bitmap;
		}

		public static Bitmap DrawNumbers(int number) {

			Bitmap result = null;
			SizeF sz;
			int blurAmount = 9;
			Color colorFore = Color.White;
			Color colorBack = ColorTranslator.FromHtml("#333");
			String text = number.ToString();
			int increase = blurAmount / 2;

			if(text.Length == 1) {
				text = String.Concat(" ", text);
			}

			using(Font font = new Font("Segoe UI", 26, FontStyle.Bold, GraphicsUnit.Point, 0)) {
				//using (Font font = new Font("Segoe UI", 8, FontStyle.Bold, GraphicsUnit.Point, 0)) {

				using(Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
					sz = g.MeasureString(text, font);
				}

				using(Bitmap bitmap = new Bitmap((int)sz.Width, (int)sz.Height))
				using(Graphics g = Graphics.FromImage(bitmap))
				using(SolidBrush brushBack = new SolidBrush(Color.FromArgb(16, colorBack.R, colorBack.G, colorBack.B)))
				using(SolidBrush brushFore = new SolidBrush(colorFore)) {

					g.SmoothingMode = SmoothingMode.HighQuality;
					g.InterpolationMode = InterpolationMode.HighQualityBilinear;
					g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

					g.DrawString(text, font, brushBack, 0, 0);

					result = new Bitmap(bitmap.Width + increase, bitmap.Height + increase);

					using(Graphics graphicsOut = Graphics.FromImage(result)) {
						graphicsOut.SmoothingMode = SmoothingMode.HighQuality;
						graphicsOut.InterpolationMode = InterpolationMode.HighQualityBilinear;
						graphicsOut.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

						// here for debugging purposes.
						//graphicsOut.FillRectangle(Brushes.White, 0, 0, result.Width, result.Height);

						for(int x = 0; x <= blurAmount; x++)
							for(int y = 0; y <= blurAmount; y++)
								graphicsOut.DrawImageUnscaled(bitmap, x, y);

						graphicsOut.DrawString(text, font, brushFore, blurAmount / 2, blurAmount / 2);
					}
				}
			}

			return result;
		}

	}

}
