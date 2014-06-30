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
         * type: Entity's type 
         */
        public string id;
        public string description;
        public string imageUrl;

        /* 
         Attributes - TypeIdentifier
         * type: TODO
         * typesList: list which will contain all the types of a given ID
         */
        public string type; //TODO: notable type; select one type; multiple types?
        public List<dynamic> mqlResult;
        public List<string> typesList = new List<string>();

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
            dynamic mql = new ExpandoObject();

            //filling the fields of the query
            mql.id = id;
            mql.type = new Dictionary<Object, Object>() {
                {"id",null}
            };

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
        }


        //Return the Entity's type - TODO
        public void getFreebaseType()
        {
            string type = "";
            IQueryable<Types> allFreebaseTypes =  unitOfWork.Get<Types>();
            List<string> freebaseTypeNames = (from t in allFreebaseTypes
                                                   select t.FreebaseName).ToList<string>();

            //looking into every type we've got
            for (int i = 0; i < this.typesList.Count; i++)
            {
                string typeName = this.typesList[i];
                if (freebaseTypeNames.Contains(typeName))
                {
                    int index = freebaseTypeNames.IndexOf(typeName);
                    type = freebaseTypeNames[index];
                    break;
                }
            }

            this.type = type;
        }
    }
}