using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GmailNotifierPlus.Controls;

using Microsoft.VisualBasic.PowerPacks;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

using Shellscape.UI.Controls.Preferences;

namespace GmailNotifierPlus.Forms {
	
	public partial class Preferences : PreferencesForm {

		private PreferencesPanel _PanelAccounts;
		private Label _LabelAccountIntro;
		private PreferencesPanel _PanelAppearance;
		private CheckBox _CheckToast;
		private CheckBox _CheckTray;
		private CheckBox _CheckFlash;
		private Label _LabelLanguage;
		private ComboBox _ComboLanguage;
		private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
		private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
		private Controls.AccountPanel _PanelNewAccount;
		private CheckBox _CheckUpdates;
		private Label _LabelSound;
		private Label _LabelInterval;
		private Label _LabelMinutes;
		private TextBox _TextInterval;
		private ComboBox _ComboSound;
		private Button _ButtonBrowse;
		private ShapeContainer shapeContainer1;
		private LineShape lineShape3;
		private LineShape lineShape1;
		private Button _ButtonNewAccount;

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		// keep your stinkin' hands off of these, designer
		private PreferencesButton _ButtonAccounts;
		private PreferencesButton _ButtonAppearance;

		public Preferences() : base() {
			InitializeComponent();

			InitButtons();

			DataBind();

			InitAccounts();
			InitLabels();
			InitPanels();

			this.Icon = Program.Icon;
			this.Text = String.Concat(Resources.WindowTitle, " - ", Locale.Current.Labels.ConfigurationShort);

			_ButtonBrowse.Click += _ButtonBrowse_Click;

			Config.Current.LanguageChanged += _Config_LanguageChanged;

			Color panelContentColor = Color.FromArgb(200, 255, 255, 255);

			foreach (PreferencesPanel panel in _panels) {
				panel.ControlBackColor = panelContentColor;
			}
			
		}

		public void InitFirstRun() {
			_ButtonAccounts.Activate();

			HidePanels();

			_PanelNewAccount.Show();

		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			SetForegroundWindow(base.Handle);
		}

		private void DataBind() {

			_TextInterval.Text = (Config.Current.Interval / 60).ToString();

			_CheckTray.Checked = Config.Current.ShowTrayIcon;
			_CheckToast.Checked = Config.Current.ShowToast;
			_CheckFlash.Checked = Config.Current.FlashTaskbar;
			_CheckUpdates.Checked = Config.Current.CheckForUpdates;

			String columnName = "Name";
			String columnValue = "Value";
			DataTable dsLang = new DataTable();
			DataTable dsSound = new DataTable();
			Config config = Config.Current;

			dsLang.Columns.Add(columnName, typeof(string));
			dsLang.Columns.Add(columnValue, typeof(string));

			foreach (KeyValuePair<String, String> kvp in Utilities.ResourceHelper.AvailableLocales) {
				dsLang.Rows.Add(new string[] { kvp.Key, kvp.Value });
			}

			dsSound.Columns.Add(columnName, typeof(string));
			dsSound.Columns.Add(columnValue, typeof(string));
			dsSound.Rows.Add(new string[] { Locale.Current.Labels.NoSound, string.Empty });
			dsSound.Rows.Add(new string[] { Locale.Current.Labels.DefaultSound, string.Empty });

			if (System.IO.File.Exists(config.Sound)) {
				dsSound.Rows.Add(new string[] { System.IO.Path.GetFileName(config.Sound), config.Sound });
			}
			else {
				dsSound.Rows.Add(new string[] { Locale.Current.Labels.CustomSound, string.Empty });
			}

			_ComboLanguage.DataSource = dsLang;
			_ComboLanguage.DisplayMember = columnName;
			_ComboLanguage.ValueMember = columnValue;
			_ComboLanguage.SelectedValue = config.Language;

			_ComboSound.DataSource = dsSound;
			_ComboSound.DisplayMember = columnName;
			_ComboSound.ValueMember = columnValue;
			_ComboSound.SelectedIndex = (int)config.SoundNotification;
		}

		private void FixInterval() {
			int num;
			if (!int.TryParse(_TextInterval.Text, out num) || (num <= 0)) {
				_TextInterval.Text = "1";
			}
		}

