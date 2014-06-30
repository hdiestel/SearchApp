using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Freebase4net;
using System.Threading.Tasks;
using SearchApp.Models;
using System.Dynamic;
using SearchApp.ApiSupportClasses;


namespace SearchApp.Controllers
{
    public class SearchController : Controller
    { 
        //
        // GET: /Search/
        public ActionResult Index(string searchString, string Domain)
        {
            //List of FreebaseEntity objects
            List<FreebaseEntity> results = new List<FreebaseEntity>();

            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(Domain))
            {
                //Creating SearchService and performing a search query
                SearchService searchService = FreebaseServices.CreateSearchService();
                SearchServiceResponse searchResponse = searchService.Read(searchString, filter: "(any domain:/" + Domain + ")");

                //Iterating over every result
                foreach(SearchResult result in searchResponse.Results)
                {
                    //creating a new instance of FreebaseEntity and retrieving description and imageURL
                    FreebaseEntity newEntity = new FreebaseEntity(result.Id);
                    newEntity.getDescription();
                    newEntity.getImageUrl();

                    //testing types
                    newEntity.identifyTypes();
                    newEntity.getFreebaseType();
                    
                    //it only takes results which does not have a empty description
                    if (!String.IsNullOrEmpty(newEntity.description))
                    {
                        results.Add(newEntity);
                    }
                }

            }

            //sending the results to the View (Index)
            ViewBag.results = results;

            var model = new SearchModel();
            return View(model);
        }
	}
}