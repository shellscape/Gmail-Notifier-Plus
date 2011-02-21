using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

using GmailNotifierPlus.Controls;
using GmailNotifierPlus.Utilities;

namespace GmailNotifierPlus.Forms {
	public partial class Settings : Form {

		private enum SourceScreen {
			About,
			Accounts
		}

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		private readonly int animationSpeed = 30;

		private int _DefaultAccountIndex;
		private bool _IsEditing;

		private Dictionary<string, Account> _ListAccounts = new Dictionary<string, Account>();
		private Config _Config = Config.Current;
		private readonly Font _FontBold;
		private readonly Font _FontRegular;

		public Settings() {
			InitializeComponent();

			this.Icon = Program.Icon;
			this.ClientSize = new Size(300, 355);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

			ToolTip tip = new ToolTip();

			_FontRegular = new Font(_ListViewAccounts.Font, FontStyle.Regular);
			_FontBold = new Font(_ListViewAccounts.Font, FontStyle.Bold);

			InitCombos();

			if (_Config.Accounts.Count > 0) {
				foreach (Account account in _Config.Accounts) {
					_ListViewAccounts.Items.Add(account.Login);
					_ListAccounts.Add(account.Login, account);
				}

				_DefaultAccountIndex = _Config.Accounts.IndexOf(_Config.Accounts.Default);

				if (_DefaultAccountIndex < _ListViewAccounts.Items.Count) {
					_ListViewAccounts.Items[this._DefaultAccountIndex].Font = this._FontBold;
				}
			}

			tip.SetToolTip(_ImgButtonAbout, Locale.Current.Tooltips.About);
			tip.SetToolTip(_ImgButtonAdd, Locale.Current.Tooltips.Add);
			tip.SetToolTip(_ImgButtonRemove, Locale.Current.Tooltips.Remove);

			this.Text = Resources.Resources.WindowTitle;
			_TextInterval.Text = (_Config.Interval / 60).ToString();
			_ComboLanguage.SelectedValue = _Config.Language;

			_LabelTitle.Text = Locale.Current.Labels.Configuration;
			_LabelSound.Text = Locale.Current.Labels.Sound;
			_LabelAdditional.Text = Locale.Current.Labels.Additional;
			_LabelInterval.Text = Locale.Current.Labels.Interval;
			_LabelMinutes.Text = Locale.Current.Labels.Minutes;
			_LabelLanguage.Text = Locale.Current.Labels.Language;
			_LabelUsername.Text = Locale.Current.Labels.Login;
			_LabelPassword.Text = Locale.Current.Labels.Password;
			_LabelError.Text = Locale.Current.Labels.Error;

			_ButtonDefault.Text = Locale.Current.Buttons.Default;
			_ButtonEdit.Text = Locale.Current.Buttons.Edit;
			_ButtonBrowse.Text = Locale.Current.Buttons.Browse;
			_ButtonOk.Text = _ButtonAboutOk.Text = Locale.Current.Buttons.OK;
			_ButtonCancel.Text = _ButtonAccountCancel.Text = Locale.Current.Buttons.Cancel;

			_PanelShellscape.Click += this._PanelShellscape_Click;

			_PanelAbout.FirstRun = Config.Current.FirstRun;

			InitImageButtons();
			UpdateHeaderSize();
			//AdjustControls();
			InitEvents();

			if (Locale.Current.IsRightToLeftLanguage) {
				MirrorControls();
			}

		}

#region .    Event Methods

		private void _ComboSound_SelectedIndexChanged(object sender, EventArgs e) {
			_ButtonBrowse.Enabled = _ComboSound.SelectedIndex == 2;

			if (_ButtonBrowse.Enabled && string.IsNullOrEmpty(_ComboSound.SelectedValue.ToString())) {
				this.SelectCustomSound();
			}
		}

