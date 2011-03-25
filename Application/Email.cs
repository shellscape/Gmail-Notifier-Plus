using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GmailNotifierPlus {
	public class Email : IComparable<Email> {

		public Email() {
			From = Title = Message = When = Url = String.Empty;
		}

		public String From { get; set; }
		public String Title { get; set; }
		public String Message { get; set; }
		public String When { get; set; }
		public String Url { get; set; }
		public DateTime DateTime { get; set; }

		public int CompareTo(Email o) {
			return this.DateTime.CompareTo(o.DateTime);
		}

		public static Email FromNode(XmlNode node, Account account) {

			DateTime time = DateTime.Parse(node.ChildNodes.Item(3).InnerText.Replace("T24:", "T00:"));

			Email email = new Email() {
				Title = string.IsNullOrEmpty(node.ChildNodes.Item(0).InnerText) ? Locale.Current.Labels.NoSubject : node.ChildNodes.Item(0).InnerText,
				Message = node.ChildNodes.Item(1).InnerText,
				When = time.ToShortDateString() + " " + time.ToShortTimeString(),
				Url = Utilities.UrlHelper.BuildMailUrl(node.ChildNodes.Item(2).Attributes["href"].Value, account),
				DateTime = time
			};

			if ((node.ChildNodes.Item(6) != null) && (node.ChildNodes.Item(6).ChildNodes.Item(1) != null)) {
				email.From = node.ChildNodes.Item(6).ChildNodes[1].InnerText;
			}

			return email;
		}
	}
}
