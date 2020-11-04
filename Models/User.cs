namespace BloggerApplication.Models
{
    class User
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
