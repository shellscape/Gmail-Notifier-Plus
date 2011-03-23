namespace GmailNotifierPlus.Forms {
	partial class Toast {
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
			this._Panel = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._PictureOpen = new System.Windows.Forms.PictureBox();
			this._PanelLine = new System.Windows.Forms.Panel();
			this._LabelTitle = new System.Windows.Forms.Label();
			this._LabelFrom = new System.Windows.Forms.Label();
			this._LabelIndex = new System.Windows.Forms.Label();
			this._LabelMessage = new System.Windows.Forms.Label();
			this._LabelDate = new System.Windows.Forms.Label();
			this._Panel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._PictureOpen)).BeginInit();
			this.SuspendLayout();
			// 
			// _Panel
			// 
			this._Panel.Controls.Add(this._PanelLine);
			this._Panel.Controls.Add(this._PictureOpen);
			this._Panel.Controls.Add(this._LabelTitle);
			this._Panel.Controls.Add(this._LabelFrom);
			this._Panel.Controls.Add(this._LabelIndex);
			this._Panel.Controls.Add(this._LabelMessage);
			this._Panel.Controls.Add(this._LabelDate);
			this._Panel.Location = new System.Drawing.Point(0, 0);
			this._Panel.Margin = new System.Windows.Forms.Padding(8, 30, 8, 24);
			this._Panel.Name = "_Panel";
			this._Panel.Size = new System.Drawing.Size(322, 108);
			this._Panel.TabIndex = 15;
			// 
			// _PictureOpen
			// 
			this._PictureOpen.Location = new System.Drawing.Point(300, 4);
			this._PictureOpen.Name = "_PictureOpen";
			this._PictureOpen.Size = new System.Drawing.Size(16, 16);
			this._PictureOpen.TabIndex = 16;
			this._PictureOpen.TabStop = false;
			// 
			// _PanelLine
			// 
			this._PanelLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(213)))), ((int)(((byte)(223)))));
			this._PanelLine.Location = new System.Drawing.Point(6, 36);
			this._PanelLine.Name = "_PanelLine";
			this._PanelLine.Size = new System.Drawing.Size(290, 1);
			this._PanelLine.TabIndex = 15;
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
			this._LabelTitle.Size = new System.Drawing.Size(294, 22);
			this._LabelTitle.TabIndex = 13;
			this._LabelTitle.Text = "Title";
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
			this._LabelFrom.Size = new System.Drawing.Size(294, 19);
			this._LabelFrom.TabIndex = 14;
			this._LabelFrom.Text = "From";
			// 
			// _LabelIndex
			// 
			this._LabelIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._LabelIndex.AutoSize = true;
			this._LabelIndex.BackColor = System.Drawing.Color.Transparent;
			this._LabelIndex.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelIndex.ForeColor = System.Drawing.Color.Gray;
			this._LabelIndex.Location = new System.Drawing.Point(298, 92);
			this._LabelIndex.Margin = new System.Windows.Forms.Padding(0);
			this._LabelIndex.Name = "_LabelIndex";
			this._LabelIndex.Size = new System.Drawing.Size(19, 12);
			this._LabelIndex.TabIndex = 12;
			this._LabelIndex.Text = "1/3";
			this._LabelIndex.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _LabelMessage
			// 
			this._LabelMessage.AutoEllipsis = true;
			this._LabelMessage.BackColor = System.Drawing.Color.Transparent;
			this._LabelMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelMessage.Location = new System.Drawing.Point(0, 36);
			this._LabelMessage.Name = "_LabelMessage";
			this._LabelMessage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 0);
			this._LabelMessage.Size = new System.Drawing.Size(294, 52);
			this._LabelMessage.TabIndex = 10;
			this._LabelMessage.Text = "Content";
			// 
			// _LabelDate
			// 
			this._LabelDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._LabelDate.AutoSize = true;
			this._LabelDate.BackColor = System.Drawing.Color.Transparent;
			this._LabelDate.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelDate.ForeColor = System.Drawing.Color.Gray;
			this._LabelDate.Location = new System.Drawing.Point(4, 92);
			this._LabelDate.Margin = new System.Windows.Forms.Padding(0);
			this._LabelDate.Name = "_LabelDate";
			this._LabelDate.Size = new System.Drawing.Size(25, 12);
			this._LabelDate.TabIndex = 8;
			this._LabelDate.Text = "Date";
			this._LabelDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Toast
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(322, 130);
			this.ControlBox = false;
			this.Controls.Add(this._Panel);
			this.Name = "Toast";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Toast";
			this._Panel.ResumeLayout(false);
			this._Panel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._PictureOpen)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Shellscape.UI.Controls.DoubleBufferedPanel _Panel;
		private System.Windows.Forms.PictureBox _PictureOpen;
		private System.Windows.Forms.Panel _PanelLine;
		private System.Windows.Forms.Label _LabelTitle;
		private System.Windows.Forms.Label _LabelFrom;
		private System.Windows.Forms.Label _LabelIndex;
		private System.Windows.Forms.Label _LabelMessage;
		private System.Windows.Forms.Label _LabelDate;



	}
}

