﻿using System.Drawing;
using System.Windows.Forms;

using Microsoft.WindowsAPI.Dialogs;
using Microsoft.WindowsAPI.Shell;
using GmailNotifierPlus.Controls;

namespace GmailNotifierPlus.Forms {
	partial class FirstRun {
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
			this._ButtonOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _ButtonOK
			// 
			this._ButtonOK.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._ButtonOK.Location = new System.Drawing.Point(6, 310);
			this._ButtonOK.Margin = new System.Windows.Forms.Padding(6);
			this._ButtonOK.Name = "_ButtonOK";
			this._ButtonOK.Padding = new System.Windows.Forms.Padding(6);
			this._ButtonOK.Size = new System.Drawing.Size(286, 42);
			this._ButtonOK.TabIndex = 0;
			this._ButtonOK.Text = "button1";
			this._ButtonOK.UseVisualStyleBackColor = true;
			// 
			// FirstRun
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(298, 358);
			this.Controls.Add(this._ButtonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FirstRun";
			this.Padding = new System.Windows.Forms.Padding(6);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Gmail Notifier Plus";
			this.ResumeLayout(false);

		}

		#endregion

		private Button _ButtonOK;

	}
}