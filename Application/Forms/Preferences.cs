using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Shellscape;
using Shellscape.UI.ControlPanel;

using GmailNotifierPlus.Localization;

using Microsoft.Win32;
using Microsoft.WindowsAPI.Dialogs;
using Microsoft.WindowsAPI.Shell;

namespace GmailNotifierPlus.Forms {
	public partial class Preferences : Shellscape.UI.ControlPanel.ControlPanelForm {

		private class AccountListItem : ListViewItem {

			public AccountListItem(Account account)
				: base() {
				this.Text = account.FullAddress;
				this.ImageIndex = 0;
				this.Account = account;
			}

			public Account Account { get; private set; }

		}

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
		public static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);

		private Account _currentAccount;

		private Bitmap _userFrame;
		private Bitmap _iconStandard;
		private Bitmap _iconApps;

		private VisualStyleRenderer _rendererListLarge;
		private VisualStyleRenderer _rendererListSmall;

		private VisualStyleElement _titleElement;

		public Preferences()
			: base() {
			
			InitializeComponent();

			Text = String.Concat(Locale.Current.Preferences.WindowTitle, " - ", Program.MainForm.Text);
			Icon = Program.MainForm.Icon;
			TopMost = true;

			_iconApps = Utilities.ResourceHelper.GetImage("gmail-limon.png");
			_iconStandard = Utilities.ResourceHelper.GetImage("gmail-bleu.png");

			_rendererListLarge = Shellscape.UI.VisualStyles.ControlPanel.GetRenderer(Shellscape.UI.VisualStyles.ControlPanel.ControlPanelPart.MessageText, 0, true);
			_rendererListSmall = Shellscape.UI.VisualStyles.ControlPanel.GetRenderer(Shellscape.UI.VisualStyles.ControlPanel.ControlPanelPart.BodyText, 0, true);
			_titleElement = Shellscape.UI.VisualStyles.ControlPanel.GetElement(
				Shellscape.UI.VisualStyles.ControlPanel.ControlPanelPart.Title,
				0,
				true
			);

			InitNavigation();
			InitGeneralPanel();
			InitAccountsPanel();
			InitAccountPanel();

			if (Config.Current.FirstRun) {
				InitFirstRun();
			}

			HidePanels();
			_PanelGeneral.Visible = true;

		}

		// FlowLayoutPanel doesn't support visual inheritance, so we have to add links manually here.
		private void InitNavigation() {
			ControlPanelTaskLink general = new ControlPanelTaskLink() {
				Text = Locale.Current.Preferences.Navigation.General,
				AssociatedPanel = _PanelGeneral
			};
			ControlPanelTaskLink accounts = new ControlPanelTaskLink() {
				Text = Locale.Current.Preferences.Navigation.Accounts,
				AssociatedPanel = _PanelAccounts
			};

			this.Tasks.Add(general);
			this.Tasks.Add(accounts);

			ControlPanelTaskLink about = new ControlPanelTaskLink() { Text = Locale.Current.JumpList.About };
			ControlPanelTaskLink donate = new ControlPanelTaskLink() { Text = Locale.Current.Preferences.Navigation.Donate};

			about.Click += delegate(object sender, EventArgs e) {
				Program.MainForm.Jumplist_ShowAbout(null);
			};

			donate.Click += delegate(object sender, EventArgs e) {
				Shellscape.Utilities.ApplicationHelper.Donate("Gmail%20Notifier%20Plus%20Donation");
			};

			this.OtherTasks.Add(about);
			this.OtherTasks.Add(donate);
		}

		public void InitFirstRun() {
			this.ManageAccount(null);
		}

		private void InitGeneralPanel() {
			_LabelGeneral.Text = Locale.Current.Preferences.Panels.General.Title;
			_LabelGeneral.ThemeElement = _titleElement;

			_LabelSound.Text = Locale.Current.Preferences.Panels.General.Sound.Title;
			_LabelInterval.Text = Locale.Current.Preferences.Panels.General.CheckEmail;
			_LabelLanguage.Text = Locale.Current.Preferences.Panels.General.Language;

			_ButtonApply.Text = Locale.Current.Preferences.Panels.General.ApplyChanges;
			_ButtonApply.Enabled = false;
			_ButtonApply.Click += _ButtonApply_Click;

			InitSound();
			InitLanguages();

			_TextInterval.Text = (Config.Current.Interval / 60).ToString();

			InitCheckboxes();

			EventHandler changed = delegate(object sender, EventArgs e) {
				_ButtonApply.Enabled = true;
			};

			foreach (var control in _PanelGeneral.Controls) {
				if (control is TextBox) {
					(control as TextBox).TextChanged += changed;
				}
				if (control is ComboBox) {
					(control as ComboBox).SelectedIndexChanged += changed;
				}
				if (control is CheckBox) {
					(control as CheckBox).CheckedChanged += changed;
				}
			}

		}

