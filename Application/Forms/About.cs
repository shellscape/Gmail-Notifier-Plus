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

using GmailNotifierPlus.Localization;
using GmailNotifierPlus.Utilities;

using Shellscape;

namespace GmailNotifierPlus.Forms {
	public partial class About : Shellscape.UI.About {

		//private AnimatedCursor _animatedCursor;
		//private Bitmap _upToDate;
		//private Bitmap _exclamation;
		//private Bitmap _download;
		
		//private Timer _timer;

		//[System.Runtime.InteropServices.DllImport("user32.dll")]
		//public static extern bool SetForegroundWindow(IntPtr hWnd);

		private Bitmap _icon;

		public About() : base() {

			this.Icon = Resources.Icons.Window;
			this.Text = String.Concat(Locale.Current.About.WindowTitle, " - ", Resources.Strings.WindowTitle);
			this._icon = Utilities.ResourceHelper.GetImage("about-icon.png");

			InitializeComponent();

			this._Flow.SuspendLayout();
			this.SuspendLayout();

			String toYear = DateTime.Now.Year.ToString();
			String linkTarget = "Shellscape Software";

			toYear = toYear == "2011" ? String.Empty : String.Concat("-", toYear);

			this._Button.Font = SystemFonts.MessageBoxFont;
			this._Button.Text = Localization.Locale.Current.About.Button;
			this._LabelCopyright.Text = String.Join("\n",
				String.Concat("Copyright © 2011", toYear, " Andrew Powell, ", linkTarget, ". All rights reserved."),
				"Based on the application originally developed by Baptiste Girod\n",
				"Gmail Notifier Plus is in no way associated with Gmail. Gmail is a registered trademark of Google, Inc."
			);
			this._LabelCopyright.Links.Add(this._LabelCopyright.Text.IndexOf(linkTarget), linkTarget.Length);
			this._LabelCopyright.Font = SystemFonts.MessageBoxFont;
			this._LabelCopyright.LinkColor = this._LabelCopyright.NormalColor = this._LabelCopyright.HoverColor;
			this._LabelCopyright.LinkBehavior = LinkBehavior.AlwaysUnderline;
			
			this._Flow.ResumeLayout(true);
			this.ResumeLayout(true);

			this._Button.Click += delegate(object sender, EventArgs e) {
				this.Close();
			};

			this._LabelCopyright.LinkClicked += delegate(object sender, LinkLabelLinkClickedEventArgs e) {
				Help.ShowHelp(this, "http://shellscape.org");
			};

			//_ButtonOK.Click += _ButtonOk_Click;
			//_ButtonOK.Text = Localization.Locale.Current.About.Button;

			//_upToDate = Utilities.ResourceHelper.GetImage("uptodate.png");
			//_exclamation = Utilities.ResourceHelper.GetImage("exclamation-updates.png");
			//_download = Utilities.ResourceHelper.GetImage("download-updates.png");

			//_animatedCursor = new AnimatedCursor(Cursors.WaitCursor);

			//_timer = new Timer() { Interval = 75 };

			//_timer.Tick += delegate(object sender, EventArgs e) {

			//  UpdateManager.UpdateStatus status = UpdateManager.Current.Status;
			//  UpdateManager.LatestVersion latest = UpdateManager.Current.Latest;

			//  if (status != UpdateManager.UpdateStatus.Checking) {
			//    _timer.Stop();

			//    if (status == UpdateManager.UpdateStatus.NewVersion) {
			//      _LabelUpdate.Text = String.Format(Locale.Current.About.NewVersion, latest.Version); ;
			//    }
			//    else if (status == UpdateManager.UpdateStatus.Problem) {
			//      _LabelUpdate.Text = Locale.Current.About.UpdateProblem;
			//    }
			//    else if (status == UpdateManager.UpdateStatus.UpToDate) {
			//      _LabelUpdate.Text = String.Format(Locale.Current.About.UpToDate, latest.Version);
			//    }
			//  }

			//  _PanelUpdate.Invalidate(new Rectangle(0, 0, 26, 26));
			//};

			//UpdateManager.Current.Start();

			//_timer.Start();
		}

		protected override void OnPaintIcon(Graphics g) {
			g.DrawImage(this._icon, 326, 7, this._icon.Width, this._icon.Height);
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);

			this.TopMost = false;
			this.Focus();
		}

		//private void _PanelUpdate_Paint(object sender, PaintEventArgs e) {

		//  UpdateManager.UpdateStatus status = UpdateManager.Current.Status;

		//  if (status == UpdateManager.UpdateStatus.Checking) {
		//    _animatedCursor.DrawStep(e.Graphics, -4, -4);
		//  }
		//  else if (status == UpdateManager.UpdateStatus.Problem) {
		//    e.Graphics.DrawImage(_exclamation, 4, 4, _exclamation.Width, _exclamation.Height);
		//  }
		//  else if (status == UpdateManager.UpdateStatus.UpToDate) {
		//    e.Graphics.DrawImage(_upToDate, 4, 4, _upToDate.Width, _upToDate.Height);
		//  }
		//  else if (status == UpdateManager.UpdateStatus.NewVersion) {
		//    e.Graphics.DrawImage(_download, 4, 4, _upToDate.Width, _upToDate.Height);
		//  }
		//}
	}
}
