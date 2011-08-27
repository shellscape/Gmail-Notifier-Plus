using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Shellscape.UI.ControlPanel;

using GmailNotifierPlus.Localization;

namespace GmailNotifierPlus.Forms {
	public partial class Prefs : Shellscape.UI.ControlPanel.ControlPanelForm {
		
		public Prefs() {
			InitializeComponent();

			TopMost = true;

			InitNavigation();
		}

		// FlowLayoutPanel doesn't support visual inheritance, so we have to add links manually here.
		private void InitNavigation() {
			ControlPanelTaskLink general = new ControlPanelTaskLink() {
				Text = Locale.Current.Preferences.Navigation.General 
			};
			ControlPanelTaskLink accounts = new ControlPanelTaskLink() { 
				Text = Locale.Current.Preferences.Navigation.Accounts 
			};
			ControlPanelTaskLink appearance = new ControlPanelTaskLink() { 
				Text = Locale.Current.Preferences.Navigation.Appearance 
			};

			this.Tasks.Add(general);
			this.Tasks.Add(accounts);
			this.Tasks.Add(appearance);

			ControlPanelTaskLink about = new ControlPanelTaskLink() { Text = Locale.Current.JumpList.About };
			ControlPanelTaskLink check = new ControlPanelTaskLink() { Text = Locale.Current.JumpList.Check };

			this.OtherTasks.Add(about);
			this.OtherTasks.Add(check);
		}

		public void InitFirstRun() {

		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			// paints the usertile frame
			//Bitmap frame = Shellscape.Utilities.ResourceHelper.GetResourcePNG("authui.dll", "12221");
			//e.Graphics.DrawImage(frame, new Rectangle(220, 20, frame.Width, frame.Height));
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);

			TopMost = false;
		}
	}
}
