/*****************************************************
 *            Vista Controls for .NET 2.0
 * 
 * http://www.codeplex.com/vistacontrols
 * 
 * @author: Lorenz Cuno Klopfenstein
 * Licensed under Microsoft Community License (Ms-CL)
 * 
 *****************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace VistaControls.ThemeText.Options {
	public class OverlayOption : IThemeTextOption {

		public OverlayOption(bool enabled) {
			Enabled = enabled;
		}

		public bool Enabled { get; set; }

		#region IThemeTextOption Members

        internal override void Apply(ref NativeMethods.DTTOPTS options) {
			options.dwFlags |= NativeMethods.DTTOPSFlags.DTT_APPLYOVERLAY;
			options.fApplyOverlay = Enabled;
		}

		#endregion
	}
}
