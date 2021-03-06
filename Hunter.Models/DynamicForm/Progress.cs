﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models.DynamicForm
{
    public class Progress
    {
        public string ID { get; set; }

        [JsonProperty("nodes")]
        public Dictionary<string, Node> Nodes { get; set; }

        [JsonProperty("lines")]
        public Dictionary<string, Line> Lines { get; set; }

        [JsonProperty("areas")]
        public Dictionary<string, Area> Areas { get; set; }

        public bool Finish { get; set; }

        public Node CurrentNode { get; set; }

        public List<FlowTrace> FlowTraces { get; set; }

        public class Node
        {

            public string ID { get; set; }

            [JsonProperty("alt")]
            public bool Alt { get; set; }

            [JsonProperty("top")]
            public int Top { get; set; }

            [JsonProperty("left")]
            public int Left { get; set; }

            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("fields")]
            public List<string> Fields { get; set; }

            [JsonProperty("permits")]
            public HashSet<string> Permits { get; set; }
        }

        public class Line
        {

            public string ID { get; set; }

            [JsonProperty("M")]
            public double M { get; set; }

            [JsonProperty("alt")]
            public bool Alt { get; set; }

            [JsonProperty("marked")]
            public bool Marked { get; set; }

            [JsonProperty("dash")]
            public bool Dash { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("from")]
            public string From { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

        }

        public class Area
        {

            public string ID { get; set; }

            [JsonProperty("alt")]
            public bool Alt { get; set; }

            [JsonProperty("top")]
            public int Top { get; set; }

            [JsonProperty("left")]
            public int Left { get; set; }

            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }
        }

    }

    public class FlowTrace
    {
        public string NodeID { get; set; }

        public string LineID { get; set; }

        public DateTime DateTime { get; set; }

        public string UserID { get; set; }

        public string UserName { get; set; }
    }



}
