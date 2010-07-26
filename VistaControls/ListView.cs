/*
* VISTA CONTROLS FOR .NET 2.0
* ENHANCED LISTVIEW
* 
* Written by Marco Minerva, mailto:marco.minerva@gmail.com
* 
* This code is released under the Microsoft Community License (Ms-CL).
* A copy of this license is available at
* http://www.microsoft.com/resources/sharedsource/licensingbasics/limitedcommunitylicense.mspx
*/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace VistaControls
{
    [ToolboxBitmap(typeof(ListView))]
    public class ListView : System.Windows.Forms.ListView
    {
        private Boolean elv = false;

        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages.
            switch (m.Msg)
            {
                case 15:
                    //Paint event
                    if (!elv)
                    {
                        //1-time run needed
                        NativeMethods.SetWindowTheme(this.Handle, "explorer", null); //Explorer style
                        NativeMethods.SendMessage(this.Handle, NativeMethods.LVM_SETEXTENDEDLISTVIEWSTYLE, NativeMethods.LVS_EX_DOUBLEBUFFER, NativeMethods.LVS_EX_DOUBLEBUFFER); //Blue selection, keeps other extended styles
                        elv = true;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
    }
}
