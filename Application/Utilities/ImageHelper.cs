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

		public static Icon GetDigitIcon(int number) {
			
			Bitmap bitmap = ResourceHelper.GetImage("Envelope.png");

			using (Graphics graphics = Graphics.FromImage(bitmap)) {
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

				using (Bitmap numbers = GetNumbers(number)) {
					int x = Math.Max(0, bitmap.Width - numbers.Width);
					int y = Math.Max(0, bitmap.Height - numbers.Height);

					graphics.DrawImage(numbers, x, y, numbers.Width, numbers.Height);
				}
			}

			Icon icon = Icon.FromHandle(bitmap.GetHicon());

			bitmap.Dispose();
			bitmap = null;

			return icon;
		}

		public static Bitmap GetNumbers(int number) {

			Bitmap result = null;
			SizeF sz;
			int blurAmount = 9;
			Color colorFore = Color.White;
			Color colorBack = Color.Black;
			String text = number.ToString();
			int increase = blurAmount / 2;

			if (text.Length == 1) {
				text = String.Concat(" ", text);
			}

			using (Font font = new Font("Segoe UI", 26, FontStyle.Bold, GraphicsUnit.Point, 0)) {

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
	}

}
