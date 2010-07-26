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
using System.Windows.Forms;
using System.Drawing;

namespace VistaControls.Dwm {
	public static class GlassHelper {

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// Handler will be kept alive by the event references on the form.
		/// As soon as the form is disposed, the handler will be disposed as well.
		/// </remarks>
		class HandleFormMovement {
			Margins _margins;
			bool _tracking = false;
			Point _lastPos;

			public HandleFormMovement(Form form, Margins margins) {
				_margins = margins;

				//Register handlers
				form.MouseDown += new MouseEventHandler(form_MouseDown);
				form.MouseUp += new MouseEventHandler(form_MouseUp);
				form.MouseMove += new MouseEventHandler(form_MouseMove);
			}

			void form_MouseMove(object sender, MouseEventArgs e) {
				if (_tracking) {
					Form f = (Form)sender;

					Point screen = f.PointToScreen(e.Location);

					Point diff = new Point(screen.X - _lastPos.X, screen.Y - _lastPos.Y);

					Point loc = f.Location;
					loc.Offset(diff);
					f.Location = loc;

					_lastPos = screen;
				}
			}

			void form_MouseUp(object sender, MouseEventArgs e) {
				if(e.Button == MouseButtons.Left)
					_tracking = false;
			}

			void form_MouseDown(object sender, MouseEventArgs e) {
				if (e.Button == MouseButtons.Left) {
					Form f = (Form)sender;
					if (_margins.IsMarginless || (
							e.X <= _margins.Left ||
							e.X >= f.ClientSize.Width - _margins.Right ||
							e.Y <= _margins.Top ||
							e.Y >= f.ClientSize.Height - _margins.Bottom)
						) {
						_tracking = true;
						_lastPos = f.PointToScreen(e.Location);
					}
				}
			}
		}

		/// <summary>
		/// Adds a handler on the Form that enables the user to move the window around
		/// by clicking on a glass margin (or the title bar, as usual).
		/// </summary>
		/// <param name="form">The form that will be controlled.</param>
		/// <param name="margins">Margins of the glass sheet.</param>
		/// <remarks>
		/// Eventual UI elements on the glass sheet will prevent the handler from receiving events
		/// (except the ThemeText control, which manually redirects mouse events to the form).
		/// </remarks>
		public static void HandleFormMovementOnGlass(Form form, Margins margins) {
			HandleFormMovement tmpHandler = new HandleFormMovement(form, margins);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// Handler will be kept alive by the event references on the form.
		/// As soon as the form is disposed, the handler will be disposed as well.
		/// </remarks>
		class HandleBackground {
			Margins _margins;

			public HandleBackground(Form form, Margins m) {
				_margins = m;

				//Hook
				form.Paint += new PaintEventHandler(form_Paint);
			}

			void form_Paint(object sender, PaintEventArgs e) {
				if (_margins.IsMarginless)
					e.Graphics.Clear(Color.Black);
				else {
					Form f = (Form)sender;

					Rectangle[] rects = new Rectangle[] {
						new Rectangle(0, 0, f.ClientSize.Width, _margins.Top),
						new Rectangle(f.ClientSize.Width - _margins.Right, 0, _margins.Right, f.ClientSize.Height),
						new Rectangle(0, f.ClientSize.Height - _margins.Bottom, f.ClientSize.Width, _margins.Bottom),
						new Rectangle(0, 0, _margins.Left, f.ClientSize.Height)
					};

					e.Graphics.FillRectangles(Brushes.Black, rects);
				}
			}
		}

		/// <summary>
		/// Adds a handler on the Form that automatically paints the glass background black
		/// </summary>
		/// <param name="form">The form that will be controlled.</param>
		/// <param name="margins">Margins of the glass sheet.</param>
		public static void HandleBackgroundPainting(Form form, Margins margins) {
			HandleBackground tmpHandler = new HandleBackground(form, margins);
		}

	}
}
