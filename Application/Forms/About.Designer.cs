using System.Drawing;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using GmailNotifierPlus.Controls;

namespace GmailNotifierPlus.Forms {
	partial class About {
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
			this._PanelAbout = new GmailNotifierPlus.Controls.AboutPanel();
			this._PanelShellscape = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._PanelAboutButtons = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._ButtonAboutOk = new System.Windows.Forms.Button();
			this._PanelAbout.SuspendLayout();
			this._PanelAboutButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// _PanelAbout
			// 
			this._PanelAbout.Controls.Add(this._PanelShellscape);
			this._PanelAbout.Controls.Add(this._PanelAboutButtons);
			this._PanelAbout.FirstRun = false;
			this._PanelAbout.Location = new System.Drawing.Point(0, 0);
			this._PanelAbout.Margin = new System.Windows.Forms.Padding(0);
			this._PanelAbout.Name = "_PanelAbout";
			this._PanelAbout.Size = new System.Drawing.Size(300, 355);
			this._PanelAbout.TabIndex = 1;
			// 
			// _PanelShellscape
			// 
			this._PanelShellscape.BackColor = System.Drawing.Color.Transparent;
			this._PanelShellscape.Cursor = System.Windows.Forms.Cursors.Hand;
			this._PanelShellscape.Location = new System.Drawing.Point(210, 10);
			this._PanelShellscape.Name = "_PanelShellscape";
			this._PanelShellscape.Size = new System.Drawing.Size(80, 86);
			this._PanelShellscape.TabIndex = 1;
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
			this._ButtonAboutOk.Click += new System.EventHandler(this._ButtonAboutOk_Click);
			// 
			// About
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(294, 355);
			this.Controls.Add(this._PanelAbout);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Gmail Notifier Plus";
			this._PanelAbout.ResumeLayout(false);
			this._PanelAboutButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private AboutPanel _PanelAbout;
		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelShellscape;
		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelAboutButtons;
		private Button _ButtonAboutOk;

	}
}