using BloggerApplication.Models;
using BloggerApplication.Seciurity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace BloggerApplication.View
{
    class RegisterLogin
    {
        public static void UserVerify()
        {
            Console.WriteLine("Write 'Register' if you want to sign up, 'Log in' if you want to sign in or 'Exit' if you want to close the console");
            string input = Console.ReadLine();
            if (input != "Register" && input != "Log in" && input != "Exit")
            {
                Console.WriteLine("Command does not exist");
                UserVerify();
            }
            else
            {
                if (input == "Exit")
                    Environment.Exit(0);
                else
                {
                    Console.WriteLine("Your command was {0}", input);
                    User user = new User();
                    Console.WriteLine("Username: ");
                    string usern = Console.ReadLine();
                    Console.WriteLine("Password: ");
                    string pass = MaskPassword();
                    user.setUsername(usern);
                    user.setPassword(pass);
                    if (input == "Register")
                    {
                        if (dbVerifyUsername(user.getUsername()))
                        {
                            Encryption.HashPassword(user.getPassword());
                            dbInsertUser(user.getUsername(), Encryption.HashPassword(user.getPassword()));
                            Console.WriteLine("You are now registered as {0}", user.getUsername());
                            //dashboard(user.getUsername());
                        }
                        else
                        {
                            Console.WriteLine("Username already exists");
                            UserVerify();
                        }
                    }
                    if (input == "Log in")
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
                                if (Encryption.verifyHashedPass(newPass, oldPass))
                                {
                                    Console.WriteLine("Grant acces");
                                    //List<User> inputList = ConnectionData.StoreData();
                                    //Console.WriteLine("Data in Blog app\n");
                                    //DisplayMenu(inputList);
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
            static bool dbVerifyUsername(string usern)
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
            static void dbInsertUser(string usern, string pass)
            {
                SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+"C:\\Modules\\Secure Software Development\\BloggerApplication\\DB\\BlogDB.mdf"+";Integrated Security=True");
                connection.Open();
                SqlCommand command = new SqlCommand("Insert into dbo.Users (username, password) values (@usern, @pass)", connection);
                command.Parameters.AddWithValue("@usern", usern);
                command.Parameters.AddWithValue("@pass", pass);
                command.ExecuteNonQuery();
                connection.Close();
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
    }
}
