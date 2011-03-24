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

			Account account = new Account("andrew@shellscape.org", String.Empty) {
				Unread = 2
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
			
			ToastManager.Pop(account);
		}
	}

	}
