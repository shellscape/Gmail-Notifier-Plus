/*****************************************************
 *            Vista Controls for .NET 2.0
 * 
 * http://www.codeplex.com/vistacontrols
 * 
 * @author: Nick Berardi
 * @www: http://www.coderjournal.com/2007/03/creating-a-vista-like-search-box/
 * @integration: Lorenz Cuno Klopfenstein
 * Licensed under Microsoft Community License (Ms-CL)
 * 
 *****************************************************/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace VistaControls
{
	/// <summary>A standard WinForms text box presenting the common Vista "search" interface.
	/// Reacts on user input by raising "SearchStarted" events.</summary>
	[Designer(typeof(VistaControls.Design.SearchTextBoxDesigner))]
	[DefaultEvent("SearchStarted")]
	[DefaultProperty("Text")]
	public partial class SearchTextBox : Control
	{
		private const string DefaultInactiveText = "Search";
		private const int DefaultTimerInterval = 500;

		private bool _active;

		private Color _hoverButtonColor;
		private Color _activeBackColor;
		private Color _activeForeColor;
		private Color _inactiveBackColor;
		private Color _inactiveForeColor;

		private Font _inactiveFont;

		private string _inactiveText;

		private Timer _timer;

		#region Construction

		protected override CreateParams CreateParams {
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get {
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= NativeMethods.WS_EX_CONTROLPARENT;
				createParams.ExStyle &= ~NativeMethods.WS_EX_CLIENTEDGE;

				// make sure WS_BORDER is present in the style
				createParams.Style |= NativeMethods.WS_BORDER;

				return createParams;
			}
		}


		public SearchTextBox() {
			//Load defaults
			_hoverButtonColor = SystemColors.GradientInactiveCaption;
			_activeBackColor = SystemColors.Window;
			_activeForeColor = SystemColors.WindowText;
			_inactiveBackColor = SystemColors.InactiveBorder;
			_inactiveForeColor = SystemColors.GrayText;

			_inactiveFont = new Font(this.Font, FontStyle.Italic);

			_inactiveText = DefaultInactiveText;

			this.Cursor = Cursors.IBeam;

			InitializeComponent();

			//Load properties
			BackColor = InactiveBackColor;
			ForeColor = InactiveForeColor;

			searchOverlayLabel.Font = InactiveFont;
			searchOverlayLabel.ForeColor = InactiveForeColor;
			searchOverlayLabel.BackColor = InactiveBackColor;
			searchOverlayLabel.Text = InactiveText;

			searchText.Font = Font;
			searchText.ForeColor = ActiveForeColor;
			searchText.BackColor = InactiveBackColor;

			StartSearchAfterDelay = true;
			StartSearchOnEnter = false;

			_active = false;

			SetTextActive(false);
			SetActive(false);

			_timer = new Timer();
			_timer.Interval = DefaultTimerInterval;
			_timer.Tick += new EventHandler(SearchTimer_Tick);
		}

		#endregion

		#region Events

		public new event EventHandler TextChanged
		{
			add { searchText.TextChanged += value; }
			remove { searchText.TextChanged -= value; }
		}

		[Description("Raised after an interval after the last user input."), Category("Action")]
		public event EventHandler SearchStarted;

		protected virtual void OnSearchStarted(EventArgs e) {
			if (SearchStarted != null) {
				SearchStarted(this, e);
			}
		}

		[Description("Raised when the user clicks on the X to cancel the search."), Category("Action")]
		public event EventHandler SearchCancelled;

		protected virtual void OnSearchCancelled(EventArgs e) {
			if (SearchCancelled != null) {
				SearchCancelled(this, e);
			}
		}

		#endregion

		#region Properties

		/// <summary>Gets or sets the background Color of the button when the mouse hovers on it.</summary>
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "GradientInactiveCaption")]
		public Color HoverButtonColor
		{
			get { return _hoverButtonColor; }
			set {
				_hoverButtonColor = value;
				RefreshColors();
			}
		}

		/// <summary>Gets or sets the ForeColor of the control when the search box is active.</summary>
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "WindowText")]
		public Color ActiveForeColor
		{
			get { return _activeForeColor; }
			set {
				_activeForeColor = value;
				RefreshColors();
			}
		}

		/// <summary>Gets or sets the BackColor of the control when the search box is active.</summary>
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "Window")]
		public Color ActiveBackColor
		{
			get { return _activeBackColor; }
			set {
				_activeBackColor = value;
				RefreshColors();
			}
		}

		/// <summary>Gets or sets the ForeColor of the control when the search box is inactive.</summary>
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "GrayText")]
		public Color InactiveForeColor
		{
			get { return _inactiveForeColor; }
			set {
				_inactiveForeColor = value;
				RefreshColors();
			}
		}

		/// <summary>Gets or sets the BackColor of the control when the search box is inactive.</summary>
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "InactiveBorder")]
		public Color InactiveBackColor
		{
			get { return _inactiveBackColor; }
			set {
				_inactiveBackColor = value;
				RefreshColors();
			}
		}

		/*   Removed 29/03/2008, put in costructor
		[Category("Appearance")]
		[DefaultValue(typeof(Cursor), "IBeam")]
		public override Cursor Cursor
		{
			get { return base.Cursor; }
			set { base.Cursor = value; }
		}*/

		/// <summary>Temporary ForeColor property of the control. You should use InactiveForeColor and ActiveForeColor instead.</summary>
		[Browsable(false)]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}

		/// <summary>Temporary BackColor property of the control. You should use InactiveBackColor and ActiveBackColor instead.</summary>
		[Browsable(false)]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		/// <summary>Gets or sets the text that is shown on top of the text box when the user hasn't entered any text.</summary>
		[Category("Appearance")]
		[DefaultValue(DefaultInactiveText)]
		public string InactiveText
		{
			get { return _inactiveText; }
			set {
				_inactiveText = value;
				
				searchOverlayLabel.Text = value;
			}
		}

		/// <summary>Gets or sets the font used in the search text box.</summary>
		/// <remarks>Equals to the Font property.</remarks>
		[Category("Appearance")]
		[DefaultValue(typeof(Font), "Microsoft Sans Serif, 8.25pt")]
		public Font ActiveFont
		{
			get { return base.Font; }
			set {
				base.Font = value;

				searchText.Font = value;
			}
		}

		/// <summary>Gets or sets the font used to write the "inactivity label" on top of the control when the user hasn't entered any text.</summary>
		[Category("Appearance")]
		[DefaultValue(typeof(Font), "Microsoft Sans Serif, 8.25pt, style=Italic")]
		public Font InactiveFont
		{
			get {
				if (_inactiveFont == null)
					return Parent.Font;
				else
					return _inactiveFont;
			}
			set {
				_inactiveFont = value;

				searchOverlayLabel.Font = value;
			}
		}

		/// <summary>Overall Font property of the control. Property changes are forwarded to the ActiveFont property.</summary>
		[Browsable(false)]
		public override Font Font
		{
			get { return base.Font; }
			set {
				base.Font = value;
				ActiveFont = value;
			}
		}

		//[Category("Appearance")]
		public override string Text
		{
			get { return searchText.Text; }
			set { searchText.Text = value; }
		}

		/// <summary>Returns true if the user entered some text in the search textbox.</summary>
		protected bool TextEntered {
			get { return !String.IsNullOrEmpty(searchText.Text); }
		}

		[Description("Determines the delay between when the text is edited and the search event is raised."), Category("Behavior"), DefaultValue(DefaultTimerInterval)]
		public int SearchTimer {
			get { return _timer.Interval; }
			set { _timer.Interval = value; }
		}

		[Description("Gets or sets whether the control raises a SearchStarted event after user input."), Category("Behavior"), DefaultValue(true)]
		public bool StartSearchAfterDelay {
			get;
			set;
		}

		[Description("Gets or sets whether the control raises a SearchStarted event when the user hits the Enter key on the text box."), Category("Behavior"), DefaultValue(false)]
		public bool StartSearchOnEnter {
			get;
			set;
		}

		#endregion

		#region Methods

		private void SetActive(bool value){
			if (TextEntered)
				value = true;

			//if (_active == value)
			//	return;

			_active = value;

			RefreshColors();
		}


		private void SetTextActive(bool value){
			bool active = value || TextEntered;

			this.searchOverlayLabel.Visible = !active;
			this.searchText.Visible = active;

			if (value && !searchText.Focused)
				this.searchText.Select();
		}

		private void RefreshColors() {
			this.SuspendLayout();

			//Set correct color
			this.BackColor = _active ? ActiveBackColor : InactiveBackColor;
			this.ForeColor = _active ? ActiveForeColor : InactiveForeColor;

			//Set color of children controls
			this.searchText.ForeColor = this.ForeColor;
			this.searchOverlayLabel.BackColor = this.BackColor;
			this.searchText.BackColor = this.BackColor;

			this.ResumeLayout(true);
		}

		/// <summary>Puts the focus on the text box and moves the caret to the end of the text, without selecting it.</summary>
		public void SetFocusWithoutSelection() {
			searchText.Select(searchText.Text.Length, 0);
			searchText.Focus();
		}

		#endregion

		#region Event Methods

		protected override void OnGotFocus(EventArgs e) {
			SetTextActive(true);
			SetActive(true);

			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e) {
			if (this.searchText.Focused)
				return;

			SetTextActive(false);
			SetActive(false);

			base.OnLostFocus(e);
		}

		protected override void OnMouseEnter(EventArgs e) {
			SetActive(true);

			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e) {
			SetActive(false);

			base.OnMouseLeave(e);
		}

		protected override void OnClick(EventArgs e) {
			this.Select();

			base.OnClick(e);
		}

		protected override void OnTextChanged(EventArgs e) {
			searchImage.Image = TextEntered ? Resources.Pictures.ActiveSearch : Resources.Pictures.InactiveSearch;

			//Start search timer
			_timer.Stop();
			if(TextEntered && StartSearchAfterDelay)
				_timer.Start();

			base.OnTextChanged(e);
		}

		#endregion

		#region Sub Controls event handling

		private void searchImage_MouseEnter(object sender, EventArgs e) {
			SetActive(true);

			if (TextEntered)
				searchImage.BackColor = HoverButtonColor;
		}

		private void searchImage_MouseLeave(object sender, EventArgs e) {
			SetActive(false);

			searchImage.BackColor = Color.Empty;
		}

		private void searchImage_Click(object sender, System.EventArgs e)
		{
			if (TextEntered) {
				this.searchText.ResetText();
				OnLostFocus(EventArgs.Empty);

				OnSearchCancelled(EventArgs.Empty);

				searchImage.BackColor = Color.Empty;
			}
		}

		private void searchText_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter && StartSearchOnEnter) {
				e.Handled = true;
				e.SuppressKeyPress = true;
				OnSearchStarted(EventArgs.Empty);
			}
		}

		private void searchText_TextChanged(object sender, EventArgs e) {
			OnTextChanged(e);
		}

		private void searchText_LostFocus(object sender, System.EventArgs e) {
			OnLostFocus(e);
		}

		private void searchText_GotFocus(object sender, System.EventArgs e) {
			OnGotFocus(e);
		}

		private void searchOverlayLabel_Click(object sender, EventArgs e) {
			OnClick(EventArgs.Empty);
		}

		private void searchOverlayLabel_MouseEnter(object sender, EventArgs e) {
			SetActive(true);
		}

		private void searchOverlayLabel_MouseLeave(object sender, EventArgs e) {
			SetActive(false);
		}

		void SearchTimer_Tick(object sender, EventArgs e) {
			_timer.Stop();

			OnSearchStarted(EventArgs.Empty);
		}

		#endregion
	}
}
