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
using System.Drawing;

namespace VistaControls.ThemeText.Options {
	public class BorderOption : IThemeTextOption {

		public BorderOption(Color c, int size) {
			BorderColor = c;
			BorderSize = size;
		}

		public Color BorderColor { get; set; }
		public int BorderSize { get; set; }


		#region IThemeTextOption Members

		internal override void Apply(ref NativeMethods.DTTOPTS options) {
			options.dwFlags |= NativeMethods.DTTOPSFlags.DTT_BORDERCOLOR | NativeMethods.DTTOPSFlags.DTT_BORDERSIZE;
			options.crBorder = ColorTranslator.ToWin32(BorderColor);
			options.iBorderSize = BorderSize;
		}

		#endregion
	}
}
