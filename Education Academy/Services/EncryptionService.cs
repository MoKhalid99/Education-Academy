using System.Security.Cryptography;
namespace EducationAcademy.Services
{
	//ملف تشفير الملفات بمفتاح مميز
    public class EncryptionService
 {
		private readonly byte[] _key = "KhalidASamir9943981234567890"u8.ToArray(); // مفتاح 

		public async Task<byte[]> EncryptFile(Stream inputStream)
		{
			using var aes = Aes.Create();
			aes.Key = _key;
			aes.GenerateIV();
			using var outputStream = new MemoryStream();
			await outputStream.WriteAsync(aes.IV, 0, aes.IV.Length);
			using (var cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
			{
				await inputStream.CopyToAsync(cryptoStream);
			}
			return outputStream.ToArray();
		}
	}
}
