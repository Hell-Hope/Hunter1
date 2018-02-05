using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Entities
{
    [BsonIgnoreExtraElements]
    public class Permit : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

    }
}