		private void _Credentials_Changed(object sender, EventArgs e) {
			if (this._IsEditing) {
				if ((_TextUsername.Text != _ListViewAccounts.SelectedItems[0].Text) && this._ListAccounts.ContainsKey(_TextUsername.Text)) {
					_ButtonAccountSave.Enabled = false;
					_PictureExclamation.Visible = _LabelError.Visible = true;
					return;
				}
			}
			else if (this._ListAccounts.ContainsKey(_TextUsername.Text)) {
				_ButtonAccountSave.Enabled = false;
				_PictureExclamation.Visible = _LabelError.Visible = true;
				return;
			}
			_ButtonAccountSave.Enabled = !string.IsNullOrEmpty(_TextUsername.Text) && !string.IsNullOrEmpty(_TextPassword.Text);
			_PictureExclamation.Visible = _LabelError.Visible = false;
		}

		private void _ImgButtonAbout_Click(object sender, EventArgs e) {
			this.SwitchToAbout();
		}

		private void _ImgButtonAdd_Click(object sender, EventArgs e) {
			this._IsEditing = false;
			this.SwitchToAccounts();
		}

		private void _ImgButtonDonate_Click(object sender, EventArgs e) {
			Help.ShowHelp(this, Utilities.UrlHelper.Uris.Donate);
		}

		private void _ImgButtonRemove_Click(object sender, EventArgs e) {

			TaskDialog dialog = new TaskDialog();
			dialog.Caption = Resources.Resources.WindowTitle;
			dialog.InstructionText = Locale.Current.Labels.RemoveConfirmation;
			dialog.Cancelable = true;
			dialog.OwnerWindowHandle = base.Handle;

			TaskDialogButton buttonYes = new TaskDialogButton("yesButton", Locale.Current.Labels.Yes);
			buttonYes.Default = true;
			buttonYes.Click += _TaskButtonYes_Click;

			TaskDialogButton buttonNo = new TaskDialogButton("noButton", Locale.Current.Labels.No);
			buttonNo.Click += _TaskButtonNo_Click;

			dialog.Controls.Add(buttonYes);
			dialog.Controls.Add(buttonNo);
			dialog.Show();
		}

		private void _ListViewAccounts_DoubleClick(object sender, EventArgs e) {
			if (_ListViewAccounts.SelectedIndices.Count > 0) {
				this.EditAccount();
			}
		}

		private void _ListViewAccounts_SelectedIndexChanged(object sender, EventArgs e) {
			_ImgButtonRemove.Enabled = _ButtonEdit.Enabled = _ListViewAccounts.SelectedItems.Count > 0;
			_ButtonDefault.Enabled = (_ListViewAccounts.SelectedItems.Count > 0) && (_ListViewAccounts.SelectedIndices[0] != this._DefaultAccountIndex);
		}

		private void Settings_Shown(object sender, EventArgs e) {
			if (!this.Focused) {
				SetForegroundWindow(base.Handle);
			}
		}

		private void _PanelShellscape_Click(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(UrlHelper.Uris.Shellscape);
			//Help.ShowHelp(this, UrlHelper.Uris.Shellscape);
		}

		private void _TextInterval_Leave(object sender, EventArgs e) {
			this.FixInterval();
		}

		private void _TaskButtonNo_Click(object sender, EventArgs e) {
			TaskDialogButton button = (TaskDialogButton)sender;
			((TaskDialog)button.HostingDialog).Close();
		}

		private void _TaskButtonYes_Click(object sender, EventArgs e) {
			TaskDialogButton button = (TaskDialogButton)sender;

			((TaskDialog)button.HostingDialog).Close();

			int num = _ListViewAccounts.SelectedIndices[0];

			_ListAccounts.Remove(_ListViewAccounts.SelectedItems[0].Text);
			_ListViewAccounts.SelectedItems[0].Remove();

			if (num == this._DefaultAccountIndex) {
				this._DefaultAccountIndex = 0;
				if (_ListViewAccounts.Items.Count > 0) {
					_ListViewAccounts.Items[this._DefaultAccountIndex].Font = this._FontBold;
				}
			}

			this.UpdateHeaderSize();
		}

		#endregion

		#region .    Button Event Methods

