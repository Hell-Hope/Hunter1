using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Entities
{
    /// <summary> 组织
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Organization : Entity
    {
        
        public string Name { get; set; }

        public string ParentID { get; set; }


    }
}
