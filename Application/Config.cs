using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GmailNotifierPlus {

	[DataContract(Name = "config")]
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
				using (StreamReader sr = file.OpenText()) {
					xml = sr.ReadToEnd();
				}

				if (!String.IsNullOrEmpty(xml)) {
					config = Utilities.Serializer.DeserializeContract<Config>(xml);
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

			for (var i = 0; i < config.Accounts.Count; i++) {
				config.Accounts[i].Init();
			}

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

			this.Accounts = new AccountList();
		}

		public event ConfigSavedEventHandler Saved;

		[DataMember(Name = "Pinned")]
		public Boolean Pinned { get; set; }

		[DataMember(Name = "interval")]
		public int Interval { get; set; }

		[DataMember(Name = "language")]
		public String Language { get; set; }

		[DataMember(Name = "sound")]
		public String Sound { get; set; }

		[DataMember(Name = "soundnotification")]
		public int SoundNotification { get; set; }

		[DataMember(Name = "playsound")]
		public Boolean PlaySound { get; set; }

		[DataMember(Name = "accounts")]
		public AccountList Accounts { get; set; }

		[DataMember(Name = "firstrun")]
		public Boolean FirstRun { get; set; }

		public void Save() {

			String serialized = Utilities.Serializer.SerializeContract<Config>(this);

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
