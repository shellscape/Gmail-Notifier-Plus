using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using Microsoft.WindowsAPI.Dialogs;

using GmailNotifierPlus.Localization;
using GmailNotifierPlus.Utilities;

namespace GmailNotifierPlus.Forms {

	public partial class Main : Form {

		[DllImport("user32.dll")]
		public static extern uint RegisterWindowMessage(string message);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		extern static bool DestroyIcon(IntPtr handle);

		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		public static extern int SHQueryUserNotificationState(out int qns);

		private const int _UNREAD_MAX = 0x63;

		private Dictionary<string, Notifier> _instances = new Dictionary<string, Notifier>();
		private JumpList _jumpList;
		private Dictionary<string, Notifier.NotifierStatus> _statusList = new Dictionary<string, Notifier.NotifierStatus>();
		private TaskbarManager _taskbarManager = TaskbarManager.Instance;
		private List<ToolStripMenuItem> _trayCategories = new List<ToolStripMenuItem>();

		private int _PreviousTotal;
		private int _UnreadTotal;

		private Config _config = Config.Current;

		private static readonly int WM_TASKBARBUTTONCREATED = ((int)RegisterWindowMessage("TaskbarButtonCreated"));

		private readonly int WM_SETTINGCHANGE = 0x1A;
		private readonly int QUNS_PRESENTATION_MODE = 0x04;
		private readonly int QUNS_ACCEPTS_NOTIFICATIONS = 0x05;
		private bool _presentationMode = false;

		public Main() {

			InitializeComponent();

			_jumpList = new JumpList();
			_jumpList.ShowFrequentCategory = false;
			_jumpList.ShowRecentCategory = false;

			var app = new System.Windows.Application();

			JumpList.SetJumpList(app, _jumpList);

			this.Icon = Resources.Icons.Window;
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(-10000, -10000);
			this.Text = this._TrayIcon.Text = GmailNotifierPlus.Resources.Strings.WindowTitle;

			this.CreateInstances();

			_config.Saved += _Config_Saved;

			_config.LanguageChanged += delegate(Config sender) {
				InitJumpList();
			};

			_config.Accounts.AccountChanged += _Account_Changed;

			_Timer.Tick += _Timer_Tick;
			_Timer.Interval = Math.Max(1, _config.Interval) * 1000;
			_Timer.Enabled = true;

			_TrayIcon.ContextMenuStrip = new ContextMenuStrip();
			_TrayIcon.MouseClick += delegate(object sender, MouseEventArgs e) {
				if(e.Button == MouseButtons.Left) {
					MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
					mi.Invoke(_TrayIcon, null);
				}
			};

			Shellscape.UpdateManager.Current.UpdateAvailable += _Updates_UpdatesAvailable;
			Shellscape.UpdateManager.Current.Start();

		}

		#region .    Overrides

		protected override void OnClosing(CancelEventArgs e) {
			base.OnClosing(e);

			try {
				ImageHelper.Cleanup();

				HideTrayIcon();

				_jumpList.RemoveCustomCategories();
				_jumpList.Apply();
			}
			catch {
			}
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			AllowTaskbarWindowMessagesThroughUIPI();

			InitJumpList();
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);

			if(_config.StartupState == StartupState.Second) {
				Jumplist_ShowAbout(null);

				_config.StartupState = StartupState.Other;
				_config.Save();
			}

			if(_config.StartupState == StartupState.First) {

				FirstRun firstRun = new FirstRun();
				firstRun.Show();
				firstRun.BringToFront();
				firstRun.Focus();

				_config.StartupState = StartupState.Second;
				_config.Save();

			}

			base.Top = 5000;
		}

		protected override bool ShowWithoutActivation {
			get { return true; }
		}

		protected override void WndProc(ref Message m) {
			if(m.Msg == WM_TASKBARBUTTONCREATED) {
				this.CheckMail();
			}

			// monitor the Windows Presentation Mode settings and hide the unread message count and disable mail checking when Presentation Mode is On
			if(m.Msg == WM_SETTINGCHANGE) {
				int state;
				SHQueryUserNotificationState(out state);

				if(state == QUNS_PRESENTATION_MODE) {
					_presentationMode = true;
					SetOfflineOverlay();
				}
				else if(state == QUNS_ACCEPTS_NOTIFICATIONS) {
					_presentationMode = false;
					CheckMail();
				}
			}

			base.WndProc(ref m);
		}

