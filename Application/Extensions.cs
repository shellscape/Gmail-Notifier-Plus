using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace GmailNotifierPlus {
	public static class Extensions {

		/// <summary>
		/// Rather than hacking up the Windows API Code Pack code, we're going to do this with some extensions and reflection.
		/// It's a low impact call, so it shouldn't effect performance all that much, and it keeps the codepack clean
		/// so I'm not scratching my head every time they release an update to it.
		/// </summary>
		/// <param name="list"></param>
		public static void RemoveCustomCategories(this JumpList list) {
			
			FieldInfo fi = typeof(JumpList).GetField("customCategoriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
			
			object categories = fi.GetValue(list);

			if (categories != null) {
				MethodInfo method = categories.GetType().GetMethod("Clear");
				method.Invoke(categories, null);
			}
		}

		// This is the code the previous author had added to the Windows API Code Pack. Hackeyhackhack.
		//public void ClearAllCustomCategories() {
		//  if (this.customCategoriesCollection != null) {
		//    this.customCategoriesCollection.Clear();
		//  }
		//}

	}
}
