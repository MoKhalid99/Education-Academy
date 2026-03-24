using System.Security.Cryptography;

public class FileSecurityService
{
	private readonly byte[] _key = Guid.Parse("YOUR-FIXED-GUID-KEY").ToByteArray(); // مفتاح ثابت

	public async Task<byte[]> DecryptFileAsync(string encryptedPath)
	{
		byte[] encryptedData = await File.ReadAllBytesAsync(encryptedPath);
		using Aes aes = Aes.Create();
		aes.Key = _key;
		aes.IV = new byte[16]; // في الإنتاج استخدم IV ديناميكي مخزن مع الملف

		using MemoryStream ms = new();
		using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
		cs.Write(encryptedData, 0, encryptedData.Length);
		cs.FlushFinalBlock();
		return ms.ToArray();
	}
}