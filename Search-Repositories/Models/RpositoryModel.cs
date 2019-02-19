using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Search_Repositories.Classes;

namespace Search_Repositories.Models
{
    public class RepositoryModel
    {
        public RepositoryModel()
        {
            this.RepositoriesList = new Dictionary<string, Repository>();
        }

        public Dictionary<string, Repository> RepositoriesList { get; set; }
    }
}
