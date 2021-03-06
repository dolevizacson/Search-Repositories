﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Search_Repositories.Models;
using Search_Repositories.Classes;

namespace Search_Repositories.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient httpClient;
        private static RepositoryModel repositoryModel;

        static HomeController()
        {
            httpClient = new HttpClient();
        }

        [HttpGet]
        public ActionResult Index()
        {     
            return View();
        }  

        [HttpGet]
        public ActionResult BookmarkedRepositories()
        {            
            return View();
        }

        [HttpGet]
        public ActionResult ClearSession()
        {
            Session.Clear();
            return RedirectToAction("BookmarkedRepositories", "Home");
        }

        [HttpPost]
        public ActionResult AddRepositories(int id)
        {
            if (Session["repositoryList"] == null)
            {
                Session["repositoryList"] = new List<Repository>();
            }
            ((List<Repository>)Session["repositoryList"]).Add(repositoryModel.RepositoriesList[id]);
            
            return null;
        }

        [HttpPost]
        public async Task<ActionResult> GetRepositories(string RepoName)
        {
            string uri = "https://api.github.com/search/repositories?q=" + RepoName;
            string responseBody = "";
            repositoryModel = new RepositoryModel();

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
                ModelState.AddModelError("serverError", "Server Error");
                return PartialView("_ViewRepositories", repositoryModel);
            }

            JObject responseObject = JObject.Parse(responseBody);

            JArray repositories = (JArray)responseObject["items"];
            
            if (repositories.HasValues)
            {
                foreach (JObject repository in repositories)
                {
                    Repository tempRepository = JsonConvert.DeserializeObject<Repository>(repository.ToString());
                    repositoryModel.RepositoriesList[tempRepository.id]= tempRepository;
                }
            }   

            return PartialView("_ViewRepositories", repositoryModel);
        }
    }
}