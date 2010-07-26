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
    /// <summary>
    /// Applies a glow on the themed text.
    /// </summary>
	public class GlowOption : IThemeTextOption {

        /// <summary>
        /// Default glow size.
        /// </summary>
		public const int DefaultSize = 10;

        /// <summary>
        /// Glow size used commonly by Office 2007 in titles.
        /// </summary>
		public const int Word2007Size = 15;

        /// <summary>
        /// Precise glow effect.
        /// </summary>
		public const int PreciseGlow = 2;

        /// <summary>
        /// Instantiates a new glow effect for themed text.
        /// </summary>
        /// <param name="size">Size of the glow effect.</param>
		public GlowOption(int size) {
			Size = size;
		}

        /// <summary>
        /// Gets or sets the size of the glow effect.
        /// </summary>
		public int Size { get; set; }

		#region IThemeTextOption Members

        internal override void Apply(ref NativeMethods.DTTOPTS options) {
			options.dwFlags |= NativeMethods.DTTOPSFlags.DTT_GLOWSIZE;
			options.iGlowSize = Size;
		}

		#endregion
	}
}
