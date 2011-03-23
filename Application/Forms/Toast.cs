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

using Shellscape;
using Shellscape.Utilities;

using System.Diagnostics;

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

		private class State {
			public const int Normal = 1;
			public const int Hot = 2;
			public const int Pressed = 3;
			public const int Disabled = 4;
		}

		private MARGINS _dwmMargins;
		private Boolean _setMargins;
		private Boolean _aeroEnabled;
		private Size _buttonSize = new Size(26, 22);
		private Boolean _rectsInitialized = false;

		private const int LeftControl = 9;
		private const int CenterControl = 10;
		private const int RightControl = 11;

		private Rectangle _rectClose = Rectangle.Empty;
		private Rectangle _rectNext = Rectangle.Empty;
		private Rectangle _rectPrev = Rectangle.Empty;
		private Rectangle _rectInbox = Rectangle.Empty;
		private Rectangle _rectClient = Rectangle.Empty;

		private int _stateClose = 1;
		private int _statePrev = 1;
		private int _stateInbox = 1;
		private int _stateNext = 1;

		private Timer _closeTimer = new Timer() { Interval = 5000, Enabled = false };

		private VisualStyleElement _elementClose = null;
		private VisualStyleElement _elementPrev = null;
		private VisualStyleElement _elementInbox = null;
		private VisualStyleElement _elementNext = null;

		private Icon _iconPrev = null;
		private Icon _iconInbox = null;
		private Icon _iconNext = null;
		private Icon _iconWindow = null;
		
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

			this.Opacity = 0;

			_closeTimer.Tick += delegate(object sender, EventArgs e) {
				this.Close();
			};

			_elementClose = VisualStyleElement.Window.SmallCloseButton.Normal;
			_elementPrev = VisualStyleElement.CreateElement("TaskbandExtendedUI", LeftControl, 1);
			_elementInbox = VisualStyleElement.CreateElement("TaskbandExtendedUI", CenterControl, 1);
			_elementNext = VisualStyleElement.CreateElement("TaskbandExtendedUI", RightControl, 1);

			_iconPrev = ResourceHelper.GetIcon("Previous.ico");
			_iconInbox = ResourceHelper.GetIcon("Inbox.ico");
			_iconNext = ResourceHelper.GetIcon("Next.ico");
			_iconWindow = ResourceHelper.GetIcon("gmail-classic.ico", 16);

		}

		protected override bool ShowWithoutActivation {
			get { return true; }
		}

		protected override void OnActivated(EventArgs e) {
			base.OnActivated(e);

			_closeTimer.Enabled = false;
		}

		protected override void OnClick(EventArgs e) {
			base.OnClick(e);

			Point mouse = this.PointToClient(MousePosition);

			if (_rectClient.Contains(mouse)) {

			}
			else if (_rectClose.Contains(mouse)) {
				//this.Close();
			}
			else if (_rectPrev.Contains(mouse)) {

			}
			else if (_rectInbox.Contains(mouse)) {

			}
			else if (_rectNext.Contains(mouse)) {

			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e) {
			base.OnFormClosing(e);

			Timer timer = new Timer() {
				Interval = 50
			};

			timer.Tick += delegate(object sender, EventArgs args) {
				this.Opacity = Math.Max(this.Opacity - 0.1, 0);

				if (this.Opacity == 0) {
					timer.Stop();
					timer.Dispose();
				}
			};

			timer.Start();
		}

		protected override void OnDeactivate(EventArgs e) {
			base.OnDeactivate(e);

			_closeTimer.Start();
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			DwmExtendFrameIntoClientArea(this.Handle, ref _dwmMargins);

			Timer timer = new Timer() {
				Interval = 50
			};

			timer.Tick += delegate(object sender, EventArgs args) {
				this.Opacity = Math.Min(this.Opacity + 0.1, 1);

				if (this.Opacity == 1) {
					timer.Stop();
					timer.Dispose();
				}
			};

			timer.Start();
			_closeTimer.Start();
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);

			if (_rectClose.Contains(e.Location)) {

				Debug.WriteLine("Mouse Down");
				_stateClose = State.Pressed;
				Invalidate(_rectClose);
			}
			else if (_rectPrev.Contains(e.Location)) {
				_statePrev = State.Pressed;
				Invalidate(_rectPrev);
			}
			else if (_rectInbox.Contains(e.Location)) {
				_stateInbox = State.Pressed;
				Invalidate(_rectInbox);
			}
			else if (_rectNext.Contains(e.Location)) {
				_stateNext = State.Pressed;
				Invalidate(_rectNext);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);

			Boolean pressed = e.Button != System.Windows.Forms.MouseButtons.None;

			if (_rectClose.Contains(e.Location) && _stateClose != State.Pressed) {
				Debug.WriteLine("Mouse Move - " + _stateClose.ToString());

				_stateClose = pressed ? State.Pressed : State.Hot;
				_statePrev = State.Normal;
				_stateInbox = State.Normal;
				_stateNext = State.Normal;
			}
			else if (_rectPrev.Contains(e.Location)) {
				_stateClose = State.Normal;
				_statePrev = pressed ? State.Pressed : State.Hot;
				_stateInbox = State.Normal;
				_stateNext = State.Normal;
			}
			else if (_rectInbox.Contains(e.Location)) {
				_stateClose = State.Normal;
				_statePrev = State.Normal;
				_stateInbox = pressed ? State.Pressed : State.Hot;
				_stateNext = State.Normal;
			}
			else if (_rectNext.Contains(e.Location)) {
				_stateClose = State.Normal;
				_statePrev = State.Normal;
				_stateInbox = State.Normal;
				_stateNext = pressed ? State.Pressed : State.Hot;
			}
			else {
				_stateClose = State.Normal;
				_statePrev = State.Normal;
				_stateInbox = State.Normal;
				_stateNext = State.Normal;
			}

			Invalidate();
		}

		protected override void OnLostFocus(EventArgs e) {
			base.OnLostFocus(e);

			_closeTimer.Enabled = true;
		}

		private void InitRects(Graphics g) {
			if (_rectsInitialized) {
				return;
			}

			int totalButtonWidth = _buttonSize.Width * 3;
			int startX = ((Width - totalButtonWidth) / 2);
			int iconX = (_buttonSize.Width - 16) / 2;
			int iconY = (_buttonSize.Height - 16) / 2;
			int buttonY = this.Height - _buttonSize.Height;

			_rectClient = new Rectangle(
				_dwmMargins.cxLeftWidth,
				_dwmMargins.cyTopHeight,
				Width - _dwmMargins.cxRightWidth - _dwmMargins.cxLeftWidth,
				Height - _dwmMargins.cyBottomHeight - _dwmMargins.cyTopHeight
			);

			_rectPrev = new Rectangle(startX, buttonY, _buttonSize.Width, _buttonSize.Height);

			startX += _buttonSize.Width;

			_rectInbox = new Rectangle(startX, buttonY, _buttonSize.Width, _buttonSize.Height);

			startX += _buttonSize.Width;

			_rectNext = new Rectangle(startX, buttonY, _buttonSize.Width, _buttonSize.Height);

			VisualStyleRenderer renderer = new VisualStyleRenderer(_elementClose);
			Size closeSize = renderer.GetPartSize(g, ThemeSizeType.True);

			_rectClose = new Rectangle(Width - _dwmMargins.cxLeftWidth - closeSize.Width, 8, closeSize.Width, closeSize.Height);

		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			InitRects(e.Graphics);

			e.Graphics.Clear(_aeroEnabled ? Color.Transparent : SystemColors.Control);
			e.Graphics.FillRectangle(SystemBrushes.Control, _rectClient);

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			Font font = SystemFonts.CaptionFont;
			Utilities.GlassHelper.DrawText(e.Graphics, "  andrew@shellscape.org", font, new Rectangle(_dwmMargins.cxLeftWidth + 16, _dwmMargins.cxLeftWidth, 200, 16), this.ForeColor, TextFormatFlags.Default, Utilities.TextStyle.Glowing);

			e.Graphics.DrawIcon(_iconWindow, new Rectangle(_dwmMargins.cxLeftWidth, _dwmMargins.cxLeftWidth, 16, 16));

			PaintElement(e.Graphics, _elementPrev, _rectPrev, _statePrev);
			PaintIcon(e.Graphics, _iconPrev, _rectPrev);

			PaintElement(e.Graphics, _elementInbox, _rectInbox, _stateInbox);
			PaintIcon(e.Graphics, _iconInbox, _rectInbox);

			PaintElement(e.Graphics, _elementNext, _rectNext, _stateNext);
			PaintIcon(e.Graphics, _iconNext, _rectNext);

			PaintElement(e.Graphics, _elementClose, _rectClose, _stateClose);
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

		private void PaintIcon(Graphics g, Icon icon, Rectangle rect) {

			int x = (rect.Width - icon.Width) / 2;
			int y = (rect.Height - icon.Height) / 2;

			g.DrawIcon(icon, new Rectangle(x + rect.Left, y + rect.Top, icon.Width, icon.Height));

		}

		private void PaintElement(Graphics g, VisualStyleElement element, Rectangle rect, int state) {

			VisualStyleElement e = VisualStyleElement.CreateElement(element.ClassName, element.Part, state);
			VisualStyleRenderer renderer = new VisualStyleRenderer(e);

			renderer.DrawBackground(g, rect);
		}

	}
}