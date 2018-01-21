using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Hunter.Managers
{
    public class DynamicFormManager : Manager
    {
        public DynamicFormManager(MongoClient mongoClient) : base(mongoClient)
        {
        }

        public IMongoCollection<BsonDocument> DynamicForms(string formID)
        {
            return this.DefaultDatabase.GetCollection<BsonDocument>("Dynamic" + formID);
        }

        public BsonDocument Find(string formID, string id)
        {
            var filter = this.BuildFilterEqualID<BsonDocument>(id);
            return this.DynamicForms(formID).Find(filter).FirstOrDefault();
        }

        public void Save(string formID, string dataID, Dictionary<string, object> dictionary)
        {
            var collection = this.DynamicForms(formID);
            var filter = this.BuildFilterEqualID<BsonDocument>(dataID);
            var data = collection.FindSync<BsonDocument>(filter).FirstOrDefault();
            if (data == null)
            {
                dictionary[nameof(Entities.Entity.ID)] = dataID;
                collection.InsertOne(new BsonDocument(dictionary));
            }
            else
            {
                var sets = new List<UpdateDefinition<BsonDocument>>();
                foreach (var item in dictionary)
                {
                    sets.Add(Builders<BsonDocument>.Update.Set(item.Key, item.Value));
                }
                collection.UpdateOne(filter, Builders<BsonDocument>.Update.Combine(sets));
            }
        }

    }
}
