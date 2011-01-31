using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
		private JumpList _JumpListManager;
		private int _PreviousTotal;
		private Settings _SettingsWindow;
		private Dictionary<string, Notifier.Status> _StatusList = new Dictionary<string, Notifier.Status>();
		private TaskbarManager _TaskbarManager = TaskbarManager.Instance;
		private int _UnreadTotal;
		private Icon _IconDigits = null;

		private Config _Config = Config.Current;

		private static readonly int WM_TASKBARBUTTONCREATED = ((int)RegisterWindowMessage("TaskbarButtonCreated"));

		public Main(string[] args) {
			InitializeComponent();

			this.Location = new Point(-10000, -10000);
			Bitmap windowBitmap = ResourceHelper.GetImage("Envelope.png");
			Icon windowIcon = Icon.FromHandle(windowBitmap.GetHicon());

			this.Icon = windowIcon;

			windowBitmap.Dispose();
			windowBitmap = null;

			this.Text = GmailNotifierPlus.Resources.Resources.WindowTitle;
			this.CreateInstances();

			if ((args.Length > 0) && (args[0] == "-settings")) {
				this.OpenSettingsWindow();
			}

			_Config.Saved += _Config_Saved;

			_Timer.Tick += _Timer_Tick;
			_Timer.Interval = _Config.Interval * 1000;
			_Timer.Enabled = true;

			SetUnreadOverlay(99);
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
				this._JumpListManager.ClearAllCustomCategories();
				this._JumpListManager.Refresh();
			}
			catch {
			}
		}

		private void Main_Load(object sender, EventArgs e) {
			_JumpListManager = JumpList.CreateJumpListForIndividualWindow(this._TaskbarManager.ApplicationId, base.Handle);
			_JumpListManager.JumpListItemsRemoved += delegate(object o, UserRemovedJumpListItemsEventArgs ev) { };
			_JumpListManager.KnownCategoryToDisplay = JumpListKnownCategoryType.Neither;
			
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

			int defaultAccountIndex = _Config.Accounts.IndexOf(_Config.Accounts.Default);

			_JumpListManager.ClearAllUserTasks();
			IJumpListTask[] taskCompose = new IJumpListTask[1];
			JumpListLink linkCompose = new JumpListLink(UrlHelper.BuildComposeUrl(defaultAccountIndex), Locale.Current.Labels.Compose);
			linkCompose.IconReference = new IconReference(Application.ExecutablePath, 1);
			taskCompose[0] = linkCompose;
			
			IJumpListTask[] taskInbox = new IJumpListTask[1];
			JumpListLink linkInbox = new JumpListLink(UrlHelper.BuildInboxUrl(defaultAccountIndex), Locale.Current.Labels.Inbox);
			linkInbox.IconReference = new IconReference(Application.ExecutablePath, 2);
			taskInbox[0] = linkInbox;

			IJumpListTask[] taskCheck = new IJumpListTask[1];
			JumpListLink linkCheck = new JumpListLink(Application.ExecutablePath, Locale.Current.Labels.CheckMail);
			linkCheck.Arguments = "-check";
			linkCheck.IconReference = new IconReference(Application.ExecutablePath, 4);
			taskCheck[0] = linkCheck;
			
			IJumpListTask[] taskConfig = new IJumpListTask[1];
			JumpListLink linkConfig = new JumpListLink(Application.ExecutablePath, Locale.Current.Labels.ConfigurationShort);
			linkConfig.Arguments = "-settings";
			linkConfig.IconReference = new IconReference(Application.ExecutablePath, 5);
			taskConfig[0] = linkConfig;

			try {
				this._JumpListManager.AddUserTasks(taskCompose);
				this._JumpListManager.AddUserTasks(taskInbox);
				this._JumpListManager.AddUserTasks(taskCheck);
				this._JumpListManager.AddUserTasks(taskConfig);

				this._JumpListManager.Refresh();
			}
			catch { }
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
					switch (1) { // TODO - config.SoundNotification) {
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
			if (_StatusList.ContainsValue(Notifier.Status.AuthenticationFailed)) {
				SetWarningOverlay();
			}
			else if (_StatusList.ContainsValue(Notifier.Status.Offline)) {
				SetOfflineOverlay();
			}
			else {
				SetUnreadOverlay(_UnreadTotal);
			}

		}

		private void SetUnreadOverlay(int count) {
			//if (count == 0) {
			//	_TaskbarManager.SetOverlayIcon(base.Handle, null, string.Empty);
			//}
			//else {
				//count = Math.Min(count, 99);
				if (_IconDigits != null) {
					_IconDigits.Dispose();
					_IconDigits = null;
				}

				int digitsNumber;
				Int32.TryParse(count.ToString("00"), out digitsNumber);

				_IconDigits = ImageHelper.GetDigitIcon(digitsNumber);

			using(System.IO.FileStream fs = new System.IO.FileStream("icon.ico",  System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite)){
				_IconDigits.Save(fs);
			}

				_TaskbarManager.SetOverlayIcon(base.Handle, _IconDigits, string.Empty);
			//}
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
			
			try {
				_JumpListManager.ClearAllCustomCategories();
			}
			catch { }
			
			Dictionary<string, List<JumpListLink>> dictionary = new Dictionary<string, List<JumpListLink>>();
			
			int num = 0;
			int num2 = Math.Min(_UnreadTotal, (int)_JumpListManager.MaxSlotsInList);
			int index = 0;
			int num4 = 0;

			while (num < num2) {
				string str;

				Notifier notifier = _Instances[_Config.Accounts[index].FullAddress];
				Account account = _Config.Accounts[notifier.AccountIndex];

				if (Locale.Current.IsRightToLeftLanguage) {
					str = string.Concat(new object[] { "(", notifier.Unread, ") ", account.FullAddress, " " });
				}
				else {
					str = string.Concat(new object[] { account.FullAddress, " (", notifier.Unread, ")" });
				}

				if (!dictionary.ContainsKey(str)) {
					dictionary.Add(str, new List<JumpListLink>());
				}

				if (num4 < notifier.XmlMail.Count) {
					XmlNode node = notifier.XmlMail[num4];
					String innerText = node.ChildNodes.Item(0).InnerText;
					String linkTitle = String.IsNullOrEmpty(innerText) ? Locale.Current.Labels.NoSubject : innerText;
					String linkUrl = UrlHelper.BuildMailUrl(node.ChildNodes.Item(2).Attributes["href"].Value, notifier.AccountIndex);

					JumpListLink item = new JumpListLink(linkUrl, linkTitle);

					item.IconReference = new IconReference(Application.ExecutablePath, 3);
					dictionary[str].Add(item);
					num++;
				}

				if (index < (_Instances.Count - 1)) {
					index++;
				}
				else {
					index = 0;
					num4++;
				}

			}

			foreach (KeyValuePair<string, List<JumpListLink>> pair in dictionary) {
				JumpListCustomCategory category = new JumpListCustomCategory(pair.Key);
				category.AddJumpListItems(pair.Value.ToArray());

				_JumpListManager.AddCustomCategories(new JumpListCustomCategory[] { category });
			}

			try {
				_JumpListManager.Refresh();
			}
			catch { }
		}


#endregion

	}

}
