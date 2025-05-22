using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accredit_Task.Models
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("login")]
        public string Username { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("avatar_url")]
        public string Avatar { get; set; }

        [JsonProperty("repos_url")]
        public string ReposUrl { get; set; }
        public List<Repo> Repos { get; set; } = new List<Repo>();
    }
}