		private void InitAccounts() {
			foreach (Account account in Config.Current.Accounts) {
				PreferencesButtonItem item = new PreferencesButtonItem() {
					ButtonText = account.FullAddress,
					AssociatedPanel = new AccountPanel() {
						Account = account,
						Dock = DockStyle.Fill,
						ControlBackColor = _PanelAccounts.ControlBackColor
					}
				};

				InitPanelShapes(item.AssociatedPanel);
				item.AssociatedPanel.Hide();
				_PanelParent.Controls.Add(item.AssociatedPanel);
				item.AssociatedPanel.BringToFront();

				if (account.Default) {
					item.Font = new Font(item.Font, FontStyle.Bold);
				}

				_ButtonAccounts.ButtonItems.Add(item);
			}
		}

		private void InitButtons() {

			_ButtonAccounts = new PreferencesButton();
			_ButtonAppearance = new PreferencesButton();

			_ButtonAccounts.AssociatedPanel = _PanelAccounts;
			_ButtonAccounts.ButtonText = "Accounts";
			_ButtonAccounts.TabIndex = 7;

			_ButtonAppearance.AssociatedPanel = _PanelAppearance;
			_ButtonAppearance.ButtonText = "Appearance";
			_ButtonAppearance.TabIndex = 8;

			// FlowLayoutPanel doesn't support visual inheritance, and ButtonGroup inherits from that, so we can't add 
			// controls to it via the designer. Way to make it suck royal ass, Microsoft.
			_ButtonGroup.Controls.Add(_ButtonAccounts);
			_ButtonGroup.Controls.Add(_ButtonAppearance);

		}

		private void InitLabels() {

			_ButtonNewAccount.Text = Locale.Current.Buttons.AddNewAccount;
			_ButtonGeneral.ButtonText = Locale.Current.Config.General;
			_ButtonAccounts.ButtonText = Locale.Current.Config.Accounts;
			_ButtonAppearance.ButtonText = Locale.Current.Config.Appearance;

			//_LabelAccountTitle.Text = Locale.Current.Buttons.AddNewAccount;
			_LabelSound.Text = Locale.Current.Labels.Sound;
			_LabelInterval.Text = Locale.Current.Labels.Interval;
			_LabelMinutes.Text = Locale.Current.Labels.Minutes;
			_LabelLanguage.Text = Locale.Current.Labels.Language;

			_TextInterval.Left = _LabelInterval.Left + _LabelInterval.Width + 4;
			_LabelMinutes.Left = _TextInterval.Left + _TextInterval.Width + 4;

			_PanelAccounts.HeaderText = Locale.Current.Config.Panels.Accounts;
			_PanelAppearance.HeaderText = Locale.Current.Config.Panels.Accounts;
			_PanelGeneral.HeaderText = Locale.Current.Config.Panels.Accounts;

			_CheckFlash.Text = Locale.Current.Checkboxes.FlashTaskbar;
			_CheckToast.Text = Locale.Current.Checkboxes.ShowToast;
			_CheckTray.Text = Locale.Current.Checkboxes.ShowTray;
			_CheckUpdates.Text = Locale.Current.Checkboxes.CheckUpdates;

		}

		private void InitPanels() {

			foreach (Control control in _PanelParent.Controls) {
				if (control is PreferencesPanel) {
					InitPanelShapes(control as PreferencesPanel);
				}
			}

		}

		private void InitPanelShapes(PreferencesPanel panel) {
			foreach (Control c in panel.Controls) {
				if (c is ShapeContainer) {
					foreach (var shape in (c as ShapeContainer).Shapes) {
						if (shape is LineShape) {
							LineShape line = (shape as LineShape);

							line.X1 = 0;
							line.X2 = _PanelGeneral.ClientSize.Width; // we can use this as a gauge since theyre all the same size.
							line.BorderColor = SystemColors.ControlLight;

							if (panel is AccountPanel) {
								line.X1 = 8;
								line.X2 -= 12; // account panels have no padding, 10 - 2 for the borders. yeah yeah, it's not dynamic. PPFTTT.
							}

						}
					}
				}
			}
		}

		private void _Config_LanguageChanged(Config sender) {
			InitLabels();
		}

		private void _ButtonNewAccount_Click(object sender, EventArgs e) {
			//_PanelAccounts.Hide();
			//_PanelNewAccount.Show();
		}

		//private void _ButtonNewCancel_Click(object sender, EventArgs e) {
		//  _PanelNewAccount.Hide();
		//  _PanelAccounts.Show();

		//  _TextUsername.Text = String.Empty;
		//  _TextPassword.Text = String.Empty;
		//  _PictureExclamation.Visible = _LabelError.Visible = false;
		//}

