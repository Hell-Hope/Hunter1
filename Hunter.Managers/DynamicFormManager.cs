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

        public void Remove(string formID, string dataID)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("ID", dataID);
            this.DynamicForms(formID).DeleteOne(filter);
        }

        public Models.PageResult<Dictionary<string, object>> Query(string formID, Models.PageParam<Models.DynamicForm.Condition> pageParam)
        {
            var filter = this.BuildFilter(pageParam.Condition);
            var collection = this.DynamicForms(formID).Find(filter);

            var result = new Models.PageResult<Dictionary<string, object>>();
            result.Total = collection.Count();
            var list = collection.Sort(pageParam).Pagination(pageParam).ToList();
            result.Data = new List<Dictionary<string, object>>();
            foreach (var item in list)
                result.Data.Add(item.ToDictionary());
            return result;
        }

        protected FilterDefinition<BsonDocument> BuildFilter(Models.DynamicForm.Condition condition)
        {
            return this.BuildFilter(this.BuildFilters(condition));
        }

        protected List<FilterDefinition<BsonDocument>> BuildFilters(Models.DynamicForm.Condition condition)
        {
            var list = new List<FilterDefinition<BsonDocument>>();
            if (condition == null)
                return list;
            return list;
        }

    }
}