		private void _ButtonAboutOk_Click(object sender, EventArgs e) {
			this.SwitchToSettings(SourceScreen.About);
		}

		private void _ButtonAccountCancel_Click(object sender, EventArgs e) {
			this.SwitchToSettings(SourceScreen.Accounts);
		}

		private void _ButtonAccountSave_Click(object sender, EventArgs e) {
			Account account = new Account(_TextUsername.Text, _TextPassword.Text);
			if (this._IsEditing) {
				ListViewItem item = _ListViewAccounts.SelectedItems[0];
				this._ListAccounts.Remove(item.Text);
				this._ListAccounts.Add(account.Login, account);
				_ListViewAccounts.Items[_ListViewAccounts.Items.IndexOf(item)] = new ListViewItem(account.Login);
			}
			else {
				this._ListAccounts.Add(account.Login, account);
				_ListViewAccounts.Items.Add(account.Login);
				if (_ListViewAccounts.Items.Count == 1) {
					this._DefaultAccountIndex = 0;
					_ListViewAccounts.Items[this._DefaultAccountIndex].Font = this._FontBold;
					_ButtonDefault.Enabled = false;
				}
			}
			this.UpdateHeaderSize();
			this.SwitchToSettings(SourceScreen.Accounts);
		}

		private void _ButtonBrowse_Click(object sender, EventArgs e) {
			this.SelectCustomSound();
		}

		private void _ButtonCancel_Click(object sender, EventArgs e) {
			base.Close();
		}

		private void _ButtonDefault_Click(object sender, EventArgs e) {
			int num = _ListViewAccounts.SelectedIndices[0];
			_ListViewAccounts.Items[this._DefaultAccountIndex].Font = this._FontRegular;
			_ListViewAccounts.Items[num].Font = this._FontBold;
			this._DefaultAccountIndex = num;
			_ButtonDefault.Enabled = false;
		}

		private void _ButtonEdit_Click(object sender, EventArgs e) {
			this.EditAccount();
		}

		private void _ButtonOk_Click(object sender, EventArgs e) {

			this.FixInterval();

			_Config.Accounts.Clear();
			_Config.Accounts.AddRange(this._ListAccounts.Values);
			_Config.Interval = Convert.ToInt32(_TextInterval.Text) * 60;
			_Config.Language = _ComboLanguage.SelectedValue.ToString();

			foreach (Account account in _Config.Accounts) {
				account.Default = false;
			}

			if (_Config.Accounts.Count > this._DefaultAccountIndex) {
				_Config.Accounts[this._DefaultAccountIndex].Default = true;
			}

			if ((_ComboSound.SelectedIndex == 2) && string.IsNullOrEmpty(_ComboSound.SelectedValue.ToString())) {
				_Config.SoundNotification = 0;
			}
			else {
				_Config.SoundNotification = _ComboSound.SelectedIndex;
				_Config.Sound = _ComboSound.SelectedValue.ToString();
			}

			_Config.Save();
			base.Close();
		}

		#endregion

		#region .    Private Methods

		//private void AdjustControls() {

		//  return;

		//  int num = 6; // arbitrary numbers? padding?
		//  int num2 = 10; // ditto?
		//  int width = Math.Max(_ButtonDefault.Width, _ButtonEdit.Width);

		//  // Not sure if this was lowsy coding, CLR mangling, or if it's really needed. Seems that autosize would work here.
		//  //if (config.Language == Resources.Code_Bulgarian) {
		//  //  _ButtonEdit.Width -= 25;
		//  //}
		//  //else if (config.Language == Resources.Code_Ukrainian) {
		//  //  _ButtonEdit.Width -= 40;
		//  //}

		//  _ButtonEdit.Left = _ListViewAccounts.Right - _ButtonEdit.Width;

		//  _ButtonDefault.AutoSize = _ButtonEdit.AutoSize = false;
		//  _ButtonDefault.Width = _ButtonEdit.Width = width - num2;
		//  _ButtonDefault.Left = (_ButtonEdit.Left - _ButtonDefault.Width) - num;

