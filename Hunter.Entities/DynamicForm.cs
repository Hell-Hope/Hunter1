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

        public string CreatedUserID { get; set; }

        public string CreatedUserName { get; set; }

        public class Node
        {
            public string ID { get; set; }

            public bool Alt { get; set; }

            public int Top { get; set; }

            public int Left { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }

            public string Name { get; set; }

            public string Type { get; set; }

            public List<string> Fields { get; set; }

            public HashSet<string> Permits { get; set; }

            [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
            public bool IsEndType { get => Helper.IsEndTypeNode(this.Type); }

            [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
            public bool IsStartType { get => Helper.IsStartTypeNode(this.Type); }

        }

        public class Line
        {
            public string ID { get; set; }

            public double M { get; set; }

            public bool Alt { get; set; }

            public bool Marked { get; set; }

            public bool Dash { get; set; }

            public string Name { get; set; }

            public string From { get; set; }

            public string To { get; set; }

            public string Type { get; set; }

        }

        public class Area
        {
            public string ID { get; set; }

            public bool Alt { get; set; }

            public int Top { get; set; }

            public int Left { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }

            public string Name { get; set; }

            public string Color { get; set; }
        }

    }
}
