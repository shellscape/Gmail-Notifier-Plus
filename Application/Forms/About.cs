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

		private Bitmap _upToDate;
		private Bitmap _exclamation;
		private Bitmap _download;

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

			this._Button.Font = this._ButtonDonate.Font = SystemFonts.MessageBoxFont;
			this._Button.Text = Localization.Locale.Current.About.Button;
			this._ButtonDonate.Text = Localization.Locale.Current.About.Donate;
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

			_upToDate = Utilities.ResourceHelper.GetImage("uptodate.png");
			_exclamation = Utilities.ResourceHelper.GetImage("exclamation-updates.png");
			_download = Utilities.ResourceHelper.GetImage("download-updates.png");

		}

		protected override string DonationDescription {
			get { return "Gmail%20Notifier%20Plus%20Donation"; }
		}

		protected override void OnPaintIcon(Graphics g) {
			g.DrawImage(this._icon, 326, 7, this._icon.Width, this._icon.Height);
		}

	}
}
