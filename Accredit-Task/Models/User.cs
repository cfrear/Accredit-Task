using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accredit_Task.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string Avatar { get; set; }
        public List<Repo> Repos { get; set; }
    }
}