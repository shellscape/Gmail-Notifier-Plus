using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Shellscape.Utilities;

namespace GmailNotifierPlus.Resources {
	public static class Strings {

		public static readonly String WindowTitle = "Gmail Notifier Plus";

	}
	//public static class Icons {

	//  public static Icon Previous { get { return ResourceHelper.GetIcon("Previous.ico"); } }
	//  public static Icon Inbox { get { return ResourceHelper.GetIcon("Inbox.ico"); } }

	//  public static Icon Next { get { return ResourceHelper.GetIcon("Next.ico"); } }
	//  public static Icon Window { get { return ResourceHelper.GetIcon("gmail-classic.ico"); } }
	//  public static Icon WindowSmall { get { return ResourceHelper.GetIcon("gmail-classic.ico", 16); } }
	//  public static Icon Open { get { return ResourceHelper.GetIcon("Open.ico"); } }
	//  public static Icon Warning { get { return ResourceHelper.GetIcon("Warning.ico"); } }
	//  public static Icon Offline { get { return ResourceHelper.GetIcon("Offline.ico"); } }

	//}

	public static class Bitmaps {

		public static Bitmap Checking { get { return ResourceHelper.GetImage("Checking.png"); } }
		public static Bitmap Offline { get { return ResourceHelper.GetImage("Offline.png"); } }
		public static Bitmap Warning { get { return ResourceHelper.GetImage("Warning.png"); } }
	}

}
