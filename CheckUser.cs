using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApp1
{
    internal class CheckUser
    {

        public string Login { get; set; }
        public bool IsAdmin { get; }
        public string Status => IsAdmin ? "admin" : "user";

        public CheckUser(string login, bool isAdmin)
        {
            Login = login.Trim();
            IsAdmin = isAdmin;
        }
    }
}
