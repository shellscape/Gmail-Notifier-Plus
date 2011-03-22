using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Shellscape.Utilities;

namespace GmailNotifierPlus.Forms {

	public partial class Toast : Form {

		#region .    API    

		[StructLayout(LayoutKind.Sequential)]
		public struct MARGINS {
			public int cxLeftWidth;
			public int cxRightWidth;
			public int cyTopHeight;
			public int cyBottomHeight;
			public MARGINS(int Left, int Right, int Top, int Bottom) {
				this.cxLeftWidth = Left;
				this.cxRightWidth = Right;
				this.cyTopHeight = Top;
				this.cyBottomHeight = Bottom;
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct RECT {
			// Fields
			[FieldOffset(12)]
			public int Bottom;
			[FieldOffset(0)]
			public int Left;
			[FieldOffset(8)]
			public int Right;
			[FieldOffset(4)]
			public int Top;

			// Methods
			public RECT(Rectangle rect) {
				this.Left = rect.Left;
				this.Top = rect.Top;
				this.Right = rect.Right;
				this.Bottom = rect.Bottom;
			}

			public RECT(int left, int top, int right, int bottom) {
				this.Left = left;
				this.Top = top;
				this.Right = right;
				this.Bottom = bottom;
			}

			public void Set() {
				this.Left = this.Top = this.Right = this.Bottom = 0;
			}

			public void Set(Rectangle rect) {
				this.Left = rect.Left;
				this.Top = rect.Top;
				this.Right = rect.Right;
				this.Bottom = rect.Bottom;
			}

			public void Set(int left, int top, int right, int bottom) {
				this.Left = left;
				this.Top = top;
				this.Right = right;
				this.Bottom = bottom;
			}

			public Rectangle ToRectangle() {
				return new Rectangle(this.Left, this.Top, this.Right - this.Left, this.Bottom - this.Top);
			}

			// Properties
			public int Height {
				get {
					return (this.Bottom - this.Top);
				}
			}

			public Size Size {
				get {
					return new Size(this.Width, this.Height);
				}
			}

			public int Width {
				get {
					return (this.Right - this.Left);
				}
			}
		}

		/// <summary>
		/// The NCCALCSIZE_PARAMS structure contains information that an application can use 
		/// while processing the WM_NCCALCSIZE message to calculate the size, position, and 
		/// valid contents of the client area of a window. 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]                   // This is the default layout for a structure
		public struct NCCALCSIZE_PARAMS {
			public RECT rect0, rect1, rect2;                    // Can't use an array here so simulate one
			public IntPtr lppos;
		}

		[DllImport("dwmapi.dll")]
		public static extern int DwmExtendFrameIntoClientArea(IntPtr hdc, ref MARGINS marInset);

		[DllImport("dwmapi.dll")]
		public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

		[DllImport("dwmapi.dll")]
		public static extern int DwmDefWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, out IntPtr result);

		#endregion

		private MARGINS _dwmMargins;
		private bool _setMargins;
		private bool _aeroEnabled;
		private Size _buttonSize = new Size(26, 22);

		private const int LeftControl = 9;
		private const int CenterControl = 10;
		private const int RightControl = 11;

		public Toast() {
			InitializeComponent();

			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.UpdateStyles();

			int enabled = 0;
			int response = DwmIsCompositionEnabled(ref enabled);

			_aeroEnabled = enabled == 1;
		}
		
		protected override void OnActivated(EventArgs e) {
			base.OnActivated(e);

			DwmExtendFrameIntoClientArea(this.Handle, ref _dwmMargins);
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			e.Graphics.Clear(_aeroEnabled ? Color.Transparent : SystemColors.Control);

			e.Graphics.FillRectangle(SystemBrushes.Control, 
				new Rectangle(
					_dwmMargins.cxLeftWidth,
					_dwmMargins.cyTopHeight,
					Width - _dwmMargins.cxRightWidth - _dwmMargins.cxLeftWidth,
					Height - _dwmMargins.cyBottomHeight - _dwmMargins.cyTopHeight
				)
			);

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			Font font = SystemFonts.CaptionFont;
			Utilities.GlassHelper.DrawText(e.Graphics, "  andrew@shellscape.org", font, new Rectangle(_dwmMargins.cxLeftWidth + 16, _dwmMargins.cxLeftWidth, 200, 16), this.ForeColor, TextFormatFlags.Default, Utilities.TextStyle.Glowing);

			using (Icon icon = ResourceHelper.GetIcon("gmail-classic.ico", 16)) {
				e.Graphics.DrawIcon(icon, new Rectangle(_dwmMargins.cxLeftWidth, _dwmMargins.cxLeftWidth, 16, 16));
			}

			// render window title
			VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);

