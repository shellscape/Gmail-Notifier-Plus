namespace GmailNotifierPlus {

	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Net;
	using System.Reflection;
	using System.Resources;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Windows.Forms;
	using System.Xml;

	using Microsoft.WindowsAPICodePack.Taskbar;

	using GmailNotifierPlus.Utilities;
	
	public class Notifier : Form {
		private const int _FEED_MAX = 20;
		private int accountIndex;
		private IContainer components;
		private Status connectionStatus;
		private int currentMailIndex;
		private XmlNodeList currentMails;
		private string currentMailUrl;
		private ThumbnailToolbarButton inboxButton;
		private Label lbl_date;
		private Label lbl_from;
		private Label lbl_index;
		private Label lbl_messageContent;
		private Label lbl_status;
		private Label lbl_title;
		private ThumbnailToolbarButton nextButton;
		private PictureBox pic_logo;
		private Panel pnl_line;
		private ThumbnailToolbarButton previousButton;
		
		private Config config = Config.Current;
		private TaskbarManager taskbarManager = TaskbarManager.Instance;
		private ResourceManager resourceManager;
		private int unreadMailCount;
		private WebClient webClient = new WebClient();

		public event CheckFinishedEventHandler CheckFinished;

		public Notifier(int accountIndex) {
			this.InitializeComponent();

			this.accountIndex = accountIndex;
			this.resourceManager = new ResourceManager("GmailNotifierPlus.Properties." + config.Language, Assembly.GetExecutingAssembly());
			this.Text = this.config.Accounts[accountIndex].FullAddress;
			this.webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(this.webClient_DownloadDataCompleted);
			this.BackgroundImage = Utilities.ResourceHelper.GetResourceImage("GmailNotifierPlus.Images.Background.png");

			config.Saved += new ConfigSavedEventHandler(config_Saved);
			
			this.lbl_status.RightToLeft = Locale.Current.IsRightToLeftLanguage ? RightToLeft.Yes : RightToLeft.No;

		}

		internal void CheckMail() {
			if (!this.webClient.IsBusy) {
				try {
					this.webClient.Credentials = new NetworkCredential(config.Accounts[this.accountIndex].Login, config.Accounts[this.accountIndex].Password);
					this.webClient.DownloadDataAsync(new Uri(UrlHelper.GetFeedUrl(this.accountIndex)));
					this.SetCheckingPreview();
				}
				catch {
				}
			}
		}

		private void CreateThumbButtons() {
			this.previousButton = new ThumbnailToolbarButton(Utilities.ResourceHelper.GetResourceIcon("GmailNotifierPlus.Icons.Previous.ico"), this.resourceManager.GetString("Tooltip_Previous"));
			this.previousButton.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(this.previousButton_Click);
			this.inboxButton = new ThumbnailToolbarButton(Utilities.ResourceHelper.GetResourceIcon("GmailNotifierPlus.Icons.Inbox.ico"), this.resourceManager.GetString("Tooltip_Inbox"));
			this.inboxButton.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(this.inboxButton_Click);
			this.nextButton = new ThumbnailToolbarButton(Utilities.ResourceHelper.GetResourceIcon("GmailNotifierPlus.Icons.Next.ico"), this.resourceManager.GetString("Tooltip_Next"));
			this.nextButton.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(this.nextButton_Click);
			this.taskbarManager.ThumbnailToolbars.AddButtons(base.Handle, new ThumbnailToolbarButton[] { this.previousButton, this.inboxButton, this.nextButton });
		}

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void inboxButton_Click(object sender, ThumbnailButtonClickedEventArgs e) {
			this.OpenCurrentMail();
		}

		private void InitializeComponent() {
			this.pic_logo = new PictureBox();
			this.lbl_title = new Label();
			this.lbl_from = new Label();
			this.lbl_messageContent = new Label();
			this.lbl_index = new Label();
			this.lbl_date = new Label();
			this.lbl_status = new Label();
			this.pnl_line = new Panel();
			((ISupportInitialize)this.pic_logo).BeginInit();
			base.SuspendLayout();
			this.pic_logo.BackColor = Color.Transparent;
			this.pic_logo.Location = new Point(0, 0);
			this.pic_logo.Name = "pic_logo";
			this.pic_logo.Size = new Size(0xc6, 0x54);
			this.pic_logo.SizeMode = PictureBoxSizeMode.CenterImage;
			this.pic_logo.TabIndex = 0;
			this.pic_logo.TabStop = false;
			this.lbl_title.AutoEllipsis = true;
			this.lbl_title.BackColor = Color.Transparent;
			this.lbl_title.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lbl_title.ForeColor = Color.FromArgb(0, 0x33, 0x99);
			this.lbl_title.Location = new Point(0, 0);
			this.lbl_title.Name = "lbl_title";
			this.lbl_title.Padding = new Padding(3, 3, 3, 0);
			this.lbl_title.Size = new Size(0xc6, 0x16);
			this.lbl_title.TabIndex = 1;
			this.lbl_title.Text = "Title";
			this.lbl_from.AutoEllipsis = true;
			this.lbl_from.BackColor = Color.Transparent;
			this.lbl_from.Font = new Font("Segoe UI", 7.5f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbl_from.ForeColor = Color.Gray;
			this.lbl_from.Location = new Point(0, 0x11);
			this.lbl_from.Name = "lbl_from";
			this.lbl_from.Padding = new Padding(4, 2, 3, 0);
			this.lbl_from.Size = new Size(0xc5, 0x13);
			this.lbl_from.TabIndex = 1;
			this.lbl_from.Text = "From";
			this.lbl_messageContent.AutoEllipsis = true;
			this.lbl_messageContent.BackColor = Color.Transparent;
			this.lbl_messageContent.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbl_messageContent.Location = new Point(0, 0x24);
			this.lbl_messageContent.Name = "lbl_messageContent";
			this.lbl_messageContent.Padding = new Padding(3, 4, 3, 0);
			this.lbl_messageContent.Size = new Size(0xc6, 0x34);
			this.lbl_messageContent.TabIndex = 1;
			this.lbl_messageContent.Text = "Content";
			this.lbl_index.BackColor = Color.Transparent;
			this.lbl_index.Font = new Font("Segoe UI", 7.5f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbl_index.ForeColor = Color.Gray;
			this.lbl_index.Location = new Point(0x6c, 0x55);
			this.lbl_index.Name = "lbl_index";
			this.lbl_index.Padding = new Padding(3, 0, 3, 0);
			this.lbl_index.Size = new Size(90, 0x19);
			this.lbl_index.TabIndex = 1;
			this.lbl_index.Text = "1/3";
			this.lbl_index.TextAlign = ContentAlignment.MiddleRight;
			this.lbl_date.BackColor = Color.Transparent;
			this.lbl_date.Font = new Font("Segoe UI", 7.5f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbl_date.ForeColor = Color.Gray;
			this.lbl_date.Location = new Point(0, 0x55);
			this.lbl_date.Name = "lbl_date";
			this.lbl_date.Padding = new Padding(4, 0, 0, 0);
			this.lbl_date.Size = new Size(150, 0x19);
			this.lbl_date.TabIndex = 1;
			this.lbl_date.Text = "Date";
			this.lbl_date.TextAlign = ContentAlignment.MiddleLeft;
			this.lbl_status.AutoEllipsis = true;
			this.lbl_status.BackColor = Color.Transparent;
			this.lbl_status.Font = new Font("Segoe UI", 8.25f, FontStyle.Italic, GraphicsUnit.Point, 0);
			this.lbl_status.ForeColor = SystemColors.ControlText;
			this.lbl_status.Location = new Point(0, 0x4f);
			this.lbl_status.Name = "lbl_status";
			this.lbl_status.Padding = new Padding(3, 0, 3, 0);
			this.lbl_status.Size = new Size(0xc6, 0x1d);
			this.lbl_status.TabIndex = 1;
			this.lbl_status.TextAlign = ContentAlignment.MiddleCenter;
			this.pnl_line.BackColor = Color.FromArgb(0xbd, 0xd5, 0xdf);
			this.pnl_line.Location = new Point(6, 0x24);
			this.pnl_line.Name = "pnl_line";
			this.pnl_line.Size = new Size(0xba, 1);
			this.pnl_line.TabIndex = 0;
			this.pnl_line.Visible = false;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.Control;
			base.ClientSize = new Size(0xc6, 0x6c);
			base.Controls.Add(this.pnl_line);
			base.Controls.Add(this.lbl_status);
			base.Controls.Add(this.pic_logo);
			base.Controls.Add(this.lbl_title);
			base.Controls.Add(this.lbl_from);
			base.Controls.Add(this.lbl_messageContent);
			base.Controls.Add(this.lbl_index);
			base.Controls.Add(this.lbl_date);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Name = "Notifier";
			base.Opacity = 0.0;
			this.RightToLeftLayout = true;
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Gmail Notifier Plus";
			base.Shown += new EventHandler(this.Notifier_Shown);
			((ISupportInitialize)this.pic_logo).EndInit();
			base.ResumeLayout(false);
		}

		private void nextButton_Click(object sender, ThumbnailButtonClickedEventArgs e) {
			int num = (this.unreadMailCount > 20) ? 20 : this.unreadMailCount;
			if (this.currentMailIndex < num) {
				this.currentMailIndex++;
				this.UpdateMailPreview();
			}
		}

		private void Notifier_Activated(object sender, EventArgs e) {
			this.Refresh();
			this.taskbarManager.TabbedThumbnail.GetThumbnailPreview(base.Handle).InvalidatePreview();
		}

		private void Notifier_Shown(object sender, EventArgs e) {
			this.CreateThumbButtons();
			this.UpdateThumbButtonsStatus();
			this.ShowStatus();
			base.Top = 0x1000;
			base.Opacity = 100.0;
		}

		protected virtual void OnMailReceived(EventArgs e) {
			if (this.CheckFinished != null) {
				this.CheckFinished(this, e);
			}
		}

		private void OpenCurrentMail() {
			Help.ShowHelp(this, string.IsNullOrEmpty(this.currentMailUrl) ? UrlHelper.BuildInboxUrl(this.accountIndex) : this.currentMailUrl);
			this.Refresh();
		}

		private void previousButton_Click(object sender, ThumbnailButtonClickedEventArgs e) {
			if (this.currentMailIndex > 0) {
				this.currentMailIndex--;
				this.UpdateMailPreview();
			}
		}

		private void SetCheckingPreview() {
			this.lbl_status.Top = 0x52;
			this.lbl_status.Height = 0x1a;
			this.lbl_status.ForeColor = SystemColors.ControlText;
			this.lbl_status.Text = this.resourceManager.GetString("Label_Connecting");
			this.pic_logo.Image = Utilities.ResourceHelper.GetResourceImage("GmailNotifierPlus.Images.Checking.png");
		}

		private void SetNoMailPreview() {
			this.lbl_status.Top = 0;
			this.lbl_status.Height = 0x6c;
			this.lbl_status.ForeColor = Color.Gray;
			this.lbl_status.Text = this.resourceManager.GetString("Label_NoMail");
			this.pic_logo.Image = null;
		}

		private void SetOfflinePreview() {
			this.lbl_status.Top = 0x4f;
			this.lbl_status.Height = 0x1d;
			this.lbl_status.ForeColor = SystemColors.ControlText;
			this.lbl_status.Text = this.resourceManager.GetString("Label_ConnectionUnavailable");
			this.pic_logo.Image = Utilities.ResourceHelper.GetResourceImage("GmailNotifierPlus.Images.Offline.png");
		}

		private void config_Saved(object sender, EventArgs e) {
			if (!base.IsDisposed) {
				this.resourceManager = new ResourceManager("GmailNotifierPlus.Properties." + config.Language, Assembly.GetExecutingAssembly());
				this.UpdateThumbButtonsStatus();
				if (Locale.Current.IsRightToLeftLanguage) {
					this.lbl_status.RightToLeft = RightToLeft.Yes;
				}
				else {
					this.lbl_status.RightToLeft = RightToLeft.No;
				}
			}
		}

		private void SetWarningPreview() {
			this.lbl_status.Top = 0x4f;
			this.lbl_status.Height = 0x1d;
			this.lbl_status.ForeColor = SystemColors.ControlText;
			this.lbl_status.Text = this.resourceManager.GetString("Label_CheckLogin");
			this.pic_logo.Image = Utilities.ResourceHelper.GetResourceImage("GmailNotifierPlus.Images.Warning.png");
		}

		private void ShowMails() {
			this.pic_logo.Visible = this.lbl_status.Visible = false;
			this.lbl_title.Visible = this.lbl_from.Visible = this.lbl_messageContent.Visible = this.lbl_date.Visible = this.lbl_index.Visible = this.pnl_line.Visible = true;
			this.Refresh();
		}

		private void ShowStatus() {
			this.pic_logo.Visible = this.lbl_status.Visible = true;
			this.lbl_title.Visible = this.lbl_from.Visible = this.lbl_messageContent.Visible = this.lbl_date.Visible = this.lbl_index.Visible = this.pnl_line.Visible = false;
			this.Refresh();
		}

		private void UpdateMailPreview() {
			this.currentMailUrl = string.Empty;
			this.UpdateThumbButtonsStatus();
			switch (this.connectionStatus) {
				case Status.AuthenticationFailed:
					this.SetWarningPreview();
					break;

				case Status.Offline:
					this.SetOfflinePreview();
					break;

				case Status.OK:
					if (this.unreadMailCount > 0) {
						XmlNode node = this.currentMails[this.currentMailIndex];
						this.lbl_title.Text = string.IsNullOrEmpty(node.ChildNodes.Item(0).InnerText) ? this.resourceManager.GetString("Label_NoSubject") : node.ChildNodes.Item(0).InnerText;
						if ((node.ChildNodes.Item(6) != null) && (node.ChildNodes.Item(6).ChildNodes.Item(1) != null)) {
							this.lbl_from.Text = node.ChildNodes.Item(6).ChildNodes[1].InnerText;
						}
						else {
							this.lbl_from.Text = string.Empty;
						}
						this.lbl_messageContent.Text = node.ChildNodes.Item(1).InnerText;
						this.currentMailUrl = UrlHelper.BuildMailUrl(node.ChildNodes.Item(2).Attributes["href"].Value, this.accountIndex);
						DateTime time = DateTime.Parse(node.ChildNodes.Item(3).InnerText.Replace("T24:", "T00:"));
						this.lbl_date.Text = time.ToShortDateString() + " " + time.ToShortTimeString();
						this.lbl_index.Text = ((this.currentMailIndex + 1)).ToString() + "/" + ((this.unreadMailCount > 20) ? 20 : this.unreadMailCount);
						this.ShowMails();
					}
					else {
						this.SetNoMailPreview();
						this.ShowStatus();
					}
					goto Label_0230;
			}
			this.previousButton.Enabled = false;
			this.nextButton.Enabled = false;
			this.ShowStatus();
		Label_0230:
			this.taskbarManager.TabbedThumbnail.GetThumbnailPreview(base.Handle).InvalidatePreview();
		}

		private void UpdateThumbButtonsStatus() {
			int num = (this.unreadMailCount > 20) ? 20 : this.unreadMailCount;
			this.previousButton.Enabled = this.currentMailIndex != 0;
			this.nextButton.Enabled = this.currentMailIndex < (num - 1);
			this.previousButton.Tooltip = this.resourceManager.GetString("Tooltip_Previous");
			this.nextButton.Tooltip = this.resourceManager.GetString("Tooltip_Next");
			if (this.unreadMailCount == 0) {
				this.inboxButton.Icon = Utilities.ResourceHelper.GetResourceIcon("GmailNotifierPlus.Icons.Inbox.ico");
				this.inboxButton.Tooltip = this.resourceManager.GetString("Tooltip_Inbox");
			}
			else {
				this.inboxButton.Icon = Utilities.ResourceHelper.GetResourceIcon("GmailNotifierPlus.Icons.Open.ico");
				this.inboxButton.Tooltip = this.resourceManager.GetString("Tooltip_OpenMail");
			}
			this.inboxButton.Enabled = this.connectionStatus == Status.OK;
		}

		private void webClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {
			if (e.Error == null) {
				this.connectionStatus = Status.OK;
				string xml = Encoding.UTF8.GetString(e.Result).Replace("<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">", "<feed>");
				XmlDocument document = new XmlDocument();
				document.LoadXml(xml);
				XmlNode node = document.SelectSingleNode("/feed/fullcount");
				this.unreadMailCount = Convert.ToInt32(node.InnerText);
				this.currentMails = document.SelectNodes("/feed/entry");
				this.currentMailIndex = 0;
			}
			else {
				WebException error = (WebException)e.Error;
				if (error.Status == WebExceptionStatus.ProtocolError) {
					this.connectionStatus = Status.AuthenticationFailed;
				}
				else {
					this.connectionStatus = Status.Offline;
				}
			}
			this.UpdateMailPreview();
			this.CheckFinished(this, EventArgs.Empty);
		}

		public int AccountIndex {
			get {
				return this.accountIndex;
			}
		}

		public Status ConnectionStatus {
			get {
				return this.connectionStatus;
			}
		}

		public XmlNodeList CurrentMails {
			get {
				return this.currentMails;
			}
		}

		public int UnreadMailCount {
			get {
				return this.unreadMailCount;
			}
		}

		public enum Status {
			OK,
			AuthenticationFailed,
			Offline
		}
	}
}

