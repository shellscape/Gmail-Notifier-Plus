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
			this._PanelAccounts = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._LabelAccountsTitle = new Shellscape.UI.Controls.ThemeLabel();
			this._ListAccounts = new Shellscape.UI.Controls.FlickerFreeListView();
			this._LinkAddNew = new Shellscape.UI.Controls.ResponsiveLinkLabel();
			this._Panels.SuspendLayout();
			this._PanelAccounts.SuspendLayout();
			this.SuspendLayout();
			// 
			// _Panels
			// 
			this._Panels.Controls.Add(this._PanelAccounts);
			this._Panels.Size = new System.Drawing.Size(554, 456);
			// 
			// _PanelAccounts
			// 
			this._PanelAccounts.AutoSize = true;
			this._PanelAccounts.Controls.Add(this._LabelAccountsTitle);
			this._PanelAccounts.Controls.Add(this._ListAccounts);
			this._PanelAccounts.Controls.Add(this._LinkAddNew);
			this._PanelAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelAccounts.Location = new System.Drawing.Point(6, 24);
			this._PanelAccounts.Name = "_PanelAccounts";
			this._PanelAccounts.Size = new System.Drawing.Size(542, 408);
			this._PanelAccounts.TabIndex = 0;
			// 
			// _LabelAccountsTitle
			// 
			this._LabelAccountsTitle.AutoSize = true;
			this._LabelAccountsTitle.BackColor = System.Drawing.Color.Transparent;
			this._LabelAccountsTitle.Location = new System.Drawing.Point(3, 3);
			this._LabelAccountsTitle.MinimumSize = new System.Drawing.Size(100, 20);
			this._LabelAccountsTitle.Name = "_LabelAccountsTitle";
			this._LabelAccountsTitle.Size = new System.Drawing.Size(100, 20);
			this._LabelAccountsTitle.TabIndex = 0;
			this._LabelAccountsTitle.ThemeElement = null;
			// 
			// _ListAccounts
			// 
			this._ListAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._ListAccounts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._ListAccounts.LabelWrap = false;
			this._ListAccounts.Location = new System.Drawing.Point(3, 29);
			this._ListAccounts.MultiSelect = false;
			this._ListAccounts.Name = "_ListAccounts";
			this._ListAccounts.Size = new System.Drawing.Size(536, 203);
			this._ListAccounts.TabIndex = 0;
			this._ListAccounts.UseCompatibleStateImageBehavior = false;
			// 
			// _LinkAddNew
			// 
			this._LinkAddNew.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(74)))), ((int)(((byte)(229)))));
			this._LinkAddNew.AutoSize = true;
			this._LinkAddNew.DisabledLinkColor = System.Drawing.Color.Gray;
			this._LinkAddNew.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(74)))), ((int)(((byte)(229)))));
			this._LinkAddNew.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this._LinkAddNew.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(85)))));
			this._LinkAddNew.Location = new System.Drawing.Point(3, 235);
			this._LinkAddNew.Name = "_LinkAddNew";
			this._LinkAddNew.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(85)))));
			this._LinkAddNew.Size = new System.Drawing.Size(109, 15);
			this._LinkAddNew.TabIndex = 2;
			this._LinkAddNew.TabStop = true;
			this._LinkAddNew.Text = "Add a new account";
			this._LinkAddNew.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(85)))));
			// 
			// Preferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(754, 456);
			this.Name = "Preferences";
			this.Text = "Prefs";
			this._Panels.ResumeLayout(false);
			this._Panels.PerformLayout();
			this._PanelAccounts.ResumeLayout(false);
			this._PanelAccounts.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Shellscape.UI.Controls.DoubleBufferedPanel _PanelAccounts;
		private Shellscape.UI.Controls.FlickerFreeListView _ListAccounts;
		private Shellscape.UI.Controls.ResponsiveLinkLabel _LinkAddNew;
		private Shellscape.UI.Controls.ThemeLabel _LabelAccountsTitle;

	}
}