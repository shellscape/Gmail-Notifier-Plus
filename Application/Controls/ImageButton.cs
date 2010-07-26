namespace GmailNotifierPlus.Controls
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ImageButton : PictureBox
    {
        private Bitmap defaultImage;
        private Bitmap disabledImage;
        private Bitmap hoverImage;
        private Bitmap pressedImage;

        public ImageButton()
        {
            base.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        [DllImport("user32.dll")]
        public static extern int LoadCursor(int hInstance, int lpCursorName);
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (this.disabledImage != null)
            {
                base.Image = base.Enabled ? this.defaultImage : this.disabledImage;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (base.Enabled && (this.pressedImage != null))
            {
                base.Image = this.pressedImage;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (base.Enabled && (this.hoverImage != null))
            {
                base.Image = this.hoverImage;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (base.Enabled && (this.hoverImage != null))
            {
                base.Image = this.defaultImage;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (base.Enabled && (this.pressedImage != null))
            {
                base.Image = this.defaultImage;
            }
        }

        [DllImport("user32.dll")]
        public static extern int SetCursor(int hCursor);
        public void SetImage(Bitmap defaultImage)
        {
            this.SetImages(defaultImage, null, null, null);
        }

        public void SetImages(Bitmap defaultImage, Bitmap disabledImage, Bitmap hoverImage, Bitmap pressedImage)
        {
            base.Image = this.defaultImage = defaultImage;
            this.disabledImage = disabledImage;
            this.hoverImage = hoverImage;
            this.pressedImage = pressedImage;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x20)
            {
                SetCursor(LoadCursor(0, 0x7f89));
                m.Result = IntPtr.Zero;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}

