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
using GmailNotifierPlus.Localization;
using GmailNotifierPlus.Utilities;

using Shellscape;

namespace GmailNotifierPlus.Forms {
	public partial class About : Form {

		private AnimatedCursor _animatedCursor;
		private Bitmap _upToDate;
		private Bitmap _exclamation;
		private Bitmap _download;
		private Boolean _firstRun;
		
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
			_download = Utilities.ResourceHelper.GetImage("download-updates.png");

			_animatedCursor = new AnimatedCursor(Cursors.WaitCursor);

			_timer = new Timer() { Interval = 75 };

			_timer.Tick += delegate(object sender, EventArgs e) {

				UpdateManager.UpdateStatus status = UpdateManager.Current.Status;
				UpdateManager.LatestVersion latest = UpdateManager.Current.Latest;

				if (status != UpdateManager.UpdateStatus.Checking) {
					_timer.Stop();

					if (status == UpdateManager.UpdateStatus.NewVersion) {
						_LabelUpdate.Text = String.Format(Locale.Current.About.NewVersion, latest.Version); ;
					}
					else if (status == UpdateManager.UpdateStatus.Problem) {
						_LabelUpdate.Text = Locale.Current.About.UpdateProblem;
					}
					else if (status == UpdateManager.UpdateStatus.UpToDate) {
						_LabelUpdate.Text = String.Format(Locale.Current.About.UpToDate, latest.Version);
					}
				}

				_PanelUpdate.Invalidate(new Rectangle(0, 0, 26, 26));
			};

			UpdateManager.Current.SafeStart();

			_timer.Start();
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

			UpdateManager.UpdateStatus status = UpdateManager.Current.Status;

			if (status == UpdateManager.UpdateStatus.Checking) {
				_animatedCursor.DrawStep(e.Graphics, -4, -4);
			}
			else if (status == UpdateManager.UpdateStatus.Problem) {
				e.Graphics.DrawImage(_exclamation, 4, 4, _exclamation.Width, _exclamation.Height);
			}
			else if (status == UpdateManager.UpdateStatus.UpToDate) {
				e.Graphics.DrawImage(_upToDate, 4, 4, _upToDate.Width, _upToDate.Height);
			}
			else if (status == UpdateManager.UpdateStatus.NewVersion) {
				e.Graphics.DrawImage(_upToDate, 4, 4, _upToDate.Width, _upToDate.Height);
			}
		}
	}
}
