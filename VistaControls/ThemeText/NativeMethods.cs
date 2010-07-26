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
using System.Runtime.InteropServices;

namespace VistaControls.ThemeText {
	
	internal static class NativeMethods {

		#region DWM Text drawing

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int DrawThemeTextEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string text, int iCharCount, int dwFlags, ref Native.RECT pRect, ref DTTOPTS pOptions);

		[StructLayout(LayoutKind.Sequential)]
		public struct DTTOPTS {
			public int dwSize;
			public DTTOPSFlags dwFlags;
			public int crText;
			public int crBorder;
			public int crShadow;
			public int iTextShadowType;
			public Native.POINT ptShadowOffset;
			public int iBorderSize;
			public int iFontPropId;
			public int iColorPropId;
			public int iStateId;
			public bool fApplyOverlay;
			public int iGlowSize;
			public int pfnDrawTextCallback;
			public IntPtr lParam;
		}

		public enum DTTOPSFlags : int {
			DTT_TEXTCOLOR = 1,
			DTT_BORDERCOLOR = 2,
			DTT_SHADOWCOLOR = 4,
			DTT_SHADOWTYPE = 8,
			DTT_SHADOWOFFSET = 16,
			DTT_BORDERSIZE = 32,
			//DTT_FONTPROP = 64,		commented values are currently unused
			//DTT_COLORPROP = 128,
			//DTT_STATEID = 256,
			//DTT_CALCRECT = 512,
			DTT_APPLYOVERLAY = 1024,
			DTT_GLOWSIZE = 2048,
			//DTT_CALLBACK = 4096,
			DTT_COMPOSITED = 8192			
		}

		public enum TextShadowType : int {
			TST_NONE = 0,
			TST_SINGLE = 1,
			TST_CONTINUOUS = 2
		}

		#endregion

	}

}
