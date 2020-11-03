using BloggerApplication.Models;
using BloggerApplication.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloggerApplication.DB
{
    static class CRUD
    {
        /// <summary>
        /// Determines whether or not to add or update a product in the List<Item> 
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        /// <param name="addOrUpdate">true = adding new item, false = updating an existing product</param>
        /// <param name="productIndex">(Used with updating a product) Determines the position of the product in the list to update</param>
        internal static void AddOrUpdate(List<BlogPost> inputList, bool addOrUpdate, int? postIndex)
        {
            bool isAuthorValid = false;
            string blogId, blogCategory, blogTitle, blogContent;
            try
            {
                do
                {
                    ValidateAuthor(out isAuthorValid, out blogId, out blogCategory, out blogTitle, out blogContent);
                    BlogPost mobileObj = new BlogPost(blogId, blogCategory, blogTitle, blogContent);
                    if (isAuthorValid)
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
                        Console.Write("Something went wrong. Please try again.");
                    }
                } while (!isAuthorValid);

                ConnectionData.SaveChanges(inputList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Validate the product that is going to be added or updated
        /// </summary>
        /// <param name="isPriceValid">Does the blogContent match the validation</param>
        /// <param name="product">Name of the product</param>
        /// <param name="price">blogContent of the product</param>
        private static void ValidateAuthor(out bool isAuthorValid, out string blogPostId,out string blogPostCategory, out string blogPostTitle, out string blogPostContent)
        {
            blogPostId = GenerateUniqueID();
            Console.WriteLine(@"your post unique number is {0}",blogPostId);
            Console.WriteLine("Provide the new category");
            blogPostCategory = Console.ReadLine();

            Console.WriteLine("Provide the new title");
            blogPostTitle = Console.ReadLine();

            Console.WriteLine("Write your new content");
            blogPostContent = Console.ReadLine();

            //Console.WriteLine("Blog Post Author");
            //blogPostAuthor = Console.ReadLine();

            isAuthorValid = true;
            //bool AuthorValid = RegisterLogin.dbVerifyUsername(blogPostAuthor);

        }

        /// <summary>
        /// Delete an item from the text file
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
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