		//  width = _ButtonBrowse.Width;

		//  _ButtonBrowse.AutoSize = false;
		//  _ButtonBrowse.Width = width - num2;
		//  _ButtonBrowse.Left = _ListViewAccounts.Right - _ButtonBrowse.Width;

		//  _ComboSound.Width = (_ButtonBrowse.Left - num) - _ComboSound.Left;

		//  Size minSize = new Size(((_ButtonDefault.Left - _LabelInterval.Left) - num) + 1, _LabelInterval.Height);

		//  _LabelInterval.MinimumSize = _LabelLanguage.MinimumSize = minSize;

		//  int left = (_LabelInterval.Left + Math.Max(_LabelInterval.Width, _LabelLanguage.Width)) + num;

		//  _TextInterval.Left = left;

		//  _LabelMinutes.Left = (_TextInterval.Left + _TextInterval.Width) + num;

		//  _ComboLanguage.Left = left;
		//  _ComboLanguage.Width = _ListViewAccounts.Right - left;
		//}

		private void EditAccount() {
			this._IsEditing = true;
			this.SwitchToAccounts();
		}

		private void FixInterval() {
			int num;
			if (!int.TryParse(_TextInterval.Text, out num) || (num <= 0)) {
				_TextInterval.Text = "1";
			}
		}

		private void InitEvents() {

			_ButtonAboutOk.Click += _ButtonAboutOk_Click;
			_ButtonAccountCancel.Click += _ButtonAccountCancel_Click;
			_ButtonAccountSave.Click += _ButtonAccountSave_Click;
			_ButtonBrowse.Click += _ButtonBrowse_Click;
			_ButtonCancel.Click += _ButtonCancel_Click;
			_ButtonDefault.Click += _ButtonDefault_Click;
			_ButtonEdit.Click += _ButtonEdit_Click;
			_ButtonOk.Click += _ButtonOk_Click;

			_ComboSound.SelectedIndexChanged += _ComboSound_SelectedIndexChanged;

			_ImgButtonAbout.Click += _ImgButtonAbout_Click;
			_ImgButtonAdd.Click += _ImgButtonAdd_Click;
			//_ImgButtonDonate.Click += _ImgButtonDonate_Click;
			_ImgButtonRemove.Click += _ImgButtonRemove_Click;

			_ListViewAccounts.DoubleClick += _ListViewAccounts_DoubleClick;
			_ListViewAccounts.SelectedIndexChanged += _ListViewAccounts_SelectedIndexChanged;

			this.Shown += Settings_Shown;

			_TextInterval.Leave += _TextInterval_Leave;
			_TextPassword.TextChanged += _Credentials_Changed;
			_TextUsername.TextChanged += _Credentials_Changed;
		}

		private void InitImageButtons() {

			//_PanelAbout.BackgroundImage = ResourceHelper.GetImage("About.png");
			//_PanelAbout.BackgroundImageLayout = ImageLayout.None;
			//_PictureBackground.Image = ResourceHelper.GetImage("About.png");
			//_PictureCopyrights.Image = ResourceHelper.GetImage("Copyrights.png");
			_PictureExclamation.Image = ResourceHelper.GetImage("Exclamation.png");
			_PictureExclamation.Height = _LabelError.Height;

			_ImgButtonAbout.SetImage(ResourceHelper.GetImage("Information.png"));
			_ImgButtonAdd.SetImages(ResourceHelper.GetImage("Add.png"), ResourceHelper.GetImage("AddDisabled.png"), ResourceHelper.GetImage("AddHover.png"), ResourceHelper.GetImage("AddPressed.png"));
			//_ImgButtonDonate.SetImage(ResourceHelper.GetImage("Donate.gif"));
			_ImgButtonRemove.SetImages(ResourceHelper.GetImage("Remove.png"), ResourceHelper.GetImage("RemoveDisabled.png"), ResourceHelper.GetImage("RemoveHover.png"), ResourceHelper.GetImage("RemovePressed.png"));
			_ImgButtonRemove.Enabled = false;

			if (_Config.FirstRun) {
				//_PictureWelcome.Image = ResourceHelper.GetImage("Welcome.png");
				//_PictureDisclaimer.Image = ResourceHelper.GetImage("Disclaimer.png");
				//_PanelMain.Left = _PanelButtons.Left = 300;
				//_PanelAbout.Left = _PanelAboutButtons.Left += 300;

				this.SwitchToAbout();

				//_PictureWelcome.Visible = _PictureDisclaimer.Visible = true;
				//_PictureCopyrights.Visible = _ImgButtonDonate.Visible = false;

				//_ImgButtonDonate.Visible = false;
				_ButtonAboutOk.Text = Locale.Current.Buttons.LetsGo;
			}
			else {
				this.SwitchToSettings(SourceScreen.About);
			}

		}

