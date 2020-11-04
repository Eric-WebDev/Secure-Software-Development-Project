using BloggerApplication.Models;
using System;
using System.Collections.Generic;

namespace BloggerApplication.DB
{
    internal static class CRUD
    {
        internal static void AddOrUpdate(List<BlogPost> inputList, bool addOrUpdate, int? postIndex)
        {
            bool isConfirmed = true;
            string blogId, blogCategory, blogTitle, blogContent;
            try
            {
                do
                {
                    ConfirmStoreData(out isConfirmed, out blogId, out blogCategory, out blogTitle, out blogContent);
                    BlogPost mobileObj = new BlogPost(blogId, blogCategory, blogTitle, blogContent);
                    if (isConfirmed)
                    {
                        // Add new post or update existing
                        if (addOrUpdate)
                        {
                            inputList.Add(mobileObj);
                        }
                        else
                        {
                            for (int i = 0; i < inputList.Count; i++)
                            {
                                if (i == postIndex)
                                {
                                    // Update post based on index in List
                                    inputList[i] = mobileObj;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.Write("Post not stored. Please try again.");
                    }
                } while (!isConfirmed);

                ConnectionData.SaveChanges(inputList);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static void ConfirmStoreData(out bool isConfirmed, out string blogPostId, out string blogPostCategory, out string blogPostTitle, out string blogPostContent)
        {
            string confirmed;
        start:
            blogPostId = GenerateUniqueID();
            Console.WriteLine(@"your post unique number is {0}", blogPostId);

            Console.WriteLine("Provide the category for your post");
            blogPostCategory = Console.ReadLine();

            Console.WriteLine("Provide the post title");
            blogPostTitle = Console.ReadLine().Replace(",", " ");

            Console.WriteLine("Write your post content");
            blogPostContent = Console.ReadLine().Replace(",", " ");
        confirm:
            Console.WriteLine(" Do you want to save your post ? \n Type \n 'Y' to confirm \n 'N' to start again");
            confirmed = Console.ReadLine();
            if (confirmed == "Y".ToLower())
            {
                isConfirmed = true;
            }
            else if (confirmed == "N".ToLower())
            {
                isConfirmed = false;
                goto start;
            }
            else
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine(" Wrong symbol typed, Please try again \n ");
                goto confirm;
            }
        }
        internal static void DeleteItem(List<BlogPost> inputList)
        {
            if (inputList.Count != 0)
            {
                int postId;
                Console.WriteLine("Which offer would you like to delete?");

                for (int i = 0; i < inputList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {inputList[i].BlogPostId}");
                }

                Console.Write("Your choice: ");
                postId = Convert.ToInt32(Console.ReadLine());

                // Remove item based on index
                inputList.RemoveAt(postId - 1);
                // Resets the variable for next use
                postId = 0;
                ConnectionData.SaveChanges(inputList);
            }
            else
            {
                Console.WriteLine("No posts yet");
            }
        }
        public static string GenerateUniqueID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
