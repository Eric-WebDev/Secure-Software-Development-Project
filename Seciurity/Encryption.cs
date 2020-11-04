using BloggerApplication.Models;
using BloggerApplication.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace BloggerApplication.Seciurity
{
    internal static class Encryption
    {
        internal static void Encrypt(List<BlogPost> inputList, byte[] key, byte[] iv)
        {
            try
            {
                using (AesManaged aesAlg = new AesManaged())
                {
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.KeySize = 128;
                    // 16 bytes for 128 bit encryption
                    aesAlg.Key = new byte[128 / 8];
                    // AES needs a 16-byte IV
                    aesAlg.IV = new byte[128 / 8];
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (FileStream fs = File.Open(RegisterLogin.location, FileMode.OpenOrCreate))
                    {
                        using (CryptoStream cStream = new CryptoStream(fs, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sWriter = new StreamWriter(cStream))
                            {
                                foreach (var line in inputList)
                                {
                                    sWriter.Write($"{line.BlogPostId},{line.BlogPostCategory},{line.BlogPostTitle},{line.BlogPostContent}|");
                                }
                            }
                        }
                    }
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Encryption error: {0}", e.Message);
            }
        }
        // Data decrypted from the text file 
        // Secret "key" used for the symmetric algorithm
        // Initialization Vector "iv"  used for the symmetric algorithm
        // Return String containing decrypted contents of text file
        internal static string Decrypt(byte[] key, byte[] iv)
        {
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("IV");

            string textString = string.Empty;
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.KeySize = 128;
                    aesAlg.Key = new byte[128 / 8];
                    aesAlg.IV = new byte[128 / 8];
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (FileStream reader = new FileStream(RegisterLogin.location, FileMode.Open))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(reader, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                textString = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                return textString;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string HashPassword(string pass)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[16];
            provider.GetBytes(salt);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(pass, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPass = Convert.ToBase64String(hashBytes);
            return savedPass;
        }
        public static bool VerifyHashedPass(string newPw, string oldPw)
        {
            byte[] hashBytes = Convert.FromBase64String(oldPw);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(newPw, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
