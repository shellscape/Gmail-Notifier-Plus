using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;

/**************************************************************************************************************
 *
 *  Aero Controls FOR .NET 2.0
 *  
 *  VerticalPanel Control
 * 
 *  This control written by Blake Pell (bpell@indiana.edu, blakepell@hotmail.com, http://www.blakepell.com)
 *  Initial Date:  07/24/2009
 *  Last Updated:  07/25/2009
 * 
 *  This code is released under the Microsoft Community License (Ms-CL).
 *
 **************************************************************************************************************/

namespace VistaControls
{

    /// <summary>
    /// A vertical panel which resembles what is used for information and navigation in the Control Panel of Windows 7 and Vista.  
    /// </summary>
    /// <remarks>
    /// This control is meant to be used on the left hand side of a form, it creates a graphic border on the right hand side.  Also
    /// I have VB code for this control if anyone needs it, just send me an e-mail at bpell@indiana.edu or blakepell@hotmail.com.
    /// </remarks>
    [ToolboxBitmap(typeof(Panel))]
    public class VerticalPanel : Panel
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// We are by default setting the background color to Color.Transparent.  The reason for this is that a lot of controls that will
        /// be used with this, namingly the Label and LinkLabel default their back color to the color of the panel and for those controls
        /// to display properly on this panel, their BackColor will need to be Color.Transparent (otherwise, they'll display as a black
        /// box).  This should help to isolate the developer from having to research this.
        /// 
        /// To reduce flicker, especially when glass is enabled, I had to set all three of the below styles.
        /// 
        /// </remarks>
        public VerticalPanel()
        {
            this.BackColor = Color.Transparent;
            this.Font = new Font("Segoe UI", 9, FontStyle.Regular, GraphicsUnit.Point, 0);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
        }

        /// <summary>
        /// When a control is added, we will check the type and if it meets certain criteria will change some default behaviors of
        /// the control so that it fits our theme by default.  The developer can still change this as they desire after it's added.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is LinkLabel)
            {
                e.Control.Font = new Font("Segoe UI", 9, FontStyle.Regular, GraphicsUnit.Point, 0);
                ((LinkLabel)e.Control).LinkBehavior = LinkBehavior.HoverUnderline;
                ((LinkLabel)e.Control).LinkColor = Color.FromArgb(64, 64, 64);
                ((LinkLabel)e.Control).ActiveLinkColor = Color.Blue;
            }
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
            Color aeroColor1 = Color.FromArgb(204, 217, 234);
            Color aeroColor2 = Color.FromArgb(217, 227, 240);
            Color aeroColor3 = Color.FromArgb(232, 238, 247);
            Color aeroColor4 = Color.FromArgb(237, 242, 249);
            Color aeroColor5 = Color.FromArgb(240, 244, 250);
            Color aeroColor6 = Color.FromArgb(241, 245, 251);

            Rectangle rect = new Rectangle(this.Width - 1, 0, 1, this.Height);
            SolidBrush sb = new SolidBrush(aeroColor1);
            e.Graphics.FillRectangle(sb, rect);
            rect = new Rectangle(this.Width + 1, 0, 1, this.Height);
            sb.Color = aeroColor1;
            e.Graphics.FillRectangle(sb, rect);
            rect = new Rectangle(this.Width - 2, 0, 1, this.Height);
            sb.Color = aeroColor2;
            e.Graphics.FillRectangle(sb, rect);
            rect = new Rectangle(this.Width - 3, 0, 1, this.Height);
            sb.Color = aeroColor3;
            e.Graphics.FillRectangle(sb, rect);
            rect = new Rectangle(this.Width - 4, 0, 1, this.Height);
            sb.Color = aeroColor4;
            e.Graphics.FillRectangle(sb, rect);
            rect = new Rectangle(this.Width - 5, 0, 1, this.Height);
            sb.Color = aeroColor5;
            e.Graphics.FillRectangle(sb, rect);
            rect = new Rectangle(0, 0, this.Width - 5, this.Height);
            sb.Color = aeroColor6;
            e.Graphics.FillRectangle(sb, rect);
            sb.Dispose();
        }

        /// <summary>
        /// This procedure will redraw any control, given it's handl as an image on the form.  This is necessary if you want to lay this
        /// control on top of the glass surface of an Aero form.
        /// </summary>
        /// <param name="hwnd"></param>
        public void RedrawControlAsBitmap(IntPtr hwnd)
        {
            Control c = Control.FromHandle(hwnd);
            using (Bitmap bm = new Bitmap(c.Width, c.Height))
            {
                c.DrawToBitmap(bm, c.ClientRectangle);
                using (Graphics g = c.CreateGraphics())
                {
                    Point p = new Point(-1, -1);
                    g.DrawImage(bm, p);
                }
            }
            c = null;
        }

        /// <summary>
        /// Handles incoming Windows Messages.
        /// </summary>
        /// <param name="m"></param>
        /// <remarks>
        /// On the paint event and if the RenderOnGlass is set to true, we will redraw the control as an image directly on
        /// the form.  This has a little extra overhead but also provides the ability to lay this control directly on the
        /// glass and have it rendered correctly.
        /// </remarks>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            int WM_PAINT = 15;
            if ((m.Msg == WM_PAINT) && this.RenderOnGlass)
            {
                this.RedrawControlAsBitmap(this.Handle);
            }
        }

        private bool _renderOnGlass = false;
        /// <summary>
        /// Whether or not the control needs to be rendered on the Glass surface.
        /// </summary>
        /// <remarks>
        /// This is false by default, it should only be toggled to true if the control needs to lay directly on
        /// the glass surface of the form.
        /// </remarks>
        [Description("Gets or sets whether the control can render on an Aero glass surface."), Category("Appearance"), DefaultValue(false)]
        public bool RenderOnGlass
        {
            get
            {
                return this._renderOnGlass;
            }
            set
            {
                this._renderOnGlass = value;
            }
        }
    }
}
