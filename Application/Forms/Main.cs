using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;

using GmailNotifierPlus.Utilities;

namespace GmailNotifierPlus.Forms {
	public partial class Main : Form {

		[DllImport("user32.dll")]
		public static extern uint RegisterWindowMessage(string message);

		private const int _UNREAD_MAX = 0x63;

		private Dictionary<string, Notifier> _instances = new Dictionary<string, Notifier>();
		private JumpList _jumpList;
		private Preferences _preferences;
		private Dictionary<string, Notifier.NotifierStatus> _statusList = new Dictionary<string, Notifier.NotifierStatus>();
		private TaskbarManager _taskbarManager = TaskbarManager.Instance;

		private int _PreviousTotal;
		private int _UnreadTotal;

		private Icon _iconDigits = null;
		private Icon _iconTray = null;
		private Icon _iconWindow = null;

		private Config _config = Config.Current;

		private static readonly int WM_TASKBARBUTTONCREATED = ((int)RegisterWindowMessage("TaskbarButtonCreated"));

		public Main(string[] args) {

			InitializeComponent();

			_iconWindow = ResourceHelper.GetIcon("gmail-classic.ico");

			this.Icon = _iconWindow;
			this.Location = new Point(-10000, -10000);
			this.Text = this._TrayIcon.Text = GmailNotifierPlus.Resources.WindowTitle;

			this.CreateInstances();
			
			if (args.Length > 0 && args[0] == Program.Arguments.Settings) {
				this.OpenSettingsWindow();
			}

			_config.Saved += _Config_Saved;

			_Timer.Tick += _Timer_Tick;
			_Timer.Interval = _config.Interval * 1000;
			_Timer.Enabled = true;
		}

		public Boolean FirstRun { get; set; }

		protected override void WndProc(ref Message m) {
			if (m.Msg == WM_TASKBARBUTTONCREATED) {
				this.CheckMail();
			}
			base.WndProc(ref m);
		}

		#region .    Event Handlers

		private void Main_FormClosing(object sender, FormClosingEventArgs e) {
			try {
				HideTrayIcon();

				this._jumpList.RemoveCustomCategories();
				this._jumpList.Refresh();
			}
			catch {
			}
		}

		private void Main_Load(object sender, EventArgs e) {

			AllowTaskbarWindowMessagesThroughUIPI();

			_jumpList = JumpList.CreateJumpListForIndividualWindow(this._taskbarManager.ApplicationId, base.Handle);
			_jumpList.JumpListItemsRemoved += delegate(object o, UserRemovedJumpListItemsEventArgs ev) { };
			_jumpList.KnownCategoryToDisplay = JumpListKnownCategoryType.Neither;

			BuildJumpList();
		}

		private void Main_Shown(object sender, EventArgs e) {
			if (_config.FirstRun) {
				this.ShowAbout();

				_config.FirstRun = false;
				_config.Save();

			}

			base.Top = 5000;
		}

		private void _Config_Saved(object sender, EventArgs e) {
			_Timer.Interval = _config.Interval * 1000;

			if (_config.Accounts.Count > _instances.Count) {
				this.CreateInstances();
			}
			else if (_config.Accounts.Count < _instances.Count) {
				this.CloseInstances();
			}

			if (!Config.Current.ShowTrayIcon) {
				_TrayIcon.Visible = false;
			}

			if (Config.Current.CheckForUpdates) {

			}
			else {

			}

			this.BuildJumpList();
			this.UpdateMailsJumpList();

			this.CheckMail();
		}

		private void _Timer_Tick(object sender, EventArgs e) {
			this.CheckMail();
		}

		private void _Notifier_CheckFinished(Notifier sender, EventArgs e) {

			if (_statusList.ContainsKey(sender.Text)) {
				_statusList[sender.Text] = sender.ConnectionStatus;
			}
			else {
				_statusList.Add(sender.Text, sender.ConnectionStatus);
			}

			_UnreadTotal += sender.Unread;

			FinalizeChecks();
		}

		private void _Notifier_FormClosed(object sender, FormClosedEventArgs e) {

			Notifier notifier = (Notifier)sender;
			notifier.FormClosed -= _Notifier_FormClosed;
			notifier.CheckMailFinished -= _Notifier_CheckFinished;

			if (_instances.ContainsKey(notifier.Text)) {
				_instances.Remove(notifier.Text);
			}

			if (_statusList.ContainsKey(notifier.Text)) {
				_statusList.Remove(notifier.Text);
			}

			notifier.Dispose();
			notifier = null;

			this.FinalizeChecks();
			if (_instances.Count == 0) {
				base.Close();
			}
			else {
				_taskbarManager.TabbedThumbnail.SetActiveTab(_instances[_config.Accounts[0].FullAddress].Handle);
			}
		}

