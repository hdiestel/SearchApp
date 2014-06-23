using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Freebase4net;
using System.Threading.Tasks;
using SearchApp.Models;
using System.Dynamic;
using SearchApp.FreebaseEntityClasses;


namespace SearchApp.Controllers
{
    public class SearchController : Controller
    { 
        //
        // GET: /Search/
        public ActionResult Index(string searchString, string Domain)
        {
            List<string> textResults = new List<string>();
            List<string> imageUrls = new List<string>();
            List<string> topicResults = new List<string>();

            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(Domain))
            {
                SearchService searchService = FreebaseServices.CreateSearchService();
                TextService textService = FreebaseServices.CreateTextService();
                ImageService imageService = FreebaseServices.CreateImageService();
                MqlReadService mqlReadService = FreebaseServices.CreateMqlReadService();
                TopicService topicService = FreebaseServices.CreateTopicService();

                string id, imageUrl, idType;
                SearchServiceResponse searchResponse = searchService.Read(searchString, filter: "(any domain:/" + Domain + ")");

                foreach(SearchResult result in searchResponse.Results)
                {
                    id = result.Id;
                    string textResponse = textService.Read(id).Result;

                    TypeIdentifier type = new TypeIdentifier(id);
                    idType = type.GetUniqueType();
                    
                    if (!String.IsNullOrEmpty(textResponse))
                    {
                        textResponse += idType; //just to check if GetUniqueType is working
                        textResults.Add(textResponse);
                        imageUrl = imageService.GetImageUrl(id, maxwidth: "150", maxheight: "150");
                        imageUrls.Add(imageUrl);
                    }
                }
            }

            ViewBag.textResults = textResults;
            ViewBag.imageUrls = imageUrls;

            var model = new SearchModel();
            return View(model);
        }
	}
}