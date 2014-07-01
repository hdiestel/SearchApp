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
using SearchApp.DataAccess.Interfaces;
using SearchApp.DataAccess.Implementation;
using System.Threading;


namespace SearchApp.Controllers
{
    public class SearchController : Controller
    {
        private IUnitOfWork unitOfWork;

        public SearchController()
        {
            unitOfWork = new UnitOfWork<DataContext>();
        }
        //
        // GET: /Search/
        public ActionResult Index(string searchString, string Domains)
        {
            //List of FreebaseEntity objects
            List<FreebaseEntity> results = new List<FreebaseEntity>();
            ViewBag.Domains = new SelectList(unitOfWork.getContext().Domain, "ID", "Name");

            if (!String.IsNullOrEmpty(searchString))
            {
                //Creating SearchService and performing a search query
                SearchService searchService = FreebaseServices.CreateSearchService();
                SearchServiceResponse searchResponse;
                if (!String.IsNullOrEmpty(Domains))
                {
                    int domainId = Convert.ToInt32(Domains);
                    searchResponse = searchService.Read(searchString, filter: "(any domain:/" + unitOfWork.GetById<Domains>(domainId).FreebaseName + ")",limit: 3);
                }
                else
                    searchResponse = searchService.Read(searchString, limit: 3);

                //Iterating over every result
                foreach(SearchResult result in searchResponse.Results)
                {
                    //creating a new instance of FreebaseEntity and retrieving description and imageURL
                    FreebaseEntity newEntity = new FreebaseEntity(result.Id);
                    newEntity.getDescription();
                    newEntity.getImageUrl();

                    //identify all the types we defined
                    newEntity.identifyTypes();
                    foreach (Types type in newEntity.identifiedTypes)
                    {
                        //and identify all attribute values according to the identified types
                        newEntity.identifyAttributes(type);
                        Thread.Sleep(100);
                    }
                    //it only takes results which does not have a empty description
                    if (!String.IsNullOrEmpty(newEntity.description))
                    {
                        results.Add(newEntity);
                    }
                }

            }

            //sending the results to the View (Index)
            ViewBag.results = results;

            return View();
        }
	}
}