			int totalButtonWidth = _buttonSize.Width * 3;
			int startX = ((Width - totalButtonWidth) / 2);
			int iconX = (_buttonSize.Width - 16) / 2;
			int iconY = (_buttonSize.Height - 16) / 2;
			int buttonY = this.Height - _buttonSize.Height;

			// render prev
			VisualStyleElement element = VisualStyleElement.CreateElement("TaskbandExtendedUI", LeftControl, 1);
			renderer.SetParameters(element);
			renderer.DrawBackground(e.Graphics, new Rectangle(startX, buttonY, _buttonSize.Width, _buttonSize.Height));

			using (Icon icon = ResourceHelper.GetIcon("Previous.ico")) {
				e.Graphics.DrawIcon(icon, new Rectangle(iconX + startX, iconY + buttonY, 16, 16));
				//ControlPaint.DrawImageDisabled(e.Graphics, icon.ToBitmap(), iconX + startX, iconY + buttonY, Color.Transparent);
			}

			startX += _buttonSize.Width;

			// render inbox
			element = VisualStyleElement.CreateElement("TaskbandExtendedUI", CenterControl, 1);
			renderer.SetParameters(element);
			renderer.DrawBackground(e.Graphics, new Rectangle(startX, buttonY, _buttonSize.Width, _buttonSize.Height));

			using (Icon icon = ResourceHelper.GetIcon("Inbox.ico")) {
				e.Graphics.DrawIcon(icon, new Rectangle(iconX + startX, iconY + buttonY, 16, 16));
			}

			startX += _buttonSize.Width;

			// render next
			element = VisualStyleElement.CreateElement("TaskbandExtendedUI", RightControl, 1);
			renderer.SetParameters(element);
			renderer.DrawBackground(e.Graphics, new Rectangle(startX, buttonY, _buttonSize.Width, _buttonSize.Height));

			using (Icon icon = ResourceHelper.GetIcon("Next.ico")) {
				e.Graphics.DrawIcon(icon, new Rectangle(iconX + startX, iconY + buttonY, 16, 16));
			}

			// render tiny close button
			renderer.SetParameters(VisualStyleElement.Window.SmallCloseButton.Normal);
			Size closeSize = renderer.GetPartSize(e.Graphics, ThemeSizeType.True);
			renderer.DrawBackground(e.Graphics, new Rectangle(Width - _dwmMargins.cxLeftWidth - closeSize.Width, 8, closeSize.Width, closeSize.Height));
		}

		protected override void WndProc(ref Message m) {
			int WM_NCCALCSIZE = 0x83;

			IntPtr result;

			int dwmHandled = DwmDefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam, out result);

			if (dwmHandled == 1) {
				m.Result = result;
				return;
			}

			if (m.Msg == WM_NCCALCSIZE && (int)m.WParam == 1) {
				NCCALCSIZE_PARAMS nccsp = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));

				// Adjust (shrink) the client rectangle to accommodate the border:
				nccsp.rect0.Top += 0;
				nccsp.rect0.Bottom += 0;
				nccsp.rect0.Left += 0;
				nccsp.rect0.Right += 0;

				if (!_setMargins) {
					//Set what client area would be for passing to DwmExtendIntoClientArea
					_dwmMargins.cyTopHeight = nccsp.rect2.Top - nccsp.rect1.Top;
					_dwmMargins.cxLeftWidth = nccsp.rect2.Left - nccsp.rect1.Left;
					_dwmMargins.cyBottomHeight = _buttonSize.Height + 2;
					_dwmMargins.cxRightWidth = nccsp.rect1.Right - nccsp.rect2.Right;
					_setMargins = true;
				}

				Marshal.StructureToPtr(nccsp, m.LParam, false);

				m.Result = IntPtr.Zero;
			}

			else {
				base.WndProc(ref m);
			}
		}

	}
}