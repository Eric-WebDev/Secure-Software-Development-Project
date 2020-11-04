using BloggerApplication.DB;
using BloggerApplication.Models;
using BloggerApplication.Seciurity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BloggerApplication.View
{
    static class RegisterLogin
    {
        public static string location;
        public static void UserVerify()
        {
            Console.WriteLine(" To continue type one of the below options : \n 'register' if you want to sign up \n 'login' if you want to sign in \n 'exit' if you want to close the console");
            string input = Console.ReadLine();
            if (input != "Register".ToLower() && input != "Login".ToLower() && input != "Exit".ToLower())
            {
                Console.WriteLine("Command does not exist");
                UserVerify();
            }
            else
            {
                if (input == "exit")
                    Environment.Exit(0);
                else
                {
                    Console.WriteLine("Your choosen option is {0}", input);
                    User user = new User();
                    Console.WriteLine("Username: ");
                    string usern = Console.ReadLine();
                    Console.WriteLine("Password: ");
                    string pass = MaskPassword();
                    user.setUsername(usern);
                    user.setPassword(pass);
                    if (input == "register")
                    {
                        if (dbVerifyUsername(user.getUsername()))
                        {
                            Encryption.HashPassword(user.getPassword());
                            dbInsertUser(user.getUsername(), Encryption.HashPassword(user.getPassword()));
                            Console.WriteLine("You are now registered as {0}", user.getUsername());
                            Dashboard(user.getUsername());
                        }
                        else
                        {
                            Console.WriteLine("Username already exists");
                            UserVerify();
                        }
                    }
                    if (input == "login")
                    {
                        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + "C:\\Modules\\Secure Software Development\\BloggerApplication\\DB\\BlogDB.mdf" + ";Integrated Security=True");
                        connection.Open();
                        SqlCommand command = new SqlCommand("Select password from dbo.Users where username=@usern", connection);
                        command.Parameters.AddWithValue("@usern", user.getUsername());

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string newPass = Convert.ToString(user.getPassword());
                                string oldPass = Convert.ToString(reader["password"]);
                                if (Encryption.VerifyHashedPass(newPass, oldPass))
                                {
                                    Dashboard(user.getUsername());
                                }

                                else
                                {
                                    Console.WriteLine("Acces denied. Passwords do not match");
                                    UserVerify();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Acces denied. Username not found in the database");

                            }
                        }
                        connection.Close();
                    }
                }
            }
            static void dbInsertUser(string usern, string pass)
            {
                SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + "C:\\Modules\\Secure Software Development\\BloggerApplication\\DB\\BlogDB.mdf" + ";Integrated Security=True");
                connection.Open();
                SqlCommand command = new SqlCommand("Insert into dbo.Users (username, password) values (@usern, @pass)", connection);
                command.Parameters.AddWithValue("@usern", usern);
                command.Parameters.AddWithValue("@pass", pass);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static bool dbVerifyUsername(string usern)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + "C:\\Modules\\Secure Software Development\\BloggerApplication\\DB\\BlogDB.mdf" + ";Integrated Security=True");
            connection.Open();
            SqlCommand command = new SqlCommand("Select username from dbo.Users where username=@usern", connection);
            command.Parameters.AddWithValue("@usern", usern);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    connection.Close();
                    return false;
                }
                else
                {
                    connection.Close();
                    return true;
                }
            }
        }
        public static string MaskPassword()
        {
            string pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return pass;
        }

        //providing storage location and display menu
        public static void Dashboard(string username)
        {
            Console.WriteLine("You can add your posts now");
            location = @"C:\Modules\Secure Software Development\BloggerApplication\DB\Storage\" + username + ".txt";
            List<BlogPost> inputList = ConnectionData.StoreData();
            Menu.DisplayMenu(inputList);
        }
    }
}