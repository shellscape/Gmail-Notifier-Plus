using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GmailNotifierPlus;

using Shellscape.UI.Controls;
using Shellscape.UI.Controls.Preferences;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

namespace GmailNotifierPlus.Controls {

	[DesignTimeVisible(true), Browsable(true)]
	public class AccountPanel : PreferencesPanel {

		private Button _ButtonDefault;
		private TextBox _TextUsername;
		private Label _LabelPassword;
		private Label _LabelUsername;
		private Label _LabelAccountTitle;
		private Label _LabelError;
		private PictureBox _PictureExclamation;
		private TextBox _TextPassword;
		private Button _ButtonRemove;
		private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
		private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;

		private Boolean _makeDefault = false;
		private String _filler = new String('x', 30);

		public AccountPanel() : base() {
			InitializeComponent();
		}

		public Account Account { get; set; }

		private Button DefaultButton {
			get { return _ButtonDefault; }
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			InitLabels();
			DataBind();

			_TextPassword.TextChanged += _Credentials_Changed;
			_TextUsername.TextChanged += _Credentials_Changed;

			_PictureExclamation.Image = Utilities.ResourceHelper.GetImage("Exclamation.png");
		}

		private void InitLabels() {

			if (DesignMode) {
				return;
			}

			_PictureExclamation.Visible = false;
			_LabelError.Visible = false;

			_ButtonDefault.Text = Locale.Current.Buttons.Default;
			_ButtonRemove.Text = Locale.Current.Buttons.Remove;

			_LabelAccountTitle.Text = Locale.Current.Labels.Edit;
			_LabelError.Text = Locale.Current.Labels.Error;
			_LabelPassword.Text = Locale.Current.Labels.Password;
			_LabelUsername.Text = Locale.Current.Labels.Login;
		}

		private void DataBind() {
			if (Account == null) {
				return;
			}

			_TextPassword.Text = _filler;
			_TextUsername.Text = Account.FullAddress;

			_ButtonDefault.Enabled = !Account.Default;				
		}

		private Boolean AccountExists() {

			Boolean result = false;
			var accounts = Config.Current.Accounts;

			// if the username has changed to something other than the original saved username,
			// and the new username exists already, take a big fat poop all over the screen.
			if (_TextUsername.Text.ToLower() != Account.FullAddress.ToLower() && 
				accounts.Where(o => o.FullAddress.ToLower() == _TextUsername.Text.ToLower()).Count() > 0){
					result = true;
			}

			_PictureExclamation.Visible = _LabelError.Visible = result;

			return result;
		}

