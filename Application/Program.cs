using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

using Microsoft.CSharp;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

using GmailNotifierPlus.Utilities;

namespace GmailNotifierPlus {

	internal static class Program {

		internal static class Arguments {
			public const String Check = "-check";
			public const String Settings = "-settings";
			public const String About = "-about";
		}

		public static System.Drawing.Icon Icon { get; private set; }

		internal static String channelName;
		internal static GmailNotifierPlus.Forms.Main mainForm;

		private static List<String> _validArgs = new List<String>() { Arguments.About, Arguments.Check, Arguments.Settings };

		private static void CallRunningInstance(string[] args) {
			RemotingService service = (RemotingService)RemotingServices.Connect(typeof(RemotingService), "ipc://" + channelName + "/service.rem");
			String firstArg = args[0];

			if (firstArg == null || !_validArgs.Contains(firstArg)) {
				return;
			}

			if (firstArg == Arguments.About) {
				service.ShowAbout();
			}
			else if (firstArg == Arguments.Check) {
				service.CheckMail();
			}
			else if (firstArg == Arguments.Settings) {
				service.OpenSettingsWindow();
			}
		}

		private static void InitRemoting() {
			ChannelServices.RegisterChannel(new IpcChannel(channelName), false);
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingService), "service.rem", WellKnownObjectMode.Singleton);
		}

		[STAThread]
		private static void Main(string[] args) {
			bool createdNew;

			channelName = String.Concat(WindowsIdentity.GetCurrent().Name, "@GmailNotifierPlus");

			Program.Icon = Utilities.ResourceHelper.GetIcon("gmail-classic.ico");

			String guid = "{421a0043-b2ab-4b86-8dec-63ce3b8bd764}";
			String name = String.Concat(@"Local\GmailNotifierPlus", guid);

			using (new Mutex(true, name, out createdNew)) {
				if (!createdNew) {
					if (args.Length > 0) {
						CallRunningInstance(args);
					}
				}
				else {
					InitRemoting();

					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					Application.ThreadException += Application_ThreadException;

					try {
						Config.Init();

						mainForm = new GmailNotifierPlus.Forms.Main(args);
					}
					catch (Exception e) {
						Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(e));
					}

					Application.Run(mainForm);
				}
			}
		}

		public class RemotingService : MarshalByRefObject {
			public void CheckMail() {
				Program.mainForm.RemoteCheckMails();
			}

			public void OpenSettingsWindow() {
				Program.mainForm.RemoteOpenSettingsWindow();
			}

			public void ShowAbout() {
				Program.mainForm.RemoteShowAbout();
			}
		}

		public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
			ErrorHelper.Report(e.Exception);
		}
	}
}