		private void InitCombos() {

			String columnName = "Name";
			String columnValue = "Value";
			DataTable table = new DataTable();
			DataTable table2 = new DataTable();

			table.Columns.Add(columnName, typeof(string));
			table.Columns.Add(columnValue, typeof(string));

			foreach (String language in ResourceHelper.AvailableLocales) {
				table.Rows.Add(new string[] { language, language });
			}

			table2.Columns.Add(columnName, typeof(string));
			table2.Columns.Add(columnValue, typeof(string));
			table2.Rows.Add(new string[] { Locale.Current.Labels.NoSound, string.Empty });
			table2.Rows.Add(new string[] { Locale.Current.Labels.DefaultSound, string.Empty });

			if (File.Exists(_Config.Sound)) {
				table2.Rows.Add(new string[] { Path.GetFileName(_Config.Sound), _Config.Sound });
			}
			else {
				table2.Rows.Add(new string[] { Locale.Current.Labels.CustomSound, string.Empty });
			}

			_ComboLanguage.DataSource = table;
			_ComboLanguage.DisplayMember = columnName;
			_ComboLanguage.ValueMember = columnValue;

			_ComboSound.DataSource = table2;
			_ComboSound.DisplayMember = columnName;
			_ComboSound.ValueMember = columnValue;
			_ComboSound.SelectedIndex = _Config.SoundNotification;

		}

		private void MirrorControls() {
			this.RightToLeft = RightToLeft.Yes;
			this.RightToLeftLayout = true;
			this.ReverseControls(ref _PanelMain);
			this.ReverseControls(ref _PanelButtons);
			this.ReverseControls(ref _PanelAccount);
			this.ReverseControls(ref _PanelAccountButtons);
			this.ReverseControls(ref _PanelAboutButtons);
		}

		private void ReverseControls(ref GmailNotifierPlus.Controls.DoubleBufferPanel container) {
			for (int i = 0; i < container.Controls.Count; i++) {
				container.Controls[i].Left = container.Width - (container.Controls[i].Left + container.Controls[i].Width);
			}
		}

		private void SelectCustomSound() {

			CommonOpenFileDialog dialog = new CommonOpenFileDialog();
			CommonFileDialogStandardFilters.TextFiles.ShowExtensions = true;
			CommonFileDialogFilter filter = new CommonFileDialogFilter(Locale.Current.Labels.WaveFiles, ".wav") { ShowExtensions = true };

			String path = string.IsNullOrEmpty(_ComboSound.SelectedValue.ToString()) ? Path.Combine(KnownFolders.Windows.Path, "Media") : Path.GetFullPath(_ComboSound.SelectedValue.ToString());

			dialog.Title = Locale.Current.Labels.BrowseDialog;
			dialog.Multiselect = false;
			dialog.DefaultDirectory = Directory.Exists(path) ? path : KnownFolders.Desktop.Path;
			dialog.Filters.Add(filter);

			if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
				DataTable dataSource = (DataTable)_ComboSound.DataSource;

				dataSource.Rows.RemoveAt(2);
				dataSource.Rows.Add(new string[] { Path.GetFileName(dialog.FileName), dialog.FileName });

				_ComboSound.SelectedIndex = 2;
			}
		}

