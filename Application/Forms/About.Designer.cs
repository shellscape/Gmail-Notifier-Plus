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
			this._Flow = new System.Windows.Forms.FlowLayoutPanel();
			this._LabelCopyright = new Shellscape.UI.Controls.ResponsiveLinkLabel();
			this._Flow.SuspendLayout();
			this.SuspendLayout();
			// 
			// _Button
			// 
			this._Button.Location = new System.Drawing.Point(435, 236);
			// 
			// _Flow
			// 
			this._Flow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._Flow.BackColor = System.Drawing.Color.Transparent;
			this._Flow.Controls.Add(this._LabelCopyright);
			this._Flow.Location = new System.Drawing.Point(3, 152);
			this._Flow.Name = "_Flow";
			this._Flow.Size = new System.Drawing.Size(519, 87);
			this._Flow.TabIndex = 2;
			// 
			// _LabelCopyright
			// 
			this._LabelCopyright.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(74)))), ((int)(((byte)(229)))));
			this._LabelCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._LabelCopyright.AutoSize = true;
			this._LabelCopyright.DisabledLinkColor = System.Drawing.Color.Gray;
			this._LabelCopyright.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this._LabelCopyright.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(85)))));
			this._LabelCopyright.Location = new System.Drawing.Point(3, 0);
			this._LabelCopyright.Name = "_LabelCopyright";
			this._LabelCopyright.Size = new System.Drawing.Size(60, 15);
			this._LabelCopyright.TabIndex = 0;
			this._LabelCopyright.TabStop = true;
			this._LabelCopyright.Text = "Copyright";
			this._LabelCopyright.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(85)))));
			// 
			// About
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(526, 269);
			this.Controls.Add(this._Flow);
			this.Name = "About";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Controls.SetChildIndex(this._Flow, 0);
			this.Controls.SetChildIndex(this._Button, 0);
			this._Flow.ResumeLayout(false);
			this._Flow.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private FlowLayoutPanel _Flow;
		private Shellscape.UI.Controls.ResponsiveLinkLabel _LabelCopyright;
	}
}