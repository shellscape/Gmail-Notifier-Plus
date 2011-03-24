using GmailNotifierPlus.Utilities;

using Microsoft.WindowsAPICodePack.Taskbar;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace GmailNotifierPlus.Forms {
	public partial class Notifier : Form {

		private const int FeedMax = 20;

		private int _previousUnread;
		private int _mailIndex;
		private String _mailUrl;
		private Boolean _thumbActivated = false;

		private TabbedThumbnail _preview = null;
		private ThumbnailToolBarButton _ButtonInbox;
		private ThumbnailToolBarButton _ButtonNext;
		private ThumbnailToolBarButton _ButtonPrev;
		private TaskbarManager _taskbarManager = TaskbarManager.Instance;

		private Config _config = Config.Current;

		private WebClient _webClient = new WebClient();

		public event CheckMailFinishedEventHandler CheckMailFinished;

		public enum NotifierStatus {
			OK,
			AuthenticationFailed,
			Offline
		}		

		public Notifier(Account account) {
			InitializeComponent();

			this.Icon = Program.Icon;

			this.Account = account;
			
			_LabelStatus.RightToLeft = Locale.Current.IsRightToLeftLanguage ? RightToLeft.Yes : RightToLeft.No;
			_LabelStatus.Width = this.Width;

			_webClient.DownloadDataCompleted += _WebClient_DownloadDataCompleted;
			_config.Saved += _Config_Saved;

			ToolTip openTip = new ToolTip();

			openTip.SetToolTip(_PictureOpen, Locale.Current.Tooltips.OpenMail);

			using (Icon icon = Utilities.ResourceHelper.GetIcon("Open.ico")) {
				_PictureOpen.Image = icon.ToBitmap();
			}

			_PictureOpen.Cursor = Cursors.Hand;

			this.Text = Account.FullAddress;	
		}

#region .    Public Properties    

		public List<Email> Emails { get { return Account.Emails; } }
		public Account Account { get; private set; }
		public NotifierStatus ConnectionStatus { get; private set; }

		/// <summary>
		/// Returns an XmlNodeList containing the last response from the server.
		/// </summary>
		//public XmlNodeList XmlMail { get; private set; }

		/// <summary>
		/// Returns the number of unread emails for associated account.
		/// </summary>
		public int Unread {
			get { return this.Account.Unread; }
			private set {
				this.Account.Unread = value;
			}
		}

		public TabbedThumbnail PreviewThumbnail {
			get { return _preview; }
			set {
				if (_preview != value) {
					_preview = value;

					if (_preview != null) {
						_preview.TabbedThumbnailActivated += delegate(object sender, TabbedThumbnailEventArgs e) {
							
							// i can't track down why exactly, but this is being fired twice.
							if (_thumbActivated) {
								_thumbActivated = false;
							}
							else {
								_thumbActivated = true;
								OpenEmail();
							}
						};
					}
				}
			}
		}

#endregion

#region .    Events    
		
		private void Notifier_Activated(object sender, EventArgs e) {
			this.Refresh();

			TabbedThumbnail thumb = _taskbarManager.TabbedThumbnail.GetThumbnailPreview(this); //_PictureLogo);
			
			if(thumb !=  null){
				thumb.InvalidatePreview();
			}
		}

		private void Notifier_Shown(object sender, EventArgs e) {
			CreateThumbButtons();
			UpdateThumbButtonsStatus();
			ShowStatus();
			base.Top = 0x1000;
			base.Opacity = 100.0;
		}

		internal void CheckMail() {
			if (_webClient.IsBusy) {
				return;
			}

			try {
				_webClient.Credentials = new NetworkCredential(Account.Login, Account.Password);
				_webClient.DownloadDataAsync(new Uri(UrlHelper.GetFeedUrl(Account)));

				SetCheckingPreview();
			}
			catch { }
		}

		private void _ButtonPrev_Click(object sender, ThumbnailButtonClickedEventArgs e) {
			if (_mailIndex > 0) {
				_mailIndex--;
				UpdateMailPreview();
			}
		}

		private void _ButtonInbox_Click(object sender, ThumbnailButtonClickedEventArgs e) {
			OpenInbox();
		}

		private void _ButtonNext_Click(object sender, ThumbnailButtonClickedEventArgs e) {

			if (_mailIndex < Unread) {
				_mailIndex++;
				UpdateMailPreview();
			}
		}

		private void _Config_Saved(object sender, EventArgs e) {
			if (base.IsDisposed) {
				return;
			}

			UpdateThumbButtonsStatus();

			_LabelStatus.RightToLeft = Locale.Current.IsRightToLeftLanguage ? RightToLeft.Yes : RightToLeft.No;
		}

		private void _WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {
			if (e.Error == null) {
				ConnectionStatus = NotifierStatus.OK;

				String xml = System.Text.Encoding.UTF8.GetString(e.Result).Replace("<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">", "<feed>");
				XmlDocument document = new XmlDocument();

				try {
					document.LoadXml(xml);

					XmlNode node = document.SelectSingleNode("/feed/fullcount");

					_previousUnread = Unread;

					Unread = Convert.ToInt32(node.InnerText);
					//XmlMail = document.SelectNodes("/feed/entry");

					Account.Emails.Clear();

					foreach (XmlNode mailNode in document.SelectNodes("/feed/entry")) {
						Account.Emails.Add(Email.FromNode(mailNode, Account));
					}

					_mailIndex = 0;
				}
				catch (System.Xml.XmlException) { }
				catch (Exception) { } // fixed issue #6. google will occasionally send back invalid xml.
			}
			else {
				WebException error = (WebException)e.Error;
				if (error.Status == WebExceptionStatus.ProtocolError) {
					ConnectionStatus = NotifierStatus.AuthenticationFailed;
				}
				else {
					ConnectionStatus = NotifierStatus.Offline;
				}
			}

			UpdateMailPreview();
			CheckMailFinished(this, EventArgs.Empty);

			if (Unread != _previousUnread && _config.ShowToast) {
				ToastManager.Pop(Account);
			}
		}

#endregion

		private void CreateThumbButtons() {

			_ButtonPrev = new ThumbnailToolBarButton(Utilities.ResourceHelper.GetIcon("Previous.ico"), Locale.Current.Tooltips.Previous);
			_ButtonPrev.Click += _ButtonPrev_Click;

			_ButtonInbox = new ThumbnailToolBarButton(Utilities.ResourceHelper.GetIcon("Inbox.ico"), Locale.Current.Tooltips.Inbox);
			_ButtonInbox.Click += _ButtonInbox_Click;

			_ButtonNext = new ThumbnailToolBarButton(Utilities.ResourceHelper.GetIcon("Next.ico"), Locale.Current.Tooltips.Next);
			_ButtonNext.Click += _ButtonNext_Click;
			
			_taskbarManager.ThumbnailToolBars.AddButtons(base.Handle, new ThumbnailToolBarButton[] { _ButtonPrev, _ButtonInbox, _ButtonNext });
		}

		private void SetCheckingPreview() {
			_LabelStatus.Top = 82;
			_LabelStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			_LabelStatus.Text = Locale.Current.Labels.Connecting;
			_PictureLogo.Image = Utilities.ResourceHelper.GetImage("Checking.png");
			_PictureOpen.Visible = false;
		}

		private void SetNoMailPreview() {
			_LabelStatus.Text = Locale.Current.Labels.NoMail;

			_LabelStatus.Location = new System.Drawing.Point(
				(this.Width - _LabelStatus.Width) / 2,
				(this.Height - _LabelStatus.Height) / 2
			);
			
			_LabelStatus.ForeColor = System.Drawing.Color.Gray;
			_PictureLogo.Image = null;
			_PictureOpen.Visible = false;
		}

		private void SetOfflinePreview() {
			_LabelStatus.Location = new Point(0, 84);
			_LabelStatus.Width = this.Width;
			_LabelStatus.TextAlign = ContentAlignment.MiddleCenter;
			_LabelStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			_LabelStatus.Text = Locale.Current.Labels.ConnectionUnavailable;
			_PictureLogo.Image = Utilities.ResourceHelper.GetImage("Offline.png");
			_PictureOpen.Visible = false;
		}

		private void SetWarningPreview() {
			_LabelStatus.Location = new Point(0, 84);
			_LabelStatus.Width = this.Width;
			_LabelStatus.TextAlign = ContentAlignment.MiddleCenter;
			_LabelStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			_LabelStatus.Text = Locale.Current.Labels.CheckLogin;
			_PictureLogo.Image = Utilities.ResourceHelper.GetImage("Warning.png");
			_PictureOpen.Visible = false;
		}

		private void ShowMails() {
			_PictureLogo.Visible = _LabelStatus.Visible = false;

			_LabelTitle.Visible =
				_LabelFrom.Visible =
				_LabelMessage.Visible =
				_LabelDate.Visible =
				_LabelIndex.Visible =
				_PanelLine.Visible =
				_PictureOpen.Visible = true;

			this.Refresh();
		}

		private void ShowStatus() {
			_PictureLogo.Visible = _LabelStatus.Visible = true;

			_LabelTitle.Visible =
				_LabelFrom.Visible =
				_LabelMessage.Visible =
				_LabelDate.Visible =
				_LabelIndex.Visible =
				_PanelLine.Visible =
				_PictureOpen.Visible = false;

			this.Refresh();
		}

		private void UpdateMailPreview() {
			_mailUrl = string.Empty;

			this.UpdateThumbButtonsStatus();

			switch (ConnectionStatus) {

				case NotifierStatus.AuthenticationFailed:
					this.SetWarningPreview();
					break;

				case NotifierStatus.Offline:
					this.SetOfflinePreview();
					break;

				case NotifierStatus.OK:

					if (Unread > 0) {
						//XmlNode node = XmlMail[_MailIndex];
						//DateTime time = DateTime.Parse(node.ChildNodes.Item(3).InnerText.Replace("T24:", "T00:"));

						//_LabelTitle.Text = string.IsNullOrEmpty(node.ChildNodes.Item(0).InnerText) ? Locale.Current.Labels.NoSubject : node.ChildNodes.Item(0).InnerText;
						//_LabelMessage.Text = node.ChildNodes.Item(1).InnerText;
						//_LabelDate.Text = time.ToShortDateString() + " " + time.ToShortTimeString();
						//_LabelIndex.Text = ((_MailIndex + 1)).ToString() + "/" + ((Unread > 20) ? 20 : Unread);

						//if ((node.ChildNodes.Item(6) != null) && (node.ChildNodes.Item(6).ChildNodes.Item(1) != null)) {
						//  _LabelFrom.Text = node.ChildNodes.Item(6).ChildNodes[1].InnerText;
						//}
						//else {
						//  _LabelFrom.Text = string.Empty;
						//}

						Email email = Account.Emails[_mailIndex];

						_LabelDate.Text = email.Date;
						_LabelFrom.Text = email.From;
						_LabelIndex.Text = ((_mailIndex + 1)).ToString() + "/" + ((Unread > 20) ? 20 : Unread);
						_LabelMessage.Text = email.Message;
						_LabelTitle.Text = email.Title;

						_mailUrl = email.Url;

						this.ShowMails();
					}
					else {
						this.SetNoMailPreview();
						this.ShowStatus();
					}

					TabbedThumbnail thumb = _taskbarManager.TabbedThumbnail.GetThumbnailPreview(this);
					
					if (thumb != null) {
						thumb.InvalidatePreview();
					}

					return;
			}

			_ButtonPrev.Enabled = false;
			_ButtonNext.Enabled = false;

			this.ShowStatus();
		}

		private void UpdateThumbButtonsStatus() {

			_ButtonPrev.Enabled = _mailIndex != 0;
			_ButtonPrev.Tooltip = Locale.Current.Tooltips.Previous;

			_ButtonNext.Enabled = _mailIndex < (Unread - 1);
			_ButtonNext.Tooltip = Locale.Current.Tooltips.Next;

			//if (Unread == 0) {
				_ButtonInbox.Icon = Utilities.ResourceHelper.GetIcon("Inbox.ico");
				_ButtonInbox.Tooltip = Locale.Current.Tooltips.Inbox;
			//}
			//else {
			//  _ButtonInbox.Icon = Utilities.ResourceHelper.GetIcon("Open.ico");
			//  _ButtonInbox.Tooltip = Locale.Current.Tooltips.OpenMail;
			//}

			_ButtonInbox.Enabled = ConnectionStatus == NotifierStatus.OK;
		}

		private void OnMailReceived(EventArgs e) {
			if (CheckMailFinished != null) {
				CheckMailFinished(this, e);
			}
		}

		private void OpenInbox() {
			Utilities.UrlHelper.Launch(this.Account, UrlHelper.BuildInboxUrl(Account));
			this.Refresh();
		}

		private void OpenEmail() {
			if (!String.IsNullOrEmpty(_mailUrl)) {
				Utilities.UrlHelper.Launch(this.Account, _mailUrl);
				this.Refresh();
			}
		}

	}
}
