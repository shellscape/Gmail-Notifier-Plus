using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;
using System.Windows.Shell;

//using Microsoft.WindowsAPI.Shell;
//using Microsoft.WindowsAPI.Taskbar;

namespace GmailNotifierPlus {
	public static class Extensions {

		/// <summary>
		/// Rather than hacking up the Windows API Code Pack code, we're going to do this with some extensions and reflection.
		/// It's a low impact call, so it shouldn't effect performance all that much, and it keeps the codepack clean
		/// so I'm not scratching my head every time they release an update to it.
		/// </summary>
		/// <param name="list"></param>
		public static void RemoveCustomCategories(this JumpList list) {
			list.JumpItems.RemoveAll(item => !String.IsNullOrEmpty(item.CustomCategory) && item.CustomCategory != "Default Account");
		}

		public static void ClearTasks(this JumpList list) {

			Type type = typeof(Shellscape.UI.JumplistTask);

			foreach(Shellscape.UI.JumplistTask task in list.JumpItems.Where(t => t.GetType() == type)) {
				Shellscape.Remoting.RemotingTaskMethods.Methods.Remove(task.Arguments);
			}

			list.JumpItems.Clear();
		}

		public static String Limit(this string str, int limit) {

			if(str.Length <= limit) {
				return str;
			}

			return str.Substring(0, limit).TrimEnd();
		}

		public static string LimitElipses(this string str, int limit) {

			if(limit < 5) {
				return str.Limit(limit);       // Can’t do much with such a short limit
			}

			if(str.Length <= (limit - 3)) {
				return str;
			}

			return str.Substring(0, limit - 3) + "…";
		}

	}
}
