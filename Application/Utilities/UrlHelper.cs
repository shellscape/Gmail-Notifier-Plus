using System;
using System.Web;

namespace GmailNotifierPlus.Utilities {

	public static class UrlHelper {

		public static class Uris {
			public const String Base = @"http://mail.google.com/{0}";
			public const String Donate = "";
			public const String Feed = @"https://mail.google.com/{0}/feed/atom";
			public const String Login = @"https://www.google.com/{0}/{1}?continue={2}&service=mail&Email={3}&Passwd={4}&null=Sign+in&GALX={5}";
			public const String Shellscape = @"http://shellscape.org";
		}

		public static class Params {
			public const String Base = "mail";
			public const String BaseApps = "a/";
			public const String Login = "accounts";
		}

		public static class Pages {
			public const String Login = "ServiceLoginAuth";
			public const String LoginApps = "LoginAction2";
		}

		private static Config _Config = Config.Current;

		public static void Launch(Account account, String url) {
			
			
			
			if (account == null || account.Browser == null) {
				System.Windows.Forms.Help.ShowHelp(Program.mainForm, url);
				return;
			}

			Boolean poop = false;
			System.Diagnostics.Process browser = new System.Diagnostics.Process();

			browser.StartInfo.Arguments = url;
			browser.StartInfo.FileName = account.Browser.Path;

			try {
				browser.Start();
			}
			catch (InvalidOperationException) { poop = true; }
			catch (System.ComponentModel.Win32Exception) { poop = true; }

			if (poop) {
				System.Windows.Forms.Help.ShowHelp(Program.mainForm, url);
			}
		}

		public static string BuildComposeUrl(int accountIndex) {
			return (GetBaseUrl(accountIndex) + "#compose");
		}

		public static string BuildInboxUrl(int accountIndex) {
			return (GetBaseUrl(accountIndex) + "#inbox");
		}

		public static string BuildInboxUrl(Account account) {
			return (GetBaseUrl(account) + "#inbox");
		}

		public static string BuildMailUrl(string link, int accountIndex) {
			Account account = _Config.Accounts[accountIndex];
			return BuildMailUrl(link, account);
		}

		public static string BuildMailUrl(string link, Account account) {
			string str = string.Empty;
			try {
				string str2 = "message_id=";
				int startIndex = link.IndexOf(str2) + str2.Length;
				int index = link.IndexOf("&view");
				str = link.Substring(startIndex, index - startIndex);
			}
			catch {
				return string.Empty;
			}
			return (GetBaseUrl(account) + "#inbox/" + str);
		}

		public static string GetBaseUrl(int accountIndex) {
			Account account = _Config.Accounts[accountIndex];

			return GetBaseUrl(account);
		}

		public static String GetBaseUrl(Account account) {
			if ((account != null) && (account.Type != AccountTypes.Regular)) {
				return string.Format(Uris.Base, Params.BaseApps + account.Domain);
			}
			return string.Format(Uris.Base, Params.Base);
		}

		public static string GetFeedUrl(Account account) {
			if (account.Type == AccountTypes.Regular) {
				return string.Format(Uris.Feed, Params.Base);
			}
			return string.Format(Uris.Feed, Params.BaseApps + account.Domain);
		}

		public static string ToLoginUrl(string continueUrl, int accountIndex) {
			string param;
			string page;
			Account account = _Config.Accounts[accountIndex];

			if (account.Type == AccountTypes.Regular) {
				param = Params.Login;
				page = Pages.Login;
			}
			else {
				param = Params.BaseApps + account.Domain;
				page = Pages.LoginApps;
			}

			object urlData = new object[] { 
				param, 
				page, 
				HttpUtility.UrlEncode(continueUrl), 
				HttpUtility.UrlEncode(account.Name), 
				HttpUtility.UrlEncode(account.Password), 
				string.Empty 
			};

			return string.Format(Uris.Login, urlData);
		}
	}
}

