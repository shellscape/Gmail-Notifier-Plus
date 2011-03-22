using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GmailNotifierPlus {

	[XmlRoot("Locale")]
	public class Locale {

		private String[] RtlLangs = new String[] { "ar-dz", "he-il", "fa-ir" };

		[XmlIgnore]
		public String LanguageCode { get; set; }

		[XmlIgnore]
		internal bool IsRightToLeftLanguage {
			get {
				return RtlLangs.Contains(this.LanguageCode);
			}
		}

		[XmlIgnore]
		public static Locale Current { get; private set; }

		public void Init() {
			Current = this;
		}

		public class LocaleButtons {

			public String AddNewAccount { get; set; }
			public String Browse { get; set; }
			public String Cancel { get; set; }
			public String Default { get; set; }
			public String Edit { get; set; }
			public String OK { get; set; }
			public String Next { get; set; }
			public String Save { get; set; }
			public String LetsGo { get; set; }
			public String Sweet { get; set; }
			public String Remove { get; set; }
		}

		public class LocaleCheckboxes {
			public String FlashTaskbar { get; set; }
			public String ShowTray { get; set; }
			public String ShowToast { get; set; }
			public String CheckUpdates { get; set; }
			public String UseMailto { get; set; }
		}

		public class LocaleConfig {

			public String Accounts { get; set; }
			public String Appearance { get; set; }
			public String General { get; set; }
			public LocaleConfigPanels Panels { get; set; }

		}

		public class LocaleConfigPanels {
			public String Accounts { get; set; }
			public String Appearance { get; set; }
			public String General { get; set; }
			public String NewAccount { get; set; }
		}

		public class LocaleLabels {

			public String About { get; set; }
			public String AccountsIntro { get; set; }
			public String AccountsIntroNone { get; set; }
			public String Additional { get; set; }
			public String Browser { get; set; }
			public String BrowseDialog { get; set; }
			public String CheckLogin { get; set; }
			public String CheckMail { get; set; }
			public String Compose { get; set; }
			public String Configuration { get; set; }
			public String ConfigurationShort { get; set; }
			public String Connecting { get; set; }
			public String ConnectionUnavailable { get; set; }
			public String CustomSound { get; set; }
			public String DefaultSound { get; set; }
			public String Edit { get; set; }
			public String Error { get; set; }
			public String Help { get; set; }
			public String Inbox { get; set; }
			public String Interval { get; set; }
			public String Language { get; set; }
			public String Login { get; set; }
			public String Minutes { get; set; }
			public String No { get; set; }
			public String NoAccount { get; set; }
			public String NoMail { get; set; }
			public String NoSound { get; set; }
			public String NoSubject { get; set; }
			public String OSErrorContent { get; set; }
			public String OSErrorTitle { get; set; }
			public String Password { get; set; }
			public String RemoveConfirmation { get; set; }
			public String RemoveConfirmationTitle { get; set; }
			public String Sound { get; set; }
			public String Unread { get; set; }
			public String WaveFiles { get; set; }
			public String Yes { get; set; }

		}

		public class LocaleTooltips {

			public String About { get; set; }
			public String Add { get; set; }
			public String Inbox { get; set; }
			public String Next { get; set; }
			public String OpenMail { get; set; }
			public String Previous { get; set; }
			public String Settings { get; set; }

		}

		public LocaleConfig Config { get; set; }
		public String Name { get; set; }
		public LocaleButtons Buttons { get; set; }
		public LocaleLabels Labels { get; set; }
		public LocaleTooltips Tooltips { get; set; }
		public LocaleCheckboxes Checkboxes { get; set; }
	}
}