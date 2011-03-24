using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GmailNotifierPlus.Forms;

namespace GmailNotifierPlus {
	public static class ToastManager {

		private static Queue<Toast> _loaf = new Queue<Toast>();

		public static void Pop(Account account) {

			Toast slice = new Toast(account);

			slice.FormClosed += SliceClosed;

			PositionSlice(slice);

			_loaf.Enqueue(slice);

			slice.Show();
		}

		private static void PositionSlice(Toast slice) {

			Screen primary = Screen.PrimaryScreen;
			Rectangle workingArea = primary.WorkingArea;
			Point start = new Point(workingArea.Width - slice.Width, workingArea.Height - slice.Height);

			start.Y -= (_loaf.Count * slice.Height); // all of the slices are a constant height

			slice.Location = start;
		}

		private static void SliceClosed(object sender, EventArgs e) {

			Toast burnt = sender as Toast;
			_loaf.Dequeue();

			foreach (Toast slice in _loaf) {
				Slide(slice);
			}
		}

		private static void Slide(Toast slice) {

			int targetY = slice.Top + slice.Height;
			Timer timer = new Timer() {
				Interval = 10
			};

			timer.Tick += delegate(object sender, EventArgs args) {
				slice.Top = Math.Min(slice.Top + 10, targetY);

				if (slice.Top == targetY) {
					timer.Stop();
					timer.Dispose();
				}
			};

			timer.Start();
		}

	}

}
