using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Shell;
using System.Xml;

using Microsoft.WindowsAPI.Taskbar;

using GmailNotifierPlus.Localization;
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

		public Main() {

			InitializeComponent();

			_iconWindow = ResourceHelper.GetIcon("gmail-classic.ico");

			// since we're using the native jumplist stuff, we really shouldn't need this. we'll see.
			//_taskbarManager.ApplicationId = String.Concat("Gmail-Notifier-Plus-", Shellscape.Utilities.AssemblyMeta.Guid, "-", Shellscape.Utilities.AssemblyMeta.Version);

			_jumpList = new JumpList(); //JumpList.GetJumpList(System.Windows.Application.Current);
			_jumpList.ShowFrequentCategory = false;
			_jumpList.ShowRecentCategory = false;

			var app = new System.Windows.Application();

			JumpList.SetJumpList(app, _jumpList);
			TaskbarItemInfo info = new TaskbarItemInfo();

			info.ProgressState = TaskbarItemProgressState.Indeterminate;
			info.ProgressValue = 20;


			this.Icon = _iconWindow;
			this.Location = new Point(-10000, -10000);
			this.Text = this._TrayIcon.Text = GmailNotifierPlus.Resources.WindowTitle;

			this.CreateInstances();

			_config.Saved += _Config_Saved;
			_config.Accounts.AccountChanged += _Account_Changed;

			_Timer.Tick += _Timer_Tick;
			_Timer.Interval = Math.Max(1, _config.Interval) * 1000;
			_Timer.Enabled = true;
		}

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

				_jumpList.RemoveCustomCategories();
				_jumpList.Apply(); //this._jumpList.Refresh();
			}
			catch {
			}
		}

		private void Main_Load(object sender, EventArgs e) {

			AllowTaskbarWindowMessagesThroughUIPI();

			InitJumpList();
		}

		private void Main_Shown(object sender, EventArgs e) {
			if (_config.FirstRun) {

				FirstRun firstRun = new FirstRun();
				firstRun.Show();
				firstRun.BringToFront();
				firstRun.Focus();

				_config.FirstRun = false;
				_config.Save();

			}

			base.Top = 5000;
		}

		private void _Account_Changed(Account account) {

			Notifier notifier = _instances.Values.Where(o => o.Account.Guid == account.Guid).FirstOrDefault();

			if (notifier != null) {
				notifier.Text = account.FullAddress;

				TabbedThumbnail thumb = _taskbarManager.TabbedThumbnail.GetThumbnailPreview(notifier.Handle);

				if (thumb != null) {
					thumb.Title = account.FullAddress;
				}
			}

			this.InitJumpList();
			this.UpdateJumpList();
			this.CheckMail();
		}

		private void _Config_Saved(object sender, EventArgs e) {
			_Timer.Interval = Math.Max(1, _config.Interval) * 1000;

			if (_config.Accounts.Count != _instances.Count) {
				if (_config.Accounts.Count < _instances.Count) {
					this.CloseInstances();
				}
				else if (_config.Accounts.Count > _instances.Count) {
					this.CreateInstances();
				}

				this.InitJumpList();
				this.UpdateJumpList();
				this.CheckMail();
			}

			if (_config.ShowTrayIcon) {
				_TrayIcon.Icon = _iconTray;
			}

			_TrayIcon.Visible = Config.Current.ShowTrayIcon;

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

			if (_instances.ContainsKey(notifier.Account.Guid)) {
				_instances.Remove(notifier.Account.Guid);
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
				_taskbarManager.TabbedThumbnail.SetActiveTab(_instances[_config.Accounts[0].Guid].Handle);
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

		/// <summary>
		/// Initializes the jumplist with the default set of items.
		/// </summary>
		private void InitJumpList() {

			int defaultAccountIndex = _config.Accounts.IndexOf(_config.Accounts.Default);

			String exePath = Application.ExecutablePath;
			String iconsPath = Path.Combine(Path.GetDirectoryName(exePath), "Resources\\Icons");
			String browserPath = UrlHelper.GetDefaultBrowserPath();
			String url = UrlHelper.BuildInboxUrl(defaultAccountIndex);
			String categoryName = Locale.Current.JumpList.DefaultAccount;

			_jumpList.JumpItems.Clear();

			_jumpList.JumpItems.Add(new JumpTask() {
				ApplicationPath = String.IsNullOrEmpty(browserPath) ? url : browserPath,
				Arguments = String.IsNullOrEmpty(browserPath) ? String.Empty : url,
				IconResourceIndex = 0,
				IconResourcePath = Path.Combine(iconsPath, "GoInbox.ico"), // there's a really whacky conflict between an embedded resource, and a content resource file name.
				Title = Locale.Current.JumpList.Inbox,
				CustomCategory = categoryName
			});

			url = UrlHelper.BuildComposeUrl(defaultAccountIndex);

			_jumpList.JumpItems.Add(new JumpTask() {
				ApplicationPath = String.IsNullOrEmpty(browserPath) ? url : browserPath,
				Arguments = String.IsNullOrEmpty(browserPath) ? String.Empty : url,
				IconResourceIndex = 0,
				IconResourcePath = Path.Combine(iconsPath, "Compose.ico"),
				Title = Locale.Current.JumpList.Compose,
				CustomCategory = categoryName
			});

			// general tasks

			_jumpList.JumpItems.Add(new JumpTask() {
				ApplicationPath = exePath,
				Arguments = "-check",
				IconResourceIndex = 0,
				IconResourcePath = Path.Combine(iconsPath, "Refresh.ico"),
				Title = Locale.Current.JumpList.Check
			});

			_jumpList.JumpItems.Add(new JumpTask() {
				ApplicationPath = exePath,
				Arguments = "-settings",
				IconResourceIndex = 0,
				IconResourcePath = Path.Combine(iconsPath, "Settings.ico"),
				Title = Locale.Current.JumpList.Preferences
			});

			_jumpList.JumpItems.Add(new JumpTask() {
				ApplicationPath = exePath,
				Arguments = "-help",
				IconResourceIndex = 0,
				IconResourcePath = Path.Combine(iconsPath, "help.ico"),
				Title = Locale.Current.JumpList.Help
			});

			_jumpList.JumpItems.Add(new JumpTask() {
				ApplicationPath = exePath,
				Arguments = "-about",
				IconResourceIndex = 0,
				IconResourcePath = Path.Combine(iconsPath, "about.ico"),
				Title = Locale.Current.JumpList.About
			});

			_jumpList.Apply();
		}

		/// <summary>
		/// Updates the portion of the JumpList which shows new/unread email.
		/// </summary>
		private void UpdateJumpList() {

			if (!_config.RecentDocsTracked) { // if the user doesn't have recent docs turned on, this will method throw errors.
				return;
			}

			_jumpList.RemoveCustomCategories();

			String iconPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources\\Icons\\Mail.ico");
			String inboxFormat = "{0} ({1})";

			foreach (Notifier notifier in _instances.Values) {

				Account account = notifier.Account;
				String category = String.Format(inboxFormat, account.FullAddress, notifier.Unread);

				foreach (Email email in notifier.Emails) {
					JumpTask task = new JumpTask() {
						ApplicationPath = String.Empty,
						IconResourceIndex = 0,
						IconResourcePath = iconPath,
						Title = email.Title,
						CustomCategory = category
					};

					_jumpList.JumpItems.Add(task);
				}

			}

			_jumpList.Apply(); // refreshes

		}
		
		private void CheckMail() {
			_statusList.Clear();
			_UnreadTotal = 0;

			foreach (Notifier notifier in _instances.Values) {
				notifier.CheckMail();
			}
		}

		private void CloseInstances() {

			foreach (String key in _instances.Keys.ToList()) {

				if (_config.Accounts.Where(o => o.Guid == key).Count() > 0) {
					continue;
				}

				Notifier form = _instances[key];
				TabbedThumbnail thumbnailPreview = _taskbarManager.TabbedThumbnail.GetThumbnailPreview(form.Handle);

				thumbnailPreview.TabbedThumbnailClosed -= _Preview_TabbedThumbnailClosed;

				_taskbarManager.TabbedThumbnail.RemoveThumbnailPreview(thumbnailPreview);

				this._UnreadTotal -= form.Unread;

				_instances.Remove(key);

				form.Close();
			}

			if (_config.Accounts.Count > 0) {
				_taskbarManager.TabbedThumbnail.SetActiveTab(_instances[_config.Accounts[0].Guid].Handle);
			}
		}

		private void CreateInstances() {

			for (int i = 0; i < _config.Accounts.Count; i++) {

				Account account = _config.Accounts[i];

				if (_instances.ContainsKey(account.Guid)) {
					continue;
				}

				Notifier notifier = new Notifier(account);
				notifier.FormClosed += _Notifier_FormClosed;
				notifier.CheckMailFinished += _Notifier_CheckFinished;

				_instances.Add(account.Guid, notifier);

				notifier.Show(this);

				TabbedThumbnail preview = new TabbedThumbnail(base.Handle, notifier.Handle);
				preview.TabbedThumbnailClosed += _Preview_TabbedThumbnailClosed;
				preview.SetWindowIcon(Utilities.ResourceHelper.GetIcon("gmail-classic.ico"));
				preview.Tooltip = String.Empty;
				preview.Title = account.FullAddress;

				notifier.PreviewThumbnail = preview;

				_taskbarManager.TabbedThumbnail.AddThumbnailPreview(preview);
			}

			if (_config.Accounts.Count > 0) {
				Account account = _config.Accounts[0];
				_taskbarManager.TabbedThumbnail.SetActiveTab(_instances[account.Guid].Handle);
			}
		}

		private void FinalizeChecks() {

			if (_statusList.Count != _instances.Count) {
				return;
			}

			if (_UnreadTotal != _PreviousTotal) {

				UpdateJumpList();

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

				try {
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
				catch (System.Runtime.InteropServices.ExternalException) {
					SetWarningOverlay();
				}

			}
		}

		internal void SetWarningOverlay() {

			if (_taskbarManager != null) {
				_taskbarManager.SetOverlayIcon(base.Handle, Utilities.ResourceHelper.GetIcon("Warning.ico"), String.Empty);

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
			else {
				this.ShowAbout();
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
