using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GmailNotifierPlus {

	public delegate void CheckMailFinishedEventHandler(Forms.Notifier sender, EventArgs e);
	public delegate void ConfigSavedEventHandler(object sender, EventArgs e);
	public delegate void LanguageChangedEventHandler(Config sender);

}
