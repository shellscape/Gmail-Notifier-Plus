using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace VistaControls.Native {

    [Flags]
    internal enum WindowThemeNonClientAttributes : uint {
        NullAttribute = 0,
        NoDrawCaption = 0x00000001,
        NoDrawIcon = 0x00000002
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WTA_OPTIONS {
        public WindowThemeNonClientAttributes Flags;
        public WindowThemeNonClientAttributes Mask;
    }

    internal static class WindowTheme {

        public enum WindowThemeAttributeType : int {
            WTA_NONCLIENT = 1
        }

        [DllImport("uxtheme.dll")]
        public static extern int SetWindowThemeAttribute(IntPtr hWnd,
            WindowThemeAttributeType wtype, ref WTA_OPTIONS attributes, uint size);

        public static int SetWindowThemeNonClientAttributes(IntPtr hwnd,
            WindowThemeNonClientAttributes mask, WindowThemeNonClientAttributes attributes) {

            var opt = new WTA_OPTIONS {
                Flags = attributes,
                Mask = mask
            };
            return SetWindowThemeAttribute(hwnd, WindowThemeAttributeType.WTA_NONCLIENT,
                ref opt, (uint)Marshal.SizeOf(typeof(WTA_OPTIONS)));
        }
       

        /*public const uint WTNCA_NODRAWCAPTION = 0x00000001;
        public const uint WTNCA_NODRAWICON = 0x00000002;*/

/*WTA_OPTIONS wta = new WTA_OPTIONS() { Flags = WTNCA_NODRAWCAPTION | WTNCA_NODRAWICON, Mask = WTNCA_NODRAWCAPTION | WTNCA_NODRAWICON };

SetWindowThemeAttribute(this.Handle, WTA_NONCLIENT, ref wta, (uint)Marshal.SizeOf(typeof(WTA_OPTIONS)));*/

    }
}
