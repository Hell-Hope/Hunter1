using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Managers
{
    public class Shared
    {

        public MongoClient MongoClient { get; set; }

        public Models.ApplicationUser ApplicationUser { get; set; }

    }
}
