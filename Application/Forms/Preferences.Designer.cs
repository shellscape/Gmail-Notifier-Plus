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
			this.doubleBufferedPanel1 = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._ListAccounts = new System.Windows.Forms.ListView();
			this._Panels.SuspendLayout();
			this.doubleBufferedPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _Panels
			// 
			this._Panels.Controls.Add(this.doubleBufferedPanel1);
			this._Panels.Size = new System.Drawing.Size(521, 456);
			// 
			// doubleBufferedPanel1
			// 
			this.doubleBufferedPanel1.Controls.Add(this._ListAccounts);
			this.doubleBufferedPanel1.Location = new System.Drawing.Point(9, 27);
			this.doubleBufferedPanel1.Name = "doubleBufferedPanel1";
			this.doubleBufferedPanel1.Size = new System.Drawing.Size(504, 417);
			this.doubleBufferedPanel1.TabIndex = 0;
			// 
			// _ListAccounts
			// 
			this._ListAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._ListAccounts.Location = new System.Drawing.Point(3, 50);
			this._ListAccounts.Name = "_ListAccounts";
			this._ListAccounts.Size = new System.Drawing.Size(498, 203);
			this._ListAccounts.TabIndex = 0;
			this._ListAccounts.UseCompatibleStateImageBehavior = false;
			// 
			// Preferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(754, 456);
			this.Name = "Preferences";
			this.Text = "Prefs";
			this._Panels.ResumeLayout(false);
			this.doubleBufferedPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Shellscape.UI.Controls.DoubleBufferedPanel doubleBufferedPanel1;
		private System.Windows.Forms.ListView _ListAccounts;


	}
}