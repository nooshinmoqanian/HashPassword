﻿using System.Security.Cryptography;
using System.Text;

namespace freeTime.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
       
}