		private void _Preview_TabbedThumbnailClosed(object sender, TabbedThumbnailEventArgs e) {
			(Control.FromHandle(e.WindowHandle) as Form).Close();
		}

		private void _SettingsWindow_FormClosed(object sender, FormClosedEventArgs e) {
			_preferences.Dispose();
			_preferences = null;
		}

		#endregion

		#region .    Private Methods

		private void BuildJumpList() {

			_jumpList.ClearAllUserTasks();

			int defaultAccountIndex = _config.Accounts.IndexOf(_config.Accounts.Default);
			String exePath = Application.ExecutablePath;
			String path = Path.Combine(Path.GetDirectoryName(exePath), "Resources\\Icons");

			JumpListTask compose = new JumpListLink(UrlHelper.BuildComposeUrl(defaultAccountIndex), Locale.Current.Labels.Compose) {
				IconReference = new IconReference(Path.Combine(path, "Compose.ico"), 0)
			};

			// we need a different icon name here, there's a really whacky conflict between an embedded resource, and a content resource file name.

			JumpListTask inbox = new JumpListLink(UrlHelper.BuildInboxUrl(defaultAccountIndex), Locale.Current.Labels.Inbox) {
				IconReference = new IconReference(Path.Combine(path, "GoInbox.ico"), 0)
			};

			JumpListTask refresh = new JumpListLink(exePath, Locale.Current.Labels.CheckMail) {
				IconReference = new IconReference(Path.Combine(path, "Refresh.ico"), 0),
				Arguments = "-check"
			};

			JumpListTask settings = new JumpListLink(exePath, Locale.Current.Labels.ConfigurationShort) {
				IconReference = new IconReference(Path.Combine(path, "Settings.ico"), 0),
				Arguments = "-settings"
			};

			JumpListTask about = new JumpListLink(exePath, Locale.Current.Labels.About) {
				IconReference = new IconReference(Path.Combine(path, "about.ico"), 0),
				Arguments = "-about"
			};

			_jumpList.AddUserTasks(compose);
			_jumpList.AddUserTasks(inbox);
			_jumpList.AddUserTasks(refresh);
			_jumpList.AddUserTasks(settings);
			_jumpList.AddUserTasks(about);
		}

		private void CheckMail() {
			_statusList.Clear();
			_UnreadTotal = 0;

			foreach (Notifier notifier in _instances.Values) {
				notifier.CheckMail();
			}
		}


		private void CloseInstances() {

			List<string> list = new List<string>();

			foreach (Notifier notifier in _instances.Values) {
				if (_config.Accounts[notifier.AccountIndex] == null) {
					list.Add(notifier.Text);
				}
			}

			foreach (string str in list) {
				TabbedThumbnail thumbnailPreview = _taskbarManager.TabbedThumbnail.GetThumbnailPreview(_instances[str].Handle);
				thumbnailPreview.TabbedThumbnailClosed -= _Preview_TabbedThumbnailClosed;

				_taskbarManager.TabbedThumbnail.RemoveThumbnailPreview(thumbnailPreview);

				this._UnreadTotal -= _instances[str].Unread;

				_instances[str].Close();
			}

			if (_config.Accounts.Count > 0) {
				_taskbarManager.TabbedThumbnail.SetActiveTab(_instances[_config.Accounts[0].FullAddress].Handle);
			}
		}

		private void CreateInstances() {

			for (int i = 0; i < _config.Accounts.Count; i++) {

				Account account = _config.Accounts[i];

				if (_instances.ContainsKey(account.FullAddress)) {
					continue;
				}

				Notifier notifier = new Notifier(i);
				notifier.FormClosed += _Notifier_FormClosed;
				notifier.CheckMailFinished += _Notifier_CheckFinished;

				_instances.Add(account.FullAddress, notifier);

				notifier.Show(this);

				TabbedThumbnail preview = new TabbedThumbnail(base.Handle, notifier.Handle);
				preview.TabbedThumbnailClosed += _Preview_TabbedThumbnailClosed;
				preview.SetWindowIcon(Utilities.ResourceHelper.GetIcon("gmail-classic.ico"));
				preview.Tooltip = String.Empty;
				preview.Title = account.FullAddress;

				_taskbarManager.TabbedThumbnail.AddThumbnailPreview(preview);

			}

			if (_config.Accounts.Count > 0) {
				Account account = _config.Accounts[0];
				_taskbarManager.TabbedThumbnail.SetActiveTab(_instances[account.FullAddress].Handle);
			}
		}

