using System;
using System.Runtime.Serialization;

using GmailNotifierPlus.Utilities;

namespace GmailNotifierPlus {

	public enum AccountTypes {
		Regular,
		GoogleApps
	}

	[DataContract(Name="account")]
	public class Account {

		public static class Domains {
			public const String Gmail = "gmail.com";
			public const String GmailAlt = "googlemail.com";
			public const String GmailUK = "googlemail.co.uk";
			public const String WindowTitle = "Gmail Notifier Plus";
		}

		public String Login { get; set; }
		public String Password { get; set; }

		[DataMember(Name="default")]
		public Boolean Default { get; set; }

		public String Domain { get; private set; }
		public String FullAddress { get; private set; }
		public String Name { get; private set; }
		public AccountTypes Type { get; private set; }

		[DataMember(Name = "ahead")]
		private String LoginEncrypted {
			get {
				return EncryptionHelper.Encrypt(this.Login);
			}
			set {
				this.Login = EncryptionHelper.Decrypt(value);
			}
		}

		[DataMember(Name = "aft")]
		private String PasswordEncrypted {
			get {
				return EncryptionHelper.Encrypt(this.Password);
			}
			set {
				this.Password = EncryptionHelper.Decrypt(value);
			}
		}

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

