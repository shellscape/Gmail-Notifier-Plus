namespace GmailNotifierPlus.Forms {
	partial class Notifier {
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
			this._LabelDate = new System.Windows.Forms.Label();
			this._LabelStatus = new System.Windows.Forms.Label();
			this._LabelMessage = new System.Windows.Forms.Label();
			this._LabelIndex = new System.Windows.Forms.Label();
			this._LabelFrom = new System.Windows.Forms.Label();
			this._LabelTitle = new System.Windows.Forms.Label();
			this._PanelLine = new System.Windows.Forms.Panel();
			this._PictureLogo = new System.Windows.Forms.PictureBox();
			this._PictureOpen = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this._PictureLogo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._PictureOpen)).BeginInit();
			this.SuspendLayout();
			// 
			// _LabelDate
			// 
			this._LabelDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._LabelDate.AutoSize = true;
			this._LabelDate.BackColor = System.Drawing.Color.Transparent;
			this._LabelDate.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelDate.ForeColor = System.Drawing.Color.Gray;
			this._LabelDate.Location = new System.Drawing.Point(4, 90);
			this._LabelDate.Margin = new System.Windows.Forms.Padding(0);
			this._LabelDate.Name = "_LabelDate";
			this._LabelDate.Size = new System.Drawing.Size(25, 12);
			this._LabelDate.TabIndex = 0;
			this._LabelDate.Text = "Date";
			this._LabelDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _LabelStatus
			// 
			this._LabelStatus.AutoEllipsis = true;
			this._LabelStatus.BackColor = System.Drawing.Color.Transparent;
			this._LabelStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			this._LabelStatus.Location = new System.Drawing.Point(53, 75);
			this._LabelStatus.Name = "_LabelStatus";
			this._LabelStatus.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this._LabelStatus.Size = new System.Drawing.Size(72, 13);
			this._LabelStatus.TabIndex = 1;
			this._LabelStatus.Text = "_LabelStatus";
			this._LabelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _LabelMessage
			// 
			this._LabelMessage.AutoEllipsis = true;
			this._LabelMessage.BackColor = System.Drawing.Color.Transparent;
			this._LabelMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelMessage.Location = new System.Drawing.Point(0, 36);
			this._LabelMessage.Name = "_LabelMessage";
			this._LabelMessage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 0);
			this._LabelMessage.Size = new System.Drawing.Size(198, 52);
			this._LabelMessage.TabIndex = 1;
			this._LabelMessage.Text = "Content";
			// 
			// _LabelIndex
			// 
			this._LabelIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._LabelIndex.AutoSize = true;
			this._LabelIndex.BackColor = System.Drawing.Color.Transparent;
			this._LabelIndex.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelIndex.ForeColor = System.Drawing.Color.Gray;
			this._LabelIndex.Location = new System.Drawing.Point(170, 90);
			this._LabelIndex.Margin = new System.Windows.Forms.Padding(0);
			this._LabelIndex.Name = "_LabelIndex";
			this._LabelIndex.Size = new System.Drawing.Size(19, 12);
			this._LabelIndex.TabIndex = 1;
			this._LabelIndex.Text = "1/3";
			this._LabelIndex.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _LabelFrom
			// 
			this._LabelFrom.AutoEllipsis = true;
			this._LabelFrom.BackColor = System.Drawing.Color.Transparent;
			this._LabelFrom.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelFrom.ForeColor = System.Drawing.Color.Gray;
			this._LabelFrom.Location = new System.Drawing.Point(0, 17);
			this._LabelFrom.Name = "_LabelFrom";
			this._LabelFrom.Padding = new System.Windows.Forms.Padding(4, 2, 3, 0);
			this._LabelFrom.Size = new System.Drawing.Size(198, 19);
			this._LabelFrom.TabIndex = 1;
			this._LabelFrom.Text = "From";
			// 
			// _LabelTitle
			// 
			this._LabelTitle.AutoEllipsis = true;
			this._LabelTitle.BackColor = System.Drawing.Color.Transparent;
			this._LabelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
			this._LabelTitle.Location = new System.Drawing.Point(0, 0);
			this._LabelTitle.Name = "_LabelTitle";
			this._LabelTitle.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this._LabelTitle.Size = new System.Drawing.Size(174, 22);
			this._LabelTitle.TabIndex = 1;
			this._LabelTitle.Text = "Title";
			// 
			// _PanelLine
			// 
			this._PanelLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(213)))), ((int)(((byte)(223)))));
			this._PanelLine.Location = new System.Drawing.Point(6, 36);
			this._PanelLine.Name = "_PanelLine";
			this._PanelLine.Size = new System.Drawing.Size(186, 1);
			this._PanelLine.TabIndex = 6;
			this._PanelLine.Visible = false;
			// 
			// _PictureLogo
			// 
			this._PictureLogo.BackColor = System.Drawing.Color.Transparent;
			this._PictureLogo.Location = new System.Drawing.Point(0, 0);
			this._PictureLogo.Name = "_PictureLogo";
			this._PictureLogo.Size = new System.Drawing.Size(198, 84);
			this._PictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this._PictureLogo.TabIndex = 0;
			this._PictureLogo.TabStop = false;
			// 
			// _PictureOpen
			// 
			this._PictureOpen.Location = new System.Drawing.Point(178, 4);
			this._PictureOpen.Name = "_PictureOpen";
			this._PictureOpen.Size = new System.Drawing.Size(16, 16);
			this._PictureOpen.TabIndex = 7;
			this._PictureOpen.TabStop = false;
			// 
			// Notifier
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(198, 108);
			this.Controls.Add(this._PictureOpen);
			this.Controls.Add(this._PanelLine);
			this.Controls.Add(this._LabelTitle);
			this.Controls.Add(this._LabelFrom);
			this.Controls.Add(this._LabelIndex);
			this.Controls.Add(this._LabelMessage);
			this.Controls.Add(this._LabelStatus);
			this.Controls.Add(this._LabelDate);
			this.Controls.Add(this._PictureLogo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Notifier";
			this.Opacity = 0D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Notifier";
			this.Activated += new System.EventHandler(this.Notifier_Activated);
			this.Shown += new System.EventHandler(this.Notifier_Shown);
			((System.ComponentModel.ISupportInitialize)(this._PictureLogo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._PictureOpen)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _LabelDate;
		private System.Windows.Forms.Label _LabelStatus;
		private System.Windows.Forms.Label _LabelMessage;
		private System.Windows.Forms.Label _LabelIndex;
		private System.Windows.Forms.Label _LabelFrom;
		private System.Windows.Forms.Label _LabelTitle;
		private System.Windows.Forms.Panel _PanelLine;
		private System.Windows.Forms.PictureBox _PictureLogo;
		private System.Windows.Forms.PictureBox _PictureOpen;
	}
}