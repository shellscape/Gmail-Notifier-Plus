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
			private static readonly Icon _Previous = ResourceHelper.GetIcon( "Previous.ico" );
			private static Icon _Inbox = ResourceHelper.GetIcon( "Inbox.ico" );
			private static readonly Icon _Next = ResourceHelper.GetIcon( "Next.ico" );
			private static readonly Icon _Window = ResourceHelper.GetIcon( "gmail-classic.ico" );
			private static readonly Icon _WindowSmall = ResourceHelper.GetIcon( "gmail-classic.ico", 16 );
			private static readonly Icon _Open = ResourceHelper.GetIcon( "Open.ico" );
			private static readonly Icon _Warning = ResourceHelper.GetIcon( "Warning.ico" );
			private static readonly Icon _Offline = ResourceHelper.GetIcon( "Offline.ico" );

			// i'm running into a shitload of Disposed issues with these and windows.
			public static Icon Previous { get { return _Previous.Clone() as Icon; } }

			public static Icon Inbox {
				get {
					try {
						return _Inbox.Clone() as Icon;
					}
					catch( System.ComponentModel.Win32Exception ) {
						// happens from time to time, can't find the cause.
						// Exception: System.ComponentModel.Win32Exception (0x80004005): The operation completed successfully
						_Inbox = ResourceHelper.GetIcon( "Inbox.ico" ); // minor leak - this exception rarely happens.
						return _Inbox.Clone() as Icon;
					}
				}
			}

			public static Icon Next { get { return _Next.Clone() as Icon; } }
			public static Icon Window { get { return _Window.Clone() as Icon; } }
			public static Icon WindowSmall { get { return _WindowSmall.Clone() as Icon; } }
			public static Icon Open { get { return _Open.Clone() as Icon; } }
			public static Icon Warning { get { return _Warning.Clone() as Icon; } }
			public static Icon Offline { get { return _Offline.Clone() as Icon; } }

		}

		public static class Bitmaps {
			private static readonly Bitmap _Checking = ResourceHelper.GetImage( "Checking.png" );
			private static readonly Bitmap _Offline = ResourceHelper.GetImage( "Offline.png" );
			private static readonly Bitmap _Warning = ResourceHelper.GetImage( "Warning.png" );

			// running into more disposed issues. the references are being disposed when a window closes (the notifier)
			public static Bitmap Checking { get { return _Checking.Clone() as Bitmap; } }
			public static Bitmap Offline { get { return _Offline.Clone() as Bitmap; } }
			public static Bitmap Warning { get { return _Warning.Clone() as Bitmap; } }
		}

	}
}
