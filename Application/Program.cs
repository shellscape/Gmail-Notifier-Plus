using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

using Microsoft.CSharp;
using Microsoft.Win32;
using Microsoft.WindowsAPI.Dialogs;
using Microsoft.WindowsAPI.Shell;

using GmailNotifierPlus.Utilities;

namespace GmailNotifierPlus {

	internal static class Program {

		//private static List<String> _validArgs = new List<String>() { RemoteMethods.About, RemoteMethods.Check, RemoteMethods.Settings, RemoteMethods.Help };
		private static readonly String _repoUser = "shellscape";
		private static readonly String _repoName = "Gmail-Notifier-Plus";

		public static GmailNotifierPlus.Forms.Main MainForm {
			get {
				return Shellscape.Program.Form as GmailNotifierPlus.Forms.Main;
			}
		}

		[STAThread]
		private static void Main(string[] args) {

			Shellscape.Program.MainInstanceStarted += delegate() {

				Shellscape.Remoting.RemotingTaskMethods.Methods.Add("mailto", delegate(String[] arguments) { Forms.Main.Remote_Mailto(arguments); });

				AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e) {
					ErrorHelper.Report(e.ExceptionObject as Exception);
				};

				Config.Init();

				Shellscape.UpdateManager updates = new Shellscape.UpdateManager(_repoUser, _repoName, _repoName, _repoName);

				updates.Error += delegate(object sender, UnhandledExceptionEventArgs e) {
					if(e.ExceptionObject is System.Net.WebException) {
						if(e.ExceptionObject != null) {
							ErrorHelper.Log(e.ExceptionObject as Exception, Guid.NewGuid());
						}
						return;
					}
		
					ErrorHelper.Report(e.ExceptionObject as Exception);
				};

				SystemEvents.SessionEnded += delegate(object sender, SessionEndedEventArgs e) {
					Shellscape.UpdateManager.Current.Stop();
					Application.Exit();
				};

			};
			
			Shellscape.Program.Run<GmailNotifierPlus.Forms.Main>(args);

		}

	}
}
