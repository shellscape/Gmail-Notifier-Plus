using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Shellscape.UI.ControlPanel;

using GmailNotifierPlus.Localization;

namespace GmailNotifierPlus.Forms {
	public partial class Preferences : Shellscape.UI.ControlPanel.ControlPanelForm {

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
		public static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);

		private Bitmap _userFrame;
		private Bitmap _iconStandard;
		private Bitmap _iconApps;

		public Preferences() : base() {
			InitializeComponent();

			Text = String.Concat(Locale.Current.Preferences.WindowTitle, " - ", Program.MainForm.Text);
			Icon = Program.MainForm.Icon;
			TopMost = true;

			InitNavigation();
			InitAccountPanel();

			if (Config.Current.FirstRun) {
				InitFirstRun();
			}

			_iconApps = Utilities.ResourceHelper.GetImage("gmail-limon.png");
			_iconStandard = Utilities.ResourceHelper.GetImage("gmail-bleu.png");
		}

		// FlowLayoutPanel doesn't support visual inheritance, so we have to add links manually here.
		private void InitNavigation() {
			ControlPanelTaskLink general = new ControlPanelTaskLink() {
				Text = "Control Panel Home" //Locale.Current.Preferences.Navigation.General 
			};
			ControlPanelTaskLink accounts = new ControlPanelTaskLink() {
				Text = Locale.Current.Preferences.Navigation.Accounts
			};
			ControlPanelTaskLink appearance = new ControlPanelTaskLink() {
				Text = Locale.Current.Preferences.Navigation.Appearance
			};

			this.Tasks.Add(general);
			this.Tasks.Add(accounts);
			this.Tasks.Add(appearance);

			ControlPanelTaskLink about = new ControlPanelTaskLink() { Text = Locale.Current.JumpList.About };
			ControlPanelTaskLink check = new ControlPanelTaskLink() { Text = Locale.Current.JumpList.Check };

			about.Click += delegate(object sender, EventArgs e) {
				Program.MainForm.RemoteShowAbout();
			};

			check.Click += delegate(object sender, EventArgs e) {
				// TODO: Check for updates
				// do a little circly thingie like chrome, show a check if up to date
			};

			this.OtherTasks.Add(about);
			this.OtherTasks.Add(check);
		}

		public void InitFirstRun() {

		}

		public void InitPanels() {

		}

		public void InitAccountPanel() {

			_userFrame = Shellscape.Utilities.ResourceHelper.GetStandardResourceBitmap("authui.dll", "#12221");
			//_userFrame = Shellscape.Utilities.ResourceHelper.GetResourcePNG("authui.dll", "12221");
			//_userFrame.RotateFlip(RotateFlipType.Rotate180FlipX);
			
			ImageList imageList = new ImageList() {
				ColorDepth = ColorDepth.Depth32Bit,
				ImageSize = new Size(1, _userFrame.Height)
			};

			imageList.Images.Add(_iconStandard);

			_ListAccounts.View = View.Tile;
			_ListAccounts.OwnerDraw = true;
			_ListAccounts.DrawItem += _ListAccounts_DrawItem;
			_ListAccounts.LargeImageList = _ListAccounts.SmallImageList = imageList;

			foreach (Account account in Config.Current.Accounts) {
				_ListAccounts.Items.Add(new ListViewItem(account.FullAddress, account.Type == AccountTypes.GoogleApps ? 0 : 1));
			}

			_ListAccounts.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

			SetWindowTheme(_ListAccounts.Handle, "explorer", null);

			_LabelAccountsTitle.Text = Locale.Current.Preferences.Panels.Accounts.Title;
			_LabelAccountsTitle.ThemeElement = Shellscape.UI.VisualStyles.ControlPanel.GetElement(
				Shellscape.UI.VisualStyles.ControlPanel.ControlPanelPart.Title,
				0,
				true
			);


			_LinkAddNew.Text = Locale.Current.Preferences.Panels.Accounts.AddNew;
			_LinkAddNew.NormalColor = Colors.ContentLinkNormal;
			_LinkAddNew.HoverColor = Colors.ContentLinkHot;
		}

		private void _ListAccounts_DrawItem(object sender, DrawListViewItemEventArgs e) {

			Image image = e.Item.ImageList.Images[e.Item.ImageIndex];
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
			else {
				elementState = 1; // Normal
			}

			if (elementState > 1) {
				VisualStyleElement element = VisualStyleElement.CreateElement("Explorer::ListView", 1, elementState);

				VisualStyleRenderer renderer = new VisualStyleRenderer(element);

				renderer.DrawBackground(e.Graphics, e.Bounds);
			}

			e.Graphics.DrawImage(image, e.Bounds.Left, e.Bounds.Top, image.Width, image.Height);
			e.Graphics.DrawImage(_userFrame, new Rectangle(e.Bounds.Left, e.Bounds.Top, _userFrame.Width, _userFrame.Height));

			//  // Draw the background and focus rectangle for a selected item.
			//  //e.Graphics.FillRectangle(Brushes.Maroon, e.Bounds);
			//  e.DrawBackground();
			//  e.DrawFocusRectangle();
				
			//}
			//else {
			//  // Draw the background for an unselected item.
			//  using (LinearGradientBrush brush = new LinearGradientBrush(e.Bounds, Color.Orange, Color.Maroon, LinearGradientMode.Horizontal)) {
			//    e.Graphics.FillRectangle(brush, e.Bounds);
			//  }
			//}

			// Draw the item text for views other than the Details view.
			//if (listView1.View != View.Details) {
			//e.DrawText();
			//}			
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);

			TopMost = false;
			this.Focus();
		}
	}
}
