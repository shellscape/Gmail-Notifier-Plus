using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;

namespace GmailNotifierPlus.Utilities {

	public class ImageHelper {

		public static Bitmap GetTrayIcon(Bitmap numbers) {

			Bitmap bitmap = new Bitmap(16, 16);
			Bitmap envelope = new Bitmap(16, 16, PixelFormat.Format32bppArgb);

			ColorMatrix cm = new ColorMatrix();
			cm.Matrix33 = 0.8f;

			try {
				using(Icon envelopeIcon = Resources.Icons.WindowSmall)
				using(Graphics iconGraphics = Graphics.FromImage(envelope)) {

					iconGraphics.DrawIcon(envelopeIcon, new Rectangle(0, 0, envelopeIcon.Width, envelopeIcon.Height));
				}
			}
			catch(ArgumentException) { }

			using(ImageAttributes ia = new ImageAttributes())
			using(Graphics graphics = Graphics.FromImage(bitmap)) {
			
				ia.SetColorMatrix(cm);

				Rectangle destRect = new Rectangle(0, Math.Max(0, 16 - envelope.Height), 16, 16);

				graphics.DrawImage(envelope, destRect, 0, 0, envelope.Width, envelope.Height, GraphicsUnit.Pixel, ia);

				graphics.DrawImage(numbers, 0, 2, numbers.Width, numbers.Height);
			}

			if(envelope != null){
				envelope.Dispose();
			}

			return bitmap;
		}

		public static Bitmap GetDigitIcon(int number) {

			// overlay icons MUST be 16x16. Stupid limitation by microsoft.
			Bitmap bitmap = new Bitmap(16, 16);

			using(Graphics graphics = Graphics.FromImage(bitmap)) {
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

				using(Bitmap numbers = GetNumbers(number)) {

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

		public static Bitmap GetNumbers(int number) {

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
