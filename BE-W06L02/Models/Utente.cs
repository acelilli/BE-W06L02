using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BE_W06L02.Models
{
    public class Utente
    {
        public int IdUtente { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
}