		private void InitializeComponent() {
			this._ButtonDefault = new System.Windows.Forms.Button();
			this._TextUsername = new System.Windows.Forms.TextBox();
			this._LabelPassword = new System.Windows.Forms.Label();
			this._LabelUsername = new System.Windows.Forms.Label();
			this._LabelAccountTitle = new System.Windows.Forms.Label();
			this._LabelError = new System.Windows.Forms.Label();
			this._PictureExclamation = new System.Windows.Forms.PictureBox();
			this._TextPassword = new System.Windows.Forms.TextBox();
			this._ButtonRemove = new System.Windows.Forms.Button();
			this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
			this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
			((System.ComponentModel.ISupportInitialize)(this._PictureExclamation)).BeginInit();
			this.SuspendLayout();
			// 
			// _ButtonDefault
			// 
			this._ButtonDefault.AutoSize = true;
			this._ButtonDefault.Enabled = false;
			this._ButtonDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonDefault.Location = new System.Drawing.Point(100, 219);
			this._ButtonDefault.MinimumSize = new System.Drawing.Size(65, 23);
			this._ButtonDefault.Name = "_ButtonDefault";
			this._ButtonDefault.Size = new System.Drawing.Size(173, 30);
			this._ButtonDefault.TabIndex = 3;
			this._ButtonDefault.Text = "Make Default";
			this._ButtonDefault.UseVisualStyleBackColor = true;
			this._ButtonDefault.Click += new System.EventHandler(this._ButtonDefault_Click);
			// 
			// _TextUsername
			// 
			this._TextUsername.Location = new System.Drawing.Point(100, 80);
			this._TextUsername.Name = "_TextUsername";
			this._TextUsername.Size = new System.Drawing.Size(249, 23);
			this._TextUsername.TabIndex = 0;
			// 
			// _LabelPassword
			// 
			this._LabelPassword.AutoSize = true;
			this._LabelPassword.Location = new System.Drawing.Point(12, 110);
			this._LabelPassword.Name = "_LabelPassword";
			this._LabelPassword.Size = new System.Drawing.Size(60, 15);
			this._LabelPassword.TabIndex = 46;
			this._LabelPassword.Text = "Password:";
			// 
			// _LabelUsername
			// 
			this._LabelUsername.AutoSize = true;
			this._LabelUsername.Location = new System.Drawing.Point(12, 80);
			this._LabelUsername.Name = "_LabelUsername";
			this._LabelUsername.Size = new System.Drawing.Size(63, 15);
			this._LabelUsername.TabIndex = 45;
			this._LabelUsername.Text = "Username:";
			// 
			// _LabelAccountTitle
			// 
			this._LabelAccountTitle.AutoSize = true;
			this._LabelAccountTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelAccountTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
			this._LabelAccountTitle.Location = new System.Drawing.Point(8, 48);
			this._LabelAccountTitle.Name = "_LabelAccountTitle";
			this._LabelAccountTitle.Size = new System.Drawing.Size(39, 21);
			this._LabelAccountTitle.TabIndex = 47;
			this._LabelAccountTitle.Text = "Title";
			// 
			// _LabelError
			// 
			this._LabelError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(38)))), ((int)(((byte)(30)))));
			this._LabelError.Location = new System.Drawing.Point(120, 138);
			this._LabelError.Name = "_LabelError";
			this._LabelError.Size = new System.Drawing.Size(229, 26);
			this._LabelError.TabIndex = 48;
			this._LabelError.Text = "An account with this username already exists. Please enter a different username.";
			// 
			// _PictureExclamation
			// 
			this._PictureExclamation.Location = new System.Drawing.Point(100, 138);
			this._PictureExclamation.MinimumSize = new System.Drawing.Size(16, 16);
			this._PictureExclamation.Name = "_PictureExclamation";
			this._PictureExclamation.Size = new System.Drawing.Size(16, 16);
			this._PictureExclamation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this._PictureExclamation.TabIndex = 49;
			this._PictureExclamation.TabStop = false;
			// 
			// _TextPassword
			// 
			this._TextPassword.Location = new System.Drawing.Point(100, 110);
			this._TextPassword.Name = "_TextPassword";
			this._TextPassword.Size = new System.Drawing.Size(249, 23);
			this._TextPassword.TabIndex = 1;
			this._TextPassword.UseSystemPasswordChar = true;
			// 
			// _ButtonRemove
			// 
			this._ButtonRemove.AutoSize = true;
			this._ButtonRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonRemove.Location = new System.Drawing.Point(100, 183);
			this._ButtonRemove.MinimumSize = new System.Drawing.Size(65, 23);
			this._ButtonRemove.Name = "_ButtonRemove";
			this._ButtonRemove.Size = new System.Drawing.Size(173, 30);
			this._ButtonRemove.TabIndex = 2;
			this._ButtonRemove.Text = "Remove Account";
			this._ButtonRemove.UseVisualStyleBackColor = true;
			this._ButtonRemove.Click += new System.EventHandler(this._ButtonRemove_Click);
			// 
			// lineShape1
			// 
			this.lineShape1.BorderColor = System.Drawing.Color.Red;
			this.lineShape1.Name = "lineShape1";
			this.lineShape1.X1 = 0;
			this.lineShape1.X2 = 300;
			this.lineShape1.Y1 = 128;
			this.lineShape1.Y2 = 128;
			// 
			// shapeContainer1
			// 
			this.shapeContainer1.Location = new System.Drawing.Point(2, 45);
			this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
			this.shapeContainer1.Name = "shapeContainer1";
			this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
			this.shapeContainer1.Size = new System.Drawing.Size(933, 759);
			this.shapeContainer1.TabIndex = 56;
			this.shapeContainer1.TabStop = false;
			// 
			// AccountPanel
			// 
			this.Controls.Add(this._ButtonDefault);
			this.Controls.Add(this._ButtonRemove);
			this.Controls.Add(this._TextPassword);
			this.Controls.Add(this._PictureExclamation);
			this.Controls.Add(this._LabelAccountTitle);
			this.Controls.Add(this._LabelError);
			this.Controls.Add(this._TextUsername);
			this.Controls.Add(this._LabelPassword);
			this.Controls.Add(this._LabelUsername);
			this.Controls.Add(this.shapeContainer1);
			this.Name = "AccountPanel";
			this.Padding = new System.Windows.Forms.Padding(2, 45, 2, 2);
			this.Size = new System.Drawing.Size(937, 806);
			((System.ComponentModel.ISupportInitialize)(this._PictureExclamation)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void _Credentials_Changed(object sender, EventArgs e) {

			if (AccountExists()) { // aww, you're naughty.
				return;
			}

			_PictureExclamation.Visible = _LabelError.Visible = false;

			Boolean somethingChanged = false;

			if((_TextUsername.Text != this.Account.Login) || (_TextUsername.Text.Length > 0)) { // dude, change something already.
				Account.Login = _TextUsername.Text;
				Account.Init();
				somethingChanged = true;
			}

			if ((_TextPassword.Text != _filler) && (_TextPassword.Text.Length > 0)) {
				Account.Password = _TextPassword.Text;
				Account.Init();
				somethingChanged = true;
			}

			if (!Config.Current.Accounts.Contains(Account)) {
				Config.Current.Accounts.Add(Account); // TODO - config should notify when this happens!
				somethingChanged = true;
			}

			if (somethingChanged) {
				Config.Current.Save();
			}
		}

		// TODO - need the account to notify that something has changed from the original value.
		// TODO - need config to notify that an account has changed. so we can not recheck on plain old save.

		private void _ButtonDefault_Click(object sender, EventArgs e) {
			_ButtonDefault.Enabled = false;

			var accountPanels = this.Parent.Controls.All().Where(o => o is AccountPanel);

			Account defaultAccount = Config.Current.Accounts.Where(o => o.Default).FirstOrDefault();
			AccountPanel defaultAccountPanel = accountPanels.Where(o => (o as AccountPanel).Account.Default).FirstOrDefault() as AccountPanel;
			Forms.Preferences form = this.ParentForm as Forms.Preferences;

			PreferencesButton buttonAccounts = form._ButtonGroup.Controls.Find("_ButtonAccounts", true)[0] as PreferencesButton;

			PreferencesButtonItem thisItem = buttonAccounts.ButtonItems.All().Where(o => o.AssociatedPanel == this).FirstOrDefault();
			PreferencesButtonItem defaultItem = buttonAccounts.ButtonItems.All().Where(o => o.ButtonText == defaultAccount.FullAddress).FirstOrDefault();

			defaultAccount.Default = false;
			Account.Default = true;

			_ButtonDefault.Enabled = false;
			defaultAccountPanel.DefaultButton.Enabled = true;

			thisItem.Font = new Font(thisItem.Font, FontStyle.Bold);
			defaultItem.Font = new Font(defaultItem.Font, FontStyle.Regular);
		}

		private void _ButtonRemove_Click(object sender, EventArgs e) {
			TaskDialog dialog = new TaskDialog();
			dialog.Caption = Resources.WindowTitle;
			dialog.InstructionText = Locale.Current.Labels.RemoveConfirmation;
			dialog.Cancelable = true;
			dialog.OwnerWindowHandle = base.Handle;

			TaskDialogButton buttonYes = new TaskDialogButton("yesButton", Locale.Current.Labels.Yes);
			buttonYes.Default = true;
			buttonYes.Click += _TaskButtonYes_Click;

			TaskDialogButton buttonNo = new TaskDialogButton("noButton", Locale.Current.Labels.No);
			buttonNo.Click += _TaskButtonNo_Click;

			dialog.Controls.Add(buttonYes);
			dialog.Controls.Add(buttonNo);
			dialog.Show();
		}
		
		private void _TaskButtonNo_Click(object sender, EventArgs e) {
			TaskDialogButton button = (TaskDialogButton)sender;
			((TaskDialog)button.HostingDialog).Close();
		}

		private void _TaskButtonYes_Click(object sender, EventArgs e) {
			TaskDialogButton button = (TaskDialogButton)sender;
			Forms.Preferences form = this.ParentForm as Forms.Preferences;

			PreferencesButton buttonAccounts = form._ButtonGroup.Controls.Find("_ButtonAccounts", true)[0] as PreferencesButton;
			PreferencesButtonItem thisItem = buttonAccounts.ButtonItems.All().Where(o => o.AssociatedPanel == this).FirstOrDefault();

			((TaskDialog)button.HostingDialog).Close();

			buttonAccounts.ButtonItems.Remove(thisItem);

			this.Parent.Controls.Remove(this);

			buttonAccounts.AssociatedPanel.Show();

			Config.Current.Accounts.Remove(this.Account);
			Config.Current.Save();
		}
	}
}
