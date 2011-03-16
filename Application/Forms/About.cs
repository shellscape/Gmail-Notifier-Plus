using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

using GmailNotifierPlus.Controls;
using GmailNotifierPlus.Utilities;

namespace GmailNotifierPlus.Forms {
	public partial class About : Form {

		private Boolean _firstRun;

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		
		public About() {
			InitializeComponent();

			this.Icon = Program.Icon;
			//this.ClientSize = new Size(300, 355);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
			this.Text = Resources.WindowTitle;

			_PanelAbout.FirstRun = _firstRun = Config.Current.FirstRun;
			_ButtonOK.Click += _ButtonOk_Click;
			_ButtonOK.Text = _firstRun ? Locale.Current.Buttons.LetsGo : Locale.Current.Buttons.Sweet;
		}

		private void _PanelShellscape_Click(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(UrlHelper.Uris.Shellscape);
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			SetForegroundWindow(base.Handle);
		}

		private void _ButtonOk_Click(object sender, EventArgs e) {

			if (_firstRun) {
				Program.mainForm.FirstRun = true;
				Program.mainForm.OpenSettingsWindow();
			}

			Close();
		}
	}
}
