namespace GmailNotifierPlus.Utilities {

	using System;
	using System.Web;

	using GmailNotifierPlus.Resources;
	
	public static class UrlHelper {

		public static class Uris {
			public const String Base = @"http://mail.google.com/{0}";
			public const String Donate = @"https://www.paypal.com/cgi-bin/webscr?cmd=_s-Url_Donate=https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=7491288";
			public const String Feed = @"https://mail.google.com/{0}/feed/atom";
			public const String Login = @"https://www.google.com/{0}/{1}?continue={2}&service=mail&Email={3}&Passwd={4}&null=Sign+in&GALX={5}";
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

		private static Config config = Config.Current;

		public static string BuildComposeUrl(int accountIndex) {
			return (GetBaseUrl(accountIndex) + "#compose");
		}

		public static string BuildInboxUrl(int accountIndex) {
			return (GetBaseUrl(accountIndex) + "#inbox");
		}

		public static string BuildMailUrl(string link, int accountIndex) {
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
			return (GetBaseUrl(accountIndex) + "#inbox/" + str);
		}

		public static string GetBaseUrl(int accountIndex) {
			Account account = config.Accounts[accountIndex];
			if ((account != null) && (account.Type != AccountTypes.Regular)) {
				return string.Format(Uris.Base, Params.BaseApps + config.Accounts[accountIndex].Domain);
			}
			return string.Format(Uris.Base, Params.Base);
		}

		public static string GetFeedUrl(int accountIndex) {
			if (config.Accounts[accountIndex].Type == AccountTypes.Regular) {
				return string.Format(Uris.Feed, Params.Base);
			}
			return string.Format(Uris.Feed, Params.BaseApps + config.Accounts[accountIndex].Domain);
		}

		public static string ToLoginUrl(string continueUrl, int accountIndex) {
			string param;
			string page;

			if (config.Accounts[accountIndex].Type == AccountTypes.Regular) {
				param = Params.Login;
				page = Pages.Login;
			}
			else {
				param = Params.BaseApps + config.Accounts[accountIndex].Domain;
				page = Pages.LoginApps;
			}
			
			object urlData = new object[] { 
				param, 
				page, 
				HttpUtility.UrlEncode(continueUrl), 
				HttpUtility.UrlEncode(config.Accounts[accountIndex].Name), 
				HttpUtility.UrlEncode(config.Accounts[accountIndex].Password), 
				string.Empty 
			};

			return string.Format(Uris.Login, urlData);
		}
	}
}

