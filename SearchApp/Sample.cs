using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace Freebase4net
{
    public class Sample
    {
        public async void SimpleUsage()
        {
            var readService = FreebaseServices.CreateMqlReadService();

            dynamic thepolice = new ExpandoObject();
            thepolice.type = "/music/artist";
            thepolice.name = FreebaseHelpers.Operators.CreateLikeOperator("^The Sco*$"); // Regex search

            MqlReadServiceResponse result = await readService.ReadAsync(thepolice);

            //Process result
            var content = result.ResultAsString;
            //get status
            var status = result.Status;
        }

        public async void DictionaryUsage()
        {
            var readService = FreebaseServices.CreateMqlReadService();

            dynamic thepolice = new ExpandoObject();
            thepolice.type = "/music/artist";
            thepolice.name = "The Scorpions";
            thepolice.album = new Dictionary<Object, Object>();
            thepolice.album.Add("name", null);
            thepolice.album.Add("limit", 2);
            thepolice.album.Add("genre", FreebaseHelpers.CreateArrayWithEmptyObject());


            MqlReadServiceResponse result = await readService.ReadAsync(thepolice);

            //Process result
            var content = result.ResultAsString;
            //get status
            var status = result.Status;
        }

        public async void ComplexEmbeddedObjectUsage()
        {
            var readService = FreebaseServices.CreateMqlReadService();

            dynamic director = new ExpandoObject();
            director.name = null;
            director.id = null;
            director.type = "/film/editor";
            director.film = new Dictionary<Object, Object>() {
                {"name",null},
                {"id",null},
                {"starring",new Dictionary<Object,Object>()
                    {
                        {"actor","Theresa Russell"}
                    }
                }
            };


            MqlReadServiceResponse result = await readService.ReadAsync(director);

            //Process result
            var content = result.ResultAsString;
            //get status
            var status = result.Status;
        }

        public void MultipleQueries_Usage()
        {
            var readService = FreebaseServices.CreateMqlReadService();

            dynamic thescorpions = new ExpandoObject();
            thescorpions.name = "The Scorpions";
            thescorpions.type = "/music/artist";

            dynamic thepolice = new ExpandoObject();
            thepolice.name = "The Police";
            thepolice.type = "/music/artist";

            MultipleMqlReadServiceAsyncResponse multiFreebaseResponse = readService.ReadMultipleAsync(thescorpions, thepolice);
            Task.WaitAll(multiFreebaseResponse.Results.ToArray());

            foreach (var t in multiFreebaseResponse.Results)
            {
                // Process results
            }
        }

        public async void TextServiceUsage()
        {
            var textService = FreebaseServices.CreateTextService();
            string id = "/en/the_animal";

            var result = await textService.ReadAsync(id);

            var description = result.Result;
        }

        public void ImageServiceUsage()
        {
            var imageService = FreebaseServices.CreateImageService();
            string id = "/en/the_animal";

            var result = imageService.GetImageUrl(id); //Image url
        }
    }
}
