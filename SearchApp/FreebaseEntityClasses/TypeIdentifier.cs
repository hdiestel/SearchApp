using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Freebase4net;
using System.Dynamic;

namespace SearchApp.FreebaseEntityClasses
{
    public class TypeIdentifier
    {
        //list which will contain all the types of a given ID
        public List<dynamic> typesList;

        //constructor
        public TypeIdentifier(string id)
        {
            //defining variables
            //this.typesList = new List<dynamic>();             
            dynamic mql = new ExpandoObject();
            MqlReadService mqlReadService = FreebaseServices.CreateMqlReadService();

            //filling the fields of the query
            mql.id = id;
            mql.type = null; // TODO - DOES NOT WORK. FIND A WAY TO REPRESENT [] FROM MQL QUERIES 

            //querying
            MqlReadServiceResponse mqlResponse = mqlReadService.Read(mql);
            this.typesList = mqlResponse.Results;
        }

        /*
        function to get the type according to our pre-defined list of types
        (see classes inside the folder FreebaseEntityClasses)    
        */
        public string GetUniqueType()
        {
            string type = "";

            //looking into every type we've got
            for (int i = 0; i<this.typesList.Count; i++)
            {
                string currentType = this.typesList[i]["types"];

               //checking if the current type is one of ours
                switch(currentType) {
                    case "/people/person":
                        type = "person";
                        break;
                    case "/business/company":
                        type = "company";
                        break;
                    case "/book/book":
                        type = "book";
                        break;
                    case "/location/country":
                        type = "country";
                        break;
                    default:                       
                        return "generic";
                }
            }
            return type;
        } 
    }
}