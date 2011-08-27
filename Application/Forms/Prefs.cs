using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Shellscape.UI.ControlPanel;

namespace GmailNotifierPlus.Forms {
	public partial class Prefs : Shellscape.UI.ControlPanel.ControlPanelForm {
		
		public Prefs() {
			InitializeComponent();

			// FlowLayoutPanel doesn't support visual inheritance, so we have to add links manually here.
		}

		private void InitButtons() {
			ControlPanelTaskLink general = new ControlPanelTaskLink() { Text = Locale.Current.Config.General };
			ControlPanelTaskLink accounts = new ControlPanelTaskLink() { Text = Locale.Current.Config.Accounts };
			ControlPanelTaskLink appearance = new ControlPanelTaskLink() { Text = Locale.Current.Config.Appearance };

			_ButtonNewAccount.Text = Locale.Current.Buttons.AddNewAccount;
			_ButtonGeneral.ButtonText = ;
			_ButtonAccounts.ButtonText = ;
			_ButtonAppearance.ButtonText = ;

			this._Tasks.Controls.Add(general);
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

		}
	}
}
