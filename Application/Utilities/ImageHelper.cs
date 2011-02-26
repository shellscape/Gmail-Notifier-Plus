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

			ColorMatrix cm = new ColorMatrix();
			cm.Matrix33 = 0.8f;

			using (ImageAttributes ia = new ImageAttributes())
			using (Graphics graphics = Graphics.FromImage(bitmap))
			using (Icon envelopeIcon = Utilities.ResourceHelper.GetIcon("gmail-classic.ico"))
			using (Bitmap envelope = envelopeIcon.ToBitmap()) {
				ia.SetColorMatrix(cm);

				Rectangle destRect = new Rectangle(0, Math.Max(0, 16 - envelope.Height), 16, 16);

				graphics.DrawImage(envelope, destRect, 0, 0, envelope.Width, envelope.Height, GraphicsUnit.Pixel, ia);

				graphics.DrawImage(numbers, 0, 0, numbers.Width, numbers.Height);
			}

			return bitmap;
		}

		public static Bitmap GetDigitIcon(int number) {

			// overlay icons MUST be 16x16. Stupid limitation by microsoft.
			Bitmap bitmap = new Bitmap(16, 16);
			//Bitmap bitmap = ResourceHelper.GetImage("Envelope.png");

			using (Graphics graphics = Graphics.FromImage(bitmap)) {
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

				// for testing purposes
				//graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, 16, 16));

				using (Bitmap numbers = GetNumbers(number)) {
					//int x = Math.Max(0, bitmap.Width - numbers.Width);
					//int y = Math.Max(0, bitmap.Height - numbers.Height);

					//graphics.DrawImage(numbers, x, y, numbers.Width, numbers.Height);

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

			if (text.Length == 1) {
				text = String.Concat(" ", text);
			}

			using (Font font = new Font("Segoe UI", 26, FontStyle.Bold, GraphicsUnit.Point, 0)) {
				//using (Font font = new Font("Segoe UI", 8, FontStyle.Bold, GraphicsUnit.Point, 0)) {

				using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
					sz = g.MeasureString(text, font);
				}

				using (Bitmap bitmap = new Bitmap((int)sz.Width, (int)sz.Height))
				using (Graphics g = Graphics.FromImage(bitmap))
				using (SolidBrush brushBack = new SolidBrush(Color.FromArgb(16, colorBack.R, colorBack.G, colorBack.B)))
				using (SolidBrush brushFore = new SolidBrush(colorFore)) {

					g.SmoothingMode = SmoothingMode.HighQuality;
					g.InterpolationMode = InterpolationMode.HighQualityBilinear;
					g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

					g.DrawString(text, font, brushBack, 0, 0);

					result = new Bitmap(bitmap.Width + increase, bitmap.Height + increase);

					using (Graphics graphicsOut = Graphics.FromImage(result)) {
						graphicsOut.SmoothingMode = SmoothingMode.HighQuality;
						graphicsOut.InterpolationMode = InterpolationMode.HighQualityBilinear;
						graphicsOut.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

						// here for debugging purposes.
						//graphicsOut.FillRectangle(Brushes.White, 0, 0, result.Width, result.Height);

						for (int x = 0; x <= blurAmount; x++)
							for (int y = 0; y <= blurAmount; y++)
								graphicsOut.DrawImageUnscaled(bitmap, x, y);

						graphicsOut.DrawString(text, font, brushFore, blurAmount / 2, blurAmount / 2);
					}
				}
			}

			return result;
		}

		// TODO - GetNumbers should use this.

		public static Bitmap FancyText(String text, int blurAmount, Color colorFore, Color colorBack, Font font, StringFormat format) {

			Bitmap result = null;
			SizeF sz;
			int increase = blurAmount / 2;

			if (text.Length == 1) {
				text = String.Concat(" ", text);
			}

			using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
				sz = g.MeasureString(text, font);
			}

			using (Bitmap bitmap = new Bitmap((int)sz.Width, (int)sz.Height))
			using (Graphics g = Graphics.FromImage(bitmap))
			using (SolidBrush brushBack = new SolidBrush(Color.FromArgb(16, colorBack.R, colorBack.G, colorBack.B)))
			using (SolidBrush brushFore = new SolidBrush(colorFore)) {

				g.SmoothingMode = SmoothingMode.HighQuality;
				g.InterpolationMode = InterpolationMode.HighQualityBilinear;
				g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

				if (format != null) {
					RectangleF rect = new RectangleF(new PointF(0, 0), sz);
					g.DrawString(text, font, brushBack, rect, format);
				}
				else {
					g.DrawString(text, font, brushBack, 0, 0);
				}

				result = new Bitmap(bitmap.Width + increase, bitmap.Height + increase);

				using (Graphics graphicsOut = Graphics.FromImage(result)) {
					graphicsOut.SmoothingMode = SmoothingMode.HighQuality;
					graphicsOut.InterpolationMode = InterpolationMode.HighQualityBilinear;
					graphicsOut.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

					// here for debugging purposes.
					//graphicsOut.FillRectangle(Brushes.White, 0, 0, result.Width, result.Height);

					for (int x = 0; x <= blurAmount; x++)
						for (int y = 0; y <= blurAmount; y++)
							graphicsOut.DrawImageUnscaled(bitmap, x, y);

					if (format != null) {
						RectangleF rect = new RectangleF(new PointF(blurAmount / 2, blurAmount / 2), sz);
						g.DrawString(text, font, brushBack, rect, format);
						graphicsOut.DrawString(text, font, brushFore, rect, format);
					}
					else {
						graphicsOut.DrawString(text, font, brushFore, blurAmount / 2, blurAmount / 2);
					}
				}
			}

			return result;
		}
	}

}
