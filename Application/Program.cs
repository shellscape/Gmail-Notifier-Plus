﻿using System;
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
			public const String Help = "-help";
		}

		public static System.Drawing.Icon Icon { get; private set; }

		internal static String channelName;
		internal static GmailNotifierPlus.Forms.Main mainForm = null;

		private static List<String> _validArgs = new List<String>() { Arguments.About, Arguments.Check, Arguments.Settings, Arguments.Help };

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
			else if (firstArg == Arguments.Help) {
				Utilities.UrlHelper.Launch(null, "https://github.com/shellscape/Gmail-Notifier-Plus/wiki");
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

            SystemEvents.SessionEnded += new SessionEndedEventHandler(SystemEvents_SessionEnded);

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

					AppDomain appDomain = AppDomain.CurrentDomain;
					appDomain.UnhandledException += AppDomain_UnhandledException;

					Form startForm = null;

					try {
						Config.Init();

						startForm = mainForm = new GmailNotifierPlus.Forms.Main(args);
						//startForm = new GmailNotifierPlus.Forms.ToastTest();
					}
					catch (Exception e) {
						Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(e));
					}

					Application.Run(startForm);
				}
			}
		}

        static void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e)
        {
            SystemEvents.SessionEnded -= SystemEvents_SessionEnded;
            Application.Exit();
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

		public static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
			ErrorHelper.Report(e.ExceptionObject as Exception);
		}

		public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
			ErrorHelper.Report(e.Exception);
		}
	}
}