		//private void _ButtonNewSave_Click(object sender, EventArgs e) {
		//  Account account = new Account(_TextUsername.Text, _TextPassword.Text);

		//  if (_ButtonAccounts.ButtonItems.Count == 0) {
		//    account.Default = true;
		//  }

		//  PreferencesButtonItem item = new PreferencesButtonItem() {
		//    ButtonText = account.FullAddress,
		//    AssociatedPanel = new AccountPanel() {
		//      Account = account,
		//      ControlBackColor = _PanelAccounts.ControlBackColor
		//    }
		//  };

		//  Config.Current.Accounts.Add(account);
		//  Config.Current.Save();

		//  _PanelParent.Controls.Add(item.AssociatedPanel);

		//  InitPanelShapes(item.AssociatedPanel);

		//  item.AssociatedPanel.BringToFront();

		//  _ButtonAccounts.ButtonItems.Add(item);

		//  _PanelNewAccount.Hide();

		//  _TextUsername.Text = String.Empty;
		//  _TextPassword.Text = String.Empty;
		//  _PictureExclamation.Visible = _LabelError.Visible = false;

		//}

		private void _ButtonSave_Click(object sender, EventArgs e) {
			this.FixInterval();

			Config.Current.Interval = Convert.ToInt32(_TextInterval.Text) * 60;
			Config.Current.Language = _ComboLanguage.SelectedValue.ToString();
			Config.Current.ShowToast = _CheckToast.Checked;
			Config.Current.ShowTrayIcon = _CheckTray.Checked;
			Config.Current.CheckForUpdates = _CheckUpdates.Checked;
			Config.Current.FlashTaskbar = _CheckFlash.Checked;

			if ((_ComboSound.SelectedIndex == 2) && string.IsNullOrEmpty(_ComboSound.SelectedValue.ToString())) {
				Config.Current.SoundNotification = 0;
			}
			else {
				Config.Current.SoundNotification = (SoundNotification)_ComboSound.SelectedIndex;
				Config.Current.Sound = _ComboSound.SelectedValue.ToString();
			}

			Config.Current.Save();

			base.Close();
		}

		private void _TextInterval_Leave(object sender, EventArgs e) {
			this.FixInterval();
		}

