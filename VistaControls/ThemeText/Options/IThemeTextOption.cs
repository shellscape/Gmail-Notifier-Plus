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
	public abstract class IThemeTextOption {

		internal abstract void Apply(ref NativeMethods.DTTOPTS options);

	}
}
