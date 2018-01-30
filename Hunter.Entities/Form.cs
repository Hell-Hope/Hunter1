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

        public List<Field> Fields { get; set; }

        public List<Dictionary<string, object>> Columns { get; set; }

        public List<Node> Nodes { get; set; }

        public List<Line> Lines { get; set; }

        public List<Area> Areas { get; set; }

        public class Field
        {
            public string Name { get; set; }

            public string Type { get; set; }
        }


    }
}
