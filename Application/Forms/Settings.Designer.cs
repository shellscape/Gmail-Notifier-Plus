using System.Drawing;
using System.Windows.Forms;

using VistaControls;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using GmailNotifierPlus.Controls;

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
			this.components = new System.ComponentModel.Container();
			this._ButtonAboutOk = new System.Windows.Forms.Button();
			this._ButtonAccountCancel = new System.Windows.Forms.Button();
			this._ButtonAccountSave = new System.Windows.Forms.Button();
			this._ButtonBrowse = new System.Windows.Forms.Button();
			this._ButtonCancel = new System.Windows.Forms.Button();
			this._ButtonDefault = new System.Windows.Forms.Button();
			this._ButtonEdit = new System.Windows.Forms.Button();
			this._ButtonOk = new System.Windows.Forms.Button();
			this._LabelAccountTitle = new System.Windows.Forms.Label();
			this._LabelAdditional = new System.Windows.Forms.Label();
			this._LabelError = new System.Windows.Forms.Label();
			this._LabelInterval = new System.Windows.Forms.Label();
			this._LabelLanguage = new System.Windows.Forms.Label();
			this._LabelMinutes = new System.Windows.Forms.Label();
			this._LabelPassword = new System.Windows.Forms.Label();
			this._LabelSound = new System.Windows.Forms.Label();
			this._LabelTitle = new System.Windows.Forms.Label();
			this._LabelUsername = new System.Windows.Forms.Label();
			this._TextPassword = new VistaControls.TextBox();
			this._TextUsername = new VistaControls.TextBox();
			this._ListViewAccounts = new VistaControls.ListView();
			this._ColumnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._PictureExclamation = new System.Windows.Forms.PictureBox();
			this._ComboLanguage = new VistaControls.ComboBox();
			this._ComboSound = new VistaControls.ComboBox();
			this._TextInterval = new System.Windows.Forms.TextBox();
			this._PanelAbout = new System.Windows.Forms.Panel();
			this._PanelAboutButtons = new System.Windows.Forms.Panel();
			this._PanelAccount = new System.Windows.Forms.Panel();
			this._PanelAccountButtons = new System.Windows.Forms.Panel();
			this._PanelButtons = new System.Windows.Forms.Panel();
			this._ImgButtonAbout = new GmailNotifierPlus.Controls.ImageButton();
			this._PanelMain = new System.Windows.Forms.Panel();
			this._ImgButtonRemove = new GmailNotifierPlus.Controls.ImageButton();
			this._ImgButtonAdd = new GmailNotifierPlus.Controls.ImageButton();
			this._PanelSlider = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this._PictureExclamation)).BeginInit();
			this._PanelAbout.SuspendLayout();
			this._PanelAboutButtons.SuspendLayout();
			this._PanelAccount.SuspendLayout();
			this._PanelAccountButtons.SuspendLayout();
			this._PanelButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._ImgButtonAbout)).BeginInit();
			this._PanelMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._ImgButtonRemove)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._ImgButtonAdd)).BeginInit();
			this._PanelSlider.SuspendLayout();
			this.SuspendLayout();
			// 
			// _ButtonAboutOk
			// 
			this._ButtonAboutOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._ButtonAboutOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonAboutOk.Location = new System.Drawing.Point(5, 5);
			this._ButtonAboutOk.Margin = new System.Windows.Forms.Padding(6);
			this._ButtonAboutOk.Name = "_ButtonAboutOk";
			this._ButtonAboutOk.Size = new System.Drawing.Size(290, 40);
			this._ButtonAboutOk.TabIndex = 0;
			this._ButtonAboutOk.TabStop = false;
			this._ButtonAboutOk.Text = "OK";
			this._ButtonAboutOk.UseVisualStyleBackColor = true;
			// 
			// _ButtonAccountCancel
			// 
			this._ButtonAccountCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._ButtonAccountCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonAccountCancel.Location = new System.Drawing.Point(129, 13);
			this._ButtonAccountCancel.Name = "_ButtonAccountCancel";
			this._ButtonAccountCancel.Size = new System.Drawing.Size(75, 23);
			this._ButtonAccountCancel.TabIndex = 3;
			this._ButtonAccountCancel.TabStop = false;
			this._ButtonAccountCancel.Text = "Cancel";
			this._ButtonAccountCancel.UseVisualStyleBackColor = true;
			// 
			// _ButtonAccountSave
			// 
			this._ButtonAccountSave.Enabled = false;
			this._ButtonAccountSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonAccountSave.Location = new System.Drawing.Point(210, 14);
			this._ButtonAccountSave.Name = "_ButtonAccountSave";
			this._ButtonAccountSave.Size = new System.Drawing.Size(75, 23);
			this._ButtonAccountSave.TabIndex = 2;
			this._ButtonAccountSave.TabStop = false;
			this._ButtonAccountSave.Text = "Save";
			this._ButtonAccountSave.UseVisualStyleBackColor = true;
			// 
			// _ButtonBrowse
			// 
			this._ButtonBrowse.AutoSize = true;
			this._ButtonBrowse.Enabled = false;
			this._ButtonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonBrowse.Location = new System.Drawing.Point(200, 179);
			this._ButtonBrowse.MinimumSize = new System.Drawing.Size(65, 23);
			this._ButtonBrowse.Name = "_ButtonBrowse";
			this._ButtonBrowse.Size = new System.Drawing.Size(65, 23);
			this._ButtonBrowse.TabIndex = 4;
			this._ButtonBrowse.Text = "Browse...";
			this._ButtonBrowse.UseVisualStyleBackColor = true;
			// 
			// _ButtonCancel
			// 
			this._ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._ButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonCancel.Location = new System.Drawing.Point(128, 13);
			this._ButtonCancel.Name = "_ButtonCancel";
			this._ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this._ButtonCancel.TabIndex = 8;
			this._ButtonCancel.Text = "Cancel";
			this._ButtonCancel.UseVisualStyleBackColor = true;
			// 
			// _ButtonDefault
			// 
			this._ButtonDefault.AutoSize = true;
			this._ButtonDefault.Enabled = false;
			this._ButtonDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonDefault.Location = new System.Drawing.Point(128, 115);
			this._ButtonDefault.MinimumSize = new System.Drawing.Size(65, 23);
			this._ButtonDefault.Name = "_ButtonDefault";
			this._ButtonDefault.Size = new System.Drawing.Size(65, 23);
			this._ButtonDefault.TabIndex = 1;
			this._ButtonDefault.Text = "Default";
			this._ButtonDefault.UseVisualStyleBackColor = true;
			// 
			// _ButtonEdit
			// 
			this._ButtonEdit.AutoSize = true;
			this._ButtonEdit.Enabled = false;
			this._ButtonEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonEdit.Location = new System.Drawing.Point(199, 115);
			this._ButtonEdit.MinimumSize = new System.Drawing.Size(65, 23);
			this._ButtonEdit.Name = "_ButtonEdit";
			this._ButtonEdit.Size = new System.Drawing.Size(65, 23);
			this._ButtonEdit.TabIndex = 2;
			this._ButtonEdit.Text = "Edit";
			this._ButtonEdit.UseVisualStyleBackColor = true;
			// 
			// _ButtonOk
			// 
			this._ButtonOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonOk.Location = new System.Drawing.Point(209, 14);
			this._ButtonOk.Name = "_ButtonOk";
			this._ButtonOk.Size = new System.Drawing.Size(75, 23);
			this._ButtonOk.TabIndex = 7;
			this._ButtonOk.Text = "OK";
			this._ButtonOk.UseVisualStyleBackColor = true;
			// 
			// _LabelAccountTitle
			// 
			this._LabelAccountTitle.AutoSize = true;
			this._LabelAccountTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelAccountTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
			this._LabelAccountTitle.Location = new System.Drawing.Point(22, 9);
			this._LabelAccountTitle.Name = "_LabelAccountTitle";
			this._LabelAccountTitle.Size = new System.Drawing.Size(39, 21);
			this._LabelAccountTitle.TabIndex = 18;
			this._LabelAccountTitle.Text = "Title";
			// 
			// _LabelAdditional
			// 
			this._LabelAdditional.AutoSize = true;
			this._LabelAdditional.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelAdditional.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
			this._LabelAdditional.Location = new System.Drawing.Point(22, 211);
			this._LabelAdditional.Name = "_LabelAdditional";
			this._LabelAdditional.Size = new System.Drawing.Size(39, 21);
			this._LabelAdditional.TabIndex = 24;
			this._LabelAdditional.Text = "Title";
			// 
			// _LabelError
			// 
			this._LabelError.AutoSize = true;
			this._LabelError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(38)))), ((int)(((byte)(30)))));
			this._LabelError.Location = new System.Drawing.Point(44, 131);
			this._LabelError.MaximumSize = new System.Drawing.Size(218, 100);
			this._LabelError.Name = "_LabelError";
			this._LabelError.Size = new System.Drawing.Size(200, 26);
			this._LabelError.TabIndex = 19;
			this._LabelError.Text = "An account with this username already exists. Please enter a different username.";
			this._LabelError.Visible = false;
			// 
			// _LabelInterval
			// 
			this._LabelInterval.AutoSize = true;
			this._LabelInterval.Location = new System.Drawing.Point(23, 245);
			this._LabelInterval.Name = "_LabelInterval";
			this._LabelInterval.Size = new System.Drawing.Size(70, 13);
			this._LabelInterval.TabIndex = 21;
			this._LabelInterval.Text = "Check every:";
			// 
			// _LabelLanguage
			// 
			this._LabelLanguage.AutoSize = true;
			this._LabelLanguage.Location = new System.Drawing.Point(23, 271);
			this._LabelLanguage.Name = "_LabelLanguage";
			this._LabelLanguage.Size = new System.Drawing.Size(91, 13);
			this._LabelLanguage.TabIndex = 23;
			this._LabelLanguage.Text = "Display language:";
			// 
			// _LabelMinutes
			// 
			this._LabelMinutes.AutoSize = true;
			this._LabelMinutes.Location = new System.Drawing.Point(161, 245);
			this._LabelMinutes.Name = "_LabelMinutes";
			this._LabelMinutes.Size = new System.Drawing.Size(49, 13);
			this._LabelMinutes.TabIndex = 22;
			this._LabelMinutes.Text = "minute(s)";
			// 
			// _LabelPassword
			// 
			this._LabelPassword.AutoSize = true;
			this._LabelPassword.Location = new System.Drawing.Point(22, 86);
			this._LabelPassword.Name = "_LabelPassword";
			this._LabelPassword.Size = new System.Drawing.Size(56, 13);
			this._LabelPassword.TabIndex = 15;
			this._LabelPassword.Text = "Password:";
			// 
			// _LabelSound
			// 
			this._LabelSound.AutoSize = true;
			this._LabelSound.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelSound.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
			this._LabelSound.Location = new System.Drawing.Point(22, 148);
			this._LabelSound.Name = "_LabelSound";
			this._LabelSound.Size = new System.Drawing.Size(39, 21);
			this._LabelSound.TabIndex = 39;
			this._LabelSound.Text = "Title";
			// 
			// _LabelTitle
			// 
			this._LabelTitle.AutoSize = true;
			this._LabelTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
			this._LabelTitle.Location = new System.Drawing.Point(22, 9);
			this._LabelTitle.Name = "_LabelTitle";
			this._LabelTitle.Size = new System.Drawing.Size(39, 21);
			this._LabelTitle.TabIndex = 20;
			this._LabelTitle.Text = "Title";
			// 
			// _LabelUsername
			// 
			this._LabelUsername.AutoSize = true;
			this._LabelUsername.Location = new System.Drawing.Point(22, 40);
			this._LabelUsername.Name = "_LabelUsername";
			this._LabelUsername.Size = new System.Drawing.Size(58, 13);
			this._LabelUsername.TabIndex = 14;
			this._LabelUsername.Text = "Username:";
			// 
			// _TextPassword
			// 
			this._TextPassword.Location = new System.Drawing.Point(25, 104);
			this._TextPassword.Name = "_TextPassword";
			this._TextPassword.Size = new System.Drawing.Size(249, 20);
			this._TextPassword.TabIndex = 1;
			this._TextPassword.TabStop = false;
			this._TextPassword.UseSystemPasswordChar = true;
			// 
			// _TextUsername
			// 
			this._TextUsername.Location = new System.Drawing.Point(25, 58);
			this._TextUsername.Name = "_TextUsername";
			this._TextUsername.Size = new System.Drawing.Size(249, 20);
			this._TextUsername.TabIndex = 0;
			this._TextUsername.TabStop = false;
			// 
			// _ListViewAccounts
			// 
			this._ListViewAccounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._ColumnHeaderName});
			this._ListViewAccounts.FullRowSelect = true;
			this._ListViewAccounts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._ListViewAccounts.LabelWrap = false;
			this._ListViewAccounts.Location = new System.Drawing.Point(25, 37);
			this._ListViewAccounts.MultiSelect = false;
			this._ListViewAccounts.Name = "_ListViewAccounts";
			this._ListViewAccounts.RightToLeftLayout = true;
			this._ListViewAccounts.ShowGroups = false;
			this._ListViewAccounts.ShowItemToolTips = true;
			this._ListViewAccounts.Size = new System.Drawing.Size(238, 72);
			this._ListViewAccounts.TabIndex = 0;
			this._ListViewAccounts.UseCompatibleStateImageBehavior = false;
			this._ListViewAccounts.View = System.Windows.Forms.View.Details;
			// 
			// _ColumnHeaderName
			// 
			this._ColumnHeaderName.Text = "Name";
			this._ColumnHeaderName.Width = 233;
			// 
			// _PictureExclamation
			// 
			this._PictureExclamation.Location = new System.Drawing.Point(25, 130);
			this._PictureExclamation.MinimumSize = new System.Drawing.Size(16, 16);
			this._PictureExclamation.Name = "_PictureExclamation";
			this._PictureExclamation.Size = new System.Drawing.Size(16, 16);
			this._PictureExclamation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this._PictureExclamation.TabIndex = 20;
			this._PictureExclamation.TabStop = false;
			this._PictureExclamation.Visible = false;
			// 
			// _ComboLanguage
			// 
			this._ComboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._ComboLanguage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ComboLanguage.FormattingEnabled = true;
			this._ComboLanguage.Location = new System.Drawing.Point(129, 268);
			this._ComboLanguage.Name = "_ComboLanguage";
			this._ComboLanguage.Size = new System.Drawing.Size(134, 21);
			this._ComboLanguage.TabIndex = 6;
			// 
			// _ComboSound
			// 
			this._ComboSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._ComboSound.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ComboSound.FormattingEnabled = true;
			this._ComboSound.Location = new System.Drawing.Point(27, 180);
			this._ComboSound.Name = "_ComboSound";
			this._ComboSound.Size = new System.Drawing.Size(164, 21);
			this._ComboSound.TabIndex = 3;
			// 
			// _TextInterval
			// 
			this._TextInterval.Location = new System.Drawing.Point(128, 242);
			this._TextInterval.Name = "_TextInterval";
			this._TextInterval.Size = new System.Drawing.Size(27, 20);
			this._TextInterval.TabIndex = 5;
			this._TextInterval.Text = "1";
			this._TextInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// _PanelAbout
			// 
			this._PanelAbout.Controls.Add(this._PanelAboutButtons);
			this._PanelAbout.Location = new System.Drawing.Point(0, 0);
			this._PanelAbout.Margin = new System.Windows.Forms.Padding(0);
			this._PanelAbout.Name = "_PanelAbout";
			this._PanelAbout.Size = new System.Drawing.Size(300, 355);
			this._PanelAbout.TabIndex = 0;
			// 
			// _PanelAboutButtons
			// 
			this._PanelAboutButtons.BackColor = System.Drawing.SystemColors.Control;
			this._PanelAboutButtons.Controls.Add(this._ButtonAboutOk);
			this._PanelAboutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._PanelAboutButtons.Location = new System.Drawing.Point(0, 305);
			this._PanelAboutButtons.Name = "_PanelAboutButtons";
			this._PanelAboutButtons.Size = new System.Drawing.Size(300, 50);
			this._PanelAboutButtons.TabIndex = 0;
			// 
			// _PanelAccount
			// 
			this._PanelAccount.Controls.Add(this._PictureExclamation);
			this._PanelAccount.Controls.Add(this._LabelError);
			this._PanelAccount.Controls.Add(this._LabelAccountTitle);
			this._PanelAccount.Controls.Add(this._PanelAccountButtons);
			this._PanelAccount.Controls.Add(this._TextPassword);
			this._PanelAccount.Controls.Add(this._TextUsername);
			this._PanelAccount.Controls.Add(this._LabelPassword);
			this._PanelAccount.Controls.Add(this._LabelUsername);
			this._PanelAccount.Location = new System.Drawing.Point(600, 0);
			this._PanelAccount.Margin = new System.Windows.Forms.Padding(0);
			this._PanelAccount.Name = "_PanelAccount";
			this._PanelAccount.Size = new System.Drawing.Size(300, 355);
			this._PanelAccount.TabIndex = 0;
			// 
			// _PanelAccountButtons
			// 
			this._PanelAccountButtons.BackColor = System.Drawing.SystemColors.Control;
			this._PanelAccountButtons.Controls.Add(this._ButtonAccountCancel);
			this._PanelAccountButtons.Controls.Add(this._ButtonAccountSave);
			this._PanelAccountButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._PanelAccountButtons.Location = new System.Drawing.Point(0, 305);
			this._PanelAccountButtons.Name = "_PanelAccountButtons";
			this._PanelAccountButtons.Size = new System.Drawing.Size(300, 50);
			this._PanelAccountButtons.TabIndex = 0;
			// 
			// _PanelButtons
			// 
			this._PanelButtons.BackColor = System.Drawing.SystemColors.Control;
			this._PanelButtons.Controls.Add(this._ImgButtonAbout);
			this._PanelButtons.Controls.Add(this._ButtonCancel);
			this._PanelButtons.Controls.Add(this._ButtonOk);
			this._PanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._PanelButtons.Location = new System.Drawing.Point(0, 305);
			this._PanelButtons.Name = "_PanelButtons";
			this._PanelButtons.Size = new System.Drawing.Size(300, 50);
			this._PanelButtons.TabIndex = 0;
			// 
			// _ImgButtonAbout
			// 
			this._ImgButtonAbout.Location = new System.Drawing.Point(13, 17);
			this._ImgButtonAbout.Name = "_ImgButtonAbout";
			this._ImgButtonAbout.Size = new System.Drawing.Size(16, 16);
			this._ImgButtonAbout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._ImgButtonAbout.TabIndex = 11;
			this._ImgButtonAbout.TabStop = false;
			// 
			// _PanelMain
			// 
			this._PanelMain.Controls.Add(this._ComboSound);
			this._PanelMain.Controls.Add(this._PanelButtons);
			this._PanelMain.Controls.Add(this._LabelSound);
			this._PanelMain.Controls.Add(this._ButtonBrowse);
			this._PanelMain.Controls.Add(this._ButtonDefault);
			this._PanelMain.Controls.Add(this._ImgButtonRemove);
			this._PanelMain.Controls.Add(this._ImgButtonAdd);
			this._PanelMain.Controls.Add(this._ButtonEdit);
			this._PanelMain.Controls.Add(this._ListViewAccounts);
			this._PanelMain.Controls.Add(this._LabelAdditional);
			this._PanelMain.Controls.Add(this._LabelLanguage);
			this._PanelMain.Controls.Add(this._ComboLanguage);
			this._PanelMain.Controls.Add(this._LabelTitle);
			this._PanelMain.Controls.Add(this._TextInterval);
			this._PanelMain.Controls.Add(this._LabelMinutes);
			this._PanelMain.Controls.Add(this._LabelInterval);
			this._PanelMain.Location = new System.Drawing.Point(300, 0);
			this._PanelMain.Margin = new System.Windows.Forms.Padding(0);
			this._PanelMain.Name = "_PanelMain";
			this._PanelMain.Size = new System.Drawing.Size(300, 355);
			this._PanelMain.TabIndex = 0;
			// 
			// _ImgButtonRemove
			// 
			this._ImgButtonRemove.Location = new System.Drawing.Point(49, 118);
			this._ImgButtonRemove.Name = "_ImgButtonRemove";
			this._ImgButtonRemove.Size = new System.Drawing.Size(16, 16);
			this._ImgButtonRemove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._ImgButtonRemove.TabIndex = 25;
			this._ImgButtonRemove.TabStop = false;
			// 
			// _ImgButtonAdd
			// 
			this._ImgButtonAdd.Location = new System.Drawing.Point(27, 118);
			this._ImgButtonAdd.Name = "_ImgButtonAdd";
			this._ImgButtonAdd.Size = new System.Drawing.Size(16, 16);
			this._ImgButtonAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._ImgButtonAdd.TabIndex = 25;
			this._ImgButtonAdd.TabStop = false;
			// 
			// _PanelSlider
			// 
			this._PanelSlider.Controls.Add(this._PanelAbout);
			this._PanelSlider.Controls.Add(this._PanelMain);
			this._PanelSlider.Controls.Add(this._PanelAccount);
			this._PanelSlider.Location = new System.Drawing.Point(0, 0);
			this._PanelSlider.Name = "_PanelSlider";
			this._PanelSlider.Size = new System.Drawing.Size(901, 355);
			this._PanelSlider.TabIndex = 1;
			// 
			// Settings
			// 
			this.AcceptButton = this._ButtonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this._ButtonCancel;
			this.ClientSize = new System.Drawing.Size(901, 456);
			this.Controls.Add(this._PanelSlider);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Settings";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Gmail Notifier Plus";
			((System.ComponentModel.ISupportInitialize)(this._PictureExclamation)).EndInit();
			this._PanelAbout.ResumeLayout(false);
			this._PanelAboutButtons.ResumeLayout(false);
			this._PanelAccount.ResumeLayout(false);
			this._PanelAccount.PerformLayout();
			this._PanelAccountButtons.ResumeLayout(false);
			this._PanelButtons.ResumeLayout(false);
			this._PanelButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._ImgButtonAbout)).EndInit();
			this._PanelMain.ResumeLayout(false);
			this._PanelMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._ImgButtonRemove)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._ImgButtonAdd)).EndInit();
			this._PanelSlider.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button _ButtonAboutOk;
		private System.Windows.Forms.Button _ButtonAccountCancel;
		private System.Windows.Forms.Button _ButtonAccountSave;
		private System.Windows.Forms.Button _ButtonBrowse;
		private System.Windows.Forms.Button _ButtonCancel;
		private System.Windows.Forms.Button _ButtonDefault;
		private System.Windows.Forms.Button _ButtonEdit;
		private System.Windows.Forms.Button _ButtonOk;
		private Label _LabelAccountTitle;
		private Label _LabelAdditional;
		private Label _LabelError;
		private Label _LabelInterval;
		private Label _LabelLanguage;
		private Label _LabelMinutes;
		private Label _LabelPassword;
		private Label _LabelSound;
		private Label _LabelTitle;
		private Label _LabelUsername;
		private ImageButton _ImgButtonAbout;
		private ImageButton _ImgButtonAdd;
		private ImageButton _ImgButtonRemove;
		private VistaControls.TextBox _TextPassword;
		private VistaControls.TextBox _TextUsername;
		private VistaControls.ListView _ListViewAccounts;
		private PictureBox _PictureExclamation;
		private VistaControls.ComboBox _ComboLanguage;
		private VistaControls.ComboBox _ComboSound;
		private ColumnHeader _ColumnHeaderName;
		private System.Windows.Forms.TextBox _TextInterval;
		private Panel _PanelAbout;
		private Panel _PanelAboutButtons;
		private Panel _PanelAccount;
		private Panel _PanelAccountButtons;
		private Panel _PanelButtons;
		private Panel _PanelMain;
		private Panel _PanelSlider;
	}
}