		private void FinalizeChecks() {
			if (_statusList.Count != _instances.Count) {
				return;
			}

			if (_UnreadTotal != _PreviousTotal) {

				UpdateMailsJumpList();

				if (_UnreadTotal > _PreviousTotal) {
					switch (Config.Current.SoundNotification) {
						case SoundNotification.Default:
							SoundHelper.PlayDefaultSound();
							break;

						case SoundNotification.Custom:
							SoundHelper.PlayCustomSound(_config.Sound);
							break;
					}

					if (_config.FlashTaskbar) {
						Utilities.TaskbarHelper.Flash(this, _config.FlashCount);
					}

				}

				_PreviousTotal = _UnreadTotal;
			}
			if (_statusList.ContainsValue(Notifier.NotifierStatus.AuthenticationFailed)) {
				SetWarningOverlay();
			}
			else if (_statusList.ContainsValue(Notifier.NotifierStatus.Offline)) {
				SetOfflineOverlay();
			}
			else {
				SetUnreadOverlay(_UnreadTotal);
			}

		}

		private void SetUnreadOverlay(int count) {

			if (_taskbarManager == null) {
				return;
			}

			CleanupDigitIcon();

			if (count == 0) {
				_taskbarManager.SetOverlayIcon(base.Handle, null, String.Empty);

				HideTrayIcon();
			}
			else {

				int digitsNumber;
				Int32.TryParse(count.ToString("00"), out digitsNumber);

				using (Bitmap numbers = ImageHelper.GetDigitIcon(digitsNumber)) {

					if (numbers == null) {
						_iconDigits = Utilities.ResourceHelper.GetIcon("Warning.ico");
					}
					else {
						_iconDigits = Icon.FromHandle(numbers.GetHicon());
					}

					using (Bitmap trayNumbers = ImageHelper.GetTrayIcon(numbers)) {
						_iconTray = Icon.FromHandle(trayNumbers.GetHicon());
					}

				}

				_taskbarManager.SetOverlayIcon(base.Handle, _iconDigits, String.Empty);

				if (_config.ShowTrayIcon) {
					_TrayIcon.Icon = _iconTray;
					_TrayIcon.Visible = true;
				}

			}
		}

		internal void SetWarningOverlay() {

			if (_taskbarManager != null) {
				_taskbarManager.SetOverlayIcon(base.Handle, Utilities.ResourceHelper.GetIcon(".Warning.ico"), String.Empty);

				CleanupDigitIcon();
				HideTrayIcon();
			}

		}

		internal void SetOfflineOverlay() {

			if (_taskbarManager != null) {
				_taskbarManager.SetOverlayIcon(base.Handle, Utilities.ResourceHelper.GetIcon("Offline.ico"), String.Empty);

				CleanupDigitIcon();
				HideTrayIcon();
			}

		}

		private void CleanupDigitIcon() {
			if (_iconDigits != null) {
				_iconDigits.Dispose();
				_iconDigits = null;
			}
		}

		private void HideTrayIcon() {
			_TrayIcon.Visible = false;

			if (_TrayIcon.Icon != null) {
				_TrayIcon.Icon.Dispose();
			}

			_TrayIcon.Icon = null;
		}

		private void ShowAbout() {
			About about = new About();
			about.Show();
			about.BringToFront();
			about.Focus();
		}

		public void OpenSettingsWindow() {

			if (_preferences == null) {
				_preferences = new Preferences();
				_preferences.FormClosed += _SettingsWindow_FormClosed;
				_preferences.Show();

			}

			if (FirstRun) {
				FirstRun = false;
				_preferences.InitFirstRun();
			}

			_preferences.Activate();
			_preferences.BringToFront();

		}

		internal void RemoteCheckMails() {
			MethodInvoker method = null;

			if (base.InvokeRequired) {
				if (method == null) {
					method = delegate { this.CheckMail(); };
				}
				base.Invoke(method);
			}
		}

		internal void RemoteOpenSettingsWindow() {
			MethodInvoker method = null;

			if (base.InvokeRequired) {
				if (method == null) {
					method = delegate { this.OpenSettingsWindow(); };
				}
				base.Invoke(method);
			}
		}

