using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Reflection;

using GmailNotifierPlus.Localization;

namespace GmailNotifierPlus.Utilities {
	
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
						ConstructorInfo constructor = typeof(T).GetConstructor(new System.Type[] { typeof(Stream) });
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

		public static Stream GetResourceStream(String fileName) {
			Assembly assembly = Assembly.GetCallingAssembly();
			return assembly.GetManifestResourceStream(String.Concat(_ResourcePrefix, fileName));
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
			String xml = String.Empty;

			// we have to load the xml via System.Xml so that character encoding is enforced and we get the right unicode output.
			// otherwise, the Deserializer just craps out poop characters.
			using (Stream stream = GetResourceStream(fileName)) {
				if (stream == null) {
					return null;
				}				
				
				System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

				try {
					doc.Load(stream);
				}
				catch (Exception) {
					return null;
				}

				xml = doc.OuterXml;

				if (String.IsNullOrEmpty(xml)) {
					return null;
				}
			}

			Locale locale = Utilities.Serializer.Deserialize<Locale>(xml);

			return locale;
		}

		private static Dictionary<String, String> _AvailableLocales = null;

		public static Dictionary<String, String> AvailableLocales {
			get {

				if (_AvailableLocales == null) {
					_AvailableLocales = new Dictionary<String, String>();

					Assembly a = Assembly.GetExecutingAssembly();
					String[] resNames = a.GetManifestResourceNames();
					String prefix = String.Concat(_ResourcePrefix, "Locales.");

					resNames = resNames.OrderBy(s => s.Replace(prefix, String.Empty)).ToArray();

					foreach (String name in resNames) {
						if(!name.StartsWith(prefix)){
							continue;
						}

						String lang = name.Replace(".xml", String.Empty).Replace(prefix, String.Empty);
						Locale locale = GetLocale(lang);

						if (locale != null) {
							_AvailableLocales.Add(lang, locale.Name);
						}
					}
				}

				return _AvailableLocales;

			}
		}

	}
}

