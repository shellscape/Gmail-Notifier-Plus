namespace GmailNotifierPlus {

	using System;
	using System.Xml;
	using System.Xml.Serialization;

	public enum AccountTypes {
		Regular,
		GoogleApps
	}

	public class Account {

		public static class Domains {
			public const String Gmail = "gmail.com";
			public const String GmailAlt = "googlemail.com";
			public const String GmailUK = "googlemail.co.uk";
			public const String WindowTitle = "Gmail Notifier Plus";
		}

		public String Login { get; set; }
		public String Password { get; set; }
		public Boolean Default { get; set; }

		[XmlIgnore]
		public String Domain { get; private set; }

		[XmlIgnore]
		public String FullAddress { get; private set; }

		[XmlIgnore]
		public String Name { get; private set; }

		[XmlIgnore]
		public AccountTypes Type { get; private set; }

		public Account() {
			this.Login = this.Password = this.Name = this.Domain = this.FullAddress = String.Empty;
			this.Type = AccountTypes.Regular;
		}

		public Account(String login, String password) {

			this.Login = login;
			this.Password = password;

			Init();
		}

		public void Init() {
			if (!String.IsNullOrEmpty(this.Login)) {
				String[] strArray = this.Login.Split(new char[] { '@' });

				if ((strArray.Length > 1) && !String.IsNullOrEmpty(strArray[1])) {
					this.Name = strArray[0];
					this.Domain = strArray[1];
				}
				else {
					this.Name = this.Login;
					this.Domain = Domains.Gmail;
				}

				if (((this.Domain == Domains.Gmail) || (this.Domain == Domains.GmailAlt)) || (this.Domain == Domains.GmailUK)) {
					this.Type = AccountTypes.Regular;
				}
				else {
					this.Type = AccountTypes.GoogleApps;
				}

				this.FullAddress = this.Name + "@" + this.Domain;
			}
		}

		[XmlIgnore]
		public bool IsEmpty {
			get {
				if (!String.IsNullOrEmpty(this.Login)) {
					return String.IsNullOrEmpty(this.Password);
				}
				return true;
			}
		}


	}
}

