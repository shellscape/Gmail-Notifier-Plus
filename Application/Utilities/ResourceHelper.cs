namespace GmailNotifierPlus.Utilities {
	
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Reflection;

	public static class ResourceHelper {

		public static Icon GetResourceIcon(string resourceName) {
			if (string.IsNullOrEmpty(resourceName)) {
				return null;
			}
			return new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));
		}

		public static Bitmap GetResourceImage(string resourceName) {
			if (string.IsNullOrEmpty(resourceName)) {
				return null;
			}
			return new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));
		}

		public static Stream GetResourceStream(string resourceName) {
			if (string.IsNullOrEmpty(resourceName)) {
				return null;
			}
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		}

		private static List<String> _AvailableLocales = null;

		public static List<String> AvailableLocales {
			get {

				if (_AvailableLocales == null) {
					_AvailableLocales = new List<String>();

					Assembly a = Assembly.GetExecutingAssembly();
					String[] resNames = a.GetManifestResourceNames();

					foreach (String name in resNames) {
						if(!name.StartsWith("Locales")){
							continue;
						}
						_AvailableLocales.Add(name); // TODO - Debug
					}

				}

				return _AvailableLocales;

			}
		}

	}
}