		#endregion

		#region .    Event Handlers

		private void _Account_Changed(Account account) {

			Notifier notifier = _instances.Values.Where(o => o.Account.Guid == account.Guid).FirstOrDefault();

			if(notifier != null) {
				notifier.Text = account.FullAddress;

				using(TabbedThumbnail thumb = _taskbarManager.TabbedThumbnail.GetThumbnailPreview(notifier.Handle)) {

					if(thumb != null) {
						thumb.Title = account.FullAddress;
					}
				}
			}

			this.InitJumpList();
			this.UpdateJumpList();
			this.CheckMail();
		}

		private void _Config_Saved(object sender, EventArgs e) {
			_Timer.Interval = Math.Max(1, _config.Interval) * 1000;

			if(_config.Accounts.Count != _instances.Count) {
				if(_config.Accounts.Count < _instances.Count) {
					this.CloseInstances();
				}
				else if(_config.Accounts.Count > _instances.Count) {
					this.CreateInstances();
				}

				this.InitJumpList();
				this.UpdateJumpList();
				this.CheckMail();
			}

			_TrayIcon.Visible = Config.Current.ShowTrayIcon && _UnreadTotal > 0;

		}

		private void _Timer_Tick(object sender, EventArgs e) {
			this.CheckMail();
		}

