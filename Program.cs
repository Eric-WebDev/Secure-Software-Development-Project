using BloggerApplication.DB;
using BloggerApplication.Models;
using BloggerApplication.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloggerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Blogger Application");
            Console.WriteLine("Share your thoughts and knwowledge !!!!!");
            Console.WriteLine("---------------------------------------------------------");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            RegisterLogin.UserVerify();
            Console.ReadLine();
            Console.ResetColor();
        }
    }
}
