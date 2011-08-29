using System.Drawing;
using System.Windows.Forms;

using Microsoft.WindowsAPI.Dialogs;
using Microsoft.WindowsAPI.Shell;
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
			this._Panel = new System.Windows.Forms.Panel();
			this._ButtonOK = new System.Windows.Forms.Button();
			this._PanelUpdate = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._LabelUpdate = new System.Windows.Forms.Label();
			this._PanelAbout = new GmailNotifierPlus.Controls.AboutPanel();
			this._PanelShellscape = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._Panel.SuspendLayout();
			this._PanelUpdate.SuspendLayout();
			this._PanelAbout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _Panel
			// 
			this._Panel.Controls.Add(this._PanelUpdate);
			this._Panel.Controls.Add(this._ButtonOK);
			this._Panel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._Panel.Location = new System.Drawing.Point(0, 302);
			this._Panel.Name = "_Panel";
			this._Panel.Padding = new System.Windows.Forms.Padding(6);
			this._Panel.Size = new System.Drawing.Size(298, 80);
			this._Panel.TabIndex = 2;
			// 
			// _ButtonOK
			// 
			this._ButtonOK.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._ButtonOK.Location = new System.Drawing.Point(6, 32);
			this._ButtonOK.Name = "_ButtonOK";
			this._ButtonOK.Size = new System.Drawing.Size(286, 42);
			this._ButtonOK.TabIndex = 0;
			this._ButtonOK.Text = "button1";
			this._ButtonOK.UseVisualStyleBackColor = true;
			// 
			// _PanelUpdate
			// 
			this._PanelUpdate.Controls.Add(this._LabelUpdate);
			this._PanelUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelUpdate.Location = new System.Drawing.Point(6, 6);
			this._PanelUpdate.Name = "_PanelUpdate";
			this._PanelUpdate.Padding = new System.Windows.Forms.Padding(22, 0, 0, 4);
			this._PanelUpdate.Size = new System.Drawing.Size(286, 26);
			this._PanelUpdate.TabIndex = 2;
			this._PanelUpdate.Paint += new System.Windows.Forms.PaintEventHandler(this._PanelUpdate_Paint);
			// 
			// _LabelUpdate
			// 
			this._LabelUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this._LabelUpdate.Location = new System.Drawing.Point(22, 0);
			this._LabelUpdate.Name = "_LabelUpdate";
			this._LabelUpdate.Size = new System.Drawing.Size(264, 22);
			this._LabelUpdate.TabIndex = 1;
			this._LabelUpdate.Text = "Checking for updates...";
			this._LabelUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _PanelAbout
			// 
			this._PanelAbout.Controls.Add(this._PanelShellscape);
			this._PanelAbout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelAbout.FirstRun = false;
			this._PanelAbout.Location = new System.Drawing.Point(0, 0);
			this._PanelAbout.Margin = new System.Windows.Forms.Padding(0);
			this._PanelAbout.Name = "_PanelAbout";
			this._PanelAbout.Size = new System.Drawing.Size(298, 382);
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
			// About
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(298, 382);
			this.Controls.Add(this._Panel);
			this.Controls.Add(this._PanelAbout);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Gmail Notifier Plus";
			this._Panel.ResumeLayout(false);
			this._PanelUpdate.ResumeLayout(false);
			this._PanelAbout.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private AboutPanel _PanelAbout;
		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelShellscape;
		private Panel _Panel;
		private Button _ButtonOK;
		private Label _LabelUpdate;
		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelUpdate;

	}
}