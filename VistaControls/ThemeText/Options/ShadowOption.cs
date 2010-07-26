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
	public class ShadowOption : IThemeTextOption {

		public enum ShadowType {
			None,
			Single,
			Continuous
		}

		public ShadowOption(Color c, Point offset, ShadowType type) {
			Color = c;
			Offset = offset;
			Type = type;
		}

		public ShadowType Type { get; set; }
		public Point Offset { get; set; }
		public Color Color { get; set; }

		#region IThemeTextOption Members

        internal override void Apply(ref NativeMethods.DTTOPTS options) {
			options.dwFlags |= NativeMethods.DTTOPSFlags.DTT_SHADOWCOLOR |
							   NativeMethods.DTTOPSFlags.DTT_SHADOWOFFSET |
							   NativeMethods.DTTOPSFlags.DTT_SHADOWTYPE;

			options.crShadow = ColorTranslator.ToWin32(Color);
			options.ptShadowOffset = new VistaControls.Native.POINT(Offset);

			switch (Type) {
				case ShadowType.None:
					options.iTextShadowType = (int)NativeMethods.TextShadowType.TST_NONE; break;

				case ShadowType.Single:
					options.iTextShadowType = (int)NativeMethods.TextShadowType.TST_SINGLE; break;

				case ShadowType.Continuous:
					options.iTextShadowType = (int)NativeMethods.TextShadowType.TST_CONTINUOUS; break;
			}
		}

		#endregion
	}
}
