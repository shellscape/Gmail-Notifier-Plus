using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GmailNotifierPlus.Controls;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

using Shellscape.UI.Skipe;

namespace GmailNotifierPlus.Forms {

	public partial class Settings : SkipeForm {

		private static String[] _backgrounds = new String[] { "bleu", "classic", "kuro", "limon", "orangish", "pinky" };

		private static Bitmap _backgroundBitmap;

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		public Settings() {
			InitializeComponent();
			InitLabels();
			DataBind();

			this.Icon = Program.Icon;
			this.Text = String.Concat(Resources.WindowTitle, " - ", Locale.Current.Labels.ConfigurationShort);

			_ButtonGroup.SendToBack();

			_TextPassword.TextChanged += _Inputs_Changed;
			_TextUsername.TextChanged += _Inputs_Changed;

			Config.Current.LanguageChanged += _Config_LanguageChanged;

			foreach (Account account in Config.Current.Accounts) {
				SkipeButtonItem item = new SkipeButtonItem() {
					ButtonText = account.FullAddress,
					AssociatedPanel = new AccountPanel() {
						Account = account,
						ControlBackColor = _PanelAccounts.ControlBackColor
					}
				};

				item.AssociatedPanel.Hide();
				this.Controls.Add(item.AssociatedPanel);
				item.AssociatedPanel.BringToFront();

				if (account.Default) {
					item.Font = new Font(item.Font, FontStyle.Bold);
				}

				_ButtonAccounts.ButtonItems.Add(item);
			}

			Color panelContentColor = Color.FromArgb(200, 255, 255, 255);

			foreach (SkipePanel panel in _panels) {
				panel.ControlBackColor = panelContentColor;
			}

			_backgroundBitmap = new Bitmap(this.Width, this.Height);

			using (Bitmap background = GetBackground())
			using (Graphics g = Graphics.FromImage(_backgroundBitmap))
			using (ImageAttributes ia = new ImageAttributes()) {

				Rectangle destRect = new Rectangle(-100, -100, background.Width + 300, background.Height + 300);
				ColorMatrix cm = new ColorMatrix();
				cm.Matrix33 = 0.3f;

				ia.SetColorMatrix(cm);

				g.DrawImage(background, destRect, 0, 0, background.Width, background.Height, GraphicsUnit.Pixel, ia);

			}
		}

		public void InitFirstRun() {
			_ButtonAccounts.Activate();

			HidePanels();

			_PanelNewAccount.Show();

		}

		private void InitLabels() {

			if (!DesignMode) {
				_PictureExclamation.Visible = false;
				_LabelError.Visible = false;
			}

			_ButtonNewAccount.Text = Locale.Current.Buttons.AddNewAccount;
			_ButtonNewSave.Text = Locale.Current.Buttons.Save;
			_ButtonNewCancel.Text = Locale.Current.Buttons.Cancel;
			_ButtonGeneral.ButtonText = Locale.Current.Config.General;
			_ButtonAccounts.ButtonText = Locale.Current.Config.Accounts;
			_ButtonAppearance.ButtonText = Locale.Current.Config.Appearance;

			_LabelUsername.Text = Locale.Current.Labels.Login;
			_LabelPassword.Text = Locale.Current.Labels.Password;
			_LabelError.Text = Locale.Current.Labels.Error;
			_LabelAccountTitle.Text = Locale.Current.Buttons.AddNewAccount;
			_LabelSound.Text = Locale.Current.Labels.Sound;
			_LabelAdditional.Text = Locale.Current.Labels.Additional;
			_LabelInterval.Text = Locale.Current.Labels.Interval;
			_LabelMinutes.Text = Locale.Current.Labels.Minutes;
			_LabelLanguage.Text = Locale.Current.Labels.Language;

			_TextInterval.Left = _LabelInterval.Left + _LabelInterval.Width + 4;
			_LabelMinutes.Left = _TextInterval.Left + _TextInterval.Width + 4;

			_PanelAccounts.HeaderText = Locale.Current.Config.Panels.Accounts;
			_PanelAppearance.HeaderText = Locale.Current.Config.Panels.Accounts;
			_PanelGeneral.HeaderText = Locale.Current.Config.Panels.Accounts;
				
		}

