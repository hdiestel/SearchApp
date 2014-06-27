using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Freebase4net;

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
        public string type; //TODO: notable type; select one type; multiple types?

        /* 
         Attributes - Freebase4Net
         * textService: service used for retrieving the description of a given entity
         * imageService: service used for retrieving the image URL of a given entity
         */
        public TextService textService;
        public ImageService imageService;

        //--------------------------------------------------------------------------------
        //Methods

        //Constructor
        public FreebaseEntity(string id) {
            this.id = id;

            //initializing the services which will be used by this object
            this.textService = FreebaseServices.CreateTextService();
            this.imageService = FreebaseServices.CreateImageService();
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


        //Return the Entity's type
        public string getFreebaseType()
        {
            return "TODO";
        }
    }
}