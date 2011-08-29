using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Shellscape.UI.Controls;

namespace GmailNotifierPlus.Controls {
	public class AboutPanel : DoubleBufferedPanel {
		
		public Boolean FirstRun { get; set; }

		private Bitmap _BackBuffer;

		protected override void OnPaintBackground(PaintEventArgs pevent) {
			//Don't allow the background to paint
		}

		protected override void OnPaint(PaintEventArgs e) {

			if (_BackBuffer == null) {
				_BackBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

				using (Bitmap about = Utilities.ResourceHelper.GetImage("About.png"))
				using (Graphics g = Graphics.FromImage(_BackBuffer)) {

					g.DrawImage(about, 0, 0, about.Width, about.Height);

					if (FirstRun) {
						using (Bitmap welcome = Utilities.ResourceHelper.GetImage("Welcome.png")) {
							g.DrawImage(welcome, 13, 119, welcome.Width, welcome.Height);

						}

						using (Bitmap start = Utilities.ResourceHelper.GetImage("GetStarted.png")) {
							g.DrawImage(start, 0, 248, start.Width, start.Height);
						}
					}

					if (!FirstRun) {

						String copy = String.Join("\n",
							String.Concat("Copyright © 2011-", DateTime.Now.Year, " Andrew Powell, Shellscape Software"),
							"Originally Created by Baptiste Girod",
							"Gmail Notifier Plus is in no way associated with Gmail.",
							"Gmail is a registered trademark of Google, Inc."
						);

						StringFormat format = new StringFormat() {
							LineAlignment = StringAlignment.Far,
							Alignment = StringAlignment.Far
						};

						using (Font font = new Font("Segoe UI", 7, FontStyle.Regular, GraphicsUnit.Point, 0))
						using (Bitmap copyImage = Utilities.ImageHelper.FancyText(copy, 4, ColorTranslator.FromHtml("#333"), Color.White, font, format)) {
							g.DrawImage(copyImage, this.Width - copyImage.Width - 10, about.Height - copyImage.Height - 10, copyImage.Width, copyImage.Height);
						}
					}

					using (Bitmap versionImage = GetVersion()) {
						g.DrawImage(versionImage, this.Width - versionImage.Width - 20, 164, versionImage.Width, versionImage.Height);
					}

				}
			}

			e.Graphics.DrawImageUnscaled(_BackBuffer, 0, 0);
		}

		private Bitmap GetVersion() {

			String version = String.Concat("version ", Shellscape.UpdateManager.Current.CurrentVersion);
			Bitmap result = null;
			RectangleF bounds;
			RectangleF rect = this.ClientRectangle;
			Font font = new Font("Arial Black", 10, FontStyle.Regular, GraphicsUnit.Point, 0);
			float dpi;

			rect.Offset(2, 0);

			using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
				dpi = g.DpiY;
			}

			using (GraphicsPath path = GetStringPath(version, dpi, rect, font, StringFormat.GenericTypographic)) {

				bounds = path.GetBounds();
				
				result = new Bitmap((int)bounds.Width + 8, (int)bounds.Height + 12);

				using (Graphics g = Graphics.FromImage(result)) {

					g.SmoothingMode = SmoothingMode.AntiAlias;
					RectangleF off = rect;
					off.Offset(2, 3);

					using (GraphicsPath offPath = GetStringPath(version, dpi, off, font, StringFormat.GenericTypographic))
					using (Brush b = new SolidBrush(Color.FromArgb(40, 0, 0, 0))) {
						g.FillPath(b, offPath);
						b.Dispose();

						using (Pen pen = new Pen(Color.White)) {
							pen.Width = 4;
							g.DrawPath(pen, path);
						}

						g.FillPath(Brushes.Black, path);

						//using (GraphicsPath penPath = GetStringPath(version, dpi, rect, font, StringFormat.GenericTypographic, 0)) {
						//  g.FillPath(Brushes.Black, penPath);
						//}
					}
				}
			}

			font.Dispose();

			return result;

		}

		GraphicsPath GetStringPath(string s, float dpi, RectangleF rect, Font font, StringFormat format) {
			GraphicsPath path = new GraphicsPath();
			// Convert font size into appropriate coordinates
			float emSize = dpi * (font.SizeInPoints) / 72;
			path.AddString(s, font.FontFamily, (int)font.Style, emSize, rect, format);

			return path;
		}

	}
}