		private void _Notifier_CheckFinished(Notifier sender, EventArgs e) {

			if(_statusList.ContainsKey(sender.Text)) {
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

			if(_instances.ContainsKey(notifier.Account.Guid)) {
				_instances.Remove(notifier.Account.Guid);
			}

			if(_statusList.ContainsKey(notifier.Text)) {
				_statusList.Remove(notifier.Text);
			}

			notifier.Dispose();
			notifier = null;

			this.FinalizeChecks();
			if(_instances.Count == 0) {
				base.Close();
			}
			else {
				if(_config.Accounts.Count > 0 && _instances.ContainsKey(_config.Accounts[0].Guid)) {
					_taskbarManager.TabbedThumbnail.SetActiveTab(_instances[_config.Accounts[0].Guid].Handle);
				}
			}
		}

		private void _Preview_TabbedThumbnailClosed(object sender, TabbedThumbnailEventArgs e) {
			(Control.FromHandle(e.WindowHandle) as Form).Close();
		}

		private void _Updates_UpdatesAvailable(Shellscape.UpdateManager sender) {

			sender.Stop();
			sender.Download(Config.Current.AppDataPath, new AsyncCompletedEventHandler(_Updates_DownloadComplete));
		}

		private void _Updates_DownloadComplete(object sender, AsyncCompletedEventArgs e) {

			_config.StartupState = StartupState.Second;
			_config.Save();

			Shellscape.UpdateManager.Current.Replace(Config.Current.AppDataPath, Path.GetDirectoryName(Application.ExecutablePath)); // @"C:\Users\Andrew\AppData\Roaming\Gmail Notifier Plus\Updates\test");

			using(TaskDialog dialog = new TaskDialog() {
				Text = Locale.Current.Updates.Text,
				Icon = TaskDialogStandardIcon.Information
			}) {

				TaskDialogCommandLink yes = new TaskDialogCommandLink("yes", Locale.Current.Updates.YesText, Locale.Current.Updates.YesInstruction);
				yes.Click += delegate(object s, EventArgs ea) {

					EventHandler handler = null;

					handler = delegate(object o, EventArgs args) {
						Application.ApplicationExit -= handler;
						System.Diagnostics.Process.Start(Application.ExecutablePath);
					};

					Application.ApplicationExit += handler;
					Invoke((Action)(() => Application.Exit()));

					dialog.Close();
				};

				TaskDialogCommandLink no = new TaskDialogCommandLink("no", Locale.Current.Updates.NoText, Locale.Current.Updates.NoInstruction);
				no.Click += delegate(object s, EventArgs ea) {
					dialog.Close();
				};

				dialog.Caption = dialog.InstructionText = "Gmail Notifier Plus " + Locale.Current.Updates.WindowTitle;
				dialog.Controls.Add(yes);
				dialog.Controls.Add(no);
				dialog.Show();

			}
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

			_jumpList.ClearTasks();

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

			Shellscape.UI.JumplistTask check = new Shellscape.UI.JumplistTask("-check", Locale.Current.JumpList.Check, "Refresh.ico");
			Shellscape.UI.JumplistTask settings = new Shellscape.UI.JumplistTask("-settings", Locale.Current.JumpList.Preferences, "Settings.ico");
			Shellscape.UI.JumplistTask help = new Shellscape.UI.JumplistTask("-help", Locale.Current.JumpList.Help, "help.ico");
			Shellscape.UI.JumplistTask about = new Shellscape.UI.JumplistTask("-about", Locale.Current.JumpList.About, "about.ico");
			Shellscape.UI.JumplistTask donate = new Shellscape.UI.JumplistTask("-donate", Locale.Current.About.Donate, "donate-paypal.ico");

			check.Click += Jumplist_CheckMail;
			settings.Click += Jumplist_ShowPreferences;
			help.Click += Jumplist_ShowHelp;
			about.Click += Jumplist_ShowAbout;
			donate.Click += LaunchDonation;

			_jumpList.JumpItems.AddRange(new List<JumpItem>() { check, settings, help, about, donate });

			_jumpList.Apply();

			InitTrayList(defaultAccountIndex);
		}

		private void InitTrayList(int defaultAccountIndex) {

			_TrayIcon.ContextMenuStrip.Items.Clear();

			using(Icon iconAbout = Resources.Icons.about)
			using(Icon iconCheck = Resources.Icons.Refresh)
			using(Icon iconHelp = Resources.Icons.help)
			using(Icon iconPrefs = Resources.Icons.Settings)
			using(Icon iconInbox = Resources.Icons.Inbox)
			using(Icon iconCompose = Resources.Icons.Compose) 
			using(Icon iconDonate = Shellscape.Resources.Icons.DonatePaypal) {

				String browserPath = UrlHelper.GetDefaultBrowserPath();

				var accountSeparator = new ToolStripMenuItem(Locale.Current.JumpList.DefaultAccount) { Enabled = false };

				var inbox = new ToolStripMenuItem(Locale.Current.JumpList.Inbox, iconInbox.ToBitmap(), delegate(object sender, EventArgs e) {
					String url = UrlHelper.BuildInboxUrl(defaultAccountIndex);

					using(Process p = new Process()) {

						p.StartInfo.FileName = String.IsNullOrEmpty(browserPath) ? url : browserPath;
						p.StartInfo.Arguments = url;

						p.Start();
					}
				});
				
				var compose = new ToolStripMenuItem(Locale.Current.JumpList.Compose, iconCompose.ToBitmap(), delegate(object sender, EventArgs e) {
					String url = UrlHelper.BuildComposeUrl(defaultAccountIndex);

					using(Process p = new Process()) {

						p.StartInfo.FileName = String.IsNullOrEmpty(browserPath) ? url : browserPath;
						p.StartInfo.Arguments = url;

						p.Start();
					}
				});

				var taskSeparator = new ToolStripMenuItem("Tasks") { Enabled = false };
				var check = new ToolStripMenuItem(Locale.Current.JumpList.Check, iconCheck.ToBitmap(), new EventHandler(CheckMail));
				var settings = new ToolStripMenuItem(Locale.Current.JumpList.Preferences, iconPrefs.ToBitmap(), new EventHandler(ShowPreferences));
				var help = new ToolStripMenuItem(Locale.Current.JumpList.Help, iconHelp.ToBitmap(), new EventHandler(ShowHelp));
				var about = new ToolStripMenuItem(Locale.Current.JumpList.About, iconAbout.ToBitmap(), new EventHandler(ShowAbout));
				var donate = new ToolStripMenuItem(Locale.Current.About.Donate, iconDonate.ToBitmap(), new EventHandler(delegate(object sender, EventArgs e) {
					LaunchDonation(null);
				}));

				_TrayIcon.ContextMenuStrip.Items.AddRange(new ToolStripMenuItem[] { accountSeparator, inbox, compose, taskSeparator, check, settings, help, about, donate });

			}

		}

		private void LaunchDonation(String[] arguments) {
			Shellscape.Utilities.ApplicationHelper.Donate("Gmail%20Notifier%20Plus%20Donation");
		}

		/// <summary>
		/// Updates the portion of the JumpList which shows new/unread email.
		/// </summary>
		internal void UpdateJumpList() {

			UpdateTrayList();

			if(!_config.RecentDocsTracked) { // if the user doesn't have recent docs turned on, this will method throw errors.
				return;
			}

			_jumpList.RemoveCustomCategories();

			String iconPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources\\Icons\\Mail.ico");
			String inboxFormat = "{0} ({1})";

			foreach(Notifier notifier in _instances.Values) {

				Account account = notifier.Account;
				String category = String.Format(inboxFormat, account.FullAddress, notifier.Unread);

				foreach(Email email in notifier.Emails) {

					JumpTask task = new JumpTask() {
						ApplicationPath = account.Browser != null ? account.Browser.Path : email.Url,
						Arguments = email.Url,
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

		internal void UpdateTrayList() {

			foreach(var item in _trayCategories) {
				_TrayIcon.ContextMenuStrip.Items.Remove(item);
				item.Dispose();
			}

			_trayCategories.Clear();

			String inboxFormat = "{0} ({1})";

			foreach(Notifier notifier in _instances.Values) {

				Account account = notifier.Account;
				String category = String.Format(inboxFormat, account.FullAddress, notifier.Unread);
				Bitmap icon = null;

				using(Icon i = Resources.Icons.Mail){
					icon = i.ToBitmap();
				}

				if(notifier.Emails.Count > 0) {
					_trayCategories.Add(new ToolStripMenuItem() { Text = category, Enabled = false });
				}

				foreach(Email email in notifier.Emails) {

					var menuitem = new ToolStripMenuItem(email.Title.LimitElipses(30), icon, delegate(object sender, EventArgs e) {
						List<String> data = (sender as ToolStripMenuItem).Tag as List<String>;
						String browserPath = data[0];
						String url = data[1];
						using(Process p = new Process()){

							p.StartInfo.FileName = String.IsNullOrEmpty(browserPath) ? url : browserPath;
							p.StartInfo.Arguments = url;

							p.Start();
						}
					});

					menuitem.ToolTipText = email.Title;
					menuitem.Tag = new List<String>() { account.Browser == null ? String.Empty : account.Browser.Path, email.Url };

					_trayCategories.Add(menuitem);
				}

			}

			_trayCategories.Reverse();

			foreach(var item in _trayCategories){
				_TrayIcon.ContextMenuStrip.Items.Insert(0, item);
			}
		}

		internal void CheckMail() {
			if (!_presentationMode) {
				_statusList.Clear();
				_UnreadTotal = 0;

				foreach(Notifier notifier in _instances.Values) {
					notifier.CheckMail();
				}
			}
		}

		private void CloseInstances() {

			foreach(String key in _instances.Keys.ToList()) {

				if(_config.Accounts.Where(o => o.Guid == key).Count() > 0) {
					continue;
				}

				Notifier form = _instances[key];

				using(TabbedThumbnail thumbnailPreview = _taskbarManager.TabbedThumbnail.GetThumbnailPreview(form.Handle)) {

					thumbnailPreview.TabbedThumbnailClosed -= _Preview_TabbedThumbnailClosed;

					_taskbarManager.TabbedThumbnail.RemoveThumbnailPreview(thumbnailPreview);
				}

				this._UnreadTotal -= form.Unread;

				_instances.Remove(key);

				form.Close();
			}

			if(_config.Accounts.Count > 0) {
				_taskbarManager.TabbedThumbnail.SetActiveTab(_instances[_config.Accounts[0].Guid].Handle);
			}
		}

		private void CreateInstances() {

			for(int i = 0; i < _config.Accounts.Count; i++) {

				Account account = _config.Accounts[i];

				if(_instances.ContainsKey(account.Guid)) {
					continue;
				}

				Notifier notifier = new Notifier(account);
				notifier.FormClosed += _Notifier_FormClosed;
				notifier.CheckMailFinished += _Notifier_CheckFinished;

				_instances.Add(account.Guid, notifier);

				notifier.Show(this);

				TabbedThumbnail preview = new TabbedThumbnail(base.Handle, notifier.Handle);
				preview.TabbedThumbnailClosed += _Preview_TabbedThumbnailClosed;
				preview.SetWindowIcon(Resources.Icons.Window);
				preview.Tooltip = String.Empty;
				preview.Title = account.FullAddress;

				notifier.PreviewThumbnail = preview;

				_taskbarManager.TabbedThumbnail.AddThumbnailPreview(preview);
			}

			if(_config.Accounts.Count > 0) {
				Account account = _config.Accounts[0];
				_taskbarManager.TabbedThumbnail.SetActiveTab(_instances[account.Guid].Handle);
			}
		}

		delegate bool FocusDelegate();

		private void FinalizeChecks() {

			if(_statusList.Count != _instances.Count) {
				return;
			}

			if(_UnreadTotal != _PreviousTotal) {

				UpdateJumpList();

				if(_UnreadTotal > _PreviousTotal && !_presentationMode) {
					switch(Config.Current.SoundNotification) {
						case SoundNotification.Default:
							SoundHelper.PlayDefaultSound();
							break;

						case SoundNotification.Custom:
							SoundHelper.PlayCustomSound(_config.Sound);
							break;
					}

					if(_config.FlashTaskbar) {
						Utilities.TaskbarHelper.Flash(this, _config.FlashCount);

						// this will fix #97, but it'll also steal focus. Not sure that's the best solution.
						//System.Threading.Timer timer = null;
						//timer = new System.Threading.Timer(delegate(object state) {
						//  this.Invoke(new FocusDelegate(Focus));
						//  timer.Dispose();
						//}, this, _config.FlashCount * 1000, 1000);
					}

				}

				_PreviousTotal = _UnreadTotal;
			}

			if(_statusList.ContainsValue(Notifier.NotifierStatus.AuthenticationFailed)) {
				SetWarningOverlay();
			}
			else if(_statusList.ContainsValue(Notifier.NotifierStatus.Offline)) {
				SetOfflineOverlay();
			}
			else {
				SetUnreadOverlay(_UnreadTotal);
			}

		}

		private void SetUnreadOverlay(int count) {

			if(_taskbarManager == null) {
				return;
			}

			//CleanupDigitIcon();

			if(count == 0) {
				_taskbarManager.SetOverlayIcon(base.Handle, null, String.Empty);

				HideTrayIcon();
			}
			else {

				try {
					int unreadNumber;
					Icon overlay;

					Int32.TryParse(count.ToString("00"), out unreadNumber);
					overlay = ImageHelper.GetNumberIcon(unreadNumber);

					if(overlay == null) {
						overlay = Resources.Icons.NewFallback;
					}

					_taskbarManager.SetOverlayIcon(base.Handle, overlay, String.Empty);

					_TrayIcon.Icon = overlay;
					_TrayIcon.Text = String.Concat(GmailNotifierPlus.Resources.Strings.WindowTitle, " (Unread: ", unreadNumber.ToString(), ")");

					if(_config.ShowTrayIcon) {
						_TrayIcon.Visible = true;
					}
				}
				catch(System.Runtime.InteropServices.ExternalException) {
					SetWarningOverlay();
				}

			}
		}

		internal void SetWarningOverlay() {

			if(_taskbarManager != null) {
				_taskbarManager.SetOverlayIcon(base.Handle, Resources.Icons.Warning, String.Empty);
			}

		}

		internal void SetOfflineOverlay() {

			if(_taskbarManager != null) {
				_taskbarManager.SetOverlayIcon(base.Handle, Resources.Icons.Offline, String.Empty);
			}

		}

		private void HideTrayIcon() {
			_TrayIcon.Visible = false;
			_TrayIcon.Icon = null;
		}

		#endregion

		#region .    Jumplist Handling

		public void CheckMail(object sender, EventArgs e) {
			Jumplist_CheckMail(null);
		}

		public void Jumplist_CheckMail(String[] arguments) {

			MethodInvoker method = delegate { this.CheckMail(); };

			if(this.InvokeRequired) {
				this.Invoke(method);
			}
			else {
				method();
			}
		}

		public void ShowPreferences(object sender, EventArgs e) {
			Jumplist_ShowPreferences(null);
		}

		public void Jumplist_ShowPreferences(String[] arguments) {

			Preferences prefs = Shellscape.Program.FindForm(typeof(Preferences)) as Preferences;
			String arg = String.Empty;

			if(arguments != null && arguments.Length >= 1) {
				arg = arguments[0];
			}

			if(prefs != null) {
				if(arg == "refresh") {
					prefs.Close();
					prefs.Dispose();
					prefs = null;
				}
			}

			if(prefs == null) {
				prefs = new Preferences();
			}

			MethodInvoker method = delegate() { // yes, all this ugly is necessary.

				if(arg == "firstrun") {
					prefs.InitFirstRun();
				}
				else if(arg == "account") { // a quick hack to show an account from the thumbnail preview without rocking the boat here.
					prefs.ShowAccount(arguments[1]);
				}

				prefs.Show();
				prefs.TopMost = true;
				prefs.BringToFront();
				prefs.Focus();
				prefs.TopMost = false;
			};

			if(prefs.InvokeRequired) {
				prefs.Invoke(method);
			}
			else {
				method();
			}
		}

		public void ShowAbout(object sender, EventArgs e) {
			Jumplist_ShowAbout(null);
		}

		public void Jumplist_ShowAbout(String[] arguments) {

			About about = Shellscape.Program.FindForm(typeof(About)) as About ?? new About();

			MethodInvoker method = delegate() { // yes, all this ugly is necessary.
				about.Show();
				about.TopMost = true;
				about.BringToFront();
				about.Focus();
				about.TopMost = false;
			};

			if(about.InvokeRequired) {
				about.Invoke(method);
			}
			else {
				method();
			}
		}

		public void ShowHelp(object sender, EventArgs e) {
			Jumplist_ShowHelp(null);
		}

		public void Jumplist_ShowHelp(String[] arguments) {
			Utilities.UrlHelper.Launch(null, "https://github.com/shellscape/Gmail-Notifier-Plus/wiki");
		}

		#endregion

		#region .    Remote Methods

		public static void Remote_Mailto(String[] arguments) {

			String mailto = arguments[0];
			Account mailtoAccount = Config.Current.Accounts.Where(o => o.HandlesMailto).FirstOrDefault();

			if(mailtoAccount == null) {
				return;
			}

			//Uri mailtoUri = new Uri(mailto);
			//System.Collections.Specialized.NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(mailtoUri.Query);
			Shellscape.Browser browser = mailtoAccount.Browser ?? Shellscape.Utilities.BrowserHelper.DefaultBrowser;
			String accountUrl = Utilities.UrlHelper.GetBaseUrl(mailtoAccount);

			//String to = String.Concat(mailtoUri.UserInfo, "@", mailtoUri.Host);
			//String parameters = String.Format("to={1}&su={2}&body={3}",  to, queryString["subject"], queryString["body"]);
			String url = String.Concat(accountUrl, "?extsrc=mailto&url=", System.Web.HttpUtility.UrlEncode(mailto));

			try {
				using(System.Diagnostics.Process process = new System.Diagnostics.Process()) {
					process.StartInfo.Arguments = url;
					process.StartInfo.FileName = browser.Path;
					process.Start();
				}
			}
			catch(Exception e) {
				Utilities.ErrorHelper.Report(e);
			} // catch-all is fine here, not a critical function
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
