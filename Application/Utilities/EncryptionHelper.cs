namespace GmailNotifierPlus.Utilities {

	using System;
	using System.Text;

	public static class EncryptionHelper {

		public static string Decrypt(string encryptedValue) {
			if (string.IsNullOrEmpty(encryptedValue)) {
				return string.Empty;
			}
	
			DataProtector protector = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);
			byte[] cipherText = Convert.FromBase64String(encryptedValue);
			return Encoding.ASCII.GetString(protector.Decrypt(cipherText, null));
		}

		public static string Encrypt(string valueToEncrypt) {
			if (string.IsNullOrEmpty(valueToEncrypt)) {
				return string.Empty;
			}
			
			DataProtector protector = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);
			byte[] bytes = Encoding.ASCII.GetBytes(valueToEncrypt);
			return Convert.ToBase64String(protector.Encrypt(bytes, null));
		}
	}
}

