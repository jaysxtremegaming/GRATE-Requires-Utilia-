using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Grate.Extensions;

public static class StringExtensions
{
	public static string Key = "THEULTIMATESUPERSECRETE";

	public static string EncryptString(this string plainText)
	{
		byte[] iV = new byte[16];
		byte[] inArray;
		using (Aes aes = Aes.Create())
		{
			aes.Key = Encoding.UTF8.GetBytes(Key);
			aes.IV = iV;
			ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
			using MemoryStream memoryStream = new MemoryStream();
			using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			using (StreamWriter streamWriter = new StreamWriter(stream))
			{
				streamWriter.Write(plainText);
			}
			inArray = memoryStream.ToArray();
		}
		return Convert.ToBase64String(inArray);
	}

	public static string DecryptString(this string cipherText)
	{
		byte[] buffer = Convert.FromBase64String(cipherText);
		using Aes aes = Aes.Create();
		aes.Key = Encoding.UTF8.GetBytes(Key);
		byte[] iV = new byte[Key.Length];
		aes.IV = iV;
		ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
		using MemoryStream stream = new MemoryStream(buffer);
		using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		using StreamReader streamReader = new StreamReader(stream2);
		return streamReader.ReadToEnd();
	}
}
