using System;
using System.Linq;
using System.Xml.Serialization;

namespace GmailNotifierPlus.Localization {

	[XmlType(AnonymousType = true)]
	[XmlRoot(ElementName = "Locale", Namespace = "", IsNullable = false)]
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

		public String Name { get; set; }
		public LocaleCommon Common { get; set; }
		public LocaleJumpList JumpList { get; set; }
		public LocaleAbout About { get; set; }
		public LocalePreferences Preferences { get; set; }
		public LocaleThumbnails Thumbnails { get; set; }
		public LocaleToast Toast { get; set; }

	}

	public class LocaleCommon {

		public String Cancel { get; set; }
		public String Close { get; set; }
		public String Next { get; set; }
		public String OK { get; set; }
		public String Previous { get; set; }
		public String No { get; set; }
		public String Yes { get; set; }
		public String Unread { get; set; }

	}

	public class LocaleJumpList {

		public String DefaultAccount { get; set; }
		public String Inbox { get; set; }
		public String Compose { get; set; }
		public String Check { get; set; }
		public String Preferences { get; set; }
		public String About { get; set; }
		public String Help { get; set; }

	}

	public class LocaleAbout {

		public String WindowTitle { get; set; }
		public String Button { get; set; }
		public String FirstRun { get; set; }
		public String Checking { get; set; }
		public String UpdateProblem { get; set; }
		public String UpToDate { get; set; }
		public String NewVersion { get; set; }
	}

	public class LocalePreferences {

		public String WindowTitle { get; set; }
		public LocalePreferencesNavigation Navigation { get; set; }
		public LocalePreferencesPanels Panels { get; set; }
	}

	public class LocalePreferencesNavigation {

		public String General { get; set; }
		public String Accounts { get; set; }
		public String Appearance { get; set; }

	}

	public class LocalePreferencesPanels {

		public LocaleGeneral General { get; set; }
		public LocaleAccountPanel Accounts { get; set; }
		public LocaleAppearance Appearance { get; set; }

	}

	public class LocaleGeneral {

		public String Title { get; set; }
		public String CheckEmail { get; set; }
		public String CheckMinutes { get; set; }
		public String CheckUpdates { get; set; }
		public LocaleGeneralSound Sound { get; set; }
	}

	public class LocaleGeneralSound {

		public String Title { get; set; }
		public String None { get; set; }
		public String Default { get; set; }
		public String Custom { get; set; }
		public String Browse { get; set; }
		public String BrowseWindowTitle { get; set; }
		public String WaveFiles { get; set; }

	}

	public class LocaleAccountPanel {

		public String Title { get; set; }
		public String AddNew { get; set; }
		public LocaleAccount Account { get; set; }
	}

	public class LocaleAccount {

		public String Title { get; set; }
		public String Address { get; set; }
		public String Password { get; set; }
		public String Mailto { get; set; }
		public String DefaultAccount { get; set; }
		public String Browser { get; set; }
		public String AddAccount { get; set; }
		public String RemoveAccount { get; set; }
		public String RemoveConfirmation { get; set; }
		public String RemoveConfirmationTitle { get; set; }
		public String AccountsIntroNone { get; set; }
		public String Error { get; set; }
	}

	public class LocaleAppearance {

		public String Title { get; set; }
		public String Language { get; set; }
		public String FlashTaskbar { get; set; }
		public String ShowTray { get; set; }
		public String ShowToast { get; set; }
	}

	public class LocaleThumbnails {

		public String CheckLogin { get; set; }
		public String Connecting { get; set; }
		public String ConnectionUnavailable { get; set; }
		public String Inbox { get; set; }
		public String NoSubject { get; set; }
		public String NoAccount { get; set; }
		public String NoMail { get; set; }
	}

	public class LocaleToast {

		public String ViewEmail { get; set; }
		public String NoSubject { get; set; }
	}

}