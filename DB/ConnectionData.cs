using BloggerApplication.Models;
using BloggerApplication.Seciurity;
using BloggerApplication.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace BloggerApplication.DB
{
    internal static class ConnectionData
    {
        // Read data from the text file and return the full list of items from the specific user text file
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
                    myAes.KeySize = 128;
                    // using 16 bytes for 128 bit encryption
                    myAes.Key = new byte[128 / 8];
                    // AES needs a 16-byte IV
                    myAes.IV = new byte[128 / 8];
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
        // create file if does not exist 
        private static void CreateFile()
        {
            if (!File.Exists(RegisterLogin.location))
            {
                using (var stream = File.Create(RegisterLogin.location)) { }
            }
        }
        internal static void SaveChanges(List<BlogPost> inputList)
        {
            try
            {
                Console.WriteLine("----------------------------------------------------");
                using (AesManaged myAes = new AesManaged())
                {
                    myAes.Padding = PaddingMode.PKCS7;
                    myAes.KeySize = 128;
                    myAes.Key = new byte[128 / 8];
                    myAes.IV = new byte[128 / 8];
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
