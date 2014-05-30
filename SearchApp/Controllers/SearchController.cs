using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Freebase4net;


namespace SearchApp.Controllers
{
    public class SearchController : Controller
    { 
        //
        // GET: /Search/
        public ActionResult Index(string searchString)
        {
            List<string> textResults = new List<string>();
            List<string> imageUrls = new List<string>();
            List<string> topicResults = new List<string>();

            if(!String.IsNullOrEmpty(searchString))
            {
                SearchService searchService = FreebaseServices.CreateSearchService();
                TextService textService = FreebaseServices.CreateTextService();
                ImageService imageService = FreebaseServices.CreateImageService();
                TopicService topicService = FreebaseServices.CreateTopicService();
                TextServiceResponse textResponse_plain;
                TopicServiceResponse topicResponse;
                string id, imageUrl;
                SearchServiceResponse searchResponse = searchService.Read(searchString);
                
                foreach(SearchResult result in searchResponse.Results)
                {
                    id = result.Id;
                    textResponse_plain = textService.Read(id, TextFormat.Plain);
                    textResults.Add(textResponse_plain.Result);
                    imageUrl = imageService.GetImageUrl(id, maxwidth: "150", maxheight: "150");
                    imageUrls.Add(imageUrl);
                }

                topicResponse = topicService.Read("/en/kiel");
                topicResults.Add(topicResponse.ToString());
            }

            ViewBag.textResults = textResults;
            ViewBag.imageUrls = imageUrls;
            ViewBag.topicResults = topicResults;

            return View();
        }
	}
}