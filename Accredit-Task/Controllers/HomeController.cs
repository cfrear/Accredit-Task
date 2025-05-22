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

        [HttpPost]
        public async Task<ActionResult> SearchUser(string username)
        {
            string apiUrl = "https://api.github.com/users/";

            //Get user
            User user = new User();
            user = await GetUser(apiUrl, username);

            if (user is null)
            {
                TempData["Errormsg"] = $"Error fetching data for user {username}.";
                return View("Error");
            }

            //Get Repos from URL
            List<Repo> repos = new List<Repo>();
            repos = GetRepos(user.ReposUrl).Result;

            if (repos is null)
            {
                TempData["Errormsg"] = $"Error fetching repos list for user {username}.";
                return View("Error");
            }

            //Get top 5 repos by stargazer count
            var sortedRepos = repos.OrderByDescending(x => x.Stargazer_count).ToList();
            int reposToList = Math.Min(sortedRepos.Count, 5);
            for (int i = 0; i < reposToList; i++)
            {
                user.Repos.Add(sortedRepos[i]);
            }

            return View("Results", user);
        }

        public async Task<User> GetUser(string apiUrl, string username)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "http://developer.github.com/v3/#user-agent-required");

                HttpResponseMessage response = client.GetAsync(username).Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                    User user = new User();
                    user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(data);
                    return user;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                return null;
            }
        }

        public async Task<List<Repo>> GetRepos(string repoUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(repoUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "http://developer.github.com/v3/#user-agent-required");

                //The task specifically asks me to pull the repos_url out of the response and use it here, so I have - but I don't like it.
                //Given that the repos_url is the same at the user url with "/repos" at the end, I would prefer to construct my request url in the same way as in the GetUser() method
                //and then append "/repos" in the GetAsync below.
                HttpResponseMessage response = client.GetAsync("").Result; 

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    List<Repo> repos = new List<Repo> { };

                    repos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Repo>>(data);

                    return repos;
                }
            }

            return null;
        }
    }
}