/*****************************************************
 *            Vista Controls for .NET 2.0
 * 
 * http://www.codeplex.com/vistacontrols
 * 
 * @author: Nicholas Kwan
 * @www: http://www.codeproject.com/KB/vista/themedvistacontrols.aspx
 * @integration: Lorenz Cuno Klopfenstein
 * Licensed under Microsoft Community License (Ms-CL)
 * 
 *****************************************************/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VistaControls {

	public class SplitButton : Button {

		protected override CreateParams CreateParams {
			get {
				CreateParams p = base.CreateParams;
				p.Style |= (this.IsDefault) ? NativeMethods.BS_DEFSPLITBUTTON : NativeMethods.BS_SPLITBUTTON;
				return p;
			}
		}

		#region Split Context Menu

		/// <summary>Occurs when the split label is clicked.</summary>
		[Description("Occurs when the split button is clicked."), Category("Action")]
		public event EventHandler<SplitMenuEventArgs> SplitClick;

		/// <summary>Occurs when the split label is clicked, but before the associated
		/// context menu is displayed by the control.</summary>
		[Description("Occurs when the split label is clicked, but before the associated context menu is displayed."), Category("Action")]
		public event EventHandler<SplitMenuEventArgs> SplitMenuOpening;

		/// <summary>Provides data for the clicking of split buttons and the opening
		/// of context menus.</summary>
		public class SplitMenuEventArgs : EventArgs {
			public SplitMenuEventArgs() {
				PreventOpening = false;
			}

			public SplitMenuEventArgs(Rectangle drawArea) {
				DrawArea = drawArea;
			}

			/// <summary>Represents the bounding box of the clicked button.</summary>
			/// <remarks>A menu should be opened, with top-left coordinates in the left-bottom point of
			/// the rectangle and with width equal (or greater) than the width of the rectangle.</remarks>
			public Rectangle DrawArea { get; set; }

			/// <summary>Set to true if you want to prevent the menu from opening.</summary>
			public bool PreventOpening { get; set; }
		}

		protected virtual void OnSplitClick(SplitMenuEventArgs e) {
			//Raise opening event before opening any menu
			if (SplitMenuOpening != null &&
				(SplitMenu != null || SplitMenuStrip != null))
				SplitMenuOpening(this, e);

			Point pos = new Point(e.DrawArea.Left, e.DrawArea.Bottom);

			if (!e.PreventOpening) {
				if (SplitMenu != null) {
					SplitMenu.Show(this, pos);
				}
				else if (SplitMenuStrip != null) {
					SplitMenuStrip.Width = e.DrawArea.Width;
					SplitMenuStrip.Show(this, pos);
				}
			}

			//Raise the event after the user click
			if (SplitClick != null)
				SplitClick(this, e);
		}

		/// <summary>Gets or sets the associated context menu that is displayed when the split
		/// glyph of the button is clicked.</summary>
		[Description("Sets the context menu that is displayed by clicking on the split button."), Category("Behavior"), DefaultValue(null)]
		public ContextMenuStrip SplitMenuStrip { get; set; }

		/// <summary>Gets or sets the associated context menu that is displayed when the split
		/// glyph of the button is clicked. Exposed for backward compatibility.</summary>
		[Description("Sets the context menu that is displayed by clicking on the split button."), Category("Behavior"), DefaultValue(null)]
		public ContextMenu SplitMenu { get; set; }

		#endregion

		#region Split button Properties

		/*bool _SplitButtonAlignLeft = false;

		[Description("Align the split button on the left side of the button."), Category("Appearance"), DefaultValue(false)]
		/// <summary>Gets or sets whether the split button should be aligned on the left side of the button.</summary>
		public bool SplitButtonAlignLeft {
			get {
				return _SplitButtonAlignLeft;
			}
			set {
				_SplitButtonAlignLeft = value;
				SendSplitStyleMsg();
			}
		}

		bool _SplitButtonNoSplit = false;

		[Description("Set No Split option."), Category("Appearance"), DefaultValue(false)]
		/// <summary>Gets or sets whether the split button should be shown or not.</summary>
		public bool SplitButtonNoSplit {
			get {
				return _SplitButtonNoSplit;
			}
			set {
				_SplitButtonNoSplit = value;
				SendSplitStyleMsg();
			}
		}

		bool _SplitButtonStretch = false;

		private void SendSplitStyleMsg() {
			NativeMethods.BUTTON_SPLITINFO info = new NativeMethods.BUTTON_SPLITINFO();
			info.Mask = NativeMethods.SplitInfoMask.Style;
			info.SplitStyle = (NativeMethods.SplitInfoStyle)(
				((uint)NativeMethods.SplitInfoStyle.AlignLeft & (_SplitButtonAlignLeft ? 1 : 0)) |
				((uint)NativeMethods.SplitInfoStyle.NoSplit & (_SplitButtonNoSplit ? 1 : 0)) |
				((uint)NativeMethods.SplitInfoStyle.Stretch & (_SplitButtonStretch ? 1 : 0))
			);

			NativeMethods.SendMessage(_innerButton.Handle, NativeMethods.BCM_SETSPLITINFO, 0, ref info);
		}*/

		#endregion

		protected override void WndProc(ref Message m) {
		    if (m.Msg == NativeMethods.BCN_SETDROPDOWNSTATE) {
		        if (m.WParam.ToInt32() == 1) {
		            OnSplitClick(new SplitMenuEventArgs(this.ClientRectangle));
		        }
		    }			

			base.WndProc(ref m);
		}
	}
}
