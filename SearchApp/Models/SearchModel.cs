using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchApp.Models
{
    public class SearchModel
    {
        public Dictionary<string, string> Domain;

        public SearchModel()
        {
            Domain = new Dictionary<string, string> {
                { "music", "Music" },
                { "book", "Book" },
                { "people", "People" },
                { "film", "Film" },
                { "sports", "Sports"}
            };
        }

    }
}