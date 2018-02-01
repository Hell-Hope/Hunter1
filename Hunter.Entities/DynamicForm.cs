using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Entities
{
    [BsonIgnoreExtraElements]
    public class DynamicForm : Entity
    {

        public Dictionary<string, object> Data { get; set; }

        public string Html { get; set; }

        public List<Node> Nodes { get; set; }

        public List<Line> Lines { get; set; }

        public List<Area> Areas { get; set; }

        public Node CurrentNode { get; set; }

        public bool Finish { get; set; } = false;

    }
}
