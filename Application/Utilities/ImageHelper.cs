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

		/// <summary>
		/// Creates an in-memory icon for a specified number.
		/// </summary>
		public static Icon GetDigitIcon(int number) {

			Rectangle rect = new Rectangle(0, 0, 64, 64);
			Bitmap bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
			Graphics graphics = Graphics.FromImage(bitmap);
			Font font = new Font("Segoe UI", 50, FontStyle.Bold, GraphicsUnit.Point, 0);
			StringFormat stringFormat = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };

			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

			graphics.FillRectangle(Brushes.LightGreen, rect);

			//DrawShadow(ref graphics, ref font, number.ToString());

			//graphics.DrawString(number.ToString(), font, Brushes.White, rect, stringFormat);
			//DrawHaloText(ref graphics, ref font, number.ToString());



			GraphicsPath path = new GraphicsPath();
			Pen pen = new Pen(Color.Black, 2.0f);

			path.AddString(number.ToString(), font.FontFamily, (int)font.Style, (int)font.Size, new Point(0, 0), stringFormat);

			graphics.DrawPath(pen, path);

			path.Reset();
			path.AddString(number.ToString(), font.FontFamily, (int)font.Style, (int)font.Size, new Point(0, 0), stringFormat);

			graphics.FillPath(Brushes.White, path);

			pen.Dispose();
			path.Dispose();



			graphics.Dispose();
			graphics = null;

			Icon icon = Icon.FromHandle(bitmap.GetHicon());

			bitmap.Dispose();
			bitmap = null;

			return icon;
		}

		private static void DrawShadow(ref Graphics graphics, ref Font font, String text) {

			//Make a small bitmap

			Rectangle rect = new Rectangle(0, 0, 64, 64);
			Bitmap bm = new Bitmap(Convert.ToInt32(rect.Width * 0.9), Convert.ToInt32(rect.Height * 0.9), PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bm);
			StringFormat stringFormat = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };
			//Brush halfsies = new SolidBrush(Color.FromArgb(100, Color.Black));

			g.TextRenderingHint = TextRenderingHint.AntiAlias;

			//this matrix zooms the text out to 1/4 size and offsets it by a little right and down
			//Matrix mx = new Matrix(0.9f, 0, 0, 0.9f, 3, 4);
			//Matrix mx = new Matrix(0.9f, 0, 0, 0.9f, 0, 0);

			//g.Transform = mx;
			g.DrawString(text, font, Brushes.Black, 0, 0, stringFormat);
			//g.ResetTransform();

			//mx = new Matrix(1.2f, 0, 0, 1.2f, -4, -3);

			//g.Transform = mx;
			//g.DrawString(text, font, Brushes.Black, 0, 0, stringFormat);

			//halfsies.Dispose();

			g.Dispose();
			g = null;

			//The destination Graphics uses a high quality mode
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

			//The small image is blown up to fill the main client rectangle
			graphics.DrawImage(bm, rect, 0, 0, bm.Width, bm.Height, GraphicsUnit.Pixel);

			//finally, the text is drawn on top
			//graphics.DrawString(text, font, Brushes.White, 0, 0, StringFormat.GenericTypographic);

			bm.Dispose();

		}

		private static void DrawHaloText(ref Graphics graphics, ref Font font, String text) {

			Bitmap bitmap = new Bitmap(64, 64, PixelFormat.Format32bppArgb);
			Rectangle rect = new Rectangle(0, 0, 64, 64);
			StringFormat stringFormat = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };
			GraphicsPath path = new GraphicsPath();

			path.AddString(text, font.FontFamily, (int)font.Style, font.Size, rect, stringFormat);

			using (Graphics g = Graphics.FromImage(bitmap)) {

				Matrix mx = new Matrix(1.0f / 5, 0, 0, 1.0f / 5, -(1.0f / 5), -(1.0f / 5));
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.FillRectangle(Brushes.Transparent, rect);
				g.Transform = mx;

				Pen pen = new Pen(Color.Black, 3);

				g.DrawPath(pen, path);
				g.FillPath(Brushes.Black, path);

				mx.Dispose();
				mx = null;

				pen.Dispose();
				pen = null;

			}

			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

			graphics.DrawImage(bitmap, rect, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);

			graphics.SmoothingMode = SmoothingMode.Default;
			graphics.FillPath(Brushes.White, path);

			path.Dispose();
			path = null;

			bitmap.Dispose();
			bitmap = null;
		}

	}
}
