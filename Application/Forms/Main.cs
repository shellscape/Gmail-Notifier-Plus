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

		private Dictionary<string, Notifier> _Instances = new Dictionary<string, Notifier>();
		private JumpList _JumpList;
		private Settings _SettingsWindow;
		private Dictionary<string, Notifier.NotifierStatus> _StatusList = new Dictionary<string, Notifier.NotifierStatus>();
		private TaskbarManager _TaskbarManager = TaskbarManager.Instance;

		private int _PreviousTotal;
		private int _UnreadTotal;

		private Icon _IconDigits = null;
		private Icon _IconWindow = null;

		private Config _Config = Config.Current;

		private static readonly int WM_TASKBARBUTTONCREATED = ((int)RegisterWindowMessage("TaskbarButtonCreated"));

		public Main(string[] args) {

			InitializeComponent();

			this.Location = new Point(-10000, -10000);

			using (System.Drawing.Bitmap windowBitmap = ResourceHelper.GetImage("Envelope.png")) {
				_IconWindow = Icon.FromHandle(windowBitmap.GetHicon());
				this.Icon = _IconWindow;
			}

			this.Text = GmailNotifierPlus.Resources.Resources.WindowTitle;
			this.CreateInstances();

			if ((args.Length > 0) && (args[0] == "-settings")) {
				this.OpenSettingsWindow();
			}

			_Config.Saved += _Config_Saved;

			_Timer.Tick += _Timer_Tick;
			_Timer.Interval = _Config.Interval * 1000;
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
				this._JumpList.RemoveCustomCategories();
				this._JumpList.Refresh();
			}
			catch {
			}
		}

		private void Main_Load(object sender, EventArgs e) {

			AllowTaskbarWindowMessagesThroughUIPI();

			_JumpList = JumpList.CreateJumpListForIndividualWindow(this._TaskbarManager.ApplicationId, base.Handle);
			_JumpList.JumpListItemsRemoved += delegate(object o, UserRemovedJumpListItemsEventArgs ev) { };
			_JumpList.KnownCategoryToDisplay = JumpListKnownCategoryType.Neither;

			BuildJumpList();
		}

		private void Main_Shown(object sender, EventArgs e) {
			if (_Config.FirstRun) {
				this.OpenSettingsWindow();

				_Config.FirstRun = false;
				_Config.Save();

			}

			base.Top = 5000;
		}

		private void _Config_Saved(object sender, EventArgs e) {
			_Timer.Interval = _Config.Interval * 1000;

			this.BuildJumpList();
			this.UpdateMailsJumpList();

			if (_Config.Accounts.Count > _Instances.Count) {
				this.CreateInstances();
			}
			else if (_Config.Accounts.Count < _Instances.Count) {
				this.CloseInstances();
			}

			this.CheckMail();
		}

		private void _Timer_Tick(object sender, EventArgs e) {
			this.CheckMail();
		}

		private void _Notifier_CheckFinished(Notifier sender, EventArgs e) {
			_StatusList.Add(sender.Text, sender.ConnectionStatus);
			_UnreadTotal += sender.Unread;

			FinalizeChecks();
		}

		private void _Notifier_FormClosed(object sender, FormClosedEventArgs e) {

			Notifier notifier = (Notifier)sender;
			notifier.FormClosed -= _Notifier_FormClosed;
			notifier.CheckMailFinished -= _Notifier_CheckFinished;

			_Instances.Remove(notifier.Text);
			_StatusList.Remove(notifier.Text);

			notifier.Dispose();
			notifier = null;

			this.FinalizeChecks();
			if (_Instances.Count == 0) {
				base.Close();
			}
			else {
				_TaskbarManager.TabbedThumbnail.SetActiveTab(_Instances[_Config.Accounts[0].FullAddress].Handle);
			}
		}

		private void _Preview_TabbedThumbnailClosed(object sender, TabbedThumbnailEventArgs e) {
			(Control.FromHandle(e.WindowHandle) as Form).Close();
		}

		private void _SettingsWindow_FormClosed(object sender, FormClosedEventArgs e) {
			_SettingsWindow.Dispose();
			_SettingsWindow = null;
		}

		#endregion

		#region .    Private Methods

		private void BuildJumpList() {

			_JumpList.ClearAllUserTasks();

			int defaultAccountIndex = _Config.Accounts.IndexOf(_Config.Accounts.Default);
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

			_JumpList.AddUserTasks(compose);
			_JumpList.AddUserTasks(inbox);
			_JumpList.AddUserTasks(refresh);
			_JumpList.AddUserTasks(settings);
		}

		private void CheckMail() {
			_StatusList.Clear();
			_UnreadTotal = 0;

			foreach (Notifier notifier in _Instances.Values) {
				notifier.CheckMail();
			}
		}


		private void CloseInstances() {

			List<string> list = new List<string>();

			foreach (Notifier notifier in _Instances.Values) {
				if (_Config.Accounts[notifier.AccountIndex] == null) {
					list.Add(notifier.Text);
				}
			}

			foreach (string str in list) {
				TabbedThumbnail thumbnailPreview = _TaskbarManager.TabbedThumbnail.GetThumbnailPreview(_Instances[str].Handle);
				thumbnailPreview.TabbedThumbnailClosed -= _Preview_TabbedThumbnailClosed;

				_TaskbarManager.TabbedThumbnail.RemoveThumbnailPreview(thumbnailPreview);
				_Instances[str].Close();
			}

			if (_Config.Accounts.Count > 0) {
				_TaskbarManager.TabbedThumbnail.SetActiveTab(_Instances[_Config.Accounts[0].FullAddress].Handle);
			}
		}

		private void CreateInstances() {

			for (int i = 0; i < _Config.Accounts.Count; i++) {

				Account account = _Config.Accounts[i];

				if (_Instances.ContainsKey(account.FullAddress)) {
					continue;
				}

				Notifier notifier = new Notifier(i);
				notifier.FormClosed += _Notifier_FormClosed;
				notifier.CheckMailFinished += _Notifier_CheckFinished;

				_Instances.Add(account.FullAddress, notifier);

				notifier.Show(this);

				TabbedThumbnail preview = new TabbedThumbnail(base.Handle, notifier.Handle);
				preview.TabbedThumbnailClosed += _Preview_TabbedThumbnailClosed;
				preview.SetWindowIcon(Utilities.ResourceHelper.GetIcon("Default.ico"));
				preview.Tooltip = String.Empty;
				preview.Title = account.FullAddress;

				_TaskbarManager.TabbedThumbnail.AddThumbnailPreview(preview);

			}

			if (_Config.Accounts.Count > 0) {
				Account account = _Config.Accounts[0];
				_TaskbarManager.TabbedThumbnail.SetActiveTab(_Instances[account.FullAddress].Handle);
			}
		}

		private void FinalizeChecks() {
			if (_StatusList.Count != _Instances.Count) {
				return;
			}

			if (_UnreadTotal != _PreviousTotal) {

				UpdateMailsJumpList();

				if (_UnreadTotal > _PreviousTotal) {
					switch (Config.Current.SoundNotification) {
						case 1:
							SoundHelper.PlayDefaultSound();
							break;

						case 2:
							SoundHelper.PlayCustomSound(_Config.Sound);
							break;
					}
				}

				_PreviousTotal = _UnreadTotal;
			}
			if (_StatusList.ContainsValue(Notifier.NotifierStatus.AuthenticationFailed)) {
				SetWarningOverlay();
			}
			else if (_StatusList.ContainsValue(Notifier.NotifierStatus.Offline)) {
				SetOfflineOverlay();
			}
			else {
				SetUnreadOverlay(_UnreadTotal);
			}

		}

		private void SetUnreadOverlay(int count) {

			_TaskbarManager.SetOverlayIcon(base.Handle, null, String.Empty);

			if (count == 0) {
				//_TaskbarManager.SetOverlayIcon(base.Handle, null, string.Empty);
				this.Icon = _IconWindow;
			}
			else {

				if (_IconDigits != null) {
					_IconDigits.Dispose();
					_IconDigits = null;
				}

				int digitsNumber;
				Int32.TryParse(count.ToString("00"), out digitsNumber);

				using (Bitmap numbers = ImageHelper.GetDigitIcon(digitsNumber)) {
					_IconDigits = Icon.FromHandle(numbers.GetHicon());
				}
				
				//_TaskbarManager.SetOverlayIcon(base.Handle, _IconDigits, string.Empty);
				
				this.Icon = _IconDigits;

			}

			String appName = GmailNotifierPlus.Resources.Resources.WindowTitle;
			String appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			String path = System.IO.Path.Combine(appData, appName, String.Concat(appName, ".ico"));

			using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write)){
				_IconDigits.Save(fs);
			}

		}

		internal void SetWarningOverlay() {
			_TaskbarManager.SetOverlayIcon(base.Handle, Utilities.ResourceHelper.GetIcon(".Warning.ico"), string.Empty);
		}

		internal void SetOfflineOverlay() {
			_TaskbarManager.SetOverlayIcon(base.Handle, Utilities.ResourceHelper.GetIcon("Offline.ico"), string.Empty);
		}

		private void OpenSettingsWindow() {

			if (_SettingsWindow == null) {
				_SettingsWindow = new Settings();
				_SettingsWindow.FormClosed += _SettingsWindow_FormClosed;
				_SettingsWindow.Show(this);
			}

			_SettingsWindow.Activate();
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

		private void UpdateMailsJumpList() {

			_JumpList.RemoveCustomCategories();

			Dictionary<string, List<JumpListLink>> dictionary = new Dictionary<string, List<JumpListLink>>();

			int i = 0;
			int unreadCount = Math.Min(_UnreadTotal, (int)_JumpList.MaxSlotsInList);
			int index = 0;
			int mailCount = 0;

			while (i < unreadCount) {
				String linkText;

				Notifier notifier = _Instances[_Config.Accounts[index].FullAddress];
				Account account = _Config.Accounts[notifier.AccountIndex];

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

				if (index < (_Instances.Count - 1)) {
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

				_JumpList.AddCustomCategories(new JumpListCustomCategory[] { category });
			}

			_JumpList.Refresh();
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
