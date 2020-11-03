using BloggerApplication.Models;
using BloggerApplication.Seciurity;
using BloggerApplication.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BloggerApplication.DB
{
   internal static class ConnectionData
    {
        /// <summary>
        /// Get the product from the text file
        /// </summary>
        /// <returns>The full list of items from the text file "Data.txt"</returns>
        internal static List<BlogPost> StoreData()
        {
            try
            {
                CreateFile();
                string textString = string.Empty;
                List<BlogPost> inputList = new List<BlogPost>();

                using (Aes myAes = Aes.Create())
                {
                    myAes.Padding = PaddingMode.PKCS7;
                    myAes.KeySize = 128;           // in bits
                    myAes.Key = new byte[128 / 8]; // 16 bytes for 128 bit encryption
                    myAes.IV = new byte[128 / 8];  // AES needs a 16-byte IV
                    textString = Encryption.Decrypt(myAes.Key, myAes.IV);
                }
                // Split the text that is being read in
                string[] dataRead = textString.Split('|');
                foreach (var item in dataRead)
                {
                    // Check if it has reached the end of the line
                    if (item == string.Empty)
                    {
                        break;
                    }
                    string[] itemInfo = item.Split(",");
                    inputList.Add(new BlogPost(itemInfo[0], itemInfo[1], itemInfo[2], itemInfo[3]));
                }
                return inputList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create the text file if it doesn't already exist
        /// </summary>
        private static void CreateFile()
        {
            if (!File.Exists(RegisterLogin.location))
            {
                using (var stream = File.Create(RegisterLogin.location)) { }
            }
        }


        /// <summary>
        /// Save changes made to list of products to text file
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        internal static void SaveChanges(List<BlogPost> inputList)
        {
            try
            {
                Console.WriteLine("----------------------------------------------------");
                using (AesManaged myAes = new AesManaged())
                {
                    myAes.Padding = PaddingMode.PKCS7;
                    myAes.KeySize = 128;           // in bits
                    myAes.Key = new byte[128 / 8]; // 16 bytes for 128 bit encryption
                    myAes.IV = new byte[128 / 8];  // AES needs a 16-byte IV
                    Encryption.Encrypt(inputList, myAes.Key, myAes.IV);
                }
                Console.WriteLine("Changes Saved. Press any key to return to the main menu.");
            }
            catch (IOException ioe)
            {
                Console.WriteLine($"Error: {ioe}");
            }
        }
    }
}
