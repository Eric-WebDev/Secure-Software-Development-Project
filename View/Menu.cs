using BloggerApplication.DB;
using BloggerApplication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloggerApplication.View
{
  static class Menu
    {
        public static int tableWidth = 120;
        public static void DisplayMenu(List<BlogPost> inputList)
        {
            string choice;
            do
            {
                Console.Write("Please choose one of the options ?:\n" +
                    "1. View All Posts\n" +
                    "2. Add New Post\n" +
                    "3. Update a Post\n" +
                    "4. Delete a Post\n" +
                    "5. Exit\n\n" +
                    "Your choice: ");
                choice = Console.ReadLine();
                MenuSelection(inputList, choice);
            } while (choice != "5");
            //GC (Garbage collector)  collects all objects that are no longer used from heap and remove them from memory.
            GC.Collect();
        }

        private static void MenuSelection(List<BlogPost> inputList, string choice)
        {
            switch (choice)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("View All Posts\n----------------------------------------------------");
                    ViewAllData(inputList);
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Add New Post");
                    CRUD.AddOrUpdate(inputList, true, null);
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Update a Post\n----------------------------------------------------");
                    GetUpdateMobileIndex(inputList);
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Delete a Post\n----------------------------------------------------");
                    CRUD.DeleteItem(inputList);
                    break;
                case "5":
                    Console.Clear();
                    Console.WriteLine("Exit\n----------------------------------------------------\nPress any key to exit the application.");
                    break;
                default:
                    Console.Write("input is not valid. Press any key to return to the main menu.\n");
                    break;
            }

            Console.ReadLine();
            Console.Clear();
        }

        // Display content of the text file
        private static void ViewAllData(List<BlogPost> inputList)
        {
            
            if (inputList.Count != 0)
            {
                foreach (var item in inputList)
                {
                    PrintLine();
                    PrintRow("Blog Post Id", "Blog Post Category");
                    PrintRow(Convert.ToString(item.BlogPostId), item.BlogPostCategory);                          
                    PrintLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(item.BlogPostTitle);
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.Write(item.BlogPostContent);
                    Console.WriteLine();
                    PrintLine();
                    Console.WriteLine();
                }
                
                Console.WriteLine("Press any key to return to the main menu.");
            }
            else
            {
                Console.WriteLine("No posts yet");
            }
        }

        // update the selected post
        private static void GetUpdateMobileIndex(List<BlogPost> inputList)
        {
            if (inputList.Count != 0)
            {
                Console.WriteLine("Choose post that you would like to update");

                for (int i = 0; i < inputList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {inputList[i].BlogPostId}");
                }

                Console.Write("Your choice: ");
                var productIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                CRUD.AddOrUpdate(inputList, false, productIndex);
            }
            else
            {
                Console.WriteLine("No posts yet");
            }
        }


        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
          
        }

    }
}
