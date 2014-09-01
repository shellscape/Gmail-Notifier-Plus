namespace GmailNotifierPlus.Forms {
	partial class Preferences {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this._PanelAccounts = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._ListAccounts = new Shellscape.UI.Controls.FlickerFreeListView();
			this._LinkAddNew = new Shellscape.UI.Controls.ResponsiveLinkLabel();
			this._LabelAccountsTitle = new Shellscape.UI.Controls.ThemeLabel();
			this._PanelAccount = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._PanelAccountActions = new Shellscape.UI.Controls.DoubleBufferedFlowPanel();
			this._ButtonAccountCancel = new System.Windows.Forms.Button();
			this._ButtonAccountAction = new System.Windows.Forms.Button();
			this._PanelAccountControls = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._ComboBrowser = new System.Windows.Forms.ComboBox();
			this._TextPassword = new System.Windows.Forms.TextBox();
			this._TextAddress = new System.Windows.Forms.TextBox();
			this._LinkRemove = new Shellscape.UI.Controls.ResponsiveLinkLabel();
			this._LabelPassword = new System.Windows.Forms.Label();
			this._CheckMailto = new System.Windows.Forms.CheckBox();
			this._CheckDefaultAccount = new System.Windows.Forms.CheckBox();
			this._LabelBrowser = new System.Windows.Forms.Label();
			this._LabelAddress = new System.Windows.Forms.Label();
			this._PanelAccountGlyph = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._LabelAccount = new Shellscape.UI.Controls.ThemeLabel();
			this._LabelGeneral = new Shellscape.UI.Controls.ThemeLabel();
			this._ComboSound = new System.Windows.Forms.ComboBox();
			this._LabelSound = new System.Windows.Forms.Label();
			this._ButtonSound = new System.Windows.Forms.Button();
			this._LabelInterval = new System.Windows.Forms.Label();
			this._TextInterval = new Shellscape.UI.Controls.NumericTextBox();
			this._ComboLanguage = new System.Windows.Forms.ComboBox();
			this._LabelLanguage = new System.Windows.Forms.Label();
			this._CheckFlashTaskbar = new System.Windows.Forms.CheckBox();
			this._CheckTray = new System.Windows.Forms.CheckBox();
			this._CheckToast = new System.Windows.Forms.CheckBox();
			this._ButtonApply = new System.Windows.Forms.Button();
			this._PanelGeneral = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._PanelGeneralActions = new Shellscape.UI.Controls.DoubleBufferedFlowPanel();
			this._Panels.SuspendLayout();
			this._PanelAccounts.SuspendLayout();
			this._PanelAccount.SuspendLayout();
			this._PanelAccountActions.SuspendLayout();
			this._PanelAccountControls.SuspendLayout();
			this._PanelGeneral.SuspendLayout();
			this._PanelGeneralActions.SuspendLayout();
			this.SuspendLayout();
			// 
			// _Panels
			// 
			this._Panels.Controls.Add(this._PanelGeneral);
			this._Panels.Controls.Add(this._PanelAccounts);
			this._Panels.Controls.Add(this._PanelAccount);
			this._Panels.Size = new System.Drawing.Size(554, 456);
			// 
			// _PanelAccounts
			// 
			this._PanelAccounts.AutoSize = true;
			this._PanelAccounts.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._PanelAccounts.Controls.Add(this._ListAccounts);
			this._PanelAccounts.Controls.Add(this._LinkAddNew);
			this._PanelAccounts.Controls.Add(this._LabelAccountsTitle);
			this._PanelAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelAccounts.Location = new System.Drawing.Point(6, 16);
			this._PanelAccounts.Name = "_PanelAccounts";
			this._PanelAccounts.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
			this._PanelAccounts.Size = new System.Drawing.Size(542, 424);
			this._PanelAccounts.TabIndex = 1;
			this._PanelAccounts.TabStop = true;
			// 
			// _ListAccounts
			// 
			this._ListAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._ListAccounts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._ListAccounts.LabelWrap = false;
			this._ListAccounts.Location = new System.Drawing.Point(3, 39);
			this._ListAccounts.MultiSelect = false;
			this._ListAccounts.Name = "_ListAccounts";
			this._ListAccounts.Size = new System.Drawing.Size(536, 203);
			this._ListAccounts.TabIndex = 0;
			this._ListAccounts.UseCompatibleStateImageBehavior = false;
			// 
			// _LinkAddNew
			// 
			this._LinkAddNew.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(74)))), ((int)(((byte)(229)))));
			this._LinkAddNew.AutoSize = true;
			this._LinkAddNew.DisabledLinkColor = System.Drawing.Color.Gray;
			this._LinkAddNew.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(74)))), ((int)(((byte)(229)))));
			this._LinkAddNew.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this._LinkAddNew.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(85)))));
			this._LinkAddNew.Location = new System.Drawing.Point(3, 245);
			this._LinkAddNew.Name = "_LinkAddNew";
			this._LinkAddNew.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(85)))));
			this._LinkAddNew.Size = new System.Drawing.Size(109, 15);
			this._LinkAddNew.TabIndex = 1;
			this._LinkAddNew.TabStop = true;
			this._LinkAddNew.Text = "Add a new account";
			this._LinkAddNew.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(85)))));
			// 
			// _LabelAccountsTitle
			// 
			this._LabelAccountsTitle.AutoSize = true;
			this._LabelAccountsTitle.BackColor = System.Drawing.Color.Transparent;
			this._LabelAccountsTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this._LabelAccountsTitle.Location = new System.Drawing.Point(3, 3);
			this._LabelAccountsTitle.MinimumSize = new System.Drawing.Size(100, 20);
			this._LabelAccountsTitle.Name = "_LabelAccountsTitle";
			this._LabelAccountsTitle.Size = new System.Drawing.Size(100, 20);
			this._LabelAccountsTitle.TabIndex = 2;
			this._LabelAccountsTitle.Text = "Theme Label";
			this._LabelAccountsTitle.ThemeElement = null;
			// 
			// _PanelAccount
			// 
			this._PanelAccount.Controls.Add(this._PanelAccountActions);
			this._PanelAccount.Controls.Add(this._PanelAccountControls);
			this._PanelAccount.Controls.Add(this._PanelAccountGlyph);
			this._PanelAccount.Controls.Add(this._LabelAccount);
			this._PanelAccount.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelAccount.Location = new System.Drawing.Point(6, 16);
			this._PanelAccount.Name = "_PanelAccount";
			this._PanelAccount.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
			this._PanelAccount.Size = new System.Drawing.Size(542, 424);
			this._PanelAccount.TabIndex = 2;
			this._PanelAccount.TabStop = true;
			// 
			// _PanelAccountActions
			// 
			this._PanelAccountActions.AutoSize = true;
			this._PanelAccountActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._PanelAccountActions.Controls.Add(this._ButtonAccountCancel);
			this._PanelAccountActions.Controls.Add(this._ButtonAccountAction);
			this._PanelAccountActions.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._PanelAccountActions.Location = new System.Drawing.Point(3, 395);
			this._PanelAccountActions.Name = "_PanelAccountActions";
			this._PanelAccountActions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._PanelAccountActions.Size = new System.Drawing.Size(539, 29);
			this._PanelAccountActions.TabIndex = 8;
			// 
			// _ButtonAccountCancel
			// 
			this._ButtonAccountCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._ButtonAccountCancel.Location = new System.Drawing.Point(461, 3);
			this._ButtonAccountCancel.Name = "_ButtonAccountCancel";
			this._ButtonAccountCancel.Size = new System.Drawing.Size(75, 23);
			this._ButtonAccountCancel.TabIndex = 7;
			this._ButtonAccountCancel.Text = "Cancel";
			this._ButtonAccountCancel.UseVisualStyleBackColor = true;
			// 
			// _ButtonAccountAction
			// 
			this._ButtonAccountAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._ButtonAccountAction.Enabled = false;
			this._ButtonAccountAction.Location = new System.Drawing.Point(330, 3);
			this._ButtonAccountAction.Name = "_ButtonAccountAction";
			this._ButtonAccountAction.Size = new System.Drawing.Size(125, 23);
			this._ButtonAccountAction.TabIndex = 6;
			this._ButtonAccountAction.Text = "Apply Changes";
			this._ButtonAccountAction.UseVisualStyleBackColor = true;
			// 
			// _PanelAccountControls
			// 
			this._PanelAccountControls.Controls.Add(this._ComboBrowser);
			this._PanelAccountControls.Controls.Add(this._TextPassword);
			this._PanelAccountControls.Controls.Add(this._TextAddress);
			this._PanelAccountControls.Controls.Add(this._LinkRemove);
			this._PanelAccountControls.Controls.Add(this._LabelPassword);
			this._PanelAccountControls.Controls.Add(this._CheckMailto);
			this._PanelAccountControls.Controls.Add(this._CheckDefaultAccount);
			this._PanelAccountControls.Controls.Add(this._LabelBrowser);
			this._PanelAccountControls.Controls.Add(this._LabelAddress);
			this._PanelAccountControls.Dock = System.Windows.Forms.DockStyle.Top;
			this._PanelAccountControls.Location = new System.Drawing.Point(3, 103);
			this._PanelAccountControls.Name = "_PanelAccountControls";
			this._PanelAccountControls.Size = new System.Drawing.Size(539, 287);
			this._PanelAccountControls.TabIndex = 0;
			this._PanelAccountControls.TabStop = true;
			// 
			// _ComboBrowser
			// 
			this._ComboBrowser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._ComboBrowser.FormattingEnabled = true;
			this._ComboBrowser.Location = new System.Drawing.Point(213, 58);
			this._ComboBrowser.Name = "_ComboBrowser";
			this._ComboBrowser.Size = new System.Drawing.Size(164, 23);
			this._ComboBrowser.TabIndex = 2;
			// 
			// _TextPassword
			// 
			this._TextPassword.Location = new System.Drawing.Point(213, 29);
			this._TextPassword.Name = "_TextPassword";
			this._TextPassword.Size = new System.Drawing.Size(260, 23);
			this._TextPassword.TabIndex = 1;
			this._TextPassword.UseSystemPasswordChar = true;
			// 
			// _TextAddress
			// 
			this._TextAddress.Location = new System.Drawing.Point(213, 0);
			this._TextAddress.Name = "_TextAddress";
			this._TextAddress.Size = new System.Drawing.Size(260, 23);
			this._TextAddress.TabIndex = 0;
			// 
			// _LinkRemove
			// 
			this._LinkRemove.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(74)))), ((int)(((byte)(229)))));
			this._LinkRemove.AutoSize = true;
			this._LinkRemove.DisabledLinkColor = System.Drawing.Color.Gray;
			this._LinkRemove.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(74)))), ((int)(((byte)(229)))));
			this._LinkRemove.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this._LinkRemove.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(74)))), ((int)(((byte)(229)))));
			this._LinkRemove.Location = new System.Drawing.Point(6, 151);
			this._LinkRemove.Name = "_LinkRemove";
			this._LinkRemove.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(74)))), ((int)(((byte)(229)))));
			this._LinkRemove.Size = new System.Drawing.Size(118, 15);
			this._LinkRemove.TabIndex = 5;
			this._LinkRemove.TabStop = true;
			this._LinkRemove.Text = "Remove this account";
			this._LinkRemove.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(74)))), ((int)(((byte)(229)))));
			// 
			// _LabelPassword
			// 
			this._LabelPassword.AutoSize = true;
			this._LabelPassword.Location = new System.Drawing.Point(3, 32);
			this._LabelPassword.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._LabelPassword.Name = "_LabelPassword";
			this._LabelPassword.Size = new System.Drawing.Size(57, 15);
			this._LabelPassword.TabIndex = 4;
			this._LabelPassword.Text = "Password";
			// 
			// _CheckMailto
			// 
			this._CheckMailto.AutoSize = true;
			this._CheckMailto.Location = new System.Drawing.Point(4, 115);
			this._CheckMailto.Name = "_CheckMailto";
			this._CheckMailto.Size = new System.Drawing.Size(195, 19);
			this._CheckMailto.TabIndex = 4;
			this._CheckMailto.Text = "Use this account for mailto links";
			this._CheckMailto.UseVisualStyleBackColor = true;
			// 
			// _CheckDefaultAccount
			// 
			this._CheckDefaultAccount.AutoSize = true;
			this._CheckDefaultAccount.Location = new System.Drawing.Point(4, 90);
			this._CheckDefaultAccount.Name = "_CheckDefaultAccount";
			this._CheckDefaultAccount.Size = new System.Drawing.Size(233, 19);
			this._CheckDefaultAccount.TabIndex = 3;
			this._CheckDefaultAccount.Text = "Use this account as the default account";
			this._CheckDefaultAccount.UseVisualStyleBackColor = true;
			// 
			// _LabelBrowser
			// 
			this._LabelBrowser.AutoSize = true;
			this._LabelBrowser.Location = new System.Drawing.Point(3, 62);
			this._LabelBrowser.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._LabelBrowser.Name = "_LabelBrowser";
			this._LabelBrowser.Size = new System.Drawing.Size(49, 15);
			this._LabelBrowser.TabIndex = 7;
			this._LabelBrowser.Text = "Browser";
			// 
			// _LabelAddress
			// 
			this._LabelAddress.AutoSize = true;
			this._LabelAddress.Location = new System.Drawing.Point(3, 3);
			this._LabelAddress.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._LabelAddress.Name = "_LabelAddress";
			this._LabelAddress.Size = new System.Drawing.Size(79, 15);
			this._LabelAddress.TabIndex = 8;
			this._LabelAddress.Text = "Email address";
			// 
			// _PanelAccountGlyph
			// 
			this._PanelAccountGlyph.Dock = System.Windows.Forms.DockStyle.Top;
			this._PanelAccountGlyph.Location = new System.Drawing.Point(3, 23);
			this._PanelAccountGlyph.Name = "_PanelAccountGlyph";
			this._PanelAccountGlyph.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this._PanelAccountGlyph.Size = new System.Drawing.Size(539, 80);
			this._PanelAccountGlyph.TabIndex = 1;
			this._PanelAccountGlyph.TabStop = true;
			// 
			// _LabelAccount
			// 
			this._LabelAccount.AutoSize = true;
			this._LabelAccount.BackColor = System.Drawing.Color.Transparent;
			this._LabelAccount.Dock = System.Windows.Forms.DockStyle.Top;
			this._LabelAccount.Location = new System.Drawing.Point(3, 3);
			this._LabelAccount.MinimumSize = new System.Drawing.Size(100, 20);
			this._LabelAccount.Name = "_LabelAccount";
			this._LabelAccount.Size = new System.Drawing.Size(100, 20);
			this._LabelAccount.TabIndex = 4;
			this._LabelAccount.Text = "Manage Account";
			this._LabelAccount.ThemeElement = null;
			// 
			// _LabelGeneral
			// 
			this._LabelGeneral.AutoSize = true;
			this._LabelGeneral.BackColor = System.Drawing.Color.Transparent;
			this._LabelGeneral.Dock = System.Windows.Forms.DockStyle.Top;
			this._LabelGeneral.Location = new System.Drawing.Point(3, 3);
			this._LabelGeneral.MinimumSize = new System.Drawing.Size(100, 20);
			this._LabelGeneral.Name = "_LabelGeneral";
			this._LabelGeneral.Size = new System.Drawing.Size(100, 20);
			this._LabelGeneral.TabIndex = 0;
			this._LabelGeneral.Text = "General";
			this._LabelGeneral.ThemeElement = null;
			// 
			// _ComboSound
			// 
			this._ComboSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._ComboSound.FormattingEnabled = true;
			this._ComboSound.Location = new System.Drawing.Point(216, 39);
			this._ComboSound.Name = "_ComboSound";
			this._ComboSound.Size = new System.Drawing.Size(164, 23);
			this._ComboSound.TabIndex = 0;
			// 
			// _LabelSound
			// 
			this._LabelSound.AutoSize = true;
			this._LabelSound.Location = new System.Drawing.Point(3, 42);
			this._LabelSound.Name = "_LabelSound";
			this._LabelSound.Size = new System.Drawing.Size(107, 15);
			this._LabelSound.TabIndex = 9;
			this._LabelSound.Text = "Sound Notification";
			// 
			// _ButtonSound
			// 
			this._ButtonSound.AutoSize = true;
			this._ButtonSound.Location = new System.Drawing.Point(386, 38);
			this._ButtonSound.Name = "_ButtonSound";
			this._ButtonSound.Size = new System.Drawing.Size(65, 23);
			this._ButtonSound.TabIndex = 1;
			this._ButtonSound.Text = "Browse...";
			this._ButtonSound.UseVisualStyleBackColor = true;
			// 
			// _LabelInterval
			// 
			this._LabelInterval.AutoSize = true;
			this._LabelInterval.Location = new System.Drawing.Point(3, 100);
			this._LabelInterval.Name = "_LabelInterval";
			this._LabelInterval.Size = new System.Drawing.Size(181, 15);
			this._LabelInterval.TabIndex = 7;
			this._LabelInterval.Text = "Check email interval (in minutes)";
			// 
			// _TextInterval
			// 
			this._TextInterval.AllowNegativeValues = true;
			this._TextInterval.Location = new System.Drawing.Point(216, 97);
			this._TextInterval.Name = "_TextInterval";
			this._TextInterval.Size = new System.Drawing.Size(40, 23);
			this._TextInterval.TabIndex = 3;
			// 
			// _ComboLanguage
			// 
			this._ComboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._ComboLanguage.FormattingEnabled = true;
			this._ComboLanguage.Location = new System.Drawing.Point(216, 68);
			this._ComboLanguage.Name = "_ComboLanguage";
			this._ComboLanguage.Size = new System.Drawing.Size(164, 23);
			this._ComboLanguage.TabIndex = 2;
			// 
			// _LabelLanguage
			// 
			this._LabelLanguage.AutoSize = true;
			this._LabelLanguage.Location = new System.Drawing.Point(3, 71);
			this._LabelLanguage.Name = "_LabelLanguage";
			this._LabelLanguage.Size = new System.Drawing.Size(100, 15);
			this._LabelLanguage.TabIndex = 4;
			this._LabelLanguage.Text = "Display Language";
			// 
			// _CheckFlashTaskbar
			// 
			this._CheckFlashTaskbar.AutoSize = true;
			this._CheckFlashTaskbar.Location = new System.Drawing.Point(6, 149);
			this._CheckFlashTaskbar.Name = "_CheckFlashTaskbar";
			this._CheckFlashTaskbar.Size = new System.Drawing.Size(189, 19);
			this._CheckFlashTaskbar.TabIndex = 4;
			this._CheckFlashTaskbar.Text = "Flash the taskbar for new email";
			this._CheckFlashTaskbar.UseVisualStyleBackColor = true;
			// 
			// _CheckTray
			// 
			this._CheckTray.AutoSize = true;
			this._CheckTray.Location = new System.Drawing.Point(6, 174);
			this._CheckTray.Name = "_CheckTray";
			this._CheckTray.Size = new System.Drawing.Size(217, 19);
			this._CheckTray.TabIndex = 5;
			this._CheckTray.Text = "Show email count in the system tray";
			this._CheckTray.UseVisualStyleBackColor = true;
			// 
			// _CheckToast
			// 
			this._CheckToast.AutoSize = true;
			this._CheckToast.Location = new System.Drawing.Point(6, 199);
			this._CheckToast.Name = "_CheckToast";
			this._CheckToast.Size = new System.Drawing.Size(231, 19);
			this._CheckToast.TabIndex = 6;
			this._CheckToast.Text = "Show Toast notifications for new email";
			this._CheckToast.UseVisualStyleBackColor = true;
			// 
			// _ButtonApply
			// 
			this._ButtonApply.Enabled = false;
			this._ButtonApply.Location = new System.Drawing.Point(411, 3);
			this._ButtonApply.Name = "_ButtonApply";
			this._ButtonApply.Size = new System.Drawing.Size(125, 23);
			this._ButtonApply.TabIndex = 0;
			this._ButtonApply.Text = "Apply Changes";
			this._ButtonApply.UseVisualStyleBackColor = true;
			// 
			// _PanelGeneral
			// 
			this._PanelGeneral.Controls.Add(this._PanelGeneralActions);
			this._PanelGeneral.Controls.Add(this._CheckToast);
			this._PanelGeneral.Controls.Add(this._CheckTray);
			this._PanelGeneral.Controls.Add(this._CheckFlashTaskbar);
			this._PanelGeneral.Controls.Add(this._LabelLanguage);
			this._PanelGeneral.Controls.Add(this._ComboLanguage);
			this._PanelGeneral.Controls.Add(this._TextInterval);
			this._PanelGeneral.Controls.Add(this._LabelInterval);
			this._PanelGeneral.Controls.Add(this._ButtonSound);
			this._PanelGeneral.Controls.Add(this._LabelSound);
			this._PanelGeneral.Controls.Add(this._ComboSound);
			this._PanelGeneral.Controls.Add(this._LabelGeneral);
			this._PanelGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelGeneral.Location = new System.Drawing.Point(6, 16);
			this._PanelGeneral.Name = "_PanelGeneral";
			this._PanelGeneral.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
			this._PanelGeneral.Size = new System.Drawing.Size(542, 424);
			this._PanelGeneral.TabIndex = 1;
			this._PanelGeneral.TabStop = true;
			// 
			// _PanelGeneralActions
			// 
			this._PanelGeneralActions.AutoSize = true;
			this._PanelGeneralActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._PanelGeneralActions.Controls.Add(this._ButtonApply);
			this._PanelGeneralActions.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._PanelGeneralActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this._PanelGeneralActions.Location = new System.Drawing.Point(3, 395);
			this._PanelGeneralActions.Name = "_PanelGeneralActions";
			this._PanelGeneralActions.Size = new System.Drawing.Size(539, 29);
			this._PanelGeneralActions.TabIndex = 10;
			// 
			// Preferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(754, 456);
			this.Name = "Preferences";
			this.Text = "Prefs";
			this._Panels.ResumeLayout(false);
			this._Panels.PerformLayout();
			this._PanelAccounts.ResumeLayout(false);
			this._PanelAccounts.PerformLayout();
			this._PanelAccount.ResumeLayout(false);
			this._PanelAccount.PerformLayout();
			this._PanelAccountActions.ResumeLayout(false);
			this._PanelAccountControls.ResumeLayout(false);
			this._PanelAccountControls.PerformLayout();
			this._PanelGeneral.ResumeLayout(false);
			this._PanelGeneral.PerformLayout();
			this._PanelGeneralActions.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelAccounts;
		private Shellscape.UI.Controls.FlickerFreeListView _ListAccounts;
		private Shellscape.UI.Controls.ResponsiveLinkLabel _LinkAddNew;
		private Shellscape.UI.Controls.ThemeLabel _LabelAccountsTitle;
		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelAccount;
		private Shellscape.UI.Controls.ThemeLabel _LabelAccount;
		private System.Windows.Forms.Button _ButtonAccountCancel;
		private System.Windows.Forms.Button _ButtonAccountAction;
		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelAccountGlyph;
		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelAccountControls;
		private Shellscape.UI.Controls.ResponsiveLinkLabel _LinkRemove;
		private System.Windows.Forms.Label _LabelPassword;
		private System.Windows.Forms.CheckBox _CheckMailto;
		private System.Windows.Forms.CheckBox _CheckDefaultAccount;
		private System.Windows.Forms.Label _LabelBrowser;
		private System.Windows.Forms.Label _LabelAddress;
		private System.Windows.Forms.ComboBox _ComboBrowser;
		private System.Windows.Forms.TextBox _TextPassword;
		private System.Windows.Forms.TextBox _TextAddress;
		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelGeneral;
		private System.Windows.Forms.Button _ButtonApply;
		private System.Windows.Forms.CheckBox _CheckToast;
		private System.Windows.Forms.CheckBox _CheckTray;
		private System.Windows.Forms.CheckBox _CheckFlashTaskbar;
		private System.Windows.Forms.Label _LabelLanguage;
		private System.Windows.Forms.ComboBox _ComboLanguage;
		private Shellscape.UI.Controls.NumericTextBox _TextInterval;
		private System.Windows.Forms.Label _LabelInterval;
		private System.Windows.Forms.Button _ButtonSound;
		private System.Windows.Forms.Label _LabelSound;
		private System.Windows.Forms.ComboBox _ComboSound;
		private Shellscape.UI.Controls.ThemeLabel _LabelGeneral;
		private Shellscape.UI.Controls.DoubleBufferedFlowPanel _PanelGeneralActions;
		private Shellscape.UI.Controls.DoubleBufferedFlowPanel _PanelAccountActions;

	}
}