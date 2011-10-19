using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Shellscape.Utilities;

namespace GmailNotifierPlus {
	public static class Resources {

		public static readonly String WindowTitle = "Gmail Notifier Plus";

		public static class Icons {
			public static readonly Icon Previous = ResourceHelper.GetIcon("Previous.ico");
			public static readonly Icon Inbox = ResourceHelper.GetIcon("Inbox.ico");
			public static readonly Icon Next = ResourceHelper.GetIcon("Next.ico");
			public static readonly Icon Window = ResourceHelper.GetIcon("gmail-classic.ico");
			public static readonly Icon WindowSmall = ResourceHelper.GetIcon("gmail-classic.ico", 16);
			public static readonly Icon Open = ResourceHelper.GetIcon("Open.ico");
			public static readonly Icon Warning = ResourceHelper.GetIcon("Warning.ico");
			public static readonly Icon Offline = ResourceHelper.GetIcon("Offline.ico");
		}

		public static class Bitmaps {
			public static readonly Bitmap Checking = ResourceHelper.GetImage("Checking.png");
			public static readonly Bitmap Offline = ResourceHelper.GetImage("Offline.png");
			public static readonly Bitmap Warning = ResourceHelper.GetImage("Warning.png");
		}

	}
}