		private void DataBind() {

			_TextInterval.Text = (Config.Current.Interval / 60).ToString();

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
			_ComboSound.SelectedIndex = config.SoundNotification;

			_PictureExclamation.Image = Utilities.ResourceHelper.GetImage("Exclamation.png");

		}

		protected override SkipeButtonGroup ButtonGroup { get { return _ButtonGroup; } }

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			SetForegroundWindow(base.Handle);
		}

		protected override void OnPaint(PaintEventArgs e) {
			e.Graphics.DrawImage(_backgroundBitmap, 0, 0, _backgroundBitmap.Width, _backgroundBitmap.Height);

			base.OnPaint(e);
		}

		private Bitmap GetBackground() {
			int day = (int)DateTime.Now.DayOfWeek;

			if (day > 5) {
				Random random = new Random();
				day = random.Next(0, 5);
			}

			String which = _backgrounds[day];

			return Utilities.ResourceHelper.GetImage(String.Concat("gmail-", which, ".png"));
		}

		private void FixInterval() {
			int num;
			if (!int.TryParse(_TextInterval.Text, out num) || (num <= 0)) {
				_TextInterval.Text = "1";
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

		private void _Config_LanguageChanged(Config sender) {
			InitLabels();
		}

		private void _ButtonNewAccount_Click(object sender, EventArgs e) {
			_PanelAccounts.Hide();
			_PanelNewAccount.Show();
		}

		private void _ButtonNewCancel_Click(object sender, EventArgs e) {
			_PanelNewAccount.Hide();
			_PanelAccounts.Show();

			_TextUsername.Text = String.Empty;
			_TextPassword.Text = String.Empty;
			_PictureExclamation.Visible = _LabelError.Visible = false;
		}

		private void _ButtonNewSave_Click(object sender, EventArgs e) {
			Account account = new Account(_TextUsername.Text, _TextPassword.Text);

			if (_ButtonAccounts.ButtonItems.Count == 0) {
				account.Default = true;
			}

			SkipeButtonItem item = new SkipeButtonItem() {
				ButtonText = account.FullAddress,
				AssociatedPanel = new AccountPanel() {
					Account = account,
					ControlBackColor = _PanelAccounts.ControlBackColor
				}
			};

			Config.Current.Accounts.Add(account);
			Config.Current.Save();

			this.Controls.Add(item.AssociatedPanel);

			item.AssociatedPanel.BringToFront();

			_ButtonAccounts.ButtonItems.Add(item);

			_PanelNewAccount.Hide();

			_TextUsername.Text = String.Empty;
			_TextPassword.Text = String.Empty;
			_PictureExclamation.Visible = _LabelError.Visible = false;

		}

		private void _ButtonCancel_Click(object sender, EventArgs e) {
			base.Close();
		}

		private void _ButtonSave_Click(object sender, EventArgs e) {
			this.FixInterval();

			//config.Accounts.Clear();
			//config.Accounts.AddRange(this._ListAccounts.Values);
			Config.Current.Interval = Convert.ToInt32(_TextInterval.Text) * 60;
			Config.Current.Language = _ComboLanguage.SelectedValue.ToString();

			//foreach (Account account in config.Accounts) {
			//  account.Default = false;
			//}

			//if (_Config.Accounts.Count > this._DefaultAccountIndex) {
			//  _Config.Accounts[this._DefaultAccountIndex].Default = true;
			//}

			if ((_ComboSound.SelectedIndex == 2) && string.IsNullOrEmpty(_ComboSound.SelectedValue.ToString())) {
				Config.Current.SoundNotification = 0;
			}
			else {
				Config.Current.SoundNotification = _ComboSound.SelectedIndex;
				Config.Current.Sound = _ComboSound.SelectedValue.ToString();
			}

			Config.Current.Save();

			base.Close();
		}

		private void _TextInterval_Leave(object sender, EventArgs e) {
			this.FixInterval();
		}

		private void _ButtonBrowse_Click(object sender, EventArgs e) {
			this.SelectCustomSound();
		}

		private void _Inputs_Changed(object sender, EventArgs e) {

			Boolean enabled = !String.IsNullOrEmpty(_TextUsername.Text) && !String.IsNullOrEmpty(_TextPassword.Text);

			_ButtonNewSave.Enabled = enabled;

		}

	}
}
