namespace GmailNotifierPlus {
	
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

	using VistaControls;

	using GmailNotifierPlus.Controls;

	public class Settings : Form {
		private Dictionary<string, Account> accountsList = new Dictionary<string, Account>();
		private const int animationOffset = 300;
		private readonly int animationSpeed = 30;
		private readonly Font BoldFont;
		private System.Windows.Forms.Button btn_aboutOk;
		private System.Windows.Forms.Button btn_accountCancel;
		private System.Windows.Forms.Button btn_accountSave;
		private System.Windows.Forms.Button btn_browse;
		private System.Windows.Forms.Button btn_cancel;
		private System.Windows.Forms.Button btn_default;
		private System.Windows.Forms.Button btn_edit;
		private System.Windows.Forms.Button btn_ok;
		private VistaControls.ComboBox cbb_language;
		private VistaControls.ComboBox cbb_sound;
		private ColumnHeader ch_name;
		private IContainer components;
		private int defaultAccountIndex;
		private bool isEdit;
		private Label lbl_accountTitle;
		private Label lbl_additional;
		private Label lbl_error;
		private Label lbl_interval;
		private Label lbl_language;
		private Label lbl_minutes;
		private Label lbl_password;
		private Label lbl_sound;
		private Label lbl_title;
		private Label lbl_username;
		private VistaControls.ListView lv_accounts;
		private ImageButton pic_about;
		private PictureBox pic_aboutBackground;
		private ImageButton pic_add;
		private PictureBox pic_copyrights;
		private PictureBox pic_disclaimer;
		private ImageButton pic_donate;
		private PictureBox pic_exclamation;
		private PictureBox pic_line;
		private ImageButton pic_remove;
		private PictureBox pic_welcome;
		private Panel pnl_about;
		private Panel pnl_aboutButtons;
		private Panel pnl_account;
		private Panel pnl_accountButtons;
		private Panel pnl_buttons;
		private Panel pnl_main;
		private readonly Font RegularFont;
		private ResourceManager resourceManager;
		private Config config = Config.Current;
		private Timer tmr_aboutAnimationIn;
		private Timer tmr_aboutAnimationOut;
		private Timer tmr_animationIn;
		private Timer tmr_animationOut;
		private System.Windows.Forms.TextBox txt_interval;
		private VistaControls.TextBox txt_password;
		private VistaControls.TextBox txt_username;

