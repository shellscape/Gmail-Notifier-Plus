namespace GmailNotifierPlus.Forms {
	partial class Settings {
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
			this._ButtonGroup = new Shellscape.UI.Skipe.SkipeButtonGroup();
			this._ButtonGeneral = new Shellscape.UI.Skipe.SkipeButton();
			this._PanelGeneral = new Shellscape.UI.Skipe.SkipePanel();
			this._ComboSound = new System.Windows.Forms.ComboBox();
			this._LabelSound = new System.Windows.Forms.Label();
			this._ButtonBrowse = new System.Windows.Forms.Button();
			this._TextInterval = new System.Windows.Forms.TextBox();
			this._LabelMinutes = new System.Windows.Forms.Label();
			this._LabelInterval = new System.Windows.Forms.Label();
			this._ButtonAccounts = new Shellscape.UI.Skipe.SkipeButton();
			this._PanelAccounts = new Shellscape.UI.Skipe.SkipePanel();
			this._LabelAccountIntro = new System.Windows.Forms.Label();
			this._ButtonNewAccount = new System.Windows.Forms.Button();
			this._ButtonAppearance = new Shellscape.UI.Skipe.SkipeButton();
			this._PanelAppearance = new Shellscape.UI.Skipe.SkipePanel();
			this._LabelLanguage = new System.Windows.Forms.Label();
			this._ComboLanguage = new System.Windows.Forms.ComboBox();
			this._PanelNewAccount = new Shellscape.UI.Skipe.SkipePanel();
			this._PictureExclamation = new System.Windows.Forms.PictureBox();
			this._PanelAccountButtons = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._ButtonNewCancel = new System.Windows.Forms.Button();
			this._ButtonNewSave = new System.Windows.Forms.Button();
			this._LabelError = new System.Windows.Forms.Label();
			this._LabelAccountTitle = new System.Windows.Forms.Label();
			this._LabelUsername = new System.Windows.Forms.Label();
			this._TextPassword = new System.Windows.Forms.TextBox();
			this._LabelPassword = new System.Windows.Forms.Label();
			this._TextUsername = new System.Windows.Forms.TextBox();
			this._PanelButtons = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._ButtonCancel = new System.Windows.Forms.Button();
			this._ButtonSave = new System.Windows.Forms.Button();
			this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
			this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
			this._CheckFlash = new System.Windows.Forms.CheckBox();
			this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
			this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
			this._CheckTray = new System.Windows.Forms.CheckBox();
			this._CheckToast = new System.Windows.Forms.CheckBox();
			this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
			this._CheckUpdates = new System.Windows.Forms.CheckBox();
			this._ButtonGroup.SuspendLayout();
			this._PanelGeneral.SuspendLayout();
			this._PanelAccounts.SuspendLayout();
			this._PanelAppearance.SuspendLayout();
			this._PanelNewAccount.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._PictureExclamation)).BeginInit();
			this._PanelAccountButtons.SuspendLayout();
			this._PanelButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// _ButtonGroup
			// 
			this._ButtonGroup.BackColor = System.Drawing.Color.Transparent;
			this._ButtonGroup.Controls.Add(this._ButtonGeneral);
			this._ButtonGroup.Controls.Add(this._ButtonAccounts);
			this._ButtonGroup.Controls.Add(this._ButtonAppearance);
			this._ButtonGroup.Dock = System.Windows.Forms.DockStyle.Left;
			this._ButtonGroup.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._ButtonGroup.Location = new System.Drawing.Point(8, 8);
			this._ButtonGroup.Name = "_ButtonGroup";
			this._ButtonGroup.Size = new System.Drawing.Size(182, 316);
			this._ButtonGroup.TabIndex = 0;
			// 
			// _ButtonGeneral
			// 
			this._ButtonGeneral.AssociatedPanel = this._PanelGeneral;
			this._ButtonGeneral.BackColor = System.Drawing.Color.Transparent;
			this._ButtonGeneral.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._ButtonGeneral.ButtonBackColor = System.Drawing.Color.Transparent;
			this._ButtonGeneral.ButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._ButtonGeneral.ButtonHeight = 40;
			this._ButtonGeneral.ButtonImage = null;
			this._ButtonGeneral.ButtonPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._ButtonGeneral.ButtonText = "General";
			this._ButtonGeneral.DownColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this._ButtonGeneral.DownColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._ButtonGeneral.Expandable = true;
			this._ButtonGeneral.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._ButtonGeneral.HoverColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._ButtonGeneral.HoverColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(251)))));
			this._ButtonGeneral.Location = new System.Drawing.Point(0, 0);
			this._ButtonGeneral.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
			this._ButtonGeneral.Name = "_ButtonGeneral";
			this._ButtonGeneral.NormalColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._ButtonGeneral.NormalColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._ButtonGeneral.ShadowColor = System.Drawing.SystemColors.ActiveBorder;
			this._ButtonGeneral.Size = new System.Drawing.Size(175, 40);
			this._ButtonGeneral.TabIndex = 1;
			// 
			// _PanelGeneral
			// 
			this._PanelGeneral.BackColor = System.Drawing.Color.Transparent;
			this._PanelGeneral.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._PanelGeneral.ControlBackColor = System.Drawing.Color.White;
			this._PanelGeneral.Controls.Add(this._CheckUpdates);
			this._PanelGeneral.Controls.Add(this._ComboSound);
			this._PanelGeneral.Controls.Add(this._LabelSound);
			this._PanelGeneral.Controls.Add(this._ButtonBrowse);
			this._PanelGeneral.Controls.Add(this._TextInterval);
			this._PanelGeneral.Controls.Add(this._LabelMinutes);
			this._PanelGeneral.Controls.Add(this._LabelInterval);
			this._PanelGeneral.Controls.Add(this.shapeContainer1);
			this._PanelGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelGeneral.DrawHeader = true;
			this._PanelGeneral.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._PanelGeneral.HeaderColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._PanelGeneral.HeaderColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._PanelGeneral.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._PanelGeneral.HeaderHeight = 39;
			this._PanelGeneral.HeaderImage = null;
			this._PanelGeneral.HeaderPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._PanelGeneral.HeaderText = "Bread n\' Butter settings for Gmail Notifier Plus";
			this._PanelGeneral.Location = new System.Drawing.Point(190, 8);
			this._PanelGeneral.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
			this._PanelGeneral.Name = "_PanelGeneral";
			this._PanelGeneral.Padding = new System.Windows.Forms.Padding(10, 45, 10, 10);
			this._PanelGeneral.SeperatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._PanelGeneral.Size = new System.Drawing.Size(475, 316);
			this._PanelGeneral.TabIndex = 1;
			// 
			// _ComboSound
			// 
			this._ComboSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._ComboSound.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ComboSound.FormattingEnabled = true;
			this._ComboSound.Location = new System.Drawing.Point(16, 120);
			this._ComboSound.Name = "_ComboSound";
			this._ComboSound.Size = new System.Drawing.Size(164, 23);
			this._ComboSound.TabIndex = 1;
			// 
			// _LabelSound
			// 
			this._LabelSound.AutoSize = true;
			this._LabelSound.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelSound.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
			this._LabelSound.Location = new System.Drawing.Point(8, 87);
			this._LabelSound.Name = "_LabelSound";
			this._LabelSound.Size = new System.Drawing.Size(39, 21);
			this._LabelSound.TabIndex = 48;
			this._LabelSound.Text = "Title";
			// 
			// _ButtonBrowse
			// 
			this._ButtonBrowse.AutoSize = true;
			this._ButtonBrowse.Enabled = false;
			this._ButtonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonBrowse.Location = new System.Drawing.Point(189, 119);
			this._ButtonBrowse.MinimumSize = new System.Drawing.Size(65, 23);
			this._ButtonBrowse.Name = "_ButtonBrowse";
			this._ButtonBrowse.Size = new System.Drawing.Size(68, 26);
			this._ButtonBrowse.TabIndex = 2;
			this._ButtonBrowse.Text = "Browse...";
			this._ButtonBrowse.UseVisualStyleBackColor = true;
			this._ButtonBrowse.Click += new System.EventHandler(this._ButtonBrowse_Click);
			// 
			// _TextInterval
			// 
			this._TextInterval.Location = new System.Drawing.Point(117, 46);
			this._TextInterval.Name = "_TextInterval";
			this._TextInterval.Size = new System.Drawing.Size(27, 23);
			this._TextInterval.TabIndex = 0;
			this._TextInterval.Text = "1";
			this._TextInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this._TextInterval.Leave += new System.EventHandler(this._TextInterval_Leave);
			// 
			// _LabelMinutes
			// 
			this._LabelMinutes.AutoSize = true;
			this._LabelMinutes.Location = new System.Drawing.Point(150, 49);
			this._LabelMinutes.Name = "_LabelMinutes";
			this._LabelMinutes.Size = new System.Drawing.Size(58, 15);
			this._LabelMinutes.TabIndex = 45;
			this._LabelMinutes.Text = "minute(s)";
			// 
			// _LabelInterval
			// 
			this._LabelInterval.AutoSize = true;
			this._LabelInterval.Location = new System.Drawing.Point(12, 48);
			this._LabelInterval.Name = "_LabelInterval";
			this._LabelInterval.Size = new System.Drawing.Size(106, 15);
			this._LabelInterval.TabIndex = 44;
			this._LabelInterval.Text = "Check Email every:";
			// 
			// _ButtonAccounts
			// 
			this._ButtonAccounts.AssociatedPanel = this._PanelAccounts;
			this._ButtonAccounts.BackColor = System.Drawing.Color.Transparent;
			this._ButtonAccounts.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._ButtonAccounts.ButtonBackColor = System.Drawing.Color.Transparent;
			this._ButtonAccounts.ButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._ButtonAccounts.ButtonHeight = 40;
			this._ButtonAccounts.ButtonImage = null;
			this._ButtonAccounts.ButtonPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._ButtonAccounts.ButtonText = "Accounts";
			this._ButtonAccounts.DownColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this._ButtonAccounts.DownColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._ButtonAccounts.Expandable = true;
			this._ButtonAccounts.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._ButtonAccounts.HoverColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._ButtonAccounts.HoverColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(251)))));
			this._ButtonAccounts.Location = new System.Drawing.Point(0, 46);
			this._ButtonAccounts.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
			this._ButtonAccounts.Name = "_ButtonAccounts";
			this._ButtonAccounts.NormalColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._ButtonAccounts.NormalColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._ButtonAccounts.ShadowColor = System.Drawing.SystemColors.ActiveBorder;
			this._ButtonAccounts.Size = new System.Drawing.Size(175, 40);
			this._ButtonAccounts.TabIndex = 0;
			// 
			// _PanelAccounts
			// 
			this._PanelAccounts.BackColor = System.Drawing.Color.Transparent;
			this._PanelAccounts.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._PanelAccounts.ControlBackColor = System.Drawing.Color.White;
			this._PanelAccounts.Controls.Add(this._LabelAccountIntro);
			this._PanelAccounts.Controls.Add(this._ButtonNewAccount);
			this._PanelAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelAccounts.DrawHeader = true;
			this._PanelAccounts.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._PanelAccounts.HeaderColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._PanelAccounts.HeaderColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._PanelAccounts.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._PanelAccounts.HeaderHeight = 39;
			this._PanelAccounts.HeaderImage = null;
			this._PanelAccounts.HeaderPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._PanelAccounts.HeaderText = "Manage your accounts";
			this._PanelAccounts.Location = new System.Drawing.Point(190, 8);
			this._PanelAccounts.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
			this._PanelAccounts.Name = "_PanelAccounts";
			this._PanelAccounts.Padding = new System.Windows.Forms.Padding(10, 45, 10, 10);
			this._PanelAccounts.SeperatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._PanelAccounts.Size = new System.Drawing.Size(475, 316);
			this._PanelAccounts.TabIndex = 2;
			// 
			// _LabelAccountIntro
			// 
			this._LabelAccountIntro.AutoSize = true;
			this._LabelAccountIntro.Location = new System.Drawing.Point(13, 45);
			this._LabelAccountIntro.MaximumSize = new System.Drawing.Size(450, 0);
			this._LabelAccountIntro.Name = "_LabelAccountIntro";
			this._LabelAccountIntro.Size = new System.Drawing.Size(436, 30);
			this._LabelAccountIntro.TabIndex = 1;
			this._LabelAccountIntro.Text = "Click an account to the left to manage your individual accounts. Click the button" +
					" below to add a new account.";
			// 
			// _ButtonNewAccount
			// 
			this._ButtonNewAccount.AutoSize = true;
			this._ButtonNewAccount.Location = new System.Drawing.Point(292, 92);
			this._ButtonNewAccount.Name = "_ButtonNewAccount";
			this._ButtonNewAccount.Size = new System.Drawing.Size(170, 30);
			this._ButtonNewAccount.TabIndex = 0;
			this._ButtonNewAccount.Text = "Add New Account";
			this._ButtonNewAccount.UseVisualStyleBackColor = true;
			this._ButtonNewAccount.Click += new System.EventHandler(this._ButtonNewAccount_Click);
			// 
			// _ButtonAppearance
			// 
			this._ButtonAppearance.AssociatedPanel = this._PanelAppearance;
			this._ButtonAppearance.BackColor = System.Drawing.Color.Transparent;
			this._ButtonAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._ButtonAppearance.ButtonBackColor = System.Drawing.Color.Transparent;
			this._ButtonAppearance.ButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._ButtonAppearance.ButtonHeight = 40;
			this._ButtonAppearance.ButtonImage = null;
			this._ButtonAppearance.ButtonPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._ButtonAppearance.ButtonText = "Appearance";
			this._ButtonAppearance.DownColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this._ButtonAppearance.DownColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._ButtonAppearance.Expandable = true;
			this._ButtonAppearance.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._ButtonAppearance.HoverColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._ButtonAppearance.HoverColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(251)))));
			this._ButtonAppearance.Location = new System.Drawing.Point(0, 92);
			this._ButtonAppearance.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
			this._ButtonAppearance.Name = "_ButtonAppearance";
			this._ButtonAppearance.NormalColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._ButtonAppearance.NormalColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._ButtonAppearance.ShadowColor = System.Drawing.SystemColors.ActiveBorder;
			this._ButtonAppearance.Size = new System.Drawing.Size(175, 40);
			this._ButtonAppearance.TabIndex = 2;
			// 
			// _PanelAppearance
			// 
			this._PanelAppearance.BackColor = System.Drawing.Color.Transparent;
			this._PanelAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._PanelAppearance.ControlBackColor = System.Drawing.Color.White;
			this._PanelAppearance.Controls.Add(this._CheckToast);
			this._PanelAppearance.Controls.Add(this._CheckTray);
			this._PanelAppearance.Controls.Add(this._CheckFlash);
			this._PanelAppearance.Controls.Add(this._LabelLanguage);
			this._PanelAppearance.Controls.Add(this._ComboLanguage);
			this._PanelAppearance.Controls.Add(this.shapeContainer2);
			this._PanelAppearance.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelAppearance.DrawHeader = true;
			this._PanelAppearance.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._PanelAppearance.HeaderColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._PanelAppearance.HeaderColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._PanelAppearance.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._PanelAppearance.HeaderHeight = 39;
			this._PanelAppearance.HeaderImage = null;
			this._PanelAppearance.HeaderPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._PanelAppearance.HeaderText = "Tell Gmail Notifier Plus how to act and look";
			this._PanelAppearance.Location = new System.Drawing.Point(190, 8);
			this._PanelAppearance.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
			this._PanelAppearance.Name = "_PanelAppearance";
			this._PanelAppearance.Padding = new System.Windows.Forms.Padding(10, 45, 10, 10);
			this._PanelAppearance.SeperatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._PanelAppearance.Size = new System.Drawing.Size(475, 316);
			this._PanelAppearance.TabIndex = 3;
			// 
			// _LabelLanguage
			// 
			this._LabelLanguage.AutoSize = true;
			this._LabelLanguage.Location = new System.Drawing.Point(11, 51);
			this._LabelLanguage.Name = "_LabelLanguage";
			this._LabelLanguage.Size = new System.Drawing.Size(100, 15);
			this._LabelLanguage.TabIndex = 48;
			this._LabelLanguage.Text = "Display language:";
			// 
			// _ComboLanguage
			// 
			this._ComboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._ComboLanguage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ComboLanguage.FormattingEnabled = true;
			this._ComboLanguage.Location = new System.Drawing.Point(117, 48);
			this._ComboLanguage.Name = "_ComboLanguage";
			this._ComboLanguage.Size = new System.Drawing.Size(134, 23);
			this._ComboLanguage.TabIndex = 0;
			// 
			// _PanelNewAccount
			// 
			this._PanelNewAccount.BackColor = System.Drawing.Color.Transparent;
			this._PanelNewAccount.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._PanelNewAccount.ControlBackColor = System.Drawing.Color.White;
			this._PanelNewAccount.Controls.Add(this._PictureExclamation);
			this._PanelNewAccount.Controls.Add(this._PanelAccountButtons);
			this._PanelNewAccount.Controls.Add(this._LabelError);
			this._PanelNewAccount.Controls.Add(this._LabelAccountTitle);
			this._PanelNewAccount.Controls.Add(this._LabelUsername);
			this._PanelNewAccount.Controls.Add(this._TextPassword);
			this._PanelNewAccount.Controls.Add(this._LabelPassword);
			this._PanelNewAccount.Controls.Add(this._TextUsername);
			this._PanelNewAccount.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelNewAccount.DrawHeader = true;
			this._PanelNewAccount.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._PanelNewAccount.HeaderColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._PanelNewAccount.HeaderColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._PanelNewAccount.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._PanelNewAccount.HeaderHeight = 39;
			this._PanelNewAccount.HeaderImage = null;
			this._PanelNewAccount.HeaderPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._PanelNewAccount.HeaderText = "Add New Account";
			this._PanelNewAccount.Location = new System.Drawing.Point(190, 8);
			this._PanelNewAccount.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
			this._PanelNewAccount.Name = "_PanelNewAccount";
			this._PanelNewAccount.Padding = new System.Windows.Forms.Padding(2, 45, 2, 2);
			this._PanelNewAccount.SeperatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._PanelNewAccount.Size = new System.Drawing.Size(475, 316);
			this._PanelNewAccount.TabIndex = 4;
			// 
			// _PictureExclamation
			// 
			this._PictureExclamation.Location = new System.Drawing.Point(100, 138);
			this._PictureExclamation.MinimumSize = new System.Drawing.Size(16, 16);
			this._PictureExclamation.Name = "_PictureExclamation";
			this._PictureExclamation.Size = new System.Drawing.Size(16, 16);
			this._PictureExclamation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this._PictureExclamation.TabIndex = 36;
			this._PictureExclamation.TabStop = false;
			// 
			// _PanelAccountButtons
			// 
			this._PanelAccountButtons.BackColor = System.Drawing.SystemColors.Control;
			this._PanelAccountButtons.Controls.Add(this._ButtonNewCancel);
			this._PanelAccountButtons.Controls.Add(this._ButtonNewSave);
			this._PanelAccountButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._PanelAccountButtons.Location = new System.Drawing.Point(2, 274);
			this._PanelAccountButtons.Name = "_PanelAccountButtons";
			this._PanelAccountButtons.Size = new System.Drawing.Size(471, 40);
			this._PanelAccountButtons.TabIndex = 2;
			// 
			// _ButtonNewCancel
			// 
			this._ButtonNewCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._ButtonNewCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonNewCancel.Location = new System.Drawing.Point(385, 6);
			this._ButtonNewCancel.Name = "_ButtonNewCancel";
			this._ButtonNewCancel.Size = new System.Drawing.Size(75, 30);
			this._ButtonNewCancel.TabIndex = 3;
			this._ButtonNewCancel.Text = "Cancel";
			this._ButtonNewCancel.UseVisualStyleBackColor = true;
			this._ButtonNewCancel.Click += new System.EventHandler(this._ButtonNewCancel_Click);
			// 
			// _ButtonNewSave
			// 
			this._ButtonNewSave.Enabled = false;
			this._ButtonNewSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonNewSave.Location = new System.Drawing.Point(304, 6);
			this._ButtonNewSave.Name = "_ButtonNewSave";
			this._ButtonNewSave.Size = new System.Drawing.Size(75, 30);
			this._ButtonNewSave.TabIndex = 2;
			this._ButtonNewSave.Text = "Save";
			this._ButtonNewSave.UseVisualStyleBackColor = true;
			this._ButtonNewSave.Click += new System.EventHandler(this._ButtonNewSave_Click);
			// 
			// _LabelError
			// 
			this._LabelError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(38)))), ((int)(((byte)(30)))));
			this._LabelError.Location = new System.Drawing.Point(120, 138);
			this._LabelError.Name = "_LabelError";
			this._LabelError.Size = new System.Drawing.Size(229, 26);
			this._LabelError.TabIndex = 35;
			this._LabelError.Text = "An account with this username already exists. Please enter a different username.";
			// 
			// _LabelAccountTitle
			// 
			this._LabelAccountTitle.AutoSize = true;
			this._LabelAccountTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelAccountTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
			this._LabelAccountTitle.Location = new System.Drawing.Point(8, 48);
			this._LabelAccountTitle.Name = "_LabelAccountTitle";
			this._LabelAccountTitle.Size = new System.Drawing.Size(39, 21);
			this._LabelAccountTitle.TabIndex = 34;
			this._LabelAccountTitle.Text = "Title";
			// 
			// _LabelUsername
			// 
			this._LabelUsername.AutoSize = true;
			this._LabelUsername.Location = new System.Drawing.Point(12, 80);
			this._LabelUsername.Name = "_LabelUsername";
			this._LabelUsername.Size = new System.Drawing.Size(63, 15);
			this._LabelUsername.TabIndex = 32;
			this._LabelUsername.Text = "Username:";
			// 
			// _TextPassword
			// 
			this._TextPassword.Location = new System.Drawing.Point(100, 110);
			this._TextPassword.Name = "_TextPassword";
			this._TextPassword.Size = new System.Drawing.Size(249, 23);
			this._TextPassword.TabIndex = 1;
			this._TextPassword.UseSystemPasswordChar = true;
			// 
			// _LabelPassword
			// 
			this._LabelPassword.AutoSize = true;
			this._LabelPassword.Location = new System.Drawing.Point(12, 110);
			this._LabelPassword.Name = "_LabelPassword";
			this._LabelPassword.Size = new System.Drawing.Size(60, 15);
			this._LabelPassword.TabIndex = 33;
			this._LabelPassword.Text = "Password:";
			// 
			// _TextUsername
			// 
			this._TextUsername.Location = new System.Drawing.Point(100, 80);
			this._TextUsername.Name = "_TextUsername";
			this._TextUsername.Size = new System.Drawing.Size(249, 23);
			this._TextUsername.TabIndex = 0;
			// 
			// _PanelButtons
			// 
			this._PanelButtons.BackColor = System.Drawing.Color.Transparent;
			this._PanelButtons.Controls.Add(this._ButtonCancel);
			this._PanelButtons.Controls.Add(this._ButtonSave);
			this._PanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._PanelButtons.Location = new System.Drawing.Point(8, 324);
			this._PanelButtons.Name = "_PanelButtons";
			this._PanelButtons.Size = new System.Drawing.Size(657, 40);
			this._PanelButtons.TabIndex = 5;
			// 
			// _ButtonCancel
			// 
			this._ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._ButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonCancel.Location = new System.Drawing.Point(579, 9);
			this._ButtonCancel.Name = "_ButtonCancel";
			this._ButtonCancel.Size = new System.Drawing.Size(75, 30);
			this._ButtonCancel.TabIndex = 3;
			this._ButtonCancel.TabStop = false;
			this._ButtonCancel.Text = "Cancel";
			this._ButtonCancel.UseVisualStyleBackColor = true;
			this._ButtonCancel.Click += new System.EventHandler(this._ButtonCancel_Click);
			// 
			// _ButtonSave
			// 
			this._ButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._ButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonSave.Location = new System.Drawing.Point(498, 9);
			this._ButtonSave.Name = "_ButtonSave";
			this._ButtonSave.Size = new System.Drawing.Size(75, 30);
			this._ButtonSave.TabIndex = 2;
			this._ButtonSave.TabStop = false;
			this._ButtonSave.Text = "Save";
			this._ButtonSave.UseVisualStyleBackColor = true;
			this._ButtonSave.Click += new System.EventHandler(this._ButtonSave_Click);
			// 
			// shapeContainer1
			// 
			this.shapeContainer1.Location = new System.Drawing.Point(10, 45);
			this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
			this.shapeContainer1.Name = "shapeContainer1";
			this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape3,
            this.lineShape1});
			this.shapeContainer1.Size = new System.Drawing.Size(455, 261);
			this.shapeContainer1.TabIndex = 49;
			this.shapeContainer1.TabStop = false;
			// 
			// lineShape1
			// 
			this.lineShape1.BorderColor = System.Drawing.Color.Red;
			this.lineShape1.Name = "lineShape1";
			this.lineShape1.X1 = 0;
			this.lineShape1.X2 = 300;
			this.lineShape1.Y1 = 31;
			this.lineShape1.Y2 = 31;
			// 
			// _CheckFlash
			// 
			this._CheckFlash.AutoSize = true;
			this._CheckFlash.Location = new System.Drawing.Point(15, 91);
			this._CheckFlash.Name = "_CheckFlash";
			this._CheckFlash.Size = new System.Drawing.Size(172, 19);
			this._CheckFlash.TabIndex = 1;
			this._CheckFlash.Text = "Flash Taskbar for new Email";
			this._CheckFlash.UseVisualStyleBackColor = true;
			// 
			// lineShape2
			// 
			this.lineShape2.BorderColor = System.Drawing.Color.Red;
			this.lineShape2.Name = "lineShape2";
			this.lineShape2.X1 = -6;
			this.lineShape2.X2 = 294;
			this.lineShape2.Y1 = 35;
			this.lineShape2.Y2 = 35;
			// 
			// shapeContainer2
			// 
			this.shapeContainer2.Location = new System.Drawing.Point(10, 45);
			this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
			this.shapeContainer2.Name = "shapeContainer2";
			this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2});
			this.shapeContainer2.Size = new System.Drawing.Size(455, 261);
			this.shapeContainer2.TabIndex = 50;
			this.shapeContainer2.TabStop = false;
			// 
			// _CheckTray
			// 
			this._CheckTray.AutoSize = true;
			this._CheckTray.Location = new System.Drawing.Point(15, 116);
			this._CheckTray.Name = "_CheckTray";
			this._CheckTray.Size = new System.Drawing.Size(201, 19);
			this._CheckTray.TabIndex = 2;
			this._CheckTray.Text = "Show Email count in System Tray";
			this._CheckTray.UseVisualStyleBackColor = true;
			// 
			// _CheckToast
			// 
			this._CheckToast.AutoSize = true;
			this._CheckToast.Location = new System.Drawing.Point(15, 141);
			this._CheckToast.Name = "_CheckToast";
			this._CheckToast.Size = new System.Drawing.Size(231, 19);
			this._CheckToast.TabIndex = 3;
			this._CheckToast.Text = "Show Toast notifications for new Email";
			this._CheckToast.UseVisualStyleBackColor = true;
			// 
			// lineShape3
			// 
			this.lineShape3.BorderColor = System.Drawing.Color.Red;
			this.lineShape3.Name = "lineShape3";
			this.lineShape3.X1 = 0;
			this.lineShape3.X2 = 300;
			this.lineShape3.Y1 = 107;
			this.lineShape3.Y2 = 107;
			// 
			// _CheckUpdates
			// 
			this._CheckUpdates.AutoSize = true;
			this._CheckUpdates.Location = new System.Drawing.Point(15, 166);
			this._CheckUpdates.Name = "_CheckUpdates";
			this._CheckUpdates.Size = new System.Drawing.Size(123, 19);
			this._CheckUpdates.TabIndex = 50;
			this._CheckUpdates.Text = "Check for Updates";
			this._CheckUpdates.UseVisualStyleBackColor = true;
			// 
			// Settings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(673, 372);
			this.Controls.Add(this._PanelAppearance);
			this.Controls.Add(this._PanelGeneral);
			this.Controls.Add(this._PanelNewAccount);
			this.Controls.Add(this._PanelAccounts);
			this.Controls.Add(this._ButtonGroup);
			this.Controls.Add(this._PanelButtons);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Settings";
			this.Text = "Settings";
			this._ButtonGroup.ResumeLayout(false);
			this._PanelGeneral.ResumeLayout(false);
			this._PanelGeneral.PerformLayout();
			this._PanelAccounts.ResumeLayout(false);
			this._PanelAccounts.PerformLayout();
			this._PanelAppearance.ResumeLayout(false);
			this._PanelAppearance.PerformLayout();
			this._PanelNewAccount.ResumeLayout(false);
			this._PanelNewAccount.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._PictureExclamation)).EndInit();
			this._PanelAccountButtons.ResumeLayout(false);
			this._PanelButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Shellscape.UI.Skipe.SkipeButtonGroup _ButtonGroup;
		private Shellscape.UI.Skipe.SkipeButton _ButtonAccounts;
		private Shellscape.UI.Skipe.SkipeButton _ButtonGeneral;
		private Shellscape.UI.Skipe.SkipeButton _ButtonAppearance;
		private Shellscape.UI.Skipe.SkipePanel _PanelGeneral;
		private Shellscape.UI.Skipe.SkipePanel _PanelAccounts;
		private Shellscape.UI.Skipe.SkipePanel _PanelAppearance;
		private System.Windows.Forms.ComboBox _ComboSound;
		private System.Windows.Forms.Label _LabelSound;
		private System.Windows.Forms.Button _ButtonBrowse;
		private System.Windows.Forms.TextBox _TextInterval;
		private System.Windows.Forms.Label _LabelMinutes;
		private System.Windows.Forms.Label _LabelInterval;
		private Shellscape.UI.Skipe.SkipePanel _PanelNewAccount;
		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelAccountButtons;
		private System.Windows.Forms.Button _ButtonNewCancel;
		private System.Windows.Forms.Button _ButtonNewSave;
		private System.Windows.Forms.Button _ButtonNewAccount;
		private System.Windows.Forms.Label _LabelAccountIntro;
		private System.Windows.Forms.Label _LabelLanguage;
		private System.Windows.Forms.ComboBox _ComboLanguage;
		private System.Windows.Forms.PictureBox _PictureExclamation;
		private System.Windows.Forms.Label _LabelError;
		private System.Windows.Forms.Label _LabelAccountTitle;
		private System.Windows.Forms.Label _LabelUsername;
		private System.Windows.Forms.TextBox _TextPassword;
		private System.Windows.Forms.Label _LabelPassword;
		private System.Windows.Forms.TextBox _TextUsername;
		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelButtons;
		private System.Windows.Forms.Button _ButtonCancel;
		private System.Windows.Forms.Button _ButtonSave;
		private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
		private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
		private System.Windows.Forms.CheckBox _CheckFlash;
		private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
		private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
		private System.Windows.Forms.CheckBox _CheckToast;
		private System.Windows.Forms.CheckBox _CheckTray;
		private System.Windows.Forms.CheckBox _CheckUpdates;
		private Microsoft.VisualBasic.PowerPacks.LineShape lineShape3;
	}
}