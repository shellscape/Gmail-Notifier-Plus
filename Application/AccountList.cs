using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GmailNotifierPlus {
	public class AccountList : List<Account> {

		[XmlIgnore]
		public Account Default {
			get {
				var accounts = (from a in this where a.Default == true select a);

				if (accounts.Count() > 0) {
					Account account = accounts.First();
					return account;
				}
				else {
					return this.Count > 0 ? this.First() : null;
				}
			}
		}

		public new Account this[int index] {
			get {
				if (index >= this.Count || index < 0) {
					return null;
				}
				return base[index];
			}
			set {
				base[index] = value;
			}
		}
 



	}
}
