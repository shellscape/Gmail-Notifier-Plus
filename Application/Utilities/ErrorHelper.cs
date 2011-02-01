using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

namespace GmailNotifierPlus.Utilities {
	public static class ErrorHelper {

		private static String _LogPath = null;

		static ErrorHelper() {
			String appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			_LogPath = Path.Combine(appData, GmailNotifierPlus.Resources.Resources.WindowTitle, "Error Logs");

			if (!Directory.Exists(_LogPath)) {
				Directory.CreateDirectory(_LogPath);
			}

		}

		/// <summary>
		/// Shows the user a dialog explaining the error, and creates a log file for the error.
		/// </summary>
		/// <param name="e"></param>
		public static void Report(Exception e) {
			Guid guid = Guid.NewGuid();
			Show(e, guid);
			Log(e, guid);
		}

		public static void Show(Exception e, Guid guid) {
			TaskDialog dialog = new TaskDialog() {
				HyperlinksEnabled = true,
				DetailsExpanded = false,
				Cancelable = false,
				Icon = TaskDialogStandardIcon.Error,
				DetailsExpandedText = e.StackTrace,
				Caption = "Gmail Notifier Plus Error",
				InstructionText = "An unhandled exception occurred:",
				Text = e.Message,
				FooterIcon = TaskDialogStandardIcon.Information,
				FooterText = String.Concat("This error's identifier is:\n\t", guid.ToString(), 
					"\n\nPlease <a href=\"#stub\">Click Here</a> to automagically report the issue.")
			};

			dialog.HyperlinkClick += delegate(object sender, TaskDialogHyperlinkClickedEventArgs evt) {
				Send(guid);
			};

			dialog.Show();
		}

		public static void Log(Exception e, Guid guid) {

			var info = new Microsoft.VisualBasic.Devices.ComputerInfo();

			String path = Path.Combine(_LogPath, guid.ToString());
			
			String os = String.Join(" ", "CPU:", info.OSFullName, info.OSVersion, info.OSPlatform);
			String memory = String.Concat("Memory: ", info.TotalPhysicalMemory);
			String date = String.Concat("Date: ", DateTime.Now.ToString());
			String processor = "Processor: Unknown";

			try {
				RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0", false);
				processor = String.Join(" ", "Processor:", 
					key.GetValue("VendorIdentifier"),
					key.GetValue("Identifier"),
					key.GetValue("ProcessorNameString"),
					key.GetValue("~Mhz"), "~Mhz"
				);
			}
			catch (System.Security.SecurityException ex) {
			}

			String system = String.Join("\n", date, os, processor, memory);
			String message = String.Concat("Message: ", e.Message);
			String stack = String.Concat("Stack Trace:\n", e.StackTrace);

			String data = String.Join("\n\n", system, message, stack);
			
			using(FileStream fs = new FileStream(_LogPath, FileMode.Create, FileAccess.Write)){
				using(StreamWriter sw = new StreamWriter(fs)){
					sw.Write(data);
				}
			}

		}
		
		public static void Send(Guid guid){
			String path = Path.Combine(_LogPath, guid.ToString());
			FileInfo file = new FileInfo(path);
			String data = null;

			using (StreamReader sr = file.OpenText()) {
				data = sr.ReadToEnd();
			}

			// TODO: send this to a hop server, which will then send to github.
		}

	}
}
