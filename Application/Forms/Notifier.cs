using GmailNotifierPlus.Utilities;

using Microsoft.WindowsAPI.Taskbar;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.Xml;

using GmailNotifierPlus.Localization;

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

			this.Icon = Resources.Icons.Window;

			this.Account = account;

			SetCheckingPreview();

			_LabelStatus.RightToLeft = Locale.Current.IsRightToLeftLanguage ? RightToLeft.Yes : RightToLeft.No;
			_LabelStatus.Width = this.Width;

			_webClient.DownloadDataCompleted += _WebClient_DownloadDataCompleted;
			
			_config.Saved += _Config_Saved;
			_config.LanguageChanged += delegate(Config sender) {
				UpdateMailPreview();
			};

			//using (Icon icon = Utilities.ResourceHelper.GetIcon("Open.ico")) {
			//  _PictureOpen.Image = icon.ToBitmap();
			//}

			_PictureOpen.Image = Resources.Icons.Open.ToBitmap();

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
						_preview.TabbedThumbnailActivated += Thumbnail_Activated;
					}
				}
			}
		}

		#endregion

		#region .    Events

		private void Thumbnail_Activated(object sender, TabbedThumbnailEventArgs e) {

			// i can't track down why exactly, but this is being fired twice.
			if (_thumbActivated) {
				_thumbActivated = false;
			}
			else {
				_thumbActivated = true;

				if (ConnectionStatus == NotifierStatus.AuthenticationFailed) {
					//Program.MainForm.Jumplist_ShowPreferences();
				}
				else {
					if (ConnectionStatus != NotifierStatus.Offline) {
						OpenEmail();
					}
				}
			}
		}

		private void Notifier_Activated(object sender, EventArgs e) {
			this.Refresh();

			TabbedThumbnail thumb = _taskbarManager.TabbedThumbnail.GetThumbnailPreview(this); //_PictureLogo);

			if (thumb != null) {
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

			_LabelStatus.RightToLeft = Localization.Locale.Current.IsRightToLeftLanguage ? RightToLeft.Yes : RightToLeft.No;
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

					// at a point, i thought that the oldest should be shown first. not sure why.
					//Account.Emails.Sort();

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

			if (CheckMailFinished != null) {
				CheckMailFinished(this, EventArgs.Empty);
			}

			if (Unread > _previousUnread && _config.ShowToast && Account.Emails.Count > 0) {
				ToastManager.Pop(Account);
			}
		}

		#endregion

		protected override bool ShowWithoutActivation {
			get { return true; }
		}

		// overriding onActivated and onGotFocus so that any focus to the notification windows
		// will give focus to the main form. this is being done so that clicking on a preview thumbnail
		// will always open the email in the set browser.
		// the way the clicks work, it activates the associated window. if the window is already activated (has focus)
		// then the click is essentially cancelled.
		protected override void OnActivated(EventArgs e) {
			base.OnActivated(e);

			if (Program.MainForm != null) {
				Program.MainForm.Activate();
			}
		}

		protected override void OnGotFocus(EventArgs e) {
			base.OnGotFocus(e);

			if (Program.MainForm != null) {
				Program.MainForm.Activate();
			}
		}

		private void CreateThumbButtons() {

			_ButtonPrev = new ThumbnailToolBarButton(Resources.Icons.Previous, Locale.Current.Common.Previous);
			_ButtonPrev.Click += _ButtonPrev_Click;

			_ButtonInbox = new ThumbnailToolBarButton(Resources.Icons.Inbox, Locale.Current.Thumbnails.Inbox);
			_ButtonInbox.Click += _ButtonInbox_Click;

			_ButtonNext = new ThumbnailToolBarButton(Resources.Icons.Next, Locale.Current.Common.Next);
			_ButtonNext.Click += _ButtonNext_Click;

			_taskbarManager.ThumbnailToolBars.AddButtons(base.Handle, new ThumbnailToolBarButton[] { _ButtonPrev, _ButtonInbox, _ButtonNext });
		}

		private void SetCheckingPreview() {
			_LabelStatus.AutoSize = false;
			_LabelStatus.Top = 82;
			_LabelStatus.TextAlign = ContentAlignment.MiddleCenter;
			_LabelStatus.Left = 0;
			_LabelStatus.Width = this.Width;
			_LabelStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			_LabelStatus.Text = Locale.Current.Thumbnails.Connecting;
			_PictureLogo.Image = Resources.Bitmaps.Checking;
			_PictureOpen.Visible = false;
		}

		private void SetNoMailPreview() {
			_LabelStatus.Text = Locale.Current.Thumbnails.NoMail;

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
			_LabelStatus.Text = Locale.Current.Thumbnails.ConnectionUnavailable;
			_PictureLogo.Image = Resources.Bitmaps.Offline;
			_PictureOpen.Visible = false;
		}

		private void SetWarningPreview() {
			_LabelStatus.Location = new Point(0, 84);
			_LabelStatus.Width = this.Width;
			_LabelStatus.TextAlign = ContentAlignment.MiddleCenter;
			_LabelStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			_LabelStatus.Text = Locale.Current.Thumbnails.CheckLogin;
			_PictureLogo.Image = Resources.Bitmaps.Warning;
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

			if (this.Disposing || this.IsDisposed) {
				return;
			}

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

						Email email = Account.Emails[_mailIndex];

						_LabelDate.Text = email.When;
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

					TabbedThumbnail thumb = null;

					// for some reason, at this point specifically, the form is sometimes disposed by the time the process enters GetThumbnailPreview.
					try {
						thumb = _taskbarManager.TabbedThumbnail.GetThumbnailPreview(this);
					}
					catch (ObjectDisposedException) { }

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
			_ButtonPrev.Tooltip = Locale.Current.Common.Previous;

			// gmail's atom feed only sends data for the first 20 unread
			_ButtonNext.Enabled = _mailIndex < 20 && _mailIndex < (Unread - 1);
			_ButtonNext.Tooltip = Locale.Current.Common.Next;

			_ButtonInbox.Icon = Resources.Icons.Inbox;
			_ButtonInbox.Tooltip = Locale.Current.Thumbnails.Inbox;

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
			if (String.IsNullOrEmpty(_mailUrl)) {
				OpenInbox();
			}
			else {
				Utilities.UrlHelper.Launch(this.Account, _mailUrl);
				this.Refresh();
			}
		}

	}
}
