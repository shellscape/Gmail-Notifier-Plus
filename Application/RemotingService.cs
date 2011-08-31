using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GmailNotifierPlus {

	public class Arguments : Shellscape.ArgumentCollection {
		public const String Check = "check";
		public const String Settings = "settings";
		public const String About = "about";
		public const String Help = "help";
	}

	public class RemotingService : Shellscape.RemotingService<Arguments> {
		
		[Shellscape.RemoteServiceMethod(Arguments.Check)]
		public void CheckMail() {
			Program.MainForm.RemoteCheckMails();
		}

		[Shellscape.RemoteServiceMethod(Arguments.Settings)]
		public void OpenSettingsWindow() {
			Program.MainForm.RemoteOpenSettingsWindow();
		}

		[Shellscape.RemoteServiceMethod(Arguments.About)]
		public void ShowAbout() {
			Program.MainForm.RemoteShowAbout();
		}

		[Shellscape.RemoteServiceMethod(Arguments.Help)]
		public void ShowHelp() {
			Utilities.UrlHelper.Launch(null, "https://github.com/shellscape/Gmail-Notifier-Plus/wiki");
		}
	}

}
