using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accredit_Task.Models
{
    public class Repo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public int Stargazer_count { get; set; }
    }
}