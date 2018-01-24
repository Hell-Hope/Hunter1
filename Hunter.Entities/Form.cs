using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Entities
{

    [BsonIgnoreExtraElements]
    public class Form : Entity
    {

        public string Name { get; set; }

        public string Html { get; set; }

        public string Remark { get; set; }

        public Dictionary<string, Field> Fields { get; set; }

        public class Field
        {
            public string Name { get; set; }

            public string Type { get; set; }
        }

    }
}
