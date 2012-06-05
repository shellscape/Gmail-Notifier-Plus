namespace GmailNotifierPlus.Forms {
	partial class Main {
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
			this._Timer = new System.Windows.Forms.Timer(this.components);
			this._TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.SuspendLayout();
			// 
			// _Timer
			// 
			this._Timer.Interval = 30000;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Location = new System.Drawing.Point(-10000, -10000);
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Main";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer _Timer;
		private System.Windows.Forms.NotifyIcon _TrayIcon;
	}
}