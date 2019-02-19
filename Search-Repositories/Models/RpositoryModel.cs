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
            this.RepositoriesList = new Dictionary<int , Repository>();
        }

        public Dictionary<int, Repository> RepositoriesList { get; set; }
    }
}