		/// <summary>
		/// Inits the Sound settings UX
		/// </summary>
		private void InitSound() {
			_ButtonSound.Text = Locale.Current.Preferences.Panels.General.Sound.Browse;

			DataTable dsSound = new DataTable();
			String columnName = "Name";
			String columnValue = "Value";
			String sound = Config.Current.Sound;

			dsSound.Columns.Add(columnName, typeof(String));
			dsSound.Columns.Add(columnValue, typeof(String));

			dsSound.Rows.Add(new String[] { Locale.Current.Preferences.Panels.General.Sound.None, String.Empty });
			dsSound.Rows.Add(new String[] { Locale.Current.Preferences.Panels.General.Sound.Default, String.Empty });

			if (System.IO.File.Exists(sound)) {
				dsSound.Rows.Add(new String[] { System.IO.Path.GetFileName(sound), sound });
			}
			else {
				dsSound.Rows.Add(new String[] { Locale.Current.Preferences.Panels.General.Sound.Custom, String.Empty });
			}

			_ComboSound.DataSource = dsSound;
			_ComboSound.DisplayMember = columnName;
			_ComboSound.ValueMember = columnValue;
			_ComboSound.SelectedIndex = (int)Config.Current.SoundNotification;
			_ComboSound.SelectedValueChanged += _ComboSound_SelectedValueChanged;

			_ButtonSound.Enabled = Config.Current.SoundNotification == SoundNotification.Custom;
			_ButtonSound.Click += _ButtonSound_Click;
		}

		/// <summary>
		/// Inits the Languages UX
		/// </summary>
		private void InitLanguages() {

			String columnName = "Name";
			String columnValue = "Value";
			DataTable dsLang = new DataTable();

			dsLang.Columns.Add(columnName, typeof(string));
			dsLang.Columns.Add(columnValue, typeof(string));

			foreach (KeyValuePair<String, String> kvp in Utilities.ResourceHelper.AvailableLocales) {
				dsLang.Rows.Add(new string[] { kvp.Value, kvp.Key });
			}

			_ComboLanguage.DataSource = dsLang;
			_ComboLanguage.DisplayMember = columnName;
			_ComboLanguage.ValueMember = columnValue;
			_ComboLanguage.SelectedValue = Config.Current.Language;
		}

		/// <summary>
		/// Inits the General > checkboxes UX
		/// </summary>
		private void InitCheckboxes() {

			_CheckTray.Text = Locale.Current.Preferences.Panels.General.ShowTray;
			_CheckToast.Text = Locale.Current.Preferences.Panels.General.ShowToast;
			_CheckFlashTaskbar.Text = Locale.Current.Preferences.Panels.General.FlashTaskbar;

			_CheckTray.Checked = Config.Current.ShowTrayIcon;
			_CheckToast.Checked = Config.Current.ShowToast;
			_CheckFlashTaskbar.Checked = Config.Current.FlashTaskbar;

		}

