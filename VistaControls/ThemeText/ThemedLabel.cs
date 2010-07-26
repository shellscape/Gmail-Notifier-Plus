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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using VistaControls.ThemeText.Options;
using System.Windows.Forms.VisualStyles;


namespace VistaControls.ThemeText {

	/// <summary>
	/// A Label containing some text that will be drawn with glowing border on top of the Glass Sheet effect.
	/// </summary>
	//[Designer("VistaControls.Design.ThemedLabelDesigner")]
	[DefaultProperty("Text")]
	public class ThemedLabel : Control {

		ThemedText _hText = null;
		bool _needsUpdate = false;

		protected override void OnHandleDestroyed(EventArgs e) {
			//Clean up
			if (_hText != null)
				_hText.Dispose();
			_hText = null;

			base.OnHandleDestroyed(e);
		}

		private void UpdateText() {
			_needsUpdate = true;

			this.Invalidate(false);
		}

		#region Control properties overriding

		public override string Text {
			set {
				base.Text = value;
				UpdateText();
			}
		}

		public override Font Font {
			set {
				base.Font = value;
				UpdateText();
			}
		}

		public new Padding Padding {
			get { return base.Padding; }
			set {
				base.Padding = value;
				UpdateText();
			}
		}

		public override Color ForeColor {
			set {
				base.ForeColor = value;
				UpdateText();
			}
		}

