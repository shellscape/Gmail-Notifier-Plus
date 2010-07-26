using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace VistaControls.Dwm {
	public class ThumbnailViewer : Control {

		Thumbnail _thumbnail = null;
		Form _topLevelForm = null;
		Control _parentControl = null;
		EventHandler _parentChangeHandler;

		public ThumbnailViewer() {
			_parentChangeHandler = new EventHandler(_parentControl_VisibleChanged);
		}

		#region Public methods

		/// <summary>Sets the origin of the thumbnail and shows the thumbnail on the control.</summary>
		/// <param name="originForm">The Form instance that will be thumbnailed.</param>
		/// <param name="trackFormUpdates">True if the control should automatically update itself in case the thumbnailed
		/// form changes size or is closed.</param>
		public void SetThumbnail(Form originForm, bool trackFormUpdates) {
			SetThumbnail(originForm.Handle);

			if (trackFormUpdates) {
				originForm.SizeChanged += new EventHandler(originForm_SizeChanged);
				originForm.FormClosed += new FormClosedEventHandler(originForm_FormClosed);
			}
		}

		public void SetThumbnail(Form originForm) {
			SetThumbnail(originForm.Handle);
		}

		public void SetThumbnail(IntPtr originHandle) {
			RecomputeParentForm();

			if (_topLevelForm != null) {
				_thumbnail = DwmManager.Register(_topLevelForm, originHandle);
				UpdateThumbnail(Visible);
			}
			else
				throw new Exception("Control must have an owner.");
		}

		/// <summary>Forces and update of the thumbnail.</summary>
		/// <remarks>Use this method if you know that the thumbnailed window has been resized and the thumbnail control should react to these changes.</remarks>
		public void Update() {
			UpdateThumbnail(Visible);
		}

		#endregion

		#region Event overrides

		protected override void OnVisibleChanged(EventArgs e) {
			base.OnVisibleChanged(e);

			UpdateThumbnail(Visible);
		}

		/* REMOVED 30/03/2008: doens't work correctly in some cases. Now the control uses an EventHandler that is registered
		 *					   on its parent.
		 *					   
		 * protected override void OnParentVisibleChanged(EventArgs e) {
			base.OnParentVisibleChanged(e);

			UpdateThumbnail(Parent.Visible);
		}*/

		protected override void OnParentChanged(EventArgs e) {
			RecomputeParentForm();

			//Remove handler from old parent
			if (_parentControl != null)
				_parentControl.VisibleChanged -= _parentChangeHandler;

			_parentControl = Parent;

			//Add handler to new parent
			_parentControl.VisibleChanged += _parentChangeHandler;

			base.OnParentChanged(e);
		}

		void _parentControl_VisibleChanged(object sender, EventArgs e) {
			UpdateThumbnail(_parentControl.Visible);
		}

		protected override void OnLocationChanged(EventArgs e) {
			base.OnLocationChanged(e);

			UpdateThumbnail(Visible);
		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);

			UpdateThumbnail(Visible);
		}

		void originForm_FormClosed(object sender, FormClosedEventArgs e) {
			if (_thumbnail != null) {
				_thumbnail.Dispose();
				_thumbnail = null;
			}
		}

		void originForm_SizeChanged(object sender, EventArgs e) {
			Update();
		}

		#endregion

		#region Properties

		bool _onlyClientArea = true;

		[Description("Determines whether to show only the client area of the window or the whole window."), Category("Appearance"), DefaultValue(true)]
		public bool ShowOnlyClientArea {
			get { return _onlyClientArea; }
			set {
				_onlyClientArea = value;
				UpdateThumbnail(Visible);
			}
		}

		byte _opacity = 255;

		[Description("Sets the opacity of the thumbnail."), Category("Appearance"), DefaultValue((byte)255)]
		public byte Opacity {
			get { return _opacity; }
			set {
				_opacity = value;
				UpdateThumbnail(Visible);
			}
		}

		ContentAlignment _alignment = ContentAlignment.MiddleCenter;

		public ContentAlignment ThumbnailAlignment {
			get { return _alignment; }
			set {
				_alignment = value;
				UpdateThumbnail(Visible);
			}
		}

		bool _scaleSmallerThumbnails = true;

		public bool ScaleSmallerThumbnails {
			get { return _scaleSmallerThumbnails; }
			set {
				_scaleSmallerThumbnails = value;
				UpdateThumbnail(Visible);
			}
		}

		#endregion

		#region Helper methods

		//Used to cache the visibility status: prevents calls to Thumbnail.Update when the thumbnail is not visible.
		bool _lastVisibilityStatus = true;

		protected void UpdateThumbnail(bool visible) {
			if (!_lastVisibilityStatus && !visible)
				return;

			if (_thumbnail != null) {
				_thumbnail.Update(RecomputeThumbnailRectangle(), _opacity, visible, _onlyClientArea);
			}

			_lastVisibilityStatus = visible;
		}

		private Rectangle RecomputeThumbnailRectangle() {
			if (_topLevelForm == null || _thumbnail == null)
				throw new Exception("whops, no parent or no thumbnail");

			Point offset = Point.Empty;

			//Compute offset of this control from top level form
			//TODO: there MUST be a less ugly way to do this...
			Control ctrl = this;
			do {
				offset = new Point(offset.X + ctrl.Location.X, offset.Y + ctrl.Location.Y);
				ctrl = ctrl.Parent;
			}
			while (ctrl != null && ctrl != this.TopLevelControl);


			/*Native.RECT sldjfns;
			Native.Windows.GetWindowRect(this.Handle, out sldjfns);*/

			//Rectangle ctrlOnScrn = RectangleToScreen(this.ClientRectangle);
			//Rectangle formOnScrn = RectangleToScreen(this.TopLevelControl.ClientRectangle);

			//Point scrnCtrlOrigin = this.PointToScreen(scrn.Location);
			//Point formCtrlOrigin = this.PointToClient(scrn.Location);

			/*Point loc = scrn.Location;
			Point shift = new Point(
				loc.X - SystemInformation.Border3DSize.Width,
				loc.Y - SystemInformation.Border3DSize.Height
			);*/

			//Fit source rectangle to thumbnail rectangle
			Size destination = this.ClientSize;
			Size source = _thumbnail.SourceSize;

			if (source.Width < destination.Width && source.Height < destination.Height && !_scaleSmallerThumbnails) {
				destination = source;
			}
			
			if (source.Width > destination.Width || source.Height > destination.Height) {
				double ratio = (double)source.Width / (double)source.Height;

				if (source.Width < source.Height) {
					//Keep height, scale width
					destination.Width = (int)(destination.Height * ratio);
				}
				else {
					//Keep width, scale height
					destination.Height = (int)(destination.Width / ratio);
				}
			}

			//Align thumbnail correctly (offset is not aligned TopLeft)
			int dx = ClientSize.Width - destination.Width;
			int dy = ClientSize.Height - destination.Height;

			//Middle
			if (ThumbnailAlignment == ContentAlignment.MiddleCenter || ThumbnailAlignment == ContentAlignment.MiddleLeft || ThumbnailAlignment == ContentAlignment.MiddleRight)
				offset = new Point(offset.X, offset.Y + dy / 2);
			//Bottom
			if(ThumbnailAlignment == ContentAlignment.BottomCenter || ThumbnailAlignment == ContentAlignment.BottomLeft || ThumbnailAlignment == ContentAlignment.BottomRight)
				offset = new Point(offset.X, offset.Y + dy);
			//Center
			if (ThumbnailAlignment == ContentAlignment.BottomCenter || ThumbnailAlignment == ContentAlignment.MiddleCenter || ThumbnailAlignment == ContentAlignment.TopCenter)
				offset = new Point(offset.X + dx / 2, offset.Y);
			//Right
			if (ThumbnailAlignment == ContentAlignment.BottomRight || ThumbnailAlignment == ContentAlignment.MiddleRight || ThumbnailAlignment == ContentAlignment.TopRight)
				offset = new Point(offset.X + dx, offset.Y);

			return new Rectangle(offset, destination);
		}

		private void RecomputeParentForm() {
			//Get the owning form
			Form nextParent = this.TopLevelControl as Form;

			if (nextParent == null) {
				//No owning form found (!)
				_topLevelForm = null;
				if (_thumbnail != null) {
					_thumbnail.Dispose();
					_thumbnail = null;
				}
				return;
			}

			if (_thumbnail != null && _topLevelForm != nextParent) {
				//Parent changed from last time
				_thumbnail.Dispose();
				_thumbnail = null;
			}

			_topLevelForm = nextParent;
		}

		#endregion

	}
}
