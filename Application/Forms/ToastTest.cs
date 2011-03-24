using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace GmailNotifierPlus.Forms {
	public partial class ToastTest : Form {
		public ToastTest() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			ToastManager.Pop();
		}
	}

	public static class ToastManager {

		private static Queue<Toast> _loaf = new Queue<Toast>();

		public static void Pop() {

			Account account = new Account("andrew@shellscape.org", String.Empty) {
				Unread = 1
			};

			account.Emails.Add(new Email() {
				Title = "Test Email",
				Date = "3/24/2011 10:27 AM",
				Url = "http://google.com",
				Message = "This is a test email.",
				From = "stevo@hotmail.com"
			});

			account.Emails.Add(new Email() {
				Title = "SECOND EMAIL",
				Date = "3/14/2011 11:27 AM",
				Url = "http://msn.com",
				Message = "This is a test  22222.",
				From = "stevo222@hotmail.com"
			});
			
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