		[Browsable(false)]
		public new Color BackColor {
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		[Browsable(false)]
		public new Image BackgroundImage {
			get { return base.BackgroundImage; }
			set { base.BackgroundImage = value; }
		}

		[Browsable(false)]
		public new ImageLayout BackgroundImageLayout {
			get { return base.BackgroundImageLayout; }
			set { base.BackgroundImageLayout = value; }
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			UpdateText();
		}

		/*bool _transparent = true;

		[Description("Sets whether the control should handle mouse events or not."), Category("Behavior"), DefaultValue(true)]
		public bool Transparent {
			get { return _transparent; }
			set { _transparent = value; }
		}*/

		#endregion

		#region Glow properties

		int _glowSize = 9;

		/// <summary>Size of the glow effect around the text.</summary>
		[Description("Size of the glow effect around the text."), Category("Appearance"), DefaultValue(9)]
		public int GlowSize {
			get { return _glowSize; }
			set { 
				_glowSize = value;
				UpdateText();
			}
		}

		bool _glowEnable = true;

		/// <summary>Enables or disables the glow effect around the text.</summary>
		[Description("Enables or disables the glow effect around the text."), Category("Appearance"), DefaultValue(true)]
		public bool GlowEnabled {
			get { return _glowEnable; }
			set {
				_glowEnable = value;
				UpdateText();
			}
		}

		#endregion

		#region Shadow properties

		Options.ShadowOption.ShadowType _shadowType = Options.ShadowOption.ShadowType.Continuous;

		/// <summary>Shadow type.</summary>
		[Description("Shadow type."), Category("Appearance"), DefaultValue(Options.ShadowOption.ShadowType.Continuous)]
		public Options.ShadowOption.ShadowType ShadowType {
			get { return _shadowType; }
			set {
				_shadowType = value;
				UpdateText();
			}
		}

		Point _shadowOffset = new Point(5, 5);
		[Description("Shadow offset in pixels."), Category("Appearance"), DefaultValue(typeof(Point), "5, 5")]
		public Point ShadowOffset {
			get { return _shadowOffset; }
			set {
				_shadowOffset = value;
				UpdateText();
			}
		}

		Color _shadowColor = Color.Black;
		[Description("Shadow color."), Category("Appearance"), DefaultValue(typeof(Color), "Black")]
		public Color ShadowColor {
			get { return _shadowColor; }
			set {
				_shadowColor = value;
				UpdateText();
			}
		}

		bool _shadowEnable = false;
		[Description("Enables or disables the text shadow."), Category("Appearance"), DefaultValue(false)]
		public bool ShadowEnabled {
			get { return _shadowEnable; }
			set {
				_shadowEnable = value;
				UpdateText();
			}
		}

		#endregion

		#region Overlay properties

		bool _overlayEnable = true;
		[Description("Enables or disables the text overlay."), Category("Appearance"), DefaultValue(true)]
		public bool OverlayEnabled {
			get { return _overlayEnable; }
			set {
				_overlayEnable = value;
				UpdateText();
			}
		}

		#endregion

		#region Border properties

		Color _borderColor = Color.Red;
		[Description("Border color."), Category("Appearance"), DefaultValue(typeof(Color), "Red")]
		public Color BorderColor {
			get { return _borderColor; }
			set {
				_borderColor = value;
				UpdateText();
			}
		}

		int _borderSize = 1;
		[Description("Border size."), Category("Appearance"), DefaultValue(1)]
		public int BorderSize {
			get { return _borderSize; }
			set {
				_borderSize = value;
				UpdateText();
			}
		}

		bool _borderEnable = false;
		[Description("Enables or disables border drawing."), Category("Appearance"), DefaultValue(false)]
		public bool BorderEnabled {
			get { return _borderEnable; }
			set {
				_borderEnable = value;
				UpdateText();
			}
		}

		#endregion

		#region Text format properties

		HorizontalAlignment _horizontal = HorizontalAlignment.Left;

		/// <summary>Gets or sets the horizontal text alignment setting.</summary>
		[Description("Horizontal text alignment."), Category("Appearance"), DefaultValue(typeof(HorizontalAlignment), "Left")]
		public HorizontalAlignment TextAlign {
			get { return _horizontal; }
			set {
				_horizontal = value;
				UpdateText();
			}
		}

		VerticalAlignment _vertical = VerticalAlignment.Top;

		/// <summary>Gets or sets the vertical text alignment setting.</summary>
		[Description("Vertical text alignment."), Category("Appearance"), DefaultValue(typeof(VerticalAlignment), "Top")]
		public VerticalAlignment TextAlignVertical {
			get { return _vertical; }
			set {
				_vertical = value;
				UpdateText();
			}
		}

		bool _singleLine = true;

		/// <summary>Gets or sets whether the text will be laid out on a single line or on multiple lines.</summary>
		[Description("Single line text."), Category("Appearance"), DefaultValue(true)]
		public bool SingleLine {
			get { return _singleLine; }
			set {
				_singleLine = value;
				UpdateText();
			}
		}

		bool _endEllipsis = false;

		/// <summary>Gets or sets whether the text lines over the label's border should be trimmed with an ellipsis.</summary>
		[Description("Removes the end of trimmed lines and replaces them with an ellipsis."), Category("Appearance"), DefaultValue(false)]
		public bool EndEllipsis {
			get { return _endEllipsis; }
			set {
				_endEllipsis = value;
				UpdateText();
			}
		}

		bool _wordBreak = false;

		/// <summary>Gets or sets whether the text should break only at the end of a word.</summary>
		[Description("Break the text at the end of a word."), Category("Appearance"), DefaultValue(false)]
		public bool WordBreak {
			get { return _wordBreak; }
			set {
				_wordBreak = value;
				UpdateText();
			}
		}

		bool _wordEllipsis = false;

		/// <summary>Gets or sets whether the text should be trimmed to the last word and an ellipse should be placed at the end of the line.</summary>
		[Description("Trims the line to the nearest word and an ellipsis is placed at the end of a trimmed line."), Category("Appearance"), DefaultValue(false)]
		public bool WordEllipsis {
			get { return _wordBreak; }
			set {
				_wordBreak = value;
				UpdateText();
			}
		}

		#endregion

		#region Events

		protected override void WndProc(ref Message m) {
			if (/*_transparent &&*/ m.Msg == VistaControls.NativeMethods.WM_NCHITTEST) {
				base.WndProc(ref m);

				m.Result = new IntPtr(VistaControls.NativeMethods.HTTRANSPARENT);

				return;
			}

			base.WndProc(ref m);
		}

		#endregion

		#region Drawing

		protected int CountOptions() {
			int count = 0;
			if (_glowEnable) ++count;
			if (_shadowEnable) ++count;
			if (!_overlayEnable) ++count;
			if (_borderEnable) ++count;

			return count;
		}

		protected IThemeTextOption[] CreateOptions() {
			IThemeTextOption[] options = new IThemeTextOption[CountOptions()];

			int curr = 0;

			if (_glowEnable) {
				options[curr] = new GlowOption(_glowSize);
				++curr;
			}
			if (_shadowEnable) {
				options[curr] = new ShadowOption(_shadowColor, _shadowOffset, _shadowType);
				++curr;
			}
			if (!_overlayEnable) {
				options[curr] = new OverlayOption(_overlayEnable);
				++curr;
			}
			if (_borderEnable) {
				options[curr] = new BorderOption(_borderColor, _borderSize);
				++curr;
			}

			return options;
		}

		protected TextFormatFlags CreateFormatFlags() {
			TextFormatFlags ret = TextFormatFlags.Default;

			switch (_horizontal) {
				case HorizontalAlignment.Left:
					ret |= TextFormatFlags.Left; break;
				case HorizontalAlignment.Center:
					ret |= TextFormatFlags.HorizontalCenter; break;
				case HorizontalAlignment.Right:
					ret |= TextFormatFlags.Right; break;
			}

			switch (_vertical) {
				case VerticalAlignment.Top:
					ret |= TextFormatFlags.Top; break;
				case VerticalAlignment.Center:
					ret |= TextFormatFlags.VerticalCenter; break;
				case VerticalAlignment.Bottom:
					ret |= TextFormatFlags.Bottom; break;
			}

			if (_singleLine)
				ret |= TextFormatFlags.SingleLine;

			if (_endEllipsis)
				ret |= TextFormatFlags.EndEllipsis;

			if (_wordBreak)
				ret |= TextFormatFlags.WordBreak;
			if (_wordEllipsis)
				ret |= TextFormatFlags.WordEllipsis;

			if (RightToLeft == RightToLeft.Yes)
				ret |= TextFormatFlags.RightToLeft;

			return ret;
		}

		protected override void OnInvalidated(InvalidateEventArgs e) {
			base.OnInvalidated(e);

			//Invalidate parent
            if(Parent != null)
			    Parent.Invalidate(this.ClientRectangle, false);
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			if (Visible) {
				//Deferred text rendering (first render)
				if (_hText == null)
					_hText = ThemedText.Create(e.Graphics, Text, Font, Padding,
						new Rectangle(0, 0, Size.Width, Size.Height), ForeColor,
						CreateFormatFlags(), CreateOptions());

				if (_needsUpdate) {
					//Draw (avoids flickering because of delayed redraw)
					_hText.Draw(e.Graphics, new Rectangle(0, 0, Size.Width, Size.Height));

					//Redraw the text element
					_hText.Update(e.Graphics, Text, Font, Padding,
						new Rectangle(0, 0, Size.Width, Size.Height), ForeColor,
						CreateFormatFlags(), CreateOptions());

					//Signal update as done, and then redraw again with updated graphics
					_needsUpdate = false;
					this.Invalidate();
					
				}
				else {
					//Simply redraw
					_hText.Draw(e.Graphics, new Rectangle(0, 0, Size.Width, Size.Height));
				}
			}
		}

		#endregion

	}

}
