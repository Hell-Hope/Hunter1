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

        public List<Column> Columns { get; set; }

        public List<Node> Nodes { get; set; }

        public List<Line> Lines { get; set; }

        public List<Area> Areas { get; set; }

        public class Field
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public override int GetHashCode()
            {
                if (this == null)
                    return 0;
                if (this.Name == null)
                    return 0;
                return this.Name.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (this != null && obj is Field temp)
                    return this.Name == temp.Name;
                return false;
            }
        }

        public class Column
        {
            public string Sequence { get; set; }

            /// <summary> 字段名
            /// </summary>
            public string Field { get; set; }

            /// <summary> 宽度
            /// </summary>
            public string Width { get; set; }

            /// <summary> 水平对齐方式
            /// </summary>
            public string Align { get; set; }

            /// <summary> 垂直对齐方式
            /// </summary>
            public string VAlign { get; set; }

            /// <summary>
            /// </summary>
            public string Title { get; set; }

            public long TimeSpan { get; set; }

            /// <summary> 查询方法
            /// </summary>
            public string Find { get; set; }

            /// <summary> 是否显示
            /// </summary>
            public bool Visible { get; set; }
        }


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
