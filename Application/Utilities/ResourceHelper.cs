namespace GmailNotifierPlus.Utilities {
	
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Reflection;

	public static class ResourceHelper {

		private const String _ResourcePrefix = "GmailNotifierPlus.Resources.";

		/// <summary>
		/// Returns the contents of a file which has been embedded as a resource.
		/// This functional has only been setup to handle the following data types: String
		/// </summary>
		/// <typeparam name="T">Datatype of the data you want returned.</typeparam>
		/// <param name="assembly"></param>
		/// <param name="fileName">Filename of the embedded resource.</param>
		/// <returns></returns>
		public static T Get<T>(Assembly assembly, String fileName){
		
			Type type = typeof(T);

			if (type == typeof(Stream)) {
				return (T)Convert.ChangeType(assembly.GetManifestResourceStream(String.Concat(_ResourcePrefix, fileName)), type);
			}

			using (Stream dataStream = assembly.GetManifestResourceStream(String.Concat(_ResourcePrefix, fileName))){
				
				if(dataStream == null){
					return default(T);
				}

				if(type == typeof(String)){
					using (StreamReader sr = new StreamReader(dataStream)){
						return (T)Convert.ChangeType(sr.ReadToEnd(), type);
					}
				}
				else if (type == typeof(Icon) || type == typeof(Image) || type == typeof(Bitmap)) {
					try {
						ConstructorInfo constructor = typeof(T).GetConstructor(new System.Type[] { typeof(object) });
						T result = (T)constructor.Invoke(new object[] { dataStream });

						return result;

					}
					catch { }
				}
			}

			return default(T);
		
		}

		/// <summary>
		/// Returns the contents of a file which has been embedded as a resource.
		/// This functional has only been setup to handle the following data types: String
		/// </summary>
		/// <typeparam name="T">Datatype of the data you want returned.</typeparam>
		/// <param name="fileName">Filename of the embedded resource.</param>
		/// <returns></returns>
		public static T Get<T>(String fileName){
			
			Assembly assembly = Assembly.GetCallingAssembly();
			
			return Get<T>(assembly, fileName);
		}

		public static Icon GetIcon(string iconName) {
			iconName = String.Concat("Icons.", iconName);

			return Get<Icon>(iconName);
		}

		public static Bitmap GetImage(string imageName) {
			imageName = String.Concat("Images.", imageName);

			return Get<Bitmap>(imageName);
		}

		public static Stream GetStream(string resourceName) {
			return Get<Stream>(resourceName);
		}

		public static Locale GetLocale(String language) {
			
			String fileName = String.Concat("Locales.", language, ".xml");
			String xml = Get<String>(fileName);
			Locale locale = Utilities.Serializer.Deserialize<Locale>(xml);

			return locale;
		}

		private static List<String> _AvailableLocales = null;

		public static List<String> AvailableLocales {
			get {

				if (_AvailableLocales == null) {
					_AvailableLocales = new List<String>();

					Assembly a = Assembly.GetExecutingAssembly();
					String[] resNames = a.GetManifestResourceNames();
					String prefix = String.Concat(_ResourcePrefix, "Locales.");

					foreach (String name in resNames) {
						if(!name.StartsWith(prefix)){
							continue;
						}

						String locale = name.Replace(".xml", String.Empty);
						_AvailableLocales.Add(locale.Replace(prefix, String.Empty));
					}

				}

				return _AvailableLocales;

			}
		}

	}
}

