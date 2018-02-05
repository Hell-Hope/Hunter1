using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Entities
{
    [BsonIgnoreExtraElements]
    public class FlowTrace : Entity
    {
        
        public string ReferenceID { get; set; }

        public string NodeID { get; set; }

        public string LineID { get; set; }

        public DateTime DateTime { get; set; }

        public string UserID { get; set; }

        public string UserName { get; set; }

        public Dictionary<string, object> Data { get; set; }

    }
}
