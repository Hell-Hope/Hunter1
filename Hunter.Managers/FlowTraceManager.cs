using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Hunter.Managers
{
    public class FlowTraceManager : Manager
    {
        internal FlowTraceManager(Shared shared) : base(shared)
        {
        }

        public IMongoCollection<Entities.FlowTrace> Collection
        {
            get
            {
                return this.DefaultDatabase.GetCollection<Entities.FlowTrace>(nameof(Entities.FlowTrace));
            }
        }

        public List<Entities.FlowTrace> FindByReferenceID(string referenceID)
        {
            var filter = Builders<Entities.FlowTrace>.Filter.Eq(nameof(Entities.FlowTrace.ReferenceID), referenceID);
            var exclude = Builders<Entities.FlowTrace>.Projection.Exclude(nameof(Entities.FlowTrace.Data));
            var list = this.Collection.Find(filter).Project(exclude).As<Entities.FlowTrace>().ToList();
            return list;
        }

        public Models.Result InsertFlowTrace(Entities.DynamicForm entity, string lineID)
        {
            var trace = new Entities.FlowTrace()
            {
                ID = this.GenerateMongoID,
                UserID = this.ApplicationUser?.ID,
                UserName = this.ApplicationUser?.Name,
                DateTime = DateTime.Now,
                ReferenceID = entity?.ID,
                Data = entity?.Data,
                NodeID = entity?.CurrentNode?.ID,
                LineID = lineID
            };
            this.Collection.InsertOne(trace);
            return Models.Result.Create();
        }

    }
}
