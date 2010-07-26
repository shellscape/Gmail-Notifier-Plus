using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GmailNotifierPlus {

	[XmlRoot("Config")]
	public class Config {

		private static readonly String _AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		private static readonly String _Path = Path.Combine(_AppData, GmailNotifierPlus.Resources.Resources.WindowTitle);
		private static readonly String _FileName = "app.config";

		public static void Init() {
			if (!Directory.Exists(_Path)) {
				Directory.CreateDirectory(_Path);
			}

			Config config = new Config();
			String xml = null;
			FileInfo file = new FileInfo(Path.Combine(_Path, _FileName));

			if (file.Exists) {
				using (FileStream fs = new FileStream(file.FullName, FileMode.Create, FileAccess.ReadWrite)) {
					using (StreamReader sr = new StreamReader(fs)) {
						xml = sr.ReadToEnd();
					}
				}

				if (!String.IsNullOrEmpty(xml)) {
					config = Utilities.Serializer.Deserialize<Config>(xml);
				}
			}
			else {
				config.Save();
			}
			
			Config.Current = config;

			// Fire up the locale information and init localization
			List<String> locales = Utilities.ResourceHelper.AvailableLocales;

			Locale locale = Utilities.ResourceHelper.GetLocale(config.Language);
			locale.Init();

		}

		public static Config Current {
			get;
			private set;
		}

		public Config() {
			this.Interval = 60;
			this.Language = "en-US";
			this.Sound = "default";
			this.PlaySound = true;
			this.FirstRun = true;
		}

		public event ConfigSavedEventHandler Saved;

		public int Interval { get; set; }
		public String Language { get; set; }
		public String Sound { get; set; }
		public Boolean PlaySound { get; set; }
		public List<Account> Accounts { get; set; }
		public Boolean FirstRun { get; set; }

		[XmlIgnore]
		public Account DefaultAccount {
			get {
				Account account = (from a in Accounts where a.Default == true select a).First();
				return account;
			}
		}

		public void Save() {

			String serialized = Utilities.Serializer.Serialize<Config>(this);

			using (FileStream fs = new FileStream(Path.Combine(_Path, _FileName), FileMode.Create, FileAccess.ReadWrite)) {
				using (StreamWriter sw = new StreamWriter(fs)) {
					sw.Write(serialized);
				}
			}

			if (this.Saved != null) {
				this.Saved(this, EventArgs.Empty);
			}

		}
	}

}
