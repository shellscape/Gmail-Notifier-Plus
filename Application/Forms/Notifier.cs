using GmailNotifierPlus.Utilities;

using Microsoft.WindowsAPICodePack.Taskbar;

using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace GmailNotifierPlus.Forms {
	public partial class Notifier : Form {

		private const int _FeedMax = 20;
		private int _MailIndex;
		private String _MailUrl;

		private ThumbnailToolBarButton _ButtonInbox;
		private ThumbnailToolBarButton _ButtonNext;
		private ThumbnailToolBarButton _ButtonPrev;
		private TaskbarManager _TaskbarManager = TaskbarManager.Instance;

		private Config _Config = Config.Current;
		private Account _Account = null;

		private WebClient _WebClient = new WebClient();

		public event CheckMailFinishedEventHandler CheckMailFinished;

		public enum NotifierStatus {
			OK,
			AuthenticationFailed,
			Offline
		}		

		public Notifier(int accountIndex) {
			InitializeComponent();

			this.Icon = Program.Icon;

			AccountIndex = accountIndex;

			_Account = _Config.Accounts[accountIndex];
			_LabelStatus.RightToLeft = Locale.Current.IsRightToLeftLanguage ? RightToLeft.Yes : RightToLeft.No;
			_WebClient.DownloadDataCompleted += _WebClient_DownloadDataCompleted;
			_Config.Saved += _Config_Saved;

			// the background image is cuasing an Out of Memory Exception. Investigate this later.
			//this.BackgroundImage = Utilities.ResourceHelper.GetImage("Background.png");
			this.Text = _Account.FullAddress;	
		}

#region .    Public Properties    

		public int AccountIndex { get; private set; }
		public NotifierStatus ConnectionStatus { get; private set; }

		/// <summary>
		/// Returns an XmlNodeList containing the last response from the server.
		/// </summary>
		public XmlNodeList XmlMail { get; private set; }

		/// <summary>
		/// Returns the number of unread emails for associated account.
		/// </summary>
		public int Unread { get; private set; }

#endregion

#region .    Events    
		
		private void Notifier_Activated(object sender, EventArgs e) {
			this.Refresh();

			TabbedThumbnail thumb = _TaskbarManager.TabbedThumbnail.GetThumbnailPreview(this); //_PictureLogo);
			
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
			if (_WebClient.IsBusy) {
				return;
			}

			try {
				_WebClient.Credentials = new NetworkCredential(_Account.Login, _Config.Accounts[AccountIndex].Password);
				_WebClient.DownloadDataAsync(new Uri(UrlHelper.GetFeedUrl(AccountIndex)));

				SetCheckingPreview();
			}
			catch { }
		}

		private void _ButtonPrev_Click(object sender, ThumbnailButtonClickedEventArgs e) {
			if (_MailIndex > 0) {
				_MailIndex--;
				UpdateMailPreview();
			}
		}

		private void _ButtonInbox_Click(object sender, ThumbnailButtonClickedEventArgs e) {
			OpenCurrentMail();
		}

		private void _ButtonNext_Click(object sender, ThumbnailButtonClickedEventArgs e) {
			int num = (Unread > 20) ? 20 : Unread;
			if (_MailIndex < num) {
				_MailIndex++;
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

				document.LoadXml(xml);

				XmlNode node = document.SelectSingleNode("/feed/fullcount");

				Unread = Convert.ToInt32(node.InnerText);
				XmlMail = document.SelectNodes("/feed/entry");
				_MailIndex = 0;
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
		}

#endregion

		private void CreateThumbButtons() {

			_ButtonPrev = new ThumbnailToolBarButton(Utilities.ResourceHelper.GetIcon("Previous.ico"), Locale.Current.Tooltips.Previous);
			_ButtonPrev.Click += _ButtonPrev_Click;

			_ButtonInbox = new ThumbnailToolBarButton(Utilities.ResourceHelper.GetIcon("Inbox.ico"), Locale.Current.Tooltips.Inbox);
			_ButtonInbox.Click += _ButtonInbox_Click;

			_ButtonNext = new ThumbnailToolBarButton(Utilities.ResourceHelper.GetIcon("Next.ico"), Locale.Current.Tooltips.Next);
			_ButtonNext.Click += _ButtonNext_Click;
			//_ButtonNext.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(this._ButtonNext_Click);

			//_ButtonNext.Click += delegate(object sender, ThumbnailButtonClickedEventArgs e) {
			//  MessageBox.Show("Clicked");
			//};
			
			_TaskbarManager.ThumbnailToolBars.AddButtons(base.Handle, new ThumbnailToolBarButton[] { _ButtonPrev, _ButtonInbox, _ButtonNext });
		}

		private void SetCheckingPreview() {
			_LabelStatus.Top = 82;
			//_LabelStatus.Height = 26;
			_LabelStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			_LabelStatus.Text = Locale.Current.Labels.Connecting;
			_PictureLogo.Image = Utilities.ResourceHelper.GetImage("Checking.png");
		}

		private void SetNoMailPreview() {
			_LabelStatus.Text = Locale.Current.Labels.NoMail;

			_LabelStatus.Location = new System.Drawing.Point(
				(this.Width - _LabelStatus.Width) / 2,
				(this.Height - _LabelStatus.Height) / 2
			);
			
			//_LabelStatus.Height = 108;
			_LabelStatus.ForeColor = System.Drawing.Color.Gray;
			_PictureLogo.Image = null;
		}

		private void SetOfflinePreview() {
			_LabelStatus.Top = 79;
			//_LabelStatus.Height = 29;
			_LabelStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			_LabelStatus.Text = Locale.Current.Labels.ConnectionUnavailable;
			_PictureLogo.Image = Utilities.ResourceHelper.GetImage("Offline.png");
		}

		private void SetWarningPreview() {
			_LabelStatus.Top = 0x4f;
			//_LabelStatus.Height = 0x1d;
			_LabelStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			_LabelStatus.Text = Locale.Current.Labels.CheckLogin;
			_PictureLogo.Image = Utilities.ResourceHelper.GetImage("Warning.png");
		}

		private void ShowMails() {
			_PictureLogo.Visible = _LabelStatus.Visible = false;

			_LabelTitle.Visible =
				_LabelFrom.Visible =
				_LabelMessage.Visible =
				_LabelDate.Visible =
				_LabelIndex.Visible =
				_PanelLine.Visible = true;

			this.Refresh();
		}

		private void ShowStatus() {
			_PictureLogo.Visible = _LabelStatus.Visible = true;

			_LabelTitle.Visible =
				_LabelFrom.Visible =
				_LabelMessage.Visible =
				_LabelDate.Visible =
				_LabelIndex.Visible =
				_PanelLine.Visible = false;

			this.Refresh();
		}

		private void UpdateMailPreview() {
			_MailUrl = string.Empty;

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
						XmlNode node = XmlMail[_MailIndex];
						DateTime time = DateTime.Parse(node.ChildNodes.Item(3).InnerText.Replace("T24:", "T00:"));

						_LabelTitle.Text = string.IsNullOrEmpty(node.ChildNodes.Item(0).InnerText) ? Locale.Current.Labels.NoSubject : node.ChildNodes.Item(0).InnerText;
						_LabelMessage.Text = node.ChildNodes.Item(1).InnerText;
						_LabelDate.Text = time.ToShortDateString() + " " + time.ToShortTimeString();
						_LabelIndex.Text = ((_MailIndex + 1)).ToString() + "/" + ((Unread > 20) ? 20 : Unread);

						if ((node.ChildNodes.Item(6) != null) && (node.ChildNodes.Item(6).ChildNodes.Item(1) != null)) {
							_LabelFrom.Text = node.ChildNodes.Item(6).ChildNodes[1].InnerText;
						}
						else {
							_LabelFrom.Text = string.Empty;
						}

						_MailUrl = UrlHelper.BuildMailUrl(node.ChildNodes.Item(2).Attributes["href"].Value, AccountIndex);

						this.ShowMails();
					}
					else {
						this.SetNoMailPreview();
						this.ShowStatus();
					}

					TabbedThumbnail thumb = _TaskbarManager.TabbedThumbnail.GetThumbnailPreview(this); //._PictureLogo);

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

			int num = (Unread > 20) ? 20 : Unread;

			_ButtonPrev.Enabled = _MailIndex != 0;
			_ButtonPrev.Tooltip = Locale.Current.Tooltips.Previous;

			_ButtonNext.Enabled = _MailIndex < (num - 1);
			_ButtonNext.Tooltip = Locale.Current.Tooltips.Next;

			if (Unread == 0) {
				_ButtonInbox.Icon = Utilities.ResourceHelper.GetIcon("Inbox.ico");
				_ButtonInbox.Tooltip = Locale.Current.Tooltips.Inbox;
			}
			else {
				_ButtonInbox.Icon = Utilities.ResourceHelper.GetIcon("Open.ico");
				_ButtonInbox.Tooltip = Locale.Current.Tooltips.OpenMail;
			}

			_ButtonInbox.Enabled = ConnectionStatus == NotifierStatus.OK;
		}

		private void OnMailReceived(EventArgs e) {
			if (CheckMailFinished != null) {
				CheckMailFinished(this, e);
			}
		}

		private void OpenCurrentMail() {
			Help.ShowHelp(this, string.IsNullOrEmpty(_MailUrl) ? UrlHelper.BuildInboxUrl(AccountIndex) : _MailUrl);
			this.Refresh();
		}

	}
}