		/// <summary>
		/// Inits the manage accounts UX
		/// </summary>
		private void InitAccountsPanel() {

			_userFrame = Shellscape.Utilities.ResourceHelper.GetStandardResourceBitmap("authui.dll", "#12222");

			ImageList imageList = new ImageList() {
				ColorDepth = ColorDepth.Depth32Bit,
				ImageSize = new Size(1, _userFrame.Height)
			};

			imageList.Images.Add(_iconStandard);

			_ListAccounts.View = View.Tile;
			_ListAccounts.OwnerDraw = true;
			_ListAccounts.LargeImageList = _ListAccounts.SmallImageList = imageList;
			_ListAccounts.DrawItem += _ListAccounts_DrawItem;
			_ListAccounts.Click += delegate(object sender, EventArgs e) {

				ListView list = sender as ListView;

				if (list.SelectedItems.Count == 0) {
					return;
				}

				ManageAccount((list.SelectedItems[0] as AccountListItem).Account);
			};

			foreach (Account account in Config.Current.Accounts) {
				_ListAccounts.Items.Add(new AccountListItem(account));
			}

			_ListAccounts.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

			SetWindowTheme(_ListAccounts.Handle, "explorer", null);

			_LabelAccountsTitle.Text = Locale.Current.Preferences.Panels.Accounts.Title;
			_LabelAccountsTitle.ThemeElement = _titleElement;

			_LinkAddNew.Text = Locale.Current.Preferences.Panels.Accounts.AddNew;
			_LinkAddNew.NormalColor = Colors.ContentLinkNormal;
			_LinkAddNew.HoverColor = Colors.ContentLinkHot;
			_LinkAddNew.Click += delegate(object sender, EventArgs e) {
				_currentAccount = null;
				ManageAccount(null);
			};
		}

		/// <summary>
		/// Inits the account panel UX
		/// </summary>
		private void InitAccountPanel() {

			_LabelAccount.ThemeElement = _titleElement;

			// another fucking .net bug. the designer isn't asserting AutoSize = false; in InitializeComponent 
			_LabelAccount.Dock = DockStyle.Top;
			_LabelAccount.AutoSize = false;

			_LabelAddress.Text = Locale.Current.Preferences.Panels.Accounts.Account.Address;
			_LabelBrowser.Text = Locale.Current.Preferences.Panels.Accounts.Account.Browser;
			_LabelPassword.Text = Locale.Current.Preferences.Panels.Accounts.Account.Password;
			_LinkRemove.Text = Locale.Current.Preferences.Panels.Accounts.Account.RemoveAccount;
			_CheckDefaultAccount.Text = Locale.Current.Preferences.Panels.Accounts.Account.DefaultAccount;
			_CheckMailto.Text = Locale.Current.Preferences.Panels.Accounts.Account.Mailto;

			_ButtonAccountCancel.Text = Locale.Current.Common.Cancel;

			_LinkRemove.Click += _LinkRemove_Click;

			_PanelAccountGlyph.Height = _userFrame.Height;
			_PanelAccountGlyph.Paint += delegate(object sender, PaintEventArgs e) {
				if (_currentAccount == null) {
					return;
				}

				PaintAccountGlyph(e.Graphics, new Rectangle(0, 0, _PanelAccountGlyph.Width, _PanelAccountGlyph.Height), _currentAccount);
			};

			List<Shellscape.Browser> browsers = Shellscape.Utilities.BrowserHelper.Enumerate();
			String columnName = "Name";
			String columnValue = "Value";
			DataTable dt = new DataTable();

			dt.Columns.Add(columnName, typeof(String));
			dt.Columns.Add(columnValue, typeof(Shellscape.Browser));

			dt.Rows.Add(new object[] { "System Default", null });

			foreach (Shellscape.Browser browser in browsers) {
				dt.Rows.Add(new object[] { browser.Name, browser });
			}

			_ComboBrowser.DataSource = dt;
			_ComboBrowser.DisplayMember = columnName;
			_ComboBrowser.ValueMember = columnValue;

			EventHandler changed = delegate(object sender, EventArgs e) {
				_ButtonAccountAction.Enabled = true;
			};

			_ButtonAccountAction.Click += _ButtonAccountAction_Click;
			_ButtonAccountCancel.Click += delegate(object sender, EventArgs e) {
				HidePanels();
				_PanelAccounts.Show();
			};

			foreach (var control in _PanelAccountControls.Controls) {
				if (control is TextBox) {
					(control as TextBox).TextChanged += changed;
				}
				if (control is ComboBox) {
					(control as ComboBox).SelectedIndexChanged += changed;
				}
				if (control is CheckBox) {
					(control as CheckBox).CheckedChanged += changed;
				}
			}

		}

		public void ShowAccount(String fullAddress) {
			Account account = Config.Current.Accounts.Where(a => a.FullAddress == fullAddress).FirstOrDefault();

			if (account == null) {
				return;
			}

			ManageAccount(account);
		}

