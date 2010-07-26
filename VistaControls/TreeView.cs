/*
* VISTA CONTROLS FOR .NET 2.0
* ENHANCED TREEVIEW
* 
* Written by Marco Minerva, mailto:marco.minerva@gmail.com
* 
* This code is released under the Microsoft Community License (Ms-CL).
* A copy of this license is available at
* http://www.microsoft.com/resources/sharedsource/licensingbasics/limitedcommunitylicense.mspx
*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VistaControls {
	[ToolboxBitmap(typeof(TreeView))]
	public class TreeView : System.Windows.Forms.TreeView {
		public TreeView() {
			base.HotTracking = true;
			base.ShowLines = false;
		}

		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.Style |= NativeMethods.TVS_NOHSCROLL;
				return cp;
			}
		}

		[Browsable(false)]
		public new bool HotTracking {
			get { return base.HotTracking; }
			set { base.HotTracking = true; }
		}

		[Browsable(false)]
		public new bool ShowLines {
			get { return base.ShowLines; }
			set { base.ShowLines = false; }
		}

		protected override void OnHandleCreated(EventArgs e) {
			base.OnHandleCreated(e);

			NativeMethods.SetWindowTheme(base.Handle, "explorer", null);

			int style = NativeMethods.SendMessage(base.Handle, Convert.ToUInt32(NativeMethods.TVM_GETEXTENDEDSTYLE), 0, 0);
			style |= (NativeMethods.TVS_EX_AUTOHSCROLL | NativeMethods.TVS_EX_FADEINOUTEXPANDOS);
			NativeMethods.SendMessage(base.Handle, NativeMethods.TVM_SETEXTENDEDSTYLE, 0, style);
			
		}
	}
}
