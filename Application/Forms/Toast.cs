using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
		private int _statePrev = State.Normal;
		private int _stateInbox = 1;
		private int _stateNext = State.Disabled;
		private int _mailIndex = 0;

		private Timer _closeTimer = new Timer() { Interval = 15000, Enabled = false };

		private VisualStyleElement _elementClose = null;
		private VisualStyleElement _elementPrev = null;
		private VisualStyleElement _elementInbox = null;
		private VisualStyleElement _elementNext = null;

		private Icon _iconPrev = null;
		private Icon _iconInbox = null;
		private Icon _iconNext = null;
		private Icon _iconWindow = null;

		public Toast(Account account) {
			InitializeComponent();

			this.Account = account;

			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			this.UpdateStyles();

			if (!VisualStyleRenderer.IsSupported) {
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
				this.ControlBox = true;
				this.Icon = Program.MainForm.Icon;
			}

			int enabled = 0;
			int response = DwmIsCompositionEnabled(ref enabled);

			_aeroEnabled = enabled == 1;

			this.Opacity = 0;
			this.TopMost = true;

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

			ToolTip openTip = new ToolTip();
			openTip.SetToolTip(_PictureOpen, Localization.Locale.Current.Toast.ViewEmail);

			_PictureOpen.Cursor = Cursors.Hand;
			_PictureOpen.Click += OpenEmail;

			using (Icon icon = ResourceHelper.GetIcon("Open.ico")) {
				_PictureOpen.Image = icon.ToBitmap();
			}

			if (this.Account.Emails.Count > 1) {
				_stateNext = State.Normal;
			}

			// show the last (newest) email
			_mailIndex = 0; //this.Account.Emails.Count - 1;

			UpdateBody();
		}

		public Account Account { get; set; }

		protected override bool ShowWithoutActivation {
			get { return true; }
		}

		protected override void OnActivated(EventArgs e) {
			base.OnActivated(e);

			_closeTimer.Enabled = false;
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnClick(e);

			Point mouse = this.PointToClient(MousePosition);

			if (_rectClose.Contains(mouse)) {
				this.Close();
			}
			else if (_rectInbox.Contains(mouse)) {
				Utilities.UrlHelper.Launch(this.Account, Utilities.UrlHelper.BuildInboxUrl(this.Account));
				this.Close();
			}
			// Prev Rect
			else if (_rectPrev.Contains(mouse) && _statePrev != State.Disabled) {
				if (_mailIndex > 0) {
					_mailIndex--;
					UpdateBody();

					if (_statePrev == State.Disabled) {
						_stateNext = State.Normal;
						Invalidate(_rectNext);
					}
				}
				else {
					_statePrev = State.Disabled;
					Invalidate(_rectPrev);
				}				
			}
			// Next Rect
			else if (_rectNext.Contains(mouse) && _stateNext != State.Disabled) {
				if (_mailIndex < this.Account.Unread - 1) {
					_mailIndex++;
					UpdateBody();

					if (_statePrev == State.Disabled) {
						_statePrev = State.Normal;
						Invalidate(_rectPrev);
					}
				}
				else {
					_stateNext = State.Disabled;
					Invalidate(_rectNext);
				}
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

			if (VisualStyleRenderer.IsSupported) {
				DwmExtendFrameIntoClientArea(this.Handle, ref _dwmMargins);
			}

			Timer timer = new Timer() {
				Interval = 50
			};

			timer.Tick += delegate(object sender, EventArgs args) {
				this.Opacity = Math.Min(this.Opacity + 0.1, 1);

				if (this.Opacity == 1) {
					timer.Stop();
					timer.Dispose();

					this.TopMost = false;
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
			else if (_rectPrev.Contains(e.Location) && _statePrev != State.Disabled) {
				_statePrev = State.Pressed;
				Invalidate(_rectPrev);
			}
			else if (_rectInbox.Contains(e.Location)) {
				_stateInbox = State.Pressed;
				Invalidate(_rectInbox);
			}
			else if (_rectNext.Contains(e.Location) && _stateNext != State.Disabled) {
				_stateNext = State.Pressed;
				Invalidate(_rectNext);
			}
		}

		/// <summary>
		/// OH LAWD this method is soooo damned fugly. but i'm just not willing to pretty it up. you're an ugly bitch, aintcha?
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);

			Boolean pressed = e.Button != System.Windows.Forms.MouseButtons.None;

			if (_rectClose.Contains(e.Location) && _stateClose != State.Pressed) {

				SetDefaultState();

				_stateClose = pressed ? State.Pressed : State.Hot;
			}
			else if (_rectPrev.Contains(e.Location) && _statePrev != State.Disabled) {

				SetDefaultState();

				_statePrev = pressed ? State.Pressed : State.Hot;
			}
			else if (_rectInbox.Contains(e.Location)) {

				SetDefaultState();

				_stateInbox = pressed ? State.Pressed : State.Hot;
			}
			else if (_rectNext.Contains(e.Location) && _stateNext != State.Disabled) {

				SetDefaultState();

				_stateNext = pressed ? State.Pressed : State.Hot;
			}
			else {
				SetDefaultState();
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
			int startX = (((VisualStyleRenderer.IsSupported ? this.Width : this.ClientSize.Width) - totalButtonWidth) / 2);
			int iconX = (_buttonSize.Width - 16) / 2;
			int iconY = (_buttonSize.Height - 16) / 2;
			int buttonY = VisualStyleRenderer.IsSupported ? (this.Height - _buttonSize.Height) : (this.ClientSize.Height - _buttonSize.Height);

			if (VisualStyleRenderer.IsSupported) {
				_rectClient = new Rectangle(
					_dwmMargins.cxLeftWidth,
					_dwmMargins.cyTopHeight,
					Width - _dwmMargins.cxRightWidth - _dwmMargins.cxLeftWidth,
					Height - _dwmMargins.cyBottomHeight - _dwmMargins.cyTopHeight
				);
			}
			else {
				_rectClient = this.ClientRectangle;
			}

			_Panel.Top = _rectClient.Top;
			_Panel.Left = _rectClient.Left;

			_rectPrev = new Rectangle(startX, buttonY, _buttonSize.Width, _buttonSize.Height);

			startX += _buttonSize.Width;

			_rectInbox = new Rectangle(startX, buttonY, _buttonSize.Width, _buttonSize.Height);

			startX += _buttonSize.Width;

			_rectNext = new Rectangle(startX, buttonY, _buttonSize.Width, _buttonSize.Height);

			Size closeSize;

			if (VisualStyleRenderer.IsSupported) {
				VisualStyleRenderer renderer = new VisualStyleRenderer(_elementClose);

				closeSize = renderer.GetPartSize(g, ThemeSizeType.True);
			}
			else {
				closeSize = new Size(16, 14);
			}

			_rectClose = new Rectangle(Width - _dwmMargins.cxLeftWidth - closeSize.Width, 8, closeSize.Width, closeSize.Height);

		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			InitRects(e.Graphics);

			e.Graphics.Clear(_aeroEnabled ? Color.Transparent : SystemColors.Control);
			e.Graphics.FillRectangle(SystemBrushes.Control, _rectClient);

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			Font font = SystemFonts.CaptionFont;
			Rectangle captionLayout = new Rectangle(_dwmMargins.cxLeftWidth + (_dwmMargins.cxLeftWidth / 2) + 16, _dwmMargins.cxLeftWidth, this.Width, _dwmMargins.cyTopHeight);
			String caption = String.Concat(" ", this.Account.FullAddress); // stupid hack. padding so the glow doesnt get cut off on the left.

			Utilities.GlassHelper.DrawText(e.Graphics, caption, font, captionLayout, this.ForeColor, TextFormatFlags.Default, Utilities.TextStyle.Glowing);

			e.Graphics.DrawIcon(_iconWindow, new Rectangle(_dwmMargins.cxLeftWidth, _dwmMargins.cxLeftWidth, 16, 16));

			PaintElement(e.Graphics, _elementPrev, _rectPrev, _statePrev);
			PaintIcon(e.Graphics, _iconPrev, _rectPrev, _statePrev);

			PaintElement(e.Graphics, _elementInbox, _rectInbox, _stateInbox);
			PaintIcon(e.Graphics, _iconInbox, _rectInbox, State.Normal);

			PaintElement(e.Graphics, _elementNext, _rectNext, _stateNext);
			PaintIcon(e.Graphics, _iconNext, _rectNext, _stateNext);

			PaintElement(e.Graphics, _elementClose, _rectClose, _stateClose);
		}

		protected override void WndProc(ref Message m) {

			if (!VisualStyleRenderer.IsSupported) {
				base.WndProc(ref m);
				return;
			}

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

		private void PaintIcon(Graphics g, Icon icon, Rectangle rect, int state) {

			int x = (rect.Width - icon.Width) / 2;
			int y = (rect.Height - icon.Height) / 2;
			Rectangle layout = new Rectangle(x + rect.Left, y + rect.Top, icon.Width, icon.Height);

			if (state == State.Disabled) {

				// ControlPaint.DrawImageDisabled sucks ass.

				ColorMatrix cm = new ColorMatrix(new float[][]{ 
					new float[]{0.3f,0.3f,0.3f,0,0},
          new float[]{0.59f,0.59f,0.59f,0,0},
          new float[]{0.11f,0.11f,0.11f,0,0},
          new float[]{0,0,0,1,0,0},
          new float[]{0,0,0,0,1,0},
          new float[]{0,0,0,0,0,1}
				});

				using (Bitmap bitmap = icon.ToBitmap())
				using (ImageAttributes ia = new ImageAttributes()) {

					ia.SetColorMatrix(cm);

					g.DrawImage(bitmap, layout, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, ia);
				}
			}
			else {
				g.DrawIcon(icon, layout);
			}
		}

		private void PaintElement(Graphics g, VisualStyleElement element, Rectangle rect, int state) {

			if (VisualStyleRenderer.IsSupported) {
				VisualStyleRenderer renderer = new VisualStyleRenderer(element.ClassName, element.Part, state);

				renderer.DrawBackground(g, rect);
			}
			else {

				ButtonState buttonState = ButtonState.Normal;

				if (state == State.Disabled) {
					buttonState = ButtonState.Inactive;
				}
				else if (state == State.Pressed) {
					buttonState = ButtonState.Pushed;
				}

				if (element == _elementClose) {
					ControlPaint.DrawCaptionButton(g, rect, CaptionButton.Close, buttonState);
				}
				else {
					ControlPaint.DrawButton(g, rect, buttonState);
				}
			}
		}

		private void UpdateBody() {

			this.Text = this.Account.FullAddress;

			if (_mailIndex > this.Account.Emails.Count) {
				_mailIndex = this.Account.Emails.Count - 1;
			}

			Email email = this.Account.Emails[_mailIndex];

			_LabelDate.Text = email.When;
			_LabelFrom.Text = email.From;
			_LabelMessage.Text = email.Message;
			_LabelTitle.Text = email.Title;

			String start = (_mailIndex + 1).ToString(); //(this.Account.Unread - _mailIndex).ToString();
			String end = this.Account.Unread.ToString();

			_LabelIndex.Text = String.Concat(start, " / ", end); //String.Concat((_mailIndex + 1).ToString(), " / ", this.Account.Unread);

			SetDefaultState();
		}

		private void SetDefaultState() {

			_stateClose = State.Normal;
			_stateInbox = State.Normal;

			if (this.Account.Emails.Count == 1) {
				_stateNext = _statePrev = State.Disabled;
				return;
			}

			if (_statePrev != State.Disabled) {
				_statePrev = State.Normal;
			}
			if (_stateNext != State.Disabled) {
				_stateNext = State.Normal;
			}

			if (_mailIndex == 0) {
				_statePrev = State.Disabled;
			}
			// gmail's atom feed only sends data for 20 messages
			else if (_mailIndex >= 19 || _mailIndex >= this.Account.Unread - 1) {
				_stateNext = State.Disabled;
			}

		}

		private void OpenEmail(object sender, EventArgs e) {
			String url = this.Account.Emails[_mailIndex].Url;

			if (!String.IsNullOrEmpty(url)) {
				Utilities.UrlHelper.Launch(this.Account, url);
			}

			this.Close();
		}
	}
}