using BloggerApplication.View;
using System;

namespace BloggerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("           Blogger Application           ");
            Console.WriteLine("    Share your thoughts and knwowledge   ");
            Console.WriteLine("-----------------------------------------");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            RegisterLogin.UserVerify();
            Console.ReadLine();
            Console.ResetColor();
        }
    }
}
