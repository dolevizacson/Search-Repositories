using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Search_Repositories.Models;

namespace Search_Repositories.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient httpClient;

        static HomeController()
        {
            httpClient = new HttpClient();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetRepositories(string RepoName)
        {
            string uri = "https://api.github.com/search/repositories?q=" + RepoName;
            string responseBody = "";

            try
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Add("User-Agent",
                                                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.96 Safari/537.36");
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                responseBody = await httpClient.GetStringAsync(uri);
            }
            catch (HttpRequestException e)
            {

                responseBody = e.Message;
            }

            JObject responseObject = JObject.Parse(responseBody);

            JArray repositories = (JArray)responseObject["items"];

            List<ShowrRpositoriesModel> list = new List<ShowrRpositoriesModel>();

            if (repositories.HasValues)
            {
                foreach (JObject repository in repositories)
                {
                    ShowrRpositoriesModel tempModel = new ShowrRpositoriesModel
                    {
                        RepositoryName = (String)repository.GetValue("name"),
                        Avatar = (String)repository["owner"]["avatar_url"]
                    };
                    list.Add(tempModel);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Search Repositories");
            }

            return PartialView("_ViewRepositories", list);
        }
    }
}