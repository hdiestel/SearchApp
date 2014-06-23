using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Freebase4net;
using System.Dynamic;
using Newtonsoft.Json;

namespace SearchApp.FreebaseEntityClasses
{
    public class TypeIdentifier
    {
        //list which will contain all the types of a given ID
        public List<dynamic> mqlResult;
        public List<string> typesList = new List<string>();

        //constructor
        public TypeIdentifier(string id)
        {
            //defining variables
            //this.typesList = new List<dynamic>();             
            dynamic mql = new ExpandoObject();
            MqlReadService mqlReadService = FreebaseServices.CreateMqlReadService();

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
                for(int j = 0; j < typeIds.Count; j++)
                {
                    string typeName = typeIds[j]["id"];
                    typesList.Add(typeName);
                }
            }
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
                string currentType = this.typesList[i];

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
                }
            }
            return type;
        } 
    }
}