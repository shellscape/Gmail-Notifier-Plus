namespace GmailNotifierPlus.Utilities {
	
	using System;
	using System.IO;
	using System.Media;
	using System.Runtime.InteropServices;

	public enum SoundOptions {
		None,
		Default,
		Custom
	}

	public static class SoundHelper {

		public static void PlayCustomSound(string path) {
			if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
				try {
					PlaySound(path, IntPtr.Zero, Flags.SND_FILENAME | Flags.SND_ASYNC);
				}
				catch {
				}
			}
		}

		public static void PlayDefaultSound() {
			UnmanagedMemoryStream stream = (UnmanagedMemoryStream)Utilities.ResourceHelper.GetRaw("WindowsNotify.wav");
			new System.Media.SoundPlayer(stream).Play();
		}

		[DllImport("winmm.dll", SetLastError = true)]
		private static extern bool PlaySound(string pszSound, IntPtr hMod, Flags sf);

		[Flags]
		private enum Flags {
			SND_ALIAS = 0x10000,
			SND_ALIAS_ID = 0x110000,
			SND_ASYNC = 1,
			SND_FILENAME = 0x20000,
			SND_LOOP = 8,
			SND_MEMORY = 4,
			SND_NODEFAULT = 2,
			SND_NOSTOP = 0x10,
			SND_NOWAIT = 0x2000,
			SND_RESOURCE = 0x40004,
			SND_SYNC = 0
		}
	}
}