		public Settings() {
			this.InitializeComponent();
			this.RegularFont = new Font(this.lv_accounts.Font, FontStyle.Regular);
			this.BoldFont = new Font(this.lv_accounts.Font, FontStyle.Bold);
			this.resourceManager = new ResourceManager("GmailNotifierPlus.Resources.Locales." + config.Language, Assembly.GetExecutingAssembly());
			string columnName = "Name";
			string str2 = "Value";
			DataTable table = new DataTable();
			table.Columns.Add(columnName, typeof(string));
			table.Columns.Add(str2, typeof(string));

			// TODO - Populate Lanuages
			//foreach (KeyValuePair<string, string> pair in Utilities.ResourceHelper.AvailableLocales) {
			//  table.Rows.Add(new string[] { pair.Key, pair.Value });
			//}

			this.cbb_language.DataSource = table;
			this.cbb_language.DisplayMember = columnName;
			this.cbb_language.ValueMember = str2;
			DataTable table2 = new DataTable();
			table2.Columns.Add(columnName, typeof(string));
			table2.Columns.Add(str2, typeof(string));
			table2.Rows.Add(new string[] { this.resourceManager.GetString("Label_NoSound"), string.Empty });
			table2.Rows.Add(new string[] { this.resourceManager.GetString("Label_DefaultSound"), string.Empty });
			if (File.Exists(config.Sound)) {
				table2.Rows.Add(new string[] { Path.GetFileName(config.Sound), config.Sound });
			}
			else {
				table2.Rows.Add(new string[] { this.resourceManager.GetString("Label_CustomSound"), string.Empty });
			}
			this.cbb_sound.DataSource = table2;
			this.cbb_sound.DisplayMember = columnName;
			this.cbb_sound.ValueMember = str2;
			foreach (Account account in config.Accounts) {
				this.lv_accounts.Items.Add(account.Login);
				this.accountsList.Add(account.Login, account);
			}
			this.defaultAccountIndex = config.Accounts.IndexOf(config.DefaultAccount);
			if (this.defaultAccountIndex < this.lv_accounts.Items.Count) {
				this.lv_accounts.Items[this.defaultAccountIndex].Font = this.BoldFont;
			}
			this.UpdateHeaderSize();
			this.cbb_sound.SelectedIndex = 0; // TODO - config.PlaySound;
			this.txt_interval.Text = (config.Interval / 60).ToString();
			this.cbb_language.SelectedValue = config.Language;
			this.Text = Resources.Resources.WindowTitle;
			this.lbl_title.Text = this.resourceManager.GetString("Label_Configuration");
			this.lbl_sound.Text = this.resourceManager.GetString("Label_Sound");
			this.lbl_additional.Text = this.resourceManager.GetString("Label_Additional");
			this.lbl_interval.Text = this.resourceManager.GetString("Label_Interval");
			this.lbl_minutes.Text = this.resourceManager.GetString("Label_Minutes");
			this.lbl_language.Text = this.resourceManager.GetString("Label_Language");
			this.btn_default.Text = this.resourceManager.GetString("Button_Default");
			this.btn_edit.Text = this.resourceManager.GetString("Button_Edit");
			this.btn_browse.Text = this.resourceManager.GetString("Button_Browse");
			this.btn_ok.Text = this.btn_aboutOk.Text = this.resourceManager.GetString("Button_OK");
			this.btn_cancel.Text = this.btn_accountCancel.Text = this.resourceManager.GetString("Button_Cancel");
			this.lbl_username.Text = this.resourceManager.GetString("Label_Login");
			this.lbl_password.Text = this.resourceManager.GetString("Label_Password");
			this.lbl_error.Text = this.resourceManager.GetString("Label_Error");
			ToolTip tip = new ToolTip();
			tip.SetToolTip(this.pic_about, this.resourceManager.GetString("Tooltip_About"));
			tip.SetToolTip(this.pic_add, this.resourceManager.GetString("Tooltip_Add"));
			tip.SetToolTip(this.pic_remove, this.resourceManager.GetString("Tooltip_Remove"));
			this.pic_exclamation.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.Exclamation.png"));
			this.pic_exclamation.Height = this.lbl_error.Height;
			this.pic_line.BackgroundImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.Line.png"));
			this.pic_aboutBackground.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.About.About.png"));
			this.pic_copyrights.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.About.Copyrights.png"));
			this.pic_about.SetImage(new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.Information.png")));
			this.pic_add.SetImages(new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.Add.png")), new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.AddDisabled.png")), new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.AddHover.png")), new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.AddPressed.png")));
			this.pic_remove.SetImages(new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.Remove.png")), new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.RemoveDisabled.png")), new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.RemoveHover.png")), new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.RemovePressed.png")));
			this.pic_donate.SetImage(new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.Donate.gif")));
			this.pic_remove.Enabled = false;
			if (config.FirstRun) {
				this.pic_welcome.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.About.Welcome.png"));
				this.pic_disclaimer.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("GmailNotifierPlus.Images.About.Disclaimer.png"));
				this.pnl_main.Left = this.pnl_buttons.Left = 300;
				this.pnl_about.Left = this.pnl_aboutButtons.Left += 300;
				this.SwitchToAbout();
				this.pic_welcome.Visible = this.pic_disclaimer.Visible = true;
				this.pic_copyrights.Visible = this.pic_donate.Visible = false;
				this.btn_aboutOk.Text = Locale.Current.Buttons.Next;
			}
			this.AdjustControls();
			if (Locale.Current.IsRightToLeftLanguage) {
				this.MirrorControls();
			}
		}

		private void AdjustControls() {
			int num = 6;
			int num2 = 10;
			int width = Math.Max(this.btn_default.Width, this.btn_edit.Width);
			this.btn_default.AutoSize = this.btn_edit.AutoSize = false;
			this.btn_default.Width = this.btn_edit.Width = width - num2;

			//if (config.Language == Resources.Code_Bulgarian) {
			//  this.btn_edit.Width -= 25;
			//}
			//else if (config.Language == Resources.Code_Ukrainian) {
			//  this.btn_edit.Width -= 40;
			//}

			this.btn_edit.Left = this.lv_accounts.Right - this.btn_edit.Width;
			this.btn_default.Left = (this.btn_edit.Left - this.btn_default.Width) - num;
			width = this.btn_browse.Width;
			this.btn_browse.AutoSize = false;
			this.btn_browse.Width = width - num2;
			this.btn_browse.Left = this.lv_accounts.Right - this.btn_browse.Width;
			this.cbb_sound.Width = (this.btn_browse.Left - num) - this.cbb_sound.Left;
			this.lbl_interval.MinimumSize = this.lbl_language.MinimumSize = new Size(((this.btn_default.Left - this.lbl_interval.Left) - num) + 1, this.lbl_interval.Height);
			int num5 = (this.lbl_interval.Left + Math.Max(this.lbl_interval.Width, this.lbl_language.Width)) + num;
			this.txt_interval.Left = num5;
			this.lbl_minutes.Left = (this.txt_interval.Left + this.txt_interval.Width) + num;
			this.cbb_language.Left = num5;
			this.cbb_language.Width = this.lv_accounts.Right - num5;
		}

		private void btn_aboutOk_Click(object sender, EventArgs e) {
			this.SwitchToSettings(SourceScreen.About);
		}

		private void btn_accountCancel_Click(object sender, EventArgs e) {
			this.SwitchToSettings(SourceScreen.Accounts);
		}

		private void btn_accountSave_Click(object sender, EventArgs e) {
			Account account = new Account(this.txt_username.Text, this.txt_password.Text);
			if (this.isEdit) {
				ListViewItem item = this.lv_accounts.SelectedItems[0];
				this.accountsList.Remove(item.Text);
				this.accountsList.Add(account.Login, account);
				this.lv_accounts.Items[this.lv_accounts.Items.IndexOf(item)] = new ListViewItem(account.Login);
			}
			else {
				this.accountsList.Add(account.Login, account);
				this.lv_accounts.Items.Add(account.Login);
				if (this.lv_accounts.Items.Count == 1) {
					this.defaultAccountIndex = 0;
					this.lv_accounts.Items[this.defaultAccountIndex].Font = this.BoldFont;
					this.btn_default.Enabled = false;
				}
			}
			this.UpdateHeaderSize();
			this.SwitchToSettings(SourceScreen.Accounts);
		}

		private void btn_browse_Click(object sender, EventArgs e) {
			this.SelectCustomSound();
		}

		private void btn_cancel_Click(object sender, EventArgs e) {
			base.Close();
		}

		private void btn_default_Click(object sender, EventArgs e) {
			int num = this.lv_accounts.SelectedIndices[0];
			this.lv_accounts.Items[this.defaultAccountIndex].Font = this.RegularFont;
			this.lv_accounts.Items[num].Font = this.BoldFont;
			this.defaultAccountIndex = num;
			this.btn_default.Enabled = false;
		}

		private void btn_edit_Click(object sender, EventArgs e) {
			this.EditAccount();
		}

		private void btn_ok_Click(object sender, EventArgs e) {
			
			this.FixInterval();

			config.Accounts.Clear();
			config.Accounts.AddRange(this.accountsList.Values);
			config.Interval = Convert.ToInt32(this.txt_interval.Text) * 60;
			config.Language = this.cbb_language.SelectedValue.ToString();

			foreach (Account account in config.Accounts) {
				account.Default = false;
			}

			config.Accounts[this.defaultAccountIndex].Default = true;

			if ((this.cbb_sound.SelectedIndex == 2) && string.IsNullOrEmpty(this.cbb_sound.SelectedValue.ToString())) {
				// TODO - config.SoundNotification = 0;
			}
			else {
				// TODO - config.SoundNotification = this.cbb_sound.SelectedIndex;
				config.Sound = this.cbb_sound.SelectedValue.ToString();
			}

			config.Save();
			base.Close();
		}

		private void cbb_sound_SelectedIndexChanged(object sender, EventArgs e) {
			this.btn_browse.Enabled = this.cbb_sound.SelectedIndex == 2;
			if (this.btn_browse.Enabled && string.IsNullOrEmpty(this.cbb_sound.SelectedValue.ToString())) {
				this.SelectCustomSound();
			}
		}

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void EditAccount() {
			this.isEdit = true;
			this.SwitchToAccounts();
		}

		private void FixInterval() {
			int num;
			if (!int.TryParse(this.txt_interval.Text, out num) || (num <= 0)) {
				this.txt_interval.Text = "1";
			}
		}

		private void InitializeComponent() {
			this.components = new Container();
			ComponentResourceManager manager = new ComponentResourceManager(typeof(Settings));
			this.btn_ok = new System.Windows.Forms.Button();
			this.btn_cancel = new System.Windows.Forms.Button();
			this.pnl_buttons = new Panel();
			this.pic_about = new ImageButton();
			this.pic_line = new PictureBox();
			this.pnl_main = new Panel();
			this.cbb_sound = new VistaControls.ComboBox();
			this.lbl_sound = new Label();
			this.btn_browse = new System.Windows.Forms.Button();
			this.btn_default = new System.Windows.Forms.Button();
			this.pic_remove = new ImageButton();
			this.pic_add = new ImageButton();
			this.btn_edit = new System.Windows.Forms.Button();
			this.lv_accounts = new VistaControls.ListView();
			this.ch_name = new ColumnHeader();
			this.lbl_additional = new Label();
			this.lbl_language = new Label();
			this.cbb_language = new VistaControls.ComboBox();
			this.lbl_title = new Label();
			this.txt_interval = new System.Windows.Forms.TextBox();
			this.lbl_minutes = new Label();
			this.lbl_interval = new Label();
			this.pnl_account = new Panel();
			this.pic_exclamation = new PictureBox();
			this.lbl_error = new Label();
			this.lbl_accountTitle = new Label();
			this.txt_password = new VistaControls.TextBox();
			this.txt_username = new VistaControls.TextBox();
			this.lbl_password = new Label();
			this.lbl_username = new Label();
			this.pnl_accountButtons = new Panel();
			this.btn_accountCancel = new System.Windows.Forms.Button();
			this.btn_accountSave = new System.Windows.Forms.Button();
			this.tmr_animationIn = new Timer(this.components);
			this.tmr_animationOut = new Timer(this.components);
			this.pnl_aboutButtons = new Panel();
			this.btn_aboutOk = new System.Windows.Forms.Button();
			this.tmr_aboutAnimationIn = new Timer(this.components);
			this.tmr_aboutAnimationOut = new Timer(this.components);
			this.pnl_about = new Panel();
			this.pic_disclaimer = new PictureBox();
			this.pic_copyrights = new PictureBox();
			this.pic_welcome = new PictureBox();
			this.pic_aboutBackground = new PictureBox();
			this.pic_donate = new ImageButton();
			this.pnl_buttons.SuspendLayout();
			((ISupportInitialize)this.pic_about).BeginInit();
			((ISupportInitialize)this.pic_line).BeginInit();
			this.pnl_main.SuspendLayout();
			((ISupportInitialize)this.pic_remove).BeginInit();
			((ISupportInitialize)this.pic_add).BeginInit();
			this.pnl_account.SuspendLayout();
			((ISupportInitialize)this.pic_exclamation).BeginInit();
			this.pnl_accountButtons.SuspendLayout();
			this.pnl_aboutButtons.SuspendLayout();
			this.pnl_about.SuspendLayout();
			((ISupportInitialize)this.pic_disclaimer).BeginInit();
			((ISupportInitialize)this.pic_copyrights).BeginInit();
			((ISupportInitialize)this.pic_welcome).BeginInit();
			((ISupportInitialize)this.pic_aboutBackground).BeginInit();
			((ISupportInitialize)this.pic_donate).BeginInit();
			base.SuspendLayout();
			this.btn_ok.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btn_ok.FlatStyle = FlatStyle.System;
			this.btn_ok.Location = new Point(0x80, 14);
			this.btn_ok.Name = "btn_ok";
			this.btn_ok.Size = new Size(0x4b, 0x17);
			this.btn_ok.TabIndex = 7;
			this.btn_ok.Text = "OK";
			this.btn_ok.UseVisualStyleBackColor = true;
			this.btn_ok.Click += new EventHandler(this.btn_ok_Click);
			this.btn_cancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btn_cancel.DialogResult = DialogResult.Cancel;
			this.btn_cancel.FlatStyle = FlatStyle.System;
			this.btn_cancel.Location = new Point(0xd1, 14);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new Size(0x4b, 0x17);
			this.btn_cancel.TabIndex = 8;
			this.btn_cancel.Text = "Cancel";
			this.btn_cancel.UseVisualStyleBackColor = true;
			this.btn_cancel.Click += new EventHandler(this.btn_cancel_Click);
			this.pnl_buttons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
			this.pnl_buttons.BackColor = SystemColors.Control;
			this.pnl_buttons.Controls.Add(this.pic_about);
			this.pnl_buttons.Controls.Add(this.btn_cancel);
			this.pnl_buttons.Controls.Add(this.btn_ok);
			this.pnl_buttons.Location = new Point(0, 0x131);
			this.pnl_buttons.Name = "pnl_buttons";
			this.pnl_buttons.Size = new Size(0x12b, 50);
			this.pnl_buttons.TabIndex = 0;
			this.pic_about.Location = new Point(13, 0x11);
			this.pic_about.Name = "pic_about";
			this.pic_about.Size = new Size(0x10, 0x10);
			this.pic_about.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pic_about.TabIndex = 11;
			this.pic_about.TabStop = false;
			this.pic_about.Click += new EventHandler(this.pic_about_Click);
			this.pic_line.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
			this.pic_line.Location = new Point(0, 0x130);
			this.pic_line.Name = "pic_line";
			this.pic_line.Size = new Size(0x12b, 1);
			this.pic_line.TabIndex = 8;
			this.pic_line.TabStop = false;
			this.pnl_main.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.pnl_main.Controls.Add(this.cbb_sound);
			this.pnl_main.Controls.Add(this.lbl_sound);
			this.pnl_main.Controls.Add(this.btn_browse);
			this.pnl_main.Controls.Add(this.btn_default);
			this.pnl_main.Controls.Add(this.pic_remove);
			this.pnl_main.Controls.Add(this.pic_add);
			this.pnl_main.Controls.Add(this.btn_edit);
			this.pnl_main.Controls.Add(this.lv_accounts);
			this.pnl_main.Controls.Add(this.lbl_additional);
			this.pnl_main.Controls.Add(this.lbl_language);
			this.pnl_main.Controls.Add(this.cbb_language);
			this.pnl_main.Controls.Add(this.lbl_title);
			this.pnl_main.Controls.Add(this.txt_interval);
			this.pnl_main.Controls.Add(this.lbl_minutes);
			this.pnl_main.Controls.Add(this.lbl_interval);
			this.pnl_main.Location = new Point(0, 0);
			this.pnl_main.Name = "pnl_main";
			this.pnl_main.Size = new Size(0x12b, 0x131);
			this.pnl_main.TabIndex = 0;
			this.cbb_sound.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbb_sound.FlatStyle = FlatStyle.System;
			this.cbb_sound.FormattingEnabled = true;
			this.cbb_sound.Location = new Point(0x1b, 180);
			this.cbb_sound.Name = "cbb_sound";
			this.cbb_sound.Size = new Size(0xa4, 0x15);
			this.cbb_sound.TabIndex = 3;
			this.cbb_sound.SelectedIndexChanged += new EventHandler(this.cbb_sound_SelectedIndexChanged);
			this.lbl_sound.AutoSize = true;
			this.lbl_sound.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbl_sound.ForeColor = Color.FromArgb(0, 0x33, 0x99);
			this.lbl_sound.Location = new Point(0x16, 0x94);
			this.lbl_sound.Name = "lbl_sound";
			this.lbl_sound.Size = new Size(0x27, 0x15);
			this.lbl_sound.TabIndex = 0x27;
			this.lbl_sound.Text = "Title";
			this.btn_browse.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btn_browse.AutoSize = true;
			this.btn_browse.Enabled = false;
			this.btn_browse.FlatStyle = FlatStyle.System;
			this.btn_browse.Location = new Point(0xc5, 0xb3);
			this.btn_browse.MinimumSize = new Size(0x41, 0x17);
			this.btn_browse.Name = "btn_browse";
			this.btn_browse.Size = new Size(0x41, 0x17);
			this.btn_browse.TabIndex = 4;
			this.btn_browse.Text = "Browse...";
			this.btn_browse.UseVisualStyleBackColor = true;
			this.btn_browse.Click += new EventHandler(this.btn_browse_Click);
			this.btn_default.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btn_default.AutoSize = true;
			this.btn_default.Enabled = false;
			this.btn_default.FlatStyle = FlatStyle.System;
			this.btn_default.Location = new Point(0x7e, 0x73);
			this.btn_default.MinimumSize = new Size(0x41, 0x17);
			this.btn_default.Name = "btn_default";
			this.btn_default.Size = new Size(0x41, 0x17);
			this.btn_default.TabIndex = 1;
			this.btn_default.Text = "Default";
			this.btn_default.UseVisualStyleBackColor = true;
			this.btn_default.Click += new EventHandler(this.btn_default_Click);
			this.pic_remove.Location = new Point(0x31, 0x76);
			this.pic_remove.Name = "pic_remove";
			this.pic_remove.Size = new Size(0x10, 0x10);
			this.pic_remove.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pic_remove.TabIndex = 0x19;
			this.pic_remove.TabStop = false;
			this.pic_remove.Click += new EventHandler(this.pic_remove_Click);
			this.pic_add.Location = new Point(0x1b, 0x76);
			this.pic_add.Name = "pic_add";
			this.pic_add.Size = new Size(0x10, 0x10);
			this.pic_add.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pic_add.TabIndex = 0x19;
			this.pic_add.TabStop = false;
			this.pic_add.Click += new EventHandler(this.pic_add_Click);
			this.btn_edit.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btn_edit.AutoSize = true;
			this.btn_edit.Enabled = false;
			this.btn_edit.FlatStyle = FlatStyle.System;
			this.btn_edit.Location = new Point(0xc5, 0x73);
			this.btn_edit.MinimumSize = new Size(0x41, 0x17);
			this.btn_edit.Name = "btn_edit";
			this.btn_edit.Size = new Size(0x41, 0x17);
			this.btn_edit.TabIndex = 2;
			this.btn_edit.Text = "Edit";
			this.btn_edit.UseVisualStyleBackColor = true;
			this.btn_edit.Click += new EventHandler(this.btn_edit_Click);
			this.lv_accounts.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.lv_accounts.Columns.AddRange(new ColumnHeader[] { this.ch_name });
			this.lv_accounts.FullRowSelect = true;
			this.lv_accounts.HeaderStyle = ColumnHeaderStyle.None;
			this.lv_accounts.LabelWrap = false;
			this.lv_accounts.Location = new Point(0x19, 0x25);
			this.lv_accounts.MultiSelect = false;
			this.lv_accounts.Name = "lv_accounts";
			this.lv_accounts.RightToLeftLayout = true;
			this.lv_accounts.ShowGroups = false;
			this.lv_accounts.ShowItemToolTips = true;
			this.lv_accounts.Size = new Size(0xed, 0x48);
			this.lv_accounts.TabIndex = 0;
			this.lv_accounts.UseCompatibleStateImageBehavior = false;
			this.lv_accounts.View = View.Details;
			this.lv_accounts.SelectedIndexChanged += new EventHandler(this.lv_accounts_SelectedIndexChanged);
			this.lv_accounts.DoubleClick += new EventHandler(this.lv_accounts_DoubleClick);
			this.ch_name.Text = "Name";
			this.ch_name.Width = 0xe9;
			this.lbl_additional.AutoSize = true;
			this.lbl_additional.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbl_additional.ForeColor = Color.FromArgb(0, 0x33, 0x99);
			this.lbl_additional.Location = new Point(0x16, 0xd3);
			this.lbl_additional.Name = "lbl_additional";
			this.lbl_additional.Size = new Size(0x27, 0x15);
			this.lbl_additional.TabIndex = 0x18;
			this.lbl_additional.Text = "Title";
			this.lbl_language.AutoSize = true;
			this.lbl_language.Location = new Point(0x17, 0x10f);
			this.lbl_language.Name = "lbl_language";
			this.lbl_language.Size = new Size(0x5b, 13);
			this.lbl_language.TabIndex = 0x17;
			this.lbl_language.Text = "Display language:";
			this.cbb_language.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.cbb_language.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbb_language.FlatStyle = FlatStyle.System;
			this.cbb_language.FormattingEnabled = true;
			this.cbb_language.Location = new Point(0x80, 0x10c);
			this.cbb_language.Name = "cbb_language";
			this.cbb_language.Size = new Size(0x86, 0x15);
			this.cbb_language.TabIndex = 6;
			this.lbl_title.AutoSize = true;
			this.lbl_title.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbl_title.ForeColor = Color.FromArgb(0, 0x33, 0x99);
			this.lbl_title.Location = new Point(0x16, 9);
			this.lbl_title.Name = "lbl_title";
			this.lbl_title.Size = new Size(0x27, 0x15);
			this.lbl_title.TabIndex = 20;
			this.lbl_title.Text = "Title";
			this.txt_interval.Location = new Point(0x80, 0xf2);
			this.txt_interval.Name = "txt_interval";
			this.txt_interval.Size = new Size(0x1b, 20);
			this.txt_interval.TabIndex = 5;
			this.txt_interval.Text = "1";
			this.txt_interval.TextAlign = HorizontalAlignment.Center;
			this.txt_interval.Leave += new EventHandler(this.txt_interval_Leave);
			this.lbl_minutes.AutoSize = true;
			this.lbl_minutes.Location = new Point(0xa1, 0xf5);
			this.lbl_minutes.Name = "lbl_minutes";
			this.lbl_minutes.Size = new Size(0x31, 13);
			this.lbl_minutes.TabIndex = 0x16;
			this.lbl_minutes.Text = "minute(s)";
			this.lbl_interval.AutoSize = true;
			this.lbl_interval.Location = new Point(0x17, 0xf5);
			this.lbl_interval.Name = "lbl_interval";
			this.lbl_interval.Size = new Size(70, 13);
			this.lbl_interval.TabIndex = 0x15;
			this.lbl_interval.Text = "Check every:";
			this.pnl_account.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.pnl_account.Controls.Add(this.pic_exclamation);
			this.pnl_account.Controls.Add(this.lbl_error);
			this.pnl_account.Controls.Add(this.lbl_accountTitle);
			this.pnl_account.Controls.Add(this.txt_password);
			this.pnl_account.Controls.Add(this.txt_username);
			this.pnl_account.Controls.Add(this.lbl_password);
			this.pnl_account.Controls.Add(this.lbl_username);
			this.pnl_account.Location = new Point(300, 0);
			this.pnl_account.Name = "pnl_account";
			this.pnl_account.Size = new Size(0x12b, 0x131);
			this.pnl_account.TabIndex = 0;
			this.pic_exclamation.Location = new Point(0x19, 130);
			this.pic_exclamation.MinimumSize = new Size(0x10, 0x10);
			this.pic_exclamation.Name = "pic_exclamation";
			this.pic_exclamation.Size = new Size(0x10, 0x10);
			this.pic_exclamation.SizeMode = PictureBoxSizeMode.CenterImage;
			this.pic_exclamation.TabIndex = 20;
			this.pic_exclamation.TabStop = false;
			this.pic_exclamation.Visible = false;
			this.lbl_error.AutoSize = true;
			this.lbl_error.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbl_error.ForeColor = Color.FromArgb(180, 0x26, 30);
			this.lbl_error.Location = new Point(0x2c, 0x83);
			this.lbl_error.MaximumSize = new Size(0xda, 100);
			this.lbl_error.Name = "lbl_error";
			this.lbl_error.Size = new Size(200, 0x1a);
			this.lbl_error.TabIndex = 0x13;
			this.lbl_error.Text = "An account with this username already exists. Please enter a different username.";
			this.lbl_error.Visible = false;
			this.lbl_accountTitle.AutoSize = true;
			this.lbl_accountTitle.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbl_accountTitle.ForeColor = Color.FromArgb(0, 0x33, 0x99);
			this.lbl_accountTitle.Location = new Point(0x16, 9);
			this.lbl_accountTitle.Name = "lbl_accountTitle";
			this.lbl_accountTitle.Size = new Size(0x27, 0x15);
			this.lbl_accountTitle.TabIndex = 0x12;
			this.lbl_accountTitle.Text = "Title";
			this.txt_password.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.txt_password.Location = new Point(0x19, 0x68);
			this.txt_password.Name = "txt_password";
			this.txt_password.Size = new Size(0xed, 20);
			this.txt_password.TabIndex = 1;
			this.txt_password.TabStop = false;
			this.txt_password.UseSystemPasswordChar = true;
			this.txt_password.TextChanged += new EventHandler(this.login_information_Changed);
			this.txt_username.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.txt_username.Location = new Point(0x19, 0x3a);
			this.txt_username.Name = "txt_username";
			this.txt_username.Size = new Size(0xed, 20);
			this.txt_username.TabIndex = 0;
			this.txt_username.TabStop = false;
			this.txt_username.TextChanged += new EventHandler(this.login_information_Changed);
			this.lbl_password.AutoSize = true;
			this.lbl_password.Location = new Point(0x16, 0x56);
			this.lbl_password.Name = "lbl_password";
			this.lbl_password.Size = new Size(0x38, 13);
			this.lbl_password.TabIndex = 15;
			this.lbl_password.Text = "Password:";
			this.lbl_username.AutoSize = true;
			this.lbl_username.Location = new Point(0x16, 40);
			this.lbl_username.Name = "lbl_username";
			this.lbl_username.Size = new Size(0x3a, 13);
			this.lbl_username.TabIndex = 14;
			this.lbl_username.Text = "Username:";
			this.pnl_accountButtons.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.pnl_accountButtons.BackColor = SystemColors.Control;
			this.pnl_accountButtons.Controls.Add(this.btn_accountCancel);
			this.pnl_accountButtons.Controls.Add(this.btn_accountSave);
			this.pnl_accountButtons.Location = new Point(300, 0x131);
			this.pnl_accountButtons.Name = "pnl_accountButtons";
			this.pnl_accountButtons.Size = new Size(0x12b, 50);
			this.pnl_accountButtons.TabIndex = 0;
			this.btn_accountCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btn_accountCancel.DialogResult = DialogResult.Cancel;
			this.btn_accountCancel.FlatStyle = FlatStyle.System;
			this.btn_accountCancel.Location = new Point(0xd1, 14);
			this.btn_accountCancel.Name = "btn_accountCancel";
			this.btn_accountCancel.Size = new Size(0x4b, 0x17);
			this.btn_accountCancel.TabIndex = 3;
			this.btn_accountCancel.TabStop = false;
			this.btn_accountCancel.Text = "Cancel";
			this.btn_accountCancel.UseVisualStyleBackColor = true;
			this.btn_accountCancel.Click += new EventHandler(this.btn_accountCancel_Click);
			this.btn_accountSave.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btn_accountSave.Enabled = false;
			this.btn_accountSave.FlatStyle = FlatStyle.System;
			this.btn_accountSave.Location = new Point(0x80, 14);
			this.btn_accountSave.Name = "btn_accountSave";
			this.btn_accountSave.Size = new Size(0x4b, 0x17);
			this.btn_accountSave.TabIndex = 2;
			this.btn_accountSave.TabStop = false;
			this.btn_accountSave.Text = "Save";
			this.btn_accountSave.UseVisualStyleBackColor = true;
			this.btn_accountSave.Click += new EventHandler(this.btn_accountSave_Click);
			this.tmr_animationIn.Interval = 1;
			this.tmr_animationIn.Tick += new EventHandler(this.tmr_animationIn_Tick);
			this.tmr_animationOut.Interval = 1;
			this.tmr_animationOut.Tick += new EventHandler(this.tmr_animationOut_Tick);
			this.pnl_aboutButtons.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.pnl_aboutButtons.BackColor = SystemColors.Control;
			this.pnl_aboutButtons.Controls.Add(this.pic_donate);
			this.pnl_aboutButtons.Controls.Add(this.btn_aboutOk);
			this.pnl_aboutButtons.Location = new Point(-300, 0x131);
			this.pnl_aboutButtons.Name = "pnl_aboutButtons";
			this.pnl_aboutButtons.Size = new Size(0x12b, 50);
			this.pnl_aboutButtons.TabIndex = 0;
			this.btn_aboutOk.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.btn_aboutOk.DialogResult = DialogResult.Cancel;
			this.btn_aboutOk.FlatStyle = FlatStyle.System;
			this.btn_aboutOk.Location = new Point(0xd1, 14);
			this.btn_aboutOk.Name = "btn_aboutOk";
			this.btn_aboutOk.Size = new Size(0x4b, 0x17);
			this.btn_aboutOk.TabIndex = 0;
			this.btn_aboutOk.TabStop = false;
			this.btn_aboutOk.Text = "OK";
			this.btn_aboutOk.UseVisualStyleBackColor = true;
			this.btn_aboutOk.Click += new EventHandler(this.btn_aboutOk_Click);
			this.tmr_aboutAnimationIn.Interval = 1;
			this.tmr_aboutAnimationIn.Tick += new EventHandler(this.tmr_aboutAnimationIn_Tick);
			this.tmr_aboutAnimationOut.Interval = 1;
			this.tmr_aboutAnimationOut.Tick += new EventHandler(this.tmr_aboutAnimationOut_Tick);
			this.pnl_about.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.pnl_about.Controls.Add(this.pic_disclaimer);
			this.pnl_about.Controls.Add(this.pic_copyrights);
			this.pnl_about.Controls.Add(this.pic_welcome);
			this.pnl_about.Controls.Add(this.pic_aboutBackground);
			this.pnl_about.Location = new Point(-300, 0);
			this.pnl_about.Name = "pnl_about";
			this.pnl_about.Size = new Size(0x12b, 0x131);
			this.pnl_about.TabIndex = 0;
			this.pic_disclaimer.Location = new Point(0, 0xf8);
			this.pic_disclaimer.Name = "pic_disclaimer";
			this.pic_disclaimer.Size = new Size(0x12b, 0x31);
			this.pic_disclaimer.TabIndex = 0x21;
			this.pic_disclaimer.TabStop = false;
			this.pic_disclaimer.Visible = false;
			this.pic_copyrights.Location = new Point(0x6d, 0x10d);
			this.pic_copyrights.Name = "pic_copyrights";
			this.pic_copyrights.Size = new Size(190, 0x24);
			this.pic_copyrights.TabIndex = 0x22;
			this.pic_copyrights.TabStop = false;
			this.pic_welcome.Location = new Point(12, 0x76);
			this.pic_welcome.Name = "pic_welcome";
			this.pic_welcome.Size = new Size(0x6f, 0x17);
			this.pic_welcome.TabIndex = 0x20;
			this.pic_welcome.TabStop = false;
			this.pic_welcome.Visible = false;
			this.pic_aboutBackground.Location = new Point(0, 0);
			this.pic_aboutBackground.Name = "pic_aboutBackground";
			this.pic_aboutBackground.Size = new Size(0x12b, 0x131);
			this.pic_aboutBackground.TabIndex = 0x23;
			this.pic_aboutBackground.TabStop = false;
			this.pic_donate.Location = new Point(13, 15);
			this.pic_donate.Name = "pic_donate";
			this.pic_donate.Size = new Size(0x4a, 0x15);
			this.pic_donate.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pic_donate.TabIndex = 12;
			this.pic_donate.TabStop = false;
			this.pic_donate.Click += new EventHandler(this.pic_donate_Click);
			base.AcceptButton = this.btn_ok;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.Window;
			base.CancelButton = this.btn_cancel;
			base.ClientSize = new Size(0x12b, 0x163);
			base.Controls.Add(this.pic_line);
			base.Controls.Add(this.pnl_about);
			base.Controls.Add(this.pnl_aboutButtons);
			base.Controls.Add(this.pnl_accountButtons);
			base.Controls.Add(this.pnl_account);
			base.Controls.Add(this.pnl_main);
			base.Controls.Add(this.pnl_buttons);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)manager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "Settings";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Gmail Notifier Plus";
			base.Shown += new EventHandler(this.Settings_Shown);
			this.pnl_buttons.ResumeLayout(false);
			this.pnl_buttons.PerformLayout();
			((ISupportInitialize)this.pic_about).EndInit();
			((ISupportInitialize)this.pic_line).EndInit();
			this.pnl_main.ResumeLayout(false);
			this.pnl_main.PerformLayout();
			((ISupportInitialize)this.pic_remove).EndInit();
			((ISupportInitialize)this.pic_add).EndInit();
			this.pnl_account.ResumeLayout(false);
			this.pnl_account.PerformLayout();
			((ISupportInitialize)this.pic_exclamation).EndInit();
			this.pnl_accountButtons.ResumeLayout(false);
			this.pnl_aboutButtons.ResumeLayout(false);
			this.pnl_aboutButtons.PerformLayout();
			this.pnl_about.ResumeLayout(false);
			((ISupportInitialize)this.pic_disclaimer).EndInit();
			((ISupportInitialize)this.pic_copyrights).EndInit();
			((ISupportInitialize)this.pic_welcome).EndInit();
			((ISupportInitialize)this.pic_aboutBackground).EndInit();
			((ISupportInitialize)this.pic_donate).EndInit();
			base.ResumeLayout(false);
		}

		private void login_information_Changed(object sender, EventArgs e) {
			if (this.isEdit) {
				if ((this.txt_username.Text != this.lv_accounts.SelectedItems[0].Text) && this.accountsList.ContainsKey(this.txt_username.Text)) {
					this.btn_accountSave.Enabled = false;
					this.pic_exclamation.Visible = this.lbl_error.Visible = true;
					return;
				}
			}
			else if (this.accountsList.ContainsKey(this.txt_username.Text)) {
				this.btn_accountSave.Enabled = false;
				this.pic_exclamation.Visible = this.lbl_error.Visible = true;
				return;
			}
			this.btn_accountSave.Enabled = !string.IsNullOrEmpty(this.txt_username.Text) && !string.IsNullOrEmpty(this.txt_password.Text);
			this.pic_exclamation.Visible = this.lbl_error.Visible = false;
		}

		private void lv_accounts_DoubleClick(object sender, EventArgs e) {
			if (this.lv_accounts.SelectedIndices.Count > 0) {
				this.EditAccount();
			}
		}

		private void lv_accounts_SelectedIndexChanged(object sender, EventArgs e) {
			this.pic_remove.Enabled = this.btn_edit.Enabled = this.lv_accounts.SelectedItems.Count > 0;
			this.btn_default.Enabled = (this.lv_accounts.SelectedItems.Count > 0) && (this.lv_accounts.SelectedIndices[0] != this.defaultAccountIndex);
		}

		private void MirrorControls() {
			this.RightToLeft = RightToLeft.Yes;
			this.RightToLeftLayout = true;
			this.ReverseControls(ref this.pnl_main);
			this.ReverseControls(ref this.pnl_buttons);
			this.ReverseControls(ref this.pnl_account);
			this.ReverseControls(ref this.pnl_accountButtons);
			this.ReverseControls(ref this.pnl_aboutButtons);
		}

		private void noButton_Click(object sender, EventArgs e) {
			TaskDialogButton button = (TaskDialogButton)sender;
			((TaskDialog)button.HostingDialog).Close();
		}

		private void pic_about_Click(object sender, EventArgs e) {
			this.SwitchToAbout();
		}

		private void pic_add_Click(object sender, EventArgs e) {
			this.isEdit = false;
			this.SwitchToAccounts();
		}

		private void pic_donate_Click(object sender, EventArgs e) {
			Help.ShowHelp(this, Utilities.UrlHelper.Uris.Donate);
		}

		private void pic_remove_Click(object sender, EventArgs e) {
			TaskDialog dialog = new TaskDialog();
			dialog.Caption = Resources.Resources.WindowTitle;
			dialog.InstructionText = this.resourceManager.GetString("Label_RemoveConfirmation");
			dialog.Cancelable = true;
			dialog.OwnerWindowHandle = base.Handle;
			TaskDialogButton item = new TaskDialogButton("yesButton", this.resourceManager.GetString("Label_Yes"));
			item.Default = true;
			item.Click += new EventHandler(this.yesButton_Click);
			dialog.Controls.Add(item);
			TaskDialogButton button2 = new TaskDialogButton("noButton", this.resourceManager.GetString("Label_No"));
			button2.Click += new EventHandler(this.noButton_Click);
			dialog.Controls.Add(button2);
			dialog.Show();
		}

		private void ReverseControls(ref Panel container) {
			for (int i = 0; i < container.Controls.Count; i++) {
				container.Controls[i].Left = container.Width - (container.Controls[i].Left + container.Controls[i].Width);
			}
		}

		private void SelectCustomSound() {
			CommonOpenFileDialog dialog = new CommonOpenFileDialog();
			string path = string.IsNullOrEmpty(this.cbb_sound.SelectedValue.ToString()) ? Path.Combine(KnownFolders.Windows.Path, "Media") : Path.GetFullPath(this.cbb_sound.SelectedValue.ToString());
			dialog.Title = this.resourceManager.GetString("Label_BrowseDialog");
			dialog.Multiselect = false;
			dialog.DefaultDirectory = Directory.Exists(path) ? path : KnownFolders.Desktop.Path;
			CommonFileDialogStandardFilters.TextFiles.ShowExtensions = true;
			CommonFileDialogFilter item = new CommonFileDialogFilter(this.resourceManager.GetString("Label_WaveFiles"), ".wav");
			item.ShowExtensions = true;
			dialog.Filters.Add(item);
			if (dialog.ShowDialog() == CommonFileDialogResult.OK) {
				DataTable dataSource = (DataTable)this.cbb_sound.DataSource;
				dataSource.Rows.RemoveAt(2);
				dataSource.Rows.Add(new string[] { Path.GetFileName(dialog.FileName), dialog.FileName });
				this.cbb_sound.SelectedIndex = 2;
			}
		}

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		private void Settings_Shown(object sender, EventArgs e) {
			if (!this.Focused) {
				SetForegroundWindow(base.Handle);
			}
		}

		private void SwitchToAbout() {
			this.lv_accounts.TabStop = this.btn_default.TabStop = this.btn_edit.TabStop = this.cbb_sound.TabStop = this.btn_browse.TabStop = this.txt_interval.TabStop = this.cbb_language.TabStop = this.btn_ok.TabStop = this.btn_cancel.TabStop = false;
			this.btn_aboutOk.TabStop = true;
			base.AcceptButton = this.btn_aboutOk;
			base.CancelButton = this.btn_aboutOk;
			this.pnl_about.Focus();
			this.tmr_aboutAnimationIn.Enabled = true;
		}

		private void SwitchToAccounts() {
			int width = this.lbl_accountTitle.Width;
			if (this.isEdit) {
				this.lbl_accountTitle.Text = this.resourceManager.GetString("Label_Edit");
				this.btn_accountSave.Text = this.resourceManager.GetString("Button_Save");
				Account account = this.accountsList[this.lv_accounts.SelectedItems[0].Text];
				this.txt_username.Text = account.Login;
				this.txt_password.Text = account.Password;
			}
			else {
				this.lbl_accountTitle.Text = this.resourceManager.GetString("Label_Add");
				this.btn_accountSave.Text = this.resourceManager.GetString("Button_OK");
				this.txt_username.Text = this.txt_password.Text = string.Empty;
			}
			this.lv_accounts.TabStop = this.btn_default.TabStop = this.btn_edit.TabStop = this.cbb_sound.TabStop = this.btn_browse.TabStop = this.txt_interval.TabStop = this.cbb_language.TabStop = this.btn_ok.TabStop = this.btn_cancel.TabStop = false;
			this.txt_username.TabStop = this.txt_password.TabStop = this.btn_accountSave.TabStop = this.btn_accountCancel.TabStop = true;
			base.AcceptButton = this.btn_accountSave;
			base.CancelButton = this.btn_accountCancel;
			this.txt_username.Focus();

			if (Locale.Current.IsRightToLeftLanguage) {
				this.lbl_accountTitle.Left += width - this.lbl_accountTitle.Width;
			}

			this.tmr_animationIn.Enabled = true;
		}

		private void SwitchToSettings(SourceScreen source) {
			this.lv_accounts.TabStop = this.btn_default.TabStop = this.btn_edit.TabStop = this.cbb_sound.TabStop = this.btn_browse.TabStop = this.txt_interval.TabStop = this.cbb_language.TabStop = this.btn_ok.TabStop = this.btn_cancel.TabStop = true;
			base.AcceptButton = this.btn_ok;
			base.CancelButton = this.btn_cancel;
			this.pnl_main.Focus();
			switch (source) {
				case SourceScreen.About:
					this.btn_aboutOk.TabStop = false;
					this.tmr_aboutAnimationOut.Enabled = true;
					return;

				case SourceScreen.Accounts:
					this.txt_username.TabStop = this.txt_password.TabStop = this.btn_accountSave.TabStop = this.btn_accountCancel.TabStop = false;
					this.tmr_animationOut.Enabled = true;
					return;
			}
		}

		private void tmr_aboutAnimationIn_Tick(object sender, EventArgs e) {
			if (this.pnl_main.Left < 300) {
				this.pnl_main.Left = this.pnl_buttons.Left += this.animationSpeed;
				this.pnl_about.Left = this.pnl_aboutButtons.Left += this.animationSpeed;
			}
			else {
				this.tmr_aboutAnimationIn.Enabled = false;
			}
		}

		private void tmr_aboutAnimationOut_Tick(object sender, EventArgs e) {
			if (this.pnl_main.Left > 0) {
				this.pnl_main.Left = this.pnl_buttons.Left -= this.animationSpeed;
				this.pnl_about.Left = this.pnl_aboutButtons.Left -= this.animationSpeed;
			}
			else {
				this.tmr_aboutAnimationOut.Enabled = false;
				if (config.FirstRun) {
					config.FirstRun = false;
					this.pic_welcome.Visible = this.pic_disclaimer.Visible = false;
					this.pic_copyrights.Visible = true;
					this.btn_aboutOk.Text = this.resourceManager.GetString("Button_OK");
				}
			}
		}

		private void tmr_animationIn_Tick(object sender, EventArgs e) {
			if (this.pnl_main.Left > -300) {
				this.pnl_main.Left = this.pnl_buttons.Left -= this.animationSpeed;
				this.pnl_account.Left = this.pnl_accountButtons.Left -= this.animationSpeed;
			}
			else {
				this.tmr_animationIn.Enabled = false;
			}
		}

		private void tmr_animationOut_Tick(object sender, EventArgs e) {
			if (this.pnl_main.Left < 0) {
				this.pnl_main.Left = this.pnl_buttons.Left += this.animationSpeed;
				this.pnl_account.Left = this.pnl_accountButtons.Left += this.animationSpeed;
			}
			else {
				this.tmr_animationOut.Enabled = false;
			}
		}

		private void txt_interval_Leave(object sender, EventArgs e) {
			this.FixInterval();
		}

		private void UpdateHeaderSize() {
			this.lv_accounts.Columns[0].Width = (this.lv_accounts.Items.Count > 5) ? (this.lv_accounts.Width - 0x15) : (this.lv_accounts.Width - 4);
		}

		private void yesButton_Click(object sender, EventArgs e) {
			TaskDialogButton button = (TaskDialogButton)sender;
			((TaskDialog)button.HostingDialog).Close();
			int num = this.lv_accounts.SelectedIndices[0];
			this.accountsList.Remove(this.lv_accounts.SelectedItems[0].Text);
			this.lv_accounts.SelectedItems[0].Remove();
			if (num == this.defaultAccountIndex) {
				this.defaultAccountIndex = 0;
				if (this.lv_accounts.Items.Count > 0) {
					this.lv_accounts.Items[this.defaultAccountIndex].Font = this.BoldFont;
				}
			}
			this.UpdateHeaderSize();
		}

		private enum SourceScreen {
			About,
			Accounts
		}
	}
}