		private void _ButtonBrowse_Click(object sender, EventArgs e) {
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

		private void InitializeComponent() {
			this._PanelAccounts = new Shellscape.UI.Controls.Preferences.PreferencesPanel();
			this._LabelAccountIntro = new System.Windows.Forms.Label();
			this._ButtonNewAccount = new System.Windows.Forms.Button();
			this._PanelAppearance = new Shellscape.UI.Controls.Preferences.PreferencesPanel();
			this._CheckToast = new System.Windows.Forms.CheckBox();
			this._CheckTray = new System.Windows.Forms.CheckBox();
			this._CheckFlash = new System.Windows.Forms.CheckBox();
			this._LabelLanguage = new System.Windows.Forms.Label();
			this._ComboLanguage = new System.Windows.Forms.ComboBox();
			this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
			this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
			this._PanelNewAccount = new GmailNotifierPlus.Controls.AccountPanel();
			this._CheckUpdates = new System.Windows.Forms.CheckBox();
			this._LabelSound = new System.Windows.Forms.Label();
			this._LabelInterval = new System.Windows.Forms.Label();
			this._LabelMinutes = new System.Windows.Forms.Label();
			this._TextInterval = new System.Windows.Forms.TextBox();
			this._ComboSound = new System.Windows.Forms.ComboBox();
			this._ButtonBrowse = new System.Windows.Forms.Button();
			this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
			this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
			this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
			this._PanelGeneral.SuspendLayout();
			this._PanelParent.SuspendLayout();
			this._PanelAccounts.SuspendLayout();
			this._PanelAppearance.SuspendLayout();
			this.SuspendLayout();
			// 
			// _PanelGeneral
			// 
			this._PanelGeneral.Controls.Add(this._ButtonBrowse);
			this._PanelGeneral.Controls.Add(this._ComboSound);
			this._PanelGeneral.Controls.Add(this._TextInterval);
			this._PanelGeneral.Controls.Add(this._LabelMinutes);
			this._PanelGeneral.Controls.Add(this._LabelInterval);
			this._PanelGeneral.Controls.Add(this._LabelSound);
			this._PanelGeneral.Controls.Add(this._CheckUpdates);
			this._PanelGeneral.Controls.Add(this.shapeContainer1);
			// 
			// _PanelParent
			// 
			this._PanelParent.Controls.Add(this._PanelNewAccount);
			this._PanelParent.Controls.Add(this._PanelAppearance);
			this._PanelParent.Controls.Add(this._PanelAccounts);
			this._PanelParent.Controls.SetChildIndex(this._PanelAccounts, 0);
			this._PanelParent.Controls.SetChildIndex(this._PanelAppearance, 0);
			this._PanelParent.Controls.SetChildIndex(this._PanelNewAccount, 0);
			this._PanelParent.Controls.SetChildIndex(this._PanelGeneral, 0);
			// 
			// _PanelAccounts
			// 
			this._PanelAccounts.BackColor = System.Drawing.Color.Transparent;
			this._PanelAccounts.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._PanelAccounts.ControlBackColor = System.Drawing.Color.White;
			this._PanelAccounts.Controls.Add(this._LabelAccountIntro);
			this._PanelAccounts.Controls.Add(this._ButtonNewAccount);
			this._PanelAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelAccounts.DrawHeader = true;
			this._PanelAccounts.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._PanelAccounts.HeaderColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._PanelAccounts.HeaderColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._PanelAccounts.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._PanelAccounts.HeaderHeight = 39;
			this._PanelAccounts.HeaderImage = null;
			this._PanelAccounts.HeaderPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._PanelAccounts.HeaderText = "Manage your accounts";
			this._PanelAccounts.Location = new System.Drawing.Point(12, 12);
			this._PanelAccounts.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
			this._PanelAccounts.Name = "_PanelAccounts";
			this._PanelAccounts.Padding = new System.Windows.Forms.Padding(10, 45, 10, 10);
			this._PanelAccounts.SeperatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._PanelAccounts.Size = new System.Drawing.Size(500, 394);
			this._PanelAccounts.TabIndex = 3;
			// 
			// _LabelAccountIntro
			// 
			this._LabelAccountIntro.AutoSize = true;
			this._LabelAccountIntro.Location = new System.Drawing.Point(13, 45);
			this._LabelAccountIntro.MaximumSize = new System.Drawing.Size(450, 0);
			this._LabelAccountIntro.Name = "_LabelAccountIntro";
			this._LabelAccountIntro.Size = new System.Drawing.Size(436, 30);
			this._LabelAccountIntro.TabIndex = 1;
			this._LabelAccountIntro.Text = "Click an account to the left to manage your individual accounts. Click the button" +
    " below to add a new account.";
			// 
			// _ButtonNewAccount
			// 
			this._ButtonNewAccount.AutoSize = true;
			this._ButtonNewAccount.Location = new System.Drawing.Point(292, 92);
			this._ButtonNewAccount.Name = "_ButtonNewAccount";
			this._ButtonNewAccount.Size = new System.Drawing.Size(170, 30);
			this._ButtonNewAccount.TabIndex = 0;
			this._ButtonNewAccount.Text = "Add New Account";
			this._ButtonNewAccount.UseVisualStyleBackColor = true;
			// 
			// _PanelAppearance
			// 
			this._PanelAppearance.BackColor = System.Drawing.Color.Transparent;
			this._PanelAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._PanelAppearance.ControlBackColor = System.Drawing.Color.White;
			this._PanelAppearance.Controls.Add(this._CheckToast);
			this._PanelAppearance.Controls.Add(this._CheckTray);
			this._PanelAppearance.Controls.Add(this._CheckFlash);
			this._PanelAppearance.Controls.Add(this._LabelLanguage);
			this._PanelAppearance.Controls.Add(this._ComboLanguage);
			this._PanelAppearance.Controls.Add(this.shapeContainer2);
			this._PanelAppearance.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelAppearance.DrawHeader = true;
			this._PanelAppearance.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._PanelAppearance.HeaderColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._PanelAppearance.HeaderColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._PanelAppearance.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._PanelAppearance.HeaderHeight = 39;
			this._PanelAppearance.HeaderImage = null;
			this._PanelAppearance.HeaderPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._PanelAppearance.HeaderText = "";
			this._PanelAppearance.Location = new System.Drawing.Point(12, 12);
			this._PanelAppearance.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
			this._PanelAppearance.Name = "_PanelAppearance";
			this._PanelAppearance.Padding = new System.Windows.Forms.Padding(10, 45, 10, 10);
			this._PanelAppearance.SeperatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._PanelAppearance.Size = new System.Drawing.Size(500, 394);
			this._PanelAppearance.TabIndex = 4;
			// 
			// _CheckToast
			// 
			this._CheckToast.AutoSize = true;
			this._CheckToast.Location = new System.Drawing.Point(15, 141);
			this._CheckToast.Name = "_CheckToast";
			this._CheckToast.Size = new System.Drawing.Size(231, 19);
			this._CheckToast.TabIndex = 3;
			this._CheckToast.Text = "Show Toast notifications for new Email";
			this._CheckToast.UseVisualStyleBackColor = true;
			// 
			// _CheckTray
			// 
			this._CheckTray.AutoSize = true;
			this._CheckTray.Location = new System.Drawing.Point(15, 116);
			this._CheckTray.Name = "_CheckTray";
			this._CheckTray.Size = new System.Drawing.Size(201, 19);
			this._CheckTray.TabIndex = 2;
			this._CheckTray.Text = "Show Email count in System Tray";
			this._CheckTray.UseVisualStyleBackColor = true;
			// 
			// _CheckFlash
			// 
			this._CheckFlash.AutoSize = true;
			this._CheckFlash.Location = new System.Drawing.Point(15, 91);
			this._CheckFlash.Name = "_CheckFlash";
			this._CheckFlash.Size = new System.Drawing.Size(172, 19);
			this._CheckFlash.TabIndex = 1;
			this._CheckFlash.Text = "Flash Taskbar for new Email";
			this._CheckFlash.UseVisualStyleBackColor = true;
			// 
			// _LabelLanguage
			// 
			this._LabelLanguage.AutoSize = true;
			this._LabelLanguage.Location = new System.Drawing.Point(11, 51);
			this._LabelLanguage.Name = "_LabelLanguage";
			this._LabelLanguage.Size = new System.Drawing.Size(100, 15);
			this._LabelLanguage.TabIndex = 48;
			this._LabelLanguage.Text = "Display language:";
			// 
			// _ComboLanguage
			// 
			this._ComboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._ComboLanguage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ComboLanguage.FormattingEnabled = true;
			this._ComboLanguage.Location = new System.Drawing.Point(117, 48);
			this._ComboLanguage.Name = "_ComboLanguage";
			this._ComboLanguage.Size = new System.Drawing.Size(134, 23);
			this._ComboLanguage.TabIndex = 0;
			// 
			// shapeContainer2
			// 
			this.shapeContainer2.Location = new System.Drawing.Point(10, 45);
			this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
			this.shapeContainer2.Name = "shapeContainer2";
			this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2});
			this.shapeContainer2.Size = new System.Drawing.Size(480, 339);
			this.shapeContainer2.TabIndex = 50;
			this.shapeContainer2.TabStop = false;
			// 
			// lineShape2
			// 
			this.lineShape2.BorderColor = System.Drawing.Color.Red;
			this.lineShape2.Name = "lineShape2";
			this.lineShape2.X1 = -6;
			this.lineShape2.X2 = 294;
			this.lineShape2.Y1 = 35;
			this.lineShape2.Y2 = 35;
			// 
			// _PanelNewAccount
			// 
			this._PanelNewAccount.Account = null;
			this._PanelNewAccount.BackColor = System.Drawing.Color.Transparent;
			this._PanelNewAccount.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._PanelNewAccount.ControlBackColor = System.Drawing.Color.White;
			this._PanelNewAccount.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelNewAccount.DrawHeader = true;
			this._PanelNewAccount.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._PanelNewAccount.HeaderColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._PanelNewAccount.HeaderColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._PanelNewAccount.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._PanelNewAccount.HeaderHeight = 39;
			this._PanelNewAccount.HeaderImage = null;
			this._PanelNewAccount.HeaderPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._PanelNewAccount.HeaderText = "Add a New Account";
			this._PanelNewAccount.Location = new System.Drawing.Point(12, 12);
			this._PanelNewAccount.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
			this._PanelNewAccount.Name = "_PanelNewAccount";
			this._PanelNewAccount.Padding = new System.Windows.Forms.Padding(2, 45, 2, 2);
			this._PanelNewAccount.SeperatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._PanelNewAccount.Size = new System.Drawing.Size(500, 394);
			this._PanelNewAccount.TabIndex = 5;
			// 
			// _CheckUpdates
			// 
			this._CheckUpdates.AutoSize = true;
			this._CheckUpdates.Location = new System.Drawing.Point(12, 165);
			this._CheckUpdates.Name = "_CheckUpdates";
			this._CheckUpdates.Size = new System.Drawing.Size(123, 19);
			this._CheckUpdates.TabIndex = 51;
			this._CheckUpdates.Text = "Check for Updates";
			this._CheckUpdates.UseVisualStyleBackColor = true;
			// 
			// _LabelSound
			// 
			this._LabelSound.AutoSize = true;
			this._LabelSound.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._LabelSound.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
			this._LabelSound.Location = new System.Drawing.Point(8, 48);
			this._LabelSound.Name = "_LabelSound";
			this._LabelSound.Size = new System.Drawing.Size(39, 21);
			this._LabelSound.TabIndex = 52;
			this._LabelSound.Text = "Title";
			// 
			// _LabelInterval
			// 
			this._LabelInterval.AutoSize = true;
			this._LabelInterval.Location = new System.Drawing.Point(12, 123);
			this._LabelInterval.Name = "_LabelInterval";
			this._LabelInterval.Size = new System.Drawing.Size(106, 15);
			this._LabelInterval.TabIndex = 53;
			this._LabelInterval.Text = "Check Email every:";
			// 
			// _LabelMinutes
			// 
			this._LabelMinutes.AutoSize = true;
			this._LabelMinutes.Location = new System.Drawing.Point(157, 123);
			this._LabelMinutes.Name = "_LabelMinutes";
			this._LabelMinutes.Size = new System.Drawing.Size(58, 15);
			this._LabelMinutes.TabIndex = 54;
			this._LabelMinutes.Text = "minute(s)";
			// 
			// _TextInterval
			// 
			this._TextInterval.Location = new System.Drawing.Point(124, 122);
			this._TextInterval.Name = "_TextInterval";
			this._TextInterval.Size = new System.Drawing.Size(27, 23);
			this._TextInterval.TabIndex = 55;
			this._TextInterval.Text = "1";
			this._TextInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// _ComboSound
			// 
			this._ComboSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._ComboSound.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ComboSound.FormattingEnabled = true;
			this._ComboSound.Location = new System.Drawing.Point(12, 80);
			this._ComboSound.Name = "_ComboSound";
			this._ComboSound.Size = new System.Drawing.Size(164, 23);
			this._ComboSound.TabIndex = 56;
			// 
			// _ButtonBrowse
			// 
			this._ButtonBrowse.AutoSize = true;
			this._ButtonBrowse.Enabled = false;
			this._ButtonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._ButtonBrowse.Location = new System.Drawing.Point(182, 79);
			this._ButtonBrowse.MinimumSize = new System.Drawing.Size(65, 23);
			this._ButtonBrowse.Name = "_ButtonBrowse";
			this._ButtonBrowse.Size = new System.Drawing.Size(68, 26);
			this._ButtonBrowse.TabIndex = 57;
			this._ButtonBrowse.Text = "Browse...";
			this._ButtonBrowse.UseVisualStyleBackColor = true;
			// 
			// lineShape1
			// 
			this.lineShape1.BorderColor = System.Drawing.Color.Red;
			this.lineShape1.Name = "lineShape1";
			this.lineShape1.X1 = -3;
			this.lineShape1.X2 = 297;
			this.lineShape1.Y1 = 67;
			this.lineShape1.Y2 = 67;
			// 
			// shapeContainer1
			// 
			this.shapeContainer1.Location = new System.Drawing.Point(10, 45);
			this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
			this.shapeContainer1.Name = "shapeContainer1";
			this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape3,
            this.lineShape1});
			this.shapeContainer1.Size = new System.Drawing.Size(480, 339);
			this.shapeContainer1.TabIndex = 58;
			this.shapeContainer1.TabStop = false;
			// 
			// lineShape3
			// 
			this.lineShape3.BorderColor = System.Drawing.Color.Red;
			this.lineShape3.Name = "lineShape3";
			this.lineShape3.X1 = 1;
			this.lineShape3.X2 = 301;
			this.lineShape3.Y1 = 109;
			this.lineShape3.Y2 = 109;
			// 
			// Preferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(724, 412);
			this.Name = "Preferences";
			this._PanelGeneral.ResumeLayout(false);
			this._PanelGeneral.PerformLayout();
			this._PanelParent.ResumeLayout(false);
			this._PanelAccounts.ResumeLayout(false);
			this._PanelAccounts.PerformLayout();
			this._PanelAppearance.ResumeLayout(false);
			this._PanelAppearance.PerformLayout();
			this.ResumeLayout(false);

		}
		
	}
}