		private void UpdateHeaderSize() {
			// more arbitrary math. this needs to be cleaned up.
			int width = (_ListViewAccounts.Items.Count > 5) ? (_ListViewAccounts.Width - 21) : (_ListViewAccounts.Width - 4);
			_ListViewAccounts.Columns[0].Width = width;
		}

		#endregion

		#region .    Panel Switching Methods

		private void SwitchToAbout() {

			_ButtonAboutOk.Text = Locale.Current.Buttons.Sweet;
			this.AcceptButton = _ButtonAboutOk;
			this.CancelButton = _ButtonAboutOk;

			_ListViewAccounts.TabStop =
				_ButtonDefault.TabStop =
				_ButtonEdit.TabStop =
				_ComboSound.TabStop =
				_ButtonBrowse.TabStop =
				_TextInterval.TabStop =
				_ComboLanguage.TabStop =
				_ButtonOk.TabStop =
				_ButtonCancel.TabStop = false;

			_ButtonAboutOk.TabStop = true;
			_PanelAbout.Focus();
			Animate(_PanelAbout);

		}

		private void SwitchToAccounts() {
			int width = _LabelAccountTitle.Width;

			if (this._IsEditing) {
				_LabelAccountTitle.Text = Locale.Current.Labels.Edit;
				_ButtonAccountSave.Text = Locale.Current.Buttons.Save;

				Account account = this._ListAccounts[_ListViewAccounts.SelectedItems[0].Text];
				_TextUsername.Text = account.Login;
				_TextPassword.Text = account.Password;
			}
			else {
				_LabelAccountTitle.Text = Locale.Current.Labels.Add;
				_ButtonAccountSave.Text = Locale.Current.Buttons.OK;
				_TextUsername.Text = _TextPassword.Text = string.Empty;
			}

			_ListViewAccounts.TabStop =
				_ButtonDefault.TabStop =
				_ButtonEdit.TabStop =
				_ComboSound.TabStop =
				_ButtonBrowse.TabStop =
				_TextInterval.TabStop =
				_ComboLanguage.TabStop =
				_ButtonOk.TabStop =
				_ButtonCancel.TabStop = false;

			_TextUsername.TabStop =
				_TextPassword.TabStop =
				_ButtonAccountSave.TabStop =
				_ButtonAccountCancel.TabStop = true;

			this.AcceptButton = _ButtonAccountSave;
			this.CancelButton = _ButtonAccountCancel;

			_TextUsername.Focus();

			if (Locale.Current.IsRightToLeftLanguage) {
				_LabelAccountTitle.Left += width - _LabelAccountTitle.Width;
			}

			Animate(_PanelAccount);
		}

		private void SwitchToSettings(SourceScreen source) {

			this.AcceptButton = _ButtonOk;
			this.CancelButton = _ButtonCancel;

			_ListViewAccounts.TabStop =
				_ButtonDefault.TabStop =
				_ButtonEdit.TabStop =
				_ComboSound.TabStop =
				_ButtonBrowse.TabStop =
				_TextInterval.TabStop =
				_ComboLanguage.TabStop =
				_ButtonOk.TabStop =
				_ButtonCancel.TabStop = true;

			Animate(_PanelMain);
			_PanelMain.Focus();

			switch (source) {
				case SourceScreen.About:
					_ButtonAboutOk.TabStop = false;
					break;

				case SourceScreen.Accounts:
					_TextUsername.TabStop = _TextPassword.TabStop = _ButtonAccountSave.TabStop = _ButtonAccountCancel.TabStop = false;
					break;
			}
		}

		private void Animate(Panel panel) {

			// if we're moving right, -1. if we're moving left, 1.
			int multiplier = panel.Left > Math.Abs(_PanelSlider.Left) ? -1 : 1;

			while (Math.Abs(_PanelSlider.Left) != panel.Left) {
				_PanelSlider.Left += (animationSpeed * multiplier);
				System.Threading.Thread.Sleep(10); // add a little pause for smoothness
				Application.DoEvents();
			}

		}

		#endregion

	}
}
