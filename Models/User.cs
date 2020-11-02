using System;
using System.Collections.Generic;
using System.Text;

namespace BloggerApplication.Models
{
    public class User
    {
        //private int userID ;
        private string username;
        private string password;

        public void setUsername(string usern)
        {
            username = usern;
        }
        public string getUsername()
        {
            return username;
        }
        public void setPassword(string pass)
        {
            password = pass;
        }

        public string getPassword()
        {
            return password;
        }
    }
}
