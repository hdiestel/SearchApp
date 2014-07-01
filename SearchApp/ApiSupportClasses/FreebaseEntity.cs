using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Freebase4net;
using System.Dynamic;
using SearchApp.DataAccess.Interfaces;
using SearchApp.DataAccess.Implementation;
using SearchApp.Models;

namespace SearchApp.ApiSupportClasses
{
    public class FreebaseEntity
    {
        /*
         Attributes - Entity
         * id: Entity's Freebase ID
         * description: Entity's short description
         * imageUrl: Entity's image URL
         */
        public string id;
        public string description;
        public string imageUrl;

        /* 
         Attributes - TypeIdentifier
         * 
         * typesList: list which will contain all the types of a given ID
         */
        public List<dynamic> mqlResult;
        public List<string> typesList = new List<string>();

        public List<Types> identifiedTypes = new List<Types>();
        public Dictionary<string, string> identifiedAttributes = new Dictionary<string, string>();

        /* 
         Attributes - Freebase4Net
         * textService: service used for retrieving the description of a given entity
         * imageService: service used for retrieving the image URL of a given entity
         */
        public TextService textService;
        public ImageService imageService;
        public MqlReadService mqlReadService;

        private IUnitOfWork unitOfWork;


        //--------------------------------------------------------------------------------
        //Methods


        //Constructor
        public FreebaseEntity(string id) {
            this.id = id;

            //initializing the services which will be used by this object
            this.textService = FreebaseServices.CreateTextService();
            this.imageService = FreebaseServices.CreateImageService();
            this.mqlReadService = FreebaseServices.CreateMqlReadService();

            unitOfWork = new UnitOfWork<DataContext>();
        }


        //Return the Entity's Description
        public string getDescription() {
            return this.description = textService.Read(id).Result;
        }


        //Return the Entity's Image URL
        public string getImageUrl()
        {
            return this.imageUrl = imageService.GetImageUrl(id, maxwidth: "150", maxheight: "150");
        }


        //Identify all the types from an Entity
        public void identifyTypes()
        {
            //defining variables            
            var mql = new ExpandoObject() as IDictionary<string, Object>;

            //filling the fields of the query
            mql.Add("type", new Dictionary<Object, Object>() {
                                {"id",null}
                            });
            mql.Add("id", this.id);

            //querying
            MqlReadServiceResponse mqlResponse = mqlReadService.Read(mql);
            this.mqlResult = mqlResponse.Results;
            for (int i = 0; i < this.mqlResult.Count; i++)
            {
                var typeIds = this.mqlResult[i]["type"];
                for (int j = 0; j < typeIds.Count; j++)
                {
                    string typeName = typeIds[j]["id"];
                    typesList.Add(typeName);
                }
            }
            List<Types> allFreebaseTypes = unitOfWork.Get<Types>().ToList<Types>();
            List<string> freebaseTypeNames = (from t in allFreebaseTypes
                                              select t.FreebaseName).ToList<string>();

            //looking into every type we've got
            for (int i = 0; i < this.typesList.Count; i++)
            {
                string typeName = this.typesList[i];
                if (freebaseTypeNames.Contains(typeName))
                {
                    int index = freebaseTypeNames.IndexOf(typeName);
                    identifiedTypes.Add(allFreebaseTypes[index]);
                }
            }
        }

        public void identifyAttributes(Types type)
        {
            var query = new ExpandoObject() as IDictionary<string, Object>;
            query.Add("type", type.FreebaseName);
            query.Add("id", id);

            //Add the linked attributes of othis type to the query
            foreach(Attributes attribute in type.Attributes)
            {
                if (attribute.resultType == "1")
                    query.Add(attribute.FreebaseName.Trim(), null);
                    
                else if (attribute.resultType == "2")
                {
                    query.Add(attribute.FreebaseName.Trim(), new Dictionary<Object, Object>() {
                                                        {"name",null}
                                                       });
                }
            }
            
            // create mqlresult
            this.mqlReadService = FreebaseServices.CreateMqlReadService();
            MqlReadServiceResponse mqlResponse = new MqlReadServiceResponse();
            mqlResponse = mqlReadService.Read(query);
            List<dynamic> results = mqlResponse.Results;

            //Get the attribute key/value pairs
            for (int i = 0; i < results.Count; i++)
            {
                foreach (Attributes attribute in type.Attributes)
                {
                    if(attribute.resultType == "1")
                        identifiedAttributes.Add(attribute.Name, (string) results[i][attribute.FreebaseName]);
                        
                    else if(attribute.resultType == "2")
                    {
                        var values = results[i][attribute.FreebaseName];
                        string refinedValues = "";
                        for (int j = 0; j < values.Count; j++)
                        {
                           refinedValues += values[j]["name"];
                           if (j != values.Count - 1)
                               refinedValues += ", ";
                        }
                        identifiedAttributes.Add(attribute.Name, refinedValues);
                    }
                }
            }
        }
    }
}