/*
* VISTA CONTROLS FOR .NET 2.0
* ENHANCED COMBOBOX
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

namespace VistaControls
{
    [ToolboxBitmap(typeof(ComboBox))]
    public class ComboBox : System.Windows.Forms.ComboBox
    {
        private string cueBannerText_ = string.Empty;
        [Description("Gets or sets the cue text that is displayed on a ComboBox control."), Category("Appearance"), DefaultValue("")]
        public string CueBannerText
        {
            get
            {
                return cueBannerText_;
            }
            set
            {
                cueBannerText_ = value;
                this.SetCueText();
            }
        }

        private void SetCueText()
        {
            NativeMethods.SendMessage(this.Handle, NativeMethods.CB_SETCUEBANNER, IntPtr.Zero, cueBannerText_);
        }

        public ComboBox()
        {
            this.FlatStyle = FlatStyle.System;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}
