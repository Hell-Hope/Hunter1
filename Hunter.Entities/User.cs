using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Entities
{
    [BsonIgnoreExtraElements]
    public class User : Entity
    {

        public string Account { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public HashSet<string> Permits { get; set; }
    }
}