		private void ManageAccount(Account account) {

			this.HidePanels();

			if (account == null) { // new account time
				_LabelAccount.Text = Locale.Current.Preferences.Panels.Accounts.AddNew;
				_ButtonAccountAction.Text = Locale.Current.Preferences.Panels.Accounts.Account.AddAccount;

				_TextAddress.Text = _TextPassword.Text = String.Empty;

				_TextAddress.SetWatermark("Gmail address or username");
				_TextPassword.SetWatermark("Gmail password");

				_ComboBrowser.SelectedIndex = 0;
				_CheckMailto.Checked = false;
				_CheckDefaultAccount.Checked = false;

				int difference = _LabelAddress.Top;

				_PanelAccountControls.Padding = new Padding(3, 39 - _LabelAccount.Height, 3, 3);

				// why the textboxes and comboboxes arent responding to padding, i have no idea. huge bug.
				difference = _LabelAddress.Top - difference;
				
				_TextAddress.Top += difference;
				_TextPassword.Top += difference;
				_ComboBrowser.Top += difference;
			}
			else {
				_currentAccount = account;
				_LabelAccount.Text = String.Format(Locale.Current.Preferences.Panels.Accounts.Account.Title, account.FullAddress);
				_ButtonAccountAction.Text = Locale.Current.Preferences.Panels.General.ApplyChanges;

				_TextAddress.Text = account.Login;

				_TextPassword.SetWatermark("Enter a new Password");
				_TextPassword.Text = String.Empty;

				if (account.Browser != null) {
					List<Shellscape.Browser> browsers = Shellscape.Utilities.BrowserHelper.Enumerate();
					Shellscape.Browser selectedBrowser = browsers.Where(o => o.Name == _currentAccount.Browser.Name).FirstOrDefault();
					_ComboBrowser.SelectedIndex = browsers.IndexOf(selectedBrowser) + 1;
				}
				else {
					_ComboBrowser.SelectedIndex = 0;
				}

				_CheckMailto.Checked = account.HandlesMailto;
				_CheckDefaultAccount.Checked = account.Default;

				_PanelAccountControls.Padding = new Padding(3);

				if (_TextAddress.Top != 0) {
					// why the textboxes and comboboxes arent responding to padding, i have no idea. huge bug.
					int difference = _TextAddress.Top;

					_TextAddress.Top -= difference;
					_TextPassword.Top -= difference;
					_ComboBrowser.Top -= difference;
				}
			}

			_PanelAccountGlyph.Visible = _LinkRemove.Visible = !(account == null);
			_ButtonAccountAction.Left = _ButtonAccountCancel.Left - 3 - _ButtonAccountAction.Width;

			_ButtonAccountAction.Enabled = false;

			_PanelAccount.Show();
		}

		/// <summary>
		/// if the username has changed to something other than the original saved username,
		/// and the new username exists already, take a big fat poop all over the screen.
		/// </summary>
		/// <returns></returns>
		private Boolean AccountExists(Account account, Account compareTo) {

			String address = account.Login.ToLower();

			if (compareTo != null && address == compareTo.Login.ToLower()) {
				return false;
			}

			return Config.Current.Accounts.Where(o => o.Login.ToLower() == address).Count() > 0;
		}

		private void PaintAccountGlyph(Graphics g, Rectangle bounds, Account account) {

			String fullAddress = account.FullAddress ?? "<account error>";
			Bitmap image = account.Type == AccountTypes.Regular ? _iconStandard : _iconApps;
			int tileWidth = _userFrame.Width;
			int tileHeight = (int)(((float)tileWidth / (float)image.Width) * image.Height);
			RectangleF clipRect = new RectangleF(bounds.X + 14, bounds.Y + 13, _userFrame.Width - 30, _userFrame.Height - 30);

			g.SetClip(clipRect);
			g.DrawImage(image, bounds.X, bounds.Y + 10, tileWidth, tileHeight);
			g.ResetClip();

			g.DrawImage(_userFrame, new Rectangle(bounds.X, bounds.Y, _userFrame.Width, _userFrame.Height));

			Point addressPoint = new Point(bounds.X + _userFrame.Width, bounds.Y);
			Rectangle addressRect = new Rectangle(addressPoint.X, addressPoint.Y, bounds.Width - _userFrame.Width, bounds.Height);
			Rectangle textExtent = _rendererListSmall.GetTextExtent(g, addressRect, fullAddress, TextFormatFlags.Left);
			int center = (_userFrame.Height - textExtent.Height) / 2;

			if (account.Default) {
				Rectangle defaultExtent = _rendererListSmall.GetTextExtent(g, addressRect, Locale.Current.JumpList.DefaultAccount, TextFormatFlags.Left);

				center -= (defaultExtent.Height / 2) + 4; // 4 is the additional padding we're adding
			}

			addressRect.Offset(0, center);

			_rendererListSmall.DrawText(g, addressRect, fullAddress, false, TextFormatFlags.Left);

			if (account.Default) {
				addressRect.Offset(0, textExtent.Height + 4);

				_rendererListSmall.DrawText(g, addressRect, Locale.Current.JumpList.DefaultAccount, true, TextFormatFlags.Left);
			}
		}

