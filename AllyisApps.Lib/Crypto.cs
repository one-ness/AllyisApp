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
		// password storage is implemented as outlined in https://nakedsecurity.sophos.com/2013/11/20/serious-security-how-to-store-your-users-passwords-safely/,
		// with the code used here paraphrased from https://cmatskas.com/-net-password-hashing-using-pbkdf2/.

		// password hashing: feel free to update these as needed. The next time a user logs on, their password will still work,
		// and the hash will get updated in the database to reflect the new values below.
		private const int SaltBytes = 24;

		private const int HashBytes = 32;
		private const int Iterations = 20000;

		// encryption
		private const int IVStringLength = 16;

		private static Encoding encoding = Encoding.UTF8;

		/// <summary>
		/// Uses the PBKDF2 algorithm to hash a password, providing a salt and iteration count according
		/// to the current values of the constants in Crypto.cs.
		/// </summary>
		/// <param name="password">Password to hash.</param>
		/// <returns>String of the format "iterationCount:salt:hash".</returns>
		public static string GetPasswordHash(string password)
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			byte[] salt = new byte[SaltBytes];
			provider.GetBytes(salt);

			byte[] hash = GetPbkdf2Bytes(password, salt, Iterations, HashBytes);
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0}:{1}:{2}", Iterations, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
			return sb.ToString();
		}

		/// <summary>
		/// Runs the PBKDF2 algorithm using the given password, salt, iteration count, and output byte count.
		/// </summary>
		/// <param name="password">Password string.</param>
		/// <param name="salt">Salt bytes.</param>
		/// <param name="iterationCount">Number of iterations to run.</param>
		/// <param name="byteCount">Number of bytes for output byte array.</param>
		/// <returns>Byte array for hashed password.</returns>
		private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterationCount, int byteCount)
		{
			Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterationCount);
			return pbkdf2.GetBytes(byteCount);
		}

		/// <summary>
		/// Validates a password against the stored password hash, and provides an updated password hash
		/// if the stored password hashing is out of date.
		/// </summary>
		/// <param name="password">Entered password.</param>
		/// <param name="correctHash">Correct hash of the password.</param>
		/// <returns>Item1 is bool, indicates if the hash of the given password matches the given hash.
		/// If Item1 indicates a match, then Item2 may contain the updated hash of the password, which the caller can update in database.</returns>
		public static PassWordValidationResult ValidateAndUpdate(string password, string correctHash)
		{
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
			if (string.IsNullOrWhiteSpace(correctHash)) throw new ArgumentNullException(nameof(correctHash));

			bool updateRequired = false;
			bool result = false;
			string newHash = null;
			string[] components = correctHash.Split(':');
			int hashIterations = int.Parse(components[0]);
			byte[] hashSalt = Convert.FromBase64String(components[1]);
			byte[] hashHash = Convert.FromBase64String(components[2]);
			if (hashIterations != Iterations || hashSalt.Length != SaltBytes || hashHash.Length != HashBytes)
			{
				// parameters of the hashing are out of date
				updateRequired = true;
			}

			byte[] testHash = GetPbkdf2Bytes(password, hashSalt, hashIterations, hashHash.Length);
			if (ByteArrayEquals(hashHash, testHash))
			{
				// good login
				result = true;
				if (updateRequired)
				{
					// if an update is needed, the updated hash is returned
					newHash = GetPasswordHash(password);
				}
			}

			return new PassWordValidationResult
			{
				successfulMatch = result,
				updatedHash = newHash
			};
		}

		private static bool ByteArrayEquals(byte[] a, byte[] b)
		{
			if (a == b) return true;
			if (a == null || b == null || a.Length != b.Length) return false;
			bool result = true;
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					result = false;
					break;
				}
			}

			return result;
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

	public class PassWordValidationResult
	{
		/// <summary>
		/// Wheather password was successfully given for login
		/// </summary>
		public bool successfulMatch { get; internal set; }

		/// <summary>
		/// Updated password hash if needed otherwise null.
		/// </summary>
		public string updatedHash { get; internal set; }
	}
}