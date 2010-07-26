using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

/**************************************************************************************************************
 *
 *  Aero Controls FOR .NET 2.0
 *  
 *  LabeledDivider Control
 * 
 *  This control written by Blake Pell (bpell@indiana.edu, blakepell@hotmail.com, http://www.blakepell.com)
 *  Initial Date:  07/25/2009
 *  Last Updated:  07/27/2009
 * 
 *  This code is released under the Microsoft Community License (Ms-CL).
 *
 **************************************************************************************************************/

namespace VistaControls
{
    /// <summary>
    /// The labeled divider provides a Aero styled divider with an option caption, similiar to what is seen in the
    /// Control Panel dialogs of Windows 7 and Vista.
    /// </summary>
    public class LabeledDivider : Control
    {        
        /// <summary>
        /// Constructor
        /// </summary>
        public LabeledDivider()
        {
            this.Font = new Font("Segoe UI", 9, FontStyle.Regular, GraphicsUnit.Point, 0);
            this._text = this.Name;
            this.ForeColor = Color.FromArgb(0, 51, 170);
            this.Width = 200;
        }

        /// <summary>
        /// The actual painting of the background of our control.
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>
        /// The colors in use here were extracted from an image of the Control Panel taken from a Windows 7 RC1 installation.
        /// </remarks>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            SolidBrush sbDividerColor = new SolidBrush(this._dividerColor);
            SolidBrush sbForeColor = new SolidBrush(this.ForeColor);

            // Draw the caption string, then get the size of it as it appears on the screen so
            // we know where to put the caption.
            e.Graphics.DrawString(this.Text, this.Font, sbForeColor, 0, 0);
            SizeF sf = e.Graphics.MeasureString(this.Text, this.Font);

            // This didn't quiet get in the cente rso I had to add 1 pixel to the sf.Height / 2
            if (this.DividerPosition == DividerPositions.Center)
            {
                Rectangle rect = new Rectangle((int)sf.Width, ((int)sf.Height / 2) + 1, this.Width - (int)sf.Width, 1);            
                e.Graphics.FillRectangle(sbDividerColor, rect);
            }
            else if (this.DividerPosition == DividerPositions.Below) 
            {
                Rectangle rect = new Rectangle(1, (int)sf.Height, this.Width, 1);
                e.Graphics.FillRectangle(sbDividerColor, rect);
            }

            sbForeColor.Dispose();
            sbDividerColor.Dispose();

        }

        /// <summary>
        /// The positions that the divider line can be drawn in
        /// </summary>
        public enum DividerPositions 
        { 
            /// <summary>
            /// The divider will be centered after the text caption and will begin drawing after the string.  This is the default behavior.
            /// </summary>
            Center, 
            /// <summary>
            /// The divider will be drawn below the text caption.
            /// </summary>
            Below 
        };

        private DividerPositions _dividerPosition = DividerPositions.Center;

        /// <summary>
        /// The position of the divider line.
        /// </summary>
        /// <remarks>
        /// The default value is the center position which is consistent on how this type of divider has been used throughout the Windows
        /// 7 and Vista UI's.
        /// </remarks>
        [Description("The placement of the divider line."), Category("Appearance"), DefaultValue(DividerPositions.Center)]
        public DividerPositions DividerPosition
        {
            get
            {
                return this._dividerPosition;
            }
            set
            {
                this._dividerPosition = value;
            }
        }

        private Color _dividerColor = Color.FromArgb(176, 191, 222);
        /// <summary>
        /// The color of the divider line.
        /// </summary>
        [Description("The color of the divider line."), Category("Appearance")]
        public Color DividerColor
        {
            get
            {
                return this._dividerColor;
            }
            set
            {
                this._dividerColor = value;
            }
        }

        private string _text = "";
        /// <summary>
        /// The text that should be used for the caption.  If the caption is set to blank and the divider position is set to center then
        /// a simple divider line will be drawn.
        /// </summary>
        /// <remarks>
        /// After a change is made to the text property we want to invalidate the control so it triggers a new paint message being sent.
        /// </remarks>
        [Description("The text that will display as the caption."), Category("Appearance"),DefaultValue("DividerLabel")]
        public override string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
                this.Invalidate();
            }
        }

    }
}
