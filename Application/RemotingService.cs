using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GmailNotifierPlus.Forms;

namespace GmailNotifierPlus {

	public class RemoteMethods : Shellscape.ArgumentCollection {
		public const String Check = "check";
		public const String Settings = "settings";
		public const String About = "about";
		public const String Help = "help";
		public const String Mailto = "mailto";
	}

	public class RemotingService : Shellscape.RemotingService<RemoteMethods> {

		private static Forms.Preferences _prefs;
		private static RemotingService _instance;

		public static RemotingService Instance {
			get {
				if (_instance == null) {
					_instance = new RemotingService();
				}
				return _instance;
			}
		}

		[Shellscape.RemoteServiceMethod(RemoteMethods.Check)]
		public void CheckMail() {
			Main form = Program.MainForm;
			MethodInvoker method = delegate() {
				form.CheckMail();
			};

			if (form.InvokeRequired) {
				form.Invoke(method);
			}
			else {
				method();
			}
		}

		[Shellscape.RemoteServiceMethod(RemoteMethods.Settings)]
		public void ShowPreferences() {

			if (_prefs == null) {
				_prefs = new Preferences();
				//_prefs.FormClosed += delegate(object sender, FormClosedEventArgs e) {
				//  _prefs.Dispose();
				//  _prefs = null;
				//}; ;
				//_prefs.Show();
			}

			MethodInvoker method = delegate() {
				_prefs.Show();
				_prefs.BringToFront();
				_prefs.Focus();
			};

			if (_prefs.InvokeRequired) {
				_prefs.Invoke(method);
			}
			else {
				method();
			}

			//_prefs.Activate();
			//_prefs.BringToFront();

			//MethodInvoker method = null;

			//if (base.InvokeRequired) {
			//  if (method == null) {
			//    method = delegate { this.OpenSettingsWindow(); };
			//  }
			//  base.Invoke(method);
			//}

		}

		[Shellscape.RemoteServiceMethod(RemoteMethods.About)]
		public void ShowAbout() {
			About about = new About();
			MethodInvoker method = delegate() {
				about.Show();
				about.BringToFront();
				about.Focus();
			};

			if (about.InvokeRequired) {
				about.Invoke(method);
			}
			else{
				method();
			}

			//MethodInvoker method = null;

			//if (base.InvokeRequired) {
			//  if (method == null) {
			//    method = delegate { this.ShowAbout(); };
			//  }
			//  base.Invoke(method);
			//}
			//else {
			//  this.ShowAbout();
			//}
		}

		[Shellscape.RemoteServiceMethod(RemoteMethods.Help)]
		public void ShowHelp() {
			Utilities.UrlHelper.Launch(null, "https://github.com/shellscape/Gmail-Notifier-Plus/wiki");
		}

		[Shellscape.RemoteServiceMethod(RemoteMethods.Mailto)]
		public void Mailto() {
			
			String mailto = this.Arguments[0];
			Account mailtoAccount = Config.Current.Accounts.Where(o => o.HandlesMailto).FirstOrDefault();

			if (mailtoAccount == null) {
				return;
			}

			//Uri mailtoUri = new Uri(mailto);
			//System.Collections.Specialized.NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(mailtoUri.Query);
			Shellscape.Browser browser = mailtoAccount.Browser ?? Shellscape.Utilities.BrowserHelper.DefaultBrowser;
			String accountUrl = Utilities.UrlHelper.GetBaseUrl(mailtoAccount);

			//String to = String.Concat(mailtoUri.UserInfo, "@", mailtoUri.Host);
			//String parameters = String.Format("to={1}&su={2}&body={3}",  to, queryString["subject"], queryString["body"]);
			String url = String.Concat(accountUrl, "?extsrc=mailto&url=", System.Web.HttpUtility.UrlEncode(mailto));

			try {
				using (System.Diagnostics.Process process = new System.Diagnostics.Process()) {
					process.StartInfo.Arguments = url;
					process.StartInfo.FileName = browser.Path;
					process.Start();
				}
			}
			catch (Exception e) {
				Utilities.ErrorHelper.Report(e);
			} // catch-all is fine here, not a critical function
		}
	}

}