		internal void RemoteShowAbout() {
			MethodInvoker method = null;

			if (base.InvokeRequired) {
				if (method == null) {
					method = delegate { this.ShowAbout(); };
				}
				base.Invoke(method);
			}
		}

		private void UpdateMailsJumpList() {

			if (!_config.RecentDocsTracked) {
				return;
			}

			_jumpList.RemoveCustomCategories();

			Dictionary<string, List<JumpListLink>> dictionary = new Dictionary<string, List<JumpListLink>>();

			int i = 0;
			int unreadCount = Math.Min(_UnreadTotal, (int)_jumpList.MaxSlotsInList);
			int index = 0;
			int mailCount = 0;

			while (i < unreadCount) {
				String linkText;

				Notifier notifier = _instances[_config.Accounts[index].FullAddress];
				Account account = _config.Accounts[notifier.AccountIndex];

				if (Locale.Current.IsRightToLeftLanguage) {
					linkText = String.Concat("(", notifier.Unread, ") ", account.FullAddress, " ");
				}
				else {
					linkText = String.Concat(account.FullAddress, " (", notifier.Unread, ")");
				}

				if (!dictionary.ContainsKey(linkText)) {
					dictionary.Add(linkText, new List<JumpListLink>());
				}

				if (mailCount < notifier.XmlMail.Count) {
					XmlNode node = notifier.XmlMail[mailCount];
					String innerText = node.ChildNodes.Item(0).InnerText;
					String linkTitle = String.IsNullOrEmpty(innerText) ? Locale.Current.Labels.NoSubject : innerText;
					String linkUrl = UrlHelper.BuildMailUrl(node.ChildNodes.Item(2).Attributes["href"].Value, notifier.AccountIndex);
					String path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources\\Icons");

					JumpListLink item = new JumpListLink(linkUrl, linkTitle) {
						IconReference = new IconReference(Path.Combine(path, "Mail.ico"), 0)
					};

					dictionary[linkText].Add(item);
					i++;
				}

				if (index < (_instances.Count - 1)) {
					index++;
				}
				else {
					index = 0;
					mailCount++;
				}

			}

			foreach (KeyValuePair<string, List<JumpListLink>> pair in dictionary) {
				JumpListCustomCategory category = new JumpListCustomCategory(pair.Key);
				category.AddJumpListItems(pair.Value.ToArray());

				_jumpList.AddCustomCategories(new JumpListCustomCategory[] { category });
			}

			try {
				_jumpList.Refresh();
			}
			catch (Exception e) {
				// https://github.com/shellscape/Gmail-Notifier-Plus/issues/#issue/3
				// Unable to remove files to be replaced. (Exception from HRESULT: 0x80070497)
				Utilities.ErrorHelper.Log(e, Guid.NewGuid());
			}
		}

		#endregion

		#region .    Fix bug in Windows API Code Pack

		// Resolves a bug with Windows API Code Pack v1.0.1
		// ThumbnailToolbarButton Click event isn’t fired when process is running elevated (i.e. as Administrator)
		// Since I run visual studio with elevated privs for debugging IIS attached asp.net apps, this needs to be here.
		// http://blogs.microsoft.co.il/blogs/arik/archive/2010/03.aspx

		// Updated to v1.1 of the code pack. Bug still present.

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr ChangeWindowMessageFilter(uint message, uint dwFlag);

		private const uint MSGFLT_ADD = 1;
		private const uint WM_COMMAND = 0x0111;
		private const uint WM_SYSCOMMAND = 0x112;
		private const uint WM_ACTIVATE = 0x0006;

		/// <summary>
		/// Specifies that the taskbar-related windows messages should pass through the Windows UIPI mechanism even if the process is
		/// running elevated. Calling this method is not required unless the process is running elevated.
		/// </summary>
		private static void AllowTaskbarWindowMessagesThroughUIPI() {
			uint WM_TaskbarButtonCreated = RegisterWindowMessage("TaskbarButtonCreated");

			ChangeWindowMessageFilter(WM_TaskbarButtonCreated, MSGFLT_ADD);
			ChangeWindowMessageFilter(WM_COMMAND, MSGFLT_ADD);
			ChangeWindowMessageFilter(WM_SYSCOMMAND, MSGFLT_ADD);
			ChangeWindowMessageFilter(WM_ACTIVATE, MSGFLT_ADD);
		}

		#endregion

	}

}
