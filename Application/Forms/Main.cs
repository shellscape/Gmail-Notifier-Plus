namespace GmailNotifierPlus {
	
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
	
	public class Main : Form {

		[DllImport("user32.dll")]
		public static extern uint RegisterWindowMessage(string message);

		private const int _UNREAD_MAX = 0x63;
		private IContainer components;
		private Dictionary<string, Notifier> instanceList = new Dictionary<string, Notifier>();
		private JumpList jumpListManager;
		private PictureBox pic_background;
		private int previousTotal;
		private ResourceManager resourceManager;
		private Settings settingsWindow;
		private Dictionary<string, Notifier.Status> statusList = new Dictionary<string, Notifier.Status>();
		private TaskbarManager taskbarManager = TaskbarManager.Instance;
		private Timer tmr_check;
		private int unreadTotal;

		private Config config = Config.Current;

		private static readonly int WM_TASKBARBUTTONCREATED = ((int)RegisterWindowMessage("TaskbarButtonCreated"));

		public Main(string[] args) {

			this.InitializeComponent();
			this.Text = GmailNotifierPlus.Resources.Resources.WindowTitle;
			this.resourceManager = new ResourceManager("GmailNotifierPlus.Properties." + config.Language, Assembly.GetExecutingAssembly());
			config.Saved += new ConfigSavedEventHandler(config_Saved);
			this.CreateInstances();
			this.tmr_check.Interval = config.Interval * 0x3e8;
			this.tmr_check.Enabled = true;
			this.pic_background.Image = Utilities.ResourceHelper.GetResourceImage("GmailNotifierPlus.Images.Main.png");
			
			if ((args.Length > 0) && (args[0] == "-settings")) {
				this.OpenSettingsWindow();
			}

		}

		private void BuildJumpList() {

			int defaultAccountIndex = config.Accounts.IndexOf(config.DefaultAccount);

			this.jumpListManager.ClearAllUserTasks();
			IJumpListTask[] tasks = new IJumpListTask[1];
			JumpListLink link = new JumpListLink(UrlHelper.BuildComposeUrl(defaultAccountIndex), this.resourceManager.GetString("Label_Compose"));
			link.IconReference = new IconReference(Application.ExecutablePath, 1);
			tasks[0] = link;
			this.jumpListManager.AddUserTasks(tasks);
			IJumpListTask[] taskArray2 = new IJumpListTask[1];
			JumpListLink link2 = new JumpListLink(UrlHelper.BuildInboxUrl(defaultAccountIndex), this.resourceManager.GetString("Label_Inbox"));
			link2.IconReference = new IconReference(Application.ExecutablePath, 2);
			taskArray2[0] = link2;
			this.jumpListManager.AddUserTasks(taskArray2);
			IJumpListTask[] taskArray3 = new IJumpListTask[1];
			JumpListLink link3 = new JumpListLink(Application.ExecutablePath, this.resourceManager.GetString("Label_CheckMail"));
			link3.Arguments = "-check";
			link3.IconReference = new IconReference(Application.ExecutablePath, 4);
			taskArray3[0] = link3;
			this.jumpListManager.AddUserTasks(taskArray3);
			IJumpListTask[] taskArray4 = new IJumpListTask[1];
			JumpListLink link4 = new JumpListLink(Application.ExecutablePath, this.resourceManager.GetString("Label_ConfigurationShort"));
			link4.Arguments = "-settings";
			link4.IconReference = new IconReference(Application.ExecutablePath, 5);
			taskArray4[0] = link4;
			this.jumpListManager.AddUserTasks(taskArray4);
			try {
				this.jumpListManager.Refresh();
			}
			catch {
			}
		}

		private void CheckMail() {
			this.statusList.Clear();
			this.unreadTotal = 0;
			foreach (Notifier notifier in this.instanceList.Values) {
				notifier.CheckMail();
			}
		}

		private void CloseInstances() {
			List<string> list = new List<string>();
			foreach (Notifier notifier in this.instanceList.Values) {
				if (config.Accounts[notifier.AccountIndex] == null) {
					list.Add(notifier.Text);
				}
			}
			foreach (string str in list) {
				TabbedThumbnail thumbnailPreview = this.taskbarManager.TabbedThumbnail.GetThumbnailPreview(this.instanceList[str].Handle);
				thumbnailPreview.TabbedThumbnailClosed -= new EventHandler<TabbedThumbnailEventArgs>(this.preview_TabbedThumbnailClosed);
				this.taskbarManager.TabbedThumbnail.RemoveThumbnailPreview(thumbnailPreview);
				this.instanceList[str].Close();
			}
			if (config.Accounts.Count > 0) {
				this.taskbarManager.TabbedThumbnail.SetActiveTab(this.instanceList[config.Accounts[0].FullAddress].Handle);
			}
		}

		private void CreateInstances() {
			for (int i = 0; i < config.Accounts.Count; i++) {
				if (!this.instanceList.ContainsKey(config.Accounts[i].FullAddress)) {
					Notifier notifier = new Notifier(i);
					notifier.FormClosed += new FormClosedEventHandler(this.notifierWindow_FormClosed);
					notifier.CheckFinished += new CheckFinishedEventHandler(this.notifierWindow_CheckFinished);
					this.instanceList.Add(config.Accounts[i].FullAddress, notifier);
					notifier.Show(this);
					TabbedThumbnail preview = new TabbedThumbnail(base.Handle, notifier.Handle);
					preview.TabbedThumbnailClosed += new EventHandler<TabbedThumbnailEventArgs>(this.preview_TabbedThumbnailClosed);
					preview.SetWindowIcon(Utilities.ResourceHelper.GetResourceIcon("GmailNotifierPlus.Icons.Default.ico"));
					preview.Tooltip = string.Empty;
					this.taskbarManager.TabbedThumbnail.AddThumbnailPreview(preview);
				}
			}
			if (config.Accounts.Count > 0) {
				this.taskbarManager.TabbedThumbnail.SetActiveTab(this.instanceList[config.Accounts[0].FullAddress].Handle);
			}
		}

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FinalizeChecks() {
			if (this.statusList.Count == this.instanceList.Count) {
				if (this.unreadTotal != this.previousTotal) {
					this.UpdateMailsJumpList();
					if (this.unreadTotal > this.previousTotal) {
						switch (1) { // TODO - config.SoundNotification) {
							case 1:
								SoundHelper.PlayDefaultSound();
								break;

							case 2:
								SoundHelper.PlayCustomSound(config.Sound);
								break;
						}
					}
					this.previousTotal = this.unreadTotal;
				}
				if (this.statusList.ContainsValue(Notifier.Status.AuthenticationFailed)) {
					this.SetWarningOverlay();
				}
				else if (this.statusList.ContainsValue(Notifier.Status.Offline)) {
					this.SetOfflineOverlay();
				}
				else {
					this.SetUnreadOverlay(this.unreadTotal);
				}
			}
		}

		private void InitializeComponent() {
			this.components = new Container();
			ComponentResourceManager manager = new ComponentResourceManager(typeof(Main));
			this.pic_background = new PictureBox();
			this.tmr_check = new Timer(this.components);
			((ISupportInitialize)this.pic_background).BeginInit();
			base.SuspendLayout();
			this.pic_background.BackColor = Color.Transparent;
			this.pic_background.Location = new Point(0, 0);
			this.pic_background.Name = "pic_background";
			this.pic_background.Size = new Size(0xc6, 0x6c);
			this.pic_background.SizeMode = PictureBoxSizeMode.CenterImage;
			this.pic_background.TabIndex = 0;
			this.pic_background.TabStop = false;
			this.tmr_check.Tick += new EventHandler(this.tmr_check_Tick);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.Control;
			base.ClientSize = new Size(0xc6, 0x6c);
			base.Controls.Add(this.pic_background);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Icon = (Icon)manager.GetObject("$this.Icon");
			this.MaximumSize = new Size(0xc6, 0x6c);
			base.Name = "Main";
			base.Opacity = 0.0;
			this.Text = "Main";
			base.Load += new EventHandler(this.Main_Load);
			base.Shown += new EventHandler(this.Main_Shown);
			base.FormClosing += new FormClosingEventHandler(this.Main_FormClosing);
			((ISupportInitialize)this.pic_background).EndInit();
			base.ResumeLayout(false);
		}

		private void Main_FormClosing(object sender, FormClosingEventArgs e) {
			try {
				this.jumpListManager.ClearAllCustomCategories();
				this.jumpListManager.Refresh();
			}
			catch {
			}
		}

		private void Main_Load(object sender, EventArgs e) {
			this.jumpListManager = JumpList.CreateJumpListForIndividualWindow(this.taskbarManager.ApplicationId, base.Handle);
			this.jumpListManager.JumpListItemsRemoved += delegate(object o, UserRemovedJumpListItemsEventArgs ev) {
			};
			this.jumpListManager.KnownCategoryToDisplay = JumpListKnownCategoryType.Neither;
			this.BuildJumpList();
		}

		private void Main_Shown(object sender, EventArgs e) {
			if (config.FirstRun) {
				this.OpenSettingsWindow();
			}
			base.Top = 0x1000;
		}

		private void notifierWindow_CheckFinished(Notifier sender, EventArgs e) {
			this.statusList.Add(sender.Text, sender.ConnectionStatus);
			this.unreadTotal += sender.UnreadMailCount;
			this.FinalizeChecks();
		}

		private void notifierWindow_FormClosed(object sender, FormClosedEventArgs e) {
			Notifier notifier = (Notifier)sender;
			notifier.FormClosed -= new FormClosedEventHandler(this.notifierWindow_FormClosed);
			notifier.CheckFinished -= new CheckFinishedEventHandler(this.notifierWindow_CheckFinished);
			this.instanceList.Remove(notifier.Text);
			this.statusList.Remove(notifier.Text);
			notifier.Dispose();
			notifier = null;
			this.FinalizeChecks();
			if (this.instanceList.Count == 0) {
				base.Close();
			}
			else {
				this.taskbarManager.TabbedThumbnail.SetActiveTab(this.instanceList[config.Accounts[0].FullAddress].Handle);
			}
		}

		private void OpenSettingsWindow() {
			if (this.settingsWindow == null) {
				this.settingsWindow = new Settings();
				this.settingsWindow.FormClosed += new FormClosedEventHandler(this.settingsWindow_FormClosed);
				this.settingsWindow.Show(this);
			}
			this.settingsWindow.Activate();
		}

		private void preview_TabbedThumbnailClosed(object sender, TabbedThumbnailEventArgs e) {
			(Control.FromHandle(e.WindowHandle) as Form).Close();
		}

		internal void RemoteCheckMails() {
			MethodInvoker method = null;
			if (base.InvokeRequired) {
				if (method == null) {
					method = delegate {
						this.CheckMail();
					};
				}
				base.Invoke(method);
			}
		}

		internal void RemoteOpenSettingsWindow() {
			MethodInvoker method = null;
			if (base.InvokeRequired) {
				if (method == null) {
					method = delegate {
						this.OpenSettingsWindow();
					};
				}
				base.Invoke(method);
			}
		}

		internal void SetOfflineOverlay() {
			this.taskbarManager.SetOverlayIcon(base.Handle, Utilities.ResourceHelper.GetResourceIcon("GmailNotifierPlus.Icons.Offline.ico"), string.Empty);
		}

		private void config_Saved(object sender, EventArgs e) {
			this.tmr_check.Interval = config.Interval * 0x3e8;
			this.resourceManager = new ResourceManager("GmailNotifierPlus.Properties." + config.Language, Assembly.GetExecutingAssembly());
			this.BuildJumpList();
			this.UpdateMailsJumpList();
			if (config.Accounts.Count > this.instanceList.Count) {
				this.CreateInstances();
			}
			else if (config.Accounts.Count < this.instanceList.Count) {
				this.CloseInstances();
			}
			this.CheckMail();
		}

		private void settingsWindow_FormClosed(object sender, FormClosedEventArgs e) {
			this.settingsWindow.Dispose();
			this.settingsWindow = null;
		}

		private void SetUnreadOverlay(int count) {
			if (count == 0) {
				this.taskbarManager.SetOverlayIcon(base.Handle, null, string.Empty);
			}
			else {
				count = Math.Min(count, 0x63);
				this.taskbarManager.SetOverlayIcon(base.Handle, Utilities.ResourceHelper.GetResourceIcon("GmailNotifierPlus.Icons.Digits." + count.ToString("00") + ".ico"), string.Empty);
			}
		}

		internal void SetWarningOverlay() {
			this.taskbarManager.SetOverlayIcon(base.Handle, Utilities.ResourceHelper.GetResourceIcon("GmailNotifierPlus.Icons.Warning.ico"), string.Empty);
		}

		private void tmr_check_Tick(object sender, EventArgs e) {
			this.CheckMail();
		}

		private void UpdateMailsJumpList() {
			try {
				this.jumpListManager.ClearAllCustomCategories();
			}
			catch {
			}
			Dictionary<string, List<JumpListLink>> dictionary = new Dictionary<string, List<JumpListLink>>();
			int num = 0;
			int num2 = Math.Min(this.unreadTotal, (int)this.jumpListManager.MaxSlotsInList);
			int index = 0;
			int num4 = 0;
			while (num < num2) {
				string str;
				
				Notifier notifier = this.instanceList[config.Accounts[index].FullAddress];
				Account account = config.Accounts[notifier.AccountIndex];

				if (Locale.Current.IsRightToLeftLanguage) {
					str = string.Concat(new object[] { "(", notifier.UnreadMailCount, ") ", account.FullAddress, " " });
				}
				else {
					str = string.Concat(new object[] { account.FullAddress, " (", notifier.UnreadMailCount, ")" });
				}

				if (!dictionary.ContainsKey(str)) {
					dictionary.Add(str, new List<JumpListLink>());
				}

				if (num4 < notifier.CurrentMails.Count) {
					XmlNode node = notifier.CurrentMails[num4];
					JumpListLink item = new JumpListLink(UrlHelper.BuildMailUrl(node.ChildNodes.Item(2).Attributes["href"].Value, notifier.AccountIndex), string.IsNullOrEmpty(node.ChildNodes.Item(0).InnerText) ? this.resourceManager.GetString("Label_NoSubject") : node.ChildNodes.Item(0).InnerText);
					item.IconReference = new IconReference(Application.ExecutablePath, 3);
					dictionary[str].Add(item);
					num++;
				}

				if (index < (this.instanceList.Count - 1)) {
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
				this.jumpListManager.AddCustomCategories(new JumpListCustomCategory[] { category });
			}
			try {
				this.jumpListManager.Refresh();
			}
			catch {
			}
		}

		protected override void WndProc(ref Message m) {
			if (m.Msg == WM_TASKBARBUTTONCREATED) {
				this.CheckMail();
			}
			base.WndProc(ref m);
		}
	}
}

