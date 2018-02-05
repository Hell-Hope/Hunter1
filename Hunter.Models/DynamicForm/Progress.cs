using Newtonsoft.Json;
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
