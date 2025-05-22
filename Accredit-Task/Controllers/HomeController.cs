using Accredit_Task.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Accredit_Task.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Results()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SearchUser(string username)
        {
            //https://api.github.com/users/USERNAME

            string apiUrl = "https://api.github.com/users/" + username;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "http://developer.github.com/v3/#user-agent-required");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    //var json = JObject.Parse(data);

                    User user = new User();
                    user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(data);

                    //Get Repos from URL
                    //List<Repo> repos = new List<Repo>();
                    //repos = GetRepos(user.ReposUrl).Result;
                    //repos.OrderBy(x => x.Stargazer_count).ToList();

                    //for (int i = 0; i <= 5; i++)
                    //{
                    //    user.Repos.Add(repos[i]);
                    //}

                    user.Repos.Add(new Repo { Id = 1, Name = "repoName", Description = "desc", Stargazer_count = 12, Link = "https://github.com/cfrear/Accredit-Task" });
                    user.Repos.Add(new Repo { Id = 2, Name = "repo2Name", Description = "desc", Stargazer_count = 22, Link = "https://github.com/cfrear/VCC-API" });

                    return View("Results", user);
                }                
            }
            return View("Error");
        }

        public async Task<List<Repo>> GetRepos(string repoUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(repoUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "http://developer.github.com/v3/#user-agent-required");
                

                HttpResponseMessage response = await client.GetAsync(repoUrl);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    List<Repo> repos = new List<Repo> { };

                    repos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Repo>>(data);

                    return repos;
                }
            }
            //return View("Error");
            return null;
        }
    }
}