		private void ToggleMailto(Boolean addAssociation) {

			RegistryKey command = Registry.CurrentUser.OpenSubKey(@"Software\Classes\mailto\shell\open\command", true);
			
			if (command == null) {
				if (addAssociation) {
					command = Registry.CurrentUser.CreateSubKey(@"Software\Classes\mailto\shell\open\command");
				}
				else {
					return; // it doesn't exist, so something must have removed it. get out of here.
				}
			}

			if (addAssociation) {
				String value = String.Concat("\"", Application.ExecutablePath, "\" -mailto %1");
				command.SetValue(null, value);
			}
			else {
				if(command.GetValue(null) != null) { // fixes #87
					command.DeleteValue(null);
				}
			}

			command.Close();
			command.Dispose();
		}

		private void _ButtonApply_Click(object sender, EventArgs e) {

			Config.Current.SoundNotification = (SoundNotification)_ComboSound.SelectedIndex;

			if (!String.IsNullOrEmpty(_ComboSound.SelectedValue.ToString())) {
				Config.Current.Sound = _ComboSound.SelectedValue.ToString();
			}

			String currentLanguage = Config.Current.Language;
			Config.Current.Language = _ComboLanguage.SelectedValue.ToString();

			if (_TextInterval.Value > 0) {
				Config.Current.Interval = _TextInterval.Value * 60;
			}

			Config.Current.FlashTaskbar = _CheckFlashTaskbar.Checked;
			Config.Current.ShowTrayIcon = _CheckTray.Checked;
			Config.Current.ShowToast = _CheckToast.Checked;

			Config.Current.Save();

			_ButtonApply.Enabled = false;

			if (currentLanguage != Config.Current.Language) {
				Program.MainForm.Jumplist_ShowPreferences(new String[] { "refresh" });
			}
		}

		private void _ButtonAccountAction_Click(object sender, EventArgs e) {

			Account account = _currentAccount ?? new Account();

			account.Login = _TextAddress.Text;

			if (AccountExists(account, _currentAccount)) {
				TaskDialog dialog = new TaskDialog() {
					Caption = Program.MainForm.Text,
					InstructionText = Locale.Current.Preferences.Panels.Accounts.Account.Error,
					Cancelable = false,
					OwnerWindowHandle = this.Handle,
					Icon = TaskDialogStandardIcon.Warning
				};

				dialog.Show();
				return;
			}
			
			account.Password = _TextPassword.Text.Length > 0 ? _TextPassword.Text : account.Password;
			account.Browser = _ComboBrowser.SelectedValue as Shellscape.Browser;

			if (_CheckDefaultAccount.Checked) {
				Config.Current.Accounts.ForEach(a => a.Default = false);
				account.Default = true;
			}

			Account mailtoAccount = Config.Current.Accounts.Where(o => o.HandlesMailto).FirstOrDefault();

			if (_CheckMailto.Checked) {
				
				if (mailtoAccount != null) {
					mailtoAccount.HandlesMailto = false;
				}

				account.HandlesMailto = true;

				ToggleMailto(true);
			}
			else {
				if (mailtoAccount == null) {
					ToggleMailto(false);
				}
			}

			if (_currentAccount == null) {
				_ListAccounts.Items.Add(new AccountListItem(account));
				Config.Current.Accounts.Add(account);
			}

			Config.Current.Save();

			if (_currentAccount != null) {
				Config.Current.Accounts.TriggerDirty(account);
			}

			_ButtonAccountAction.Enabled = false;

			this.HidePanels();
			_PanelAccounts.Show();
		}

		private void _ButtonSound_Click(object sender, EventArgs e) {
			CommonOpenFileDialog dialog = new CommonOpenFileDialog();
			CommonFileDialogStandardFilters.TextFiles.ShowExtensions = true;
			CommonFileDialogFilter filter = new CommonFileDialogFilter(Locale.Current.Preferences.Panels.General.Sound.WaveFiles, ".wav") { ShowExtensions = true };

			String path = string.IsNullOrEmpty(_ComboSound.SelectedValue.ToString()) ?
				Path.Combine(KnownFolders.Windows.Path, "Media") :
				Path.GetFullPath(_ComboSound.SelectedValue.ToString());

			dialog.Title = Locale.Current.Preferences.Panels.General.Sound.BrowseWindowTitle;
			dialog.Multiselect = false;
			dialog.DefaultDirectory = Directory.Exists(path) ? path : KnownFolders.Desktop.Path;
			dialog.Filters.Add(filter);

			DataTable dataSource = (DataTable)_ComboSound.DataSource;
			String label = Locale.Current.Preferences.Panels.General.Sound.Custom;
			String data = String.Empty;

			dataSource.Rows.RemoveAt(2);

			if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
				data = Path.GetFileName(dialog.FileName);
				label = dialog.FileName;
			}

			dataSource.Rows.Add(new string[] { data, label });

			_ComboSound.SelectedIndex = 2;
		}

		private void _ComboSound_SelectedValueChanged(object sender, EventArgs e) {

			if (_ComboSound.SelectedIndex == (int)SoundNotification.Custom) {
				_ButtonSound.Enabled = true;
			}
			else {
				_ButtonSound.Enabled = false;
			}
		}

		private void _ListAccounts_DrawItem(object sender, DrawListViewItemEventArgs e) {

			AccountListItem item = e.Item as AccountListItem;
			Bitmap image = item.Account.Type == AccountTypes.Regular ? _iconStandard : _iconApps;
			int elementState = 1;

			if ((e.State & ListViewItemStates.Selected) != 0) {
				if ((e.State & ListViewItemStates.Focused) != 0) {
					elementState = 3; // Selected
				}
				else {
					elementState = 5; // SelectedNotFocus;
				}
			}
			else if ((e.State & ListViewItemStates.Hot) != 0) {
				elementState = 2; // Hot
			}

			if (elementState > 1) {
				VisualStyleElement element = VisualStyleElement.CreateElement("Explorer::ListView", 1, elementState);

				VisualStyleRenderer renderer = new VisualStyleRenderer(element);

				renderer.DrawBackground(e.Graphics, item.Bounds);
			}

			if(e.Item.Bounds != null && item.Account != null) {
				PaintAccountGlyph(e.Graphics, e.Item.Bounds, item.Account);
			}
		}

		private void _LinkRemove_Click(object sender, EventArgs e) {
			TaskDialog dialog = new TaskDialog() {
				Caption = Program.MainForm.Text,
				InstructionText = Locale.Current.Preferences.Panels.Accounts.Account.RemoveConfirmation,
				OwnerWindowHandle = base.Handle
			};

			TaskDialogButton buttonYes = new TaskDialogButton("yesButton", Locale.Current.Preferences.Panels.Accounts.Account.RemoveAccount);
			buttonYes.Default = true;
			buttonYes.Click += delegate(object s, EventArgs ev) {

				AccountListItem target = _ListAccounts.Items.Cast<AccountListItem>().Where(o => o.Account == _currentAccount).FirstOrDefault();

				if(target != null){
					_ListAccounts.Items.Remove(target);
				}

				Config.Current.Accounts.Remove(_currentAccount);
				Config.Current.Save();

				_currentAccount = null;

				dialog.Close();

				HidePanels();
				_PanelAccounts.Show();
			};

			TaskDialogButton buttonNo = new TaskDialogButton("noButton", Locale.Current.Common.No);
			buttonNo.Click += delegate(object s, EventArgs ev) {
				dialog.Close();
			};

			dialog.Controls.Add(buttonYes);
			dialog.Controls.Add(buttonNo);
			dialog.Show();
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);

			TopMost = false;
			this.Focus();
		}
	}
}
