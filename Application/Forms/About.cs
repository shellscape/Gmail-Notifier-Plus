using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.WindowsAPI.Dialogs;
using Microsoft.WindowsAPI.Shell;

using GmailNotifierPlus.Controls;
using GmailNotifierPlus.Utilities;

using Shellscape;

namespace GmailNotifierPlus.Forms {
	public partial class About : Form {

		public enum UpdateStatus {
			Checking,
			UpToDate,
			Problem
		}

		private AnimatedCursor _animatedCursor;
		private Bitmap _upToDate;
		private Bitmap _exclamation;
		private Boolean _firstRun;

		private UpdateStatus _status;

		private Timer _timer;

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		public About() {
			InitializeComponent();

			this.TopMost = true;
			this.Icon = Program.Icon;

			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
			this.Text = Resources.WindowTitle;

			_PanelAbout.FirstRun = _firstRun = Config.Current.FirstRun;
			_ButtonOK.Click += _ButtonOk_Click;
			_ButtonOK.Text = _firstRun ? Localization.Locale.Current.About.FirstRun : Localization.Locale.Current.About.Button;

			_upToDate = Utilities.ResourceHelper.GetImage("uptodate.png");
			_exclamation = Utilities.ResourceHelper.GetImage("exclamation-updates.png");

			_animatedCursor = new AnimatedCursor(Cursors.WaitCursor);
			_status = UpdateStatus.Checking;

			_timer = new Timer() { Interval = 75 };

			_timer.Tick += delegate(object sender, EventArgs e) {
				_PanelUpdate.Invalidate(new Rectangle(0, 0, 26, 26));

				if (_status != UpdateStatus.Checking) {
					_timer.Stop();
				}
			};

			_timer.Start();

			Random random = new Random();
			Timer temp = new Timer() { Interval = random.Next(5000, 10000) };

			// this is just temporary until the code gets written to check github.
			temp.Tick += delegate(object sender, EventArgs e) {
				
				String version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

				_status = UpdateStatus.UpToDate;
				_LabelUpdate.Text = String.Concat("Gmail Notifier Plus is up to date (", version, ")");

				temp.Stop();
			};

			temp.Start();
		}

		private void _PanelShellscape_Click(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(UrlHelper.Uris.Shellscape);
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);

			this.TopMost = false;
			SetForegroundWindow(base.Handle);

		}

		private void _ButtonOk_Click(object sender, EventArgs e) {

			if (_firstRun) {
				Program.mainForm.FirstRun = true;
				Program.mainForm.OpenSettingsWindow();
			}

			Close();
		}

		private void _PanelUpdate_Paint(object sender, PaintEventArgs e) {

			if (_status == UpdateStatus.Checking) {
				_animatedCursor.DrawStep(e.Graphics, -4, -4);
			}
			else if (_status == UpdateStatus.Problem) {
				e.Graphics.DrawImage(_exclamation, 4, 4, _exclamation.Width, _exclamation.Height);
			}
			else if (_status == UpdateStatus.UpToDate) {
				e.Graphics.DrawImage(_upToDate, 4, 4, _upToDate.Width, _upToDate.Height);
			}

		}
	}
}
