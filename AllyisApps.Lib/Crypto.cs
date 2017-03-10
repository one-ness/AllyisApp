//------------------------------------------------------------------------------
// <copyright file="Crypto.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AllyisApps.Lib
{
	/// <summary>
	/// Crypto utilities.
	/// </summary>
	public static class Crypto
	{
		private const int KeyStringLength = 32;
		private const int IVStringLength = 16;
		private static Encoding encoding = Encoding.UTF8;

		/// <summary>
		/// Computes the SHA512 hash for the given data.
		/// </summary>
		/// <param name="data">The data for which the SHA512 hash is to be computed.</param>
		/// <returns>The SHA512 hash for the given data.</returns>
		public static string ComputeSHA512Hash(string data)
		{
			SHA512Managed sha = new SHA512Managed();
			byte[] hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
			return Convert.ToBase64String(hashed);
		}

		/// <summary>
		/// Encrypt the given data using AES algorithm.
		/// </summary>
		/// <param name="input">The data to encrypt.</param>
		/// <param name="base64Key">The encryption key.</param>
		/// <param name="base64Iv">The initialization vector.</param>
		/// <returns>The encrypted data.</returns>
		public static string Protect(string input, string base64Key, string base64Iv)
		{
			try
			{
				byte[] to = encoding.GetBytes(input);
				byte[] keyBytes = Convert.FromBase64String(base64Key);
				byte[] ivBytes = Convert.FromBase64String(base64Iv);

				byte[] encrypted = Encrypt(keyBytes, ivBytes, to);
				return Convert.ToBase64String(encrypted);
			}
			catch (EncoderFallbackException ex)
			{
				// unable to encode the strings to bytes.
				throw new AllyisAppsLibraryException("Encryption Failed.", ex);
			}
		}

		/// <summary>
		/// Decrypts the data previously encrypted by Protect.
		/// </summary>
		/// <param name="base64Input">The input that needs to be decrypted. This should be the output obtained from the "Protect" method.</param>
		/// <param name="base64Key">The 32 byte, base 64 encoded key, that was used in the Protect method.</param>
		/// <param name="base64Iv">The 16 byte, base64 encoded initialization vector, that was used in the Protect method.</param>
		/// <exception cref="AllyisAppsLibraryException">Thrown when decryption fails.</exception>
		/// <returns>The decrypted data.</returns>
		public static string Unprotect(string base64Input, string base64Key, string base64Iv)
		{
			try
			{
				byte[] from = Convert.FromBase64String(base64Input);
				byte[] keyBytes = Convert.FromBase64String(base64Key);
				byte[] ivBytes = Convert.FromBase64String(base64Iv);

				byte[] decrypted = Decrypt(keyBytes, ivBytes, from);
				return encoding.GetString(decrypted);
			}
			catch (Exception ex) when (ex is EncoderFallbackException || ex is DecoderFallbackException || ex is FormatException)
			{
				throw new AllyisAppsLibraryException("Decryption Failed.", ex);
			}
		}

		/// <summary>
		/// Encrypts "to" using key and initialization vector and returns that stream.
		/// </summary>
		/// <param name="key">The encryption key.</param>
		/// <param name="iv">The initialization vector.</param>
		/// <param name="to">The "to".</param>
		/// <returns>The encrypted byte stream.</returns>
		[SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "BrainSlugs83 said so on StackOverflow.")]
		private static byte[] Encrypt(byte[] key, byte[] iv, byte[] to)
		{
			try
			{
				// Get an encryptor.
				using (RijndaelManaged algorithm = new RijndaelManaged())
				{
					using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
					{
						// Encrypt the data.
						using (MemoryStream msEncrypt = new MemoryStream())
						{
							using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
							{
								// Write all data to the crypto stream and flush it.
								csEncrypt.Write(to, 0, to.Length);
								csEncrypt.FlushFinalBlock();

								return msEncrypt.ToArray();
							}
						}
					}
				}
			}
			catch (Exception ex) when (ex is CryptographicException || ex is NotSupportedException)
			{
				throw new AllyisAppsLibraryException("Encryption Failed.", ex);
			}
		}

		/// <summary>
		/// Decrypts from using key and initialization vector and returns that stream.
		/// </summary>
		/// <param name="key">The decryption key.</param>
		/// <param name="iv">The initialization vector.</param>
		/// <param name="from">The "from".</param>
		/// <returns>The decrypted data.</returns>
		[SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "BrainSlugs83 said so on StackOverflow.")]
		private static byte[] Decrypt(byte[] key, byte[] iv, byte[] from)
		{
			try
			{
				// Get a decryptor.
				using (RijndaelManaged algorithm = new RijndaelManaged())
				{
					using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
					{
						// Decrypt the data.
						using (MemoryStream msDecrypt = new MemoryStream(from))
						{
							using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
							{
								byte[] decrypted = new byte[from.Length];

								// Read all data from crypto stream.
								int readCount = csDecrypt.Read(decrypted, 0, decrypted.Length);

								// get bytes read and create array.
								byte[] retValue = new byte[readCount];
								Array.Copy(decrypted, retValue, readCount);
								return retValue;
							}
						}
					}
				}
			}
			catch (Exception ex) when (ex is CryptographicException || ex is NotSupportedException)
			{
				throw new AllyisAppsLibraryException("Decryption Failed.", ex);
			}
		}
	}
}
