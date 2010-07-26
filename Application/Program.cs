namespace GmailNotifierPlus {
	
	using System;
	using System.Runtime.Remoting;
	using System.Runtime.Remoting.Channels;
	using System.Runtime.Remoting.Channels.Ipc;
	using System.Security.Principal;
	using System.Threading;
	using System.Windows.Forms;

	internal static class Program {

		internal static String channelName;
		internal static GmailNotifierPlus.Forms.Main mainForm;

		private static void CallRunningInstance(string[] args) {
			RemotingService service = (RemotingService)RemotingServices.Connect(typeof(RemotingService), "ipc://" + channelName + "/service.rem");
			String str = args[0];
			if (str != null) {
				if (!(str == "-check")) {
					if (!(str == "-settings")) {
						return;
					}
				}
				else {
					service.CheckMail();
					return;
				}
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

					Config.Init();
					
					mainForm = new GmailNotifierPlus.Forms.Main(args);

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
		}
	}
}

