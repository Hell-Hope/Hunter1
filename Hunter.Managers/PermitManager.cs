using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Hunter.Managers
{
    public class PermitManager : Manager
    {
        public PermitManager(MongoClient mongoClient) : base(mongoClient)
        {

        }

        public IMongoCollection<Entities.Permit> Collection
        {
            get
            {
                return this.DefaultDatabase.GetCollection<Entities.Permit>(nameof(Entities.Permit));
            }
        }

        public Entities.Permit Find(string id)
        {
            var filter = this.BuildFilterEqualID<Entities.Permit>(id);
            return this.Collection.Find(filter).FirstOrDefault();
        }

        public Models.Permit.Edit GetEdit(string id)
        {
            var entity = this.Find(id);
            if (entity == null)
                return null;
            return AutoMapper.Mapper.Map<Models.Permit.Edit>(entity);
        }

        public void Save(Models.Permit.Edit edit)
        {
            var entity = this.Find(edit.ID);
            if (entity == null)
                entity = AutoMapper.Mapper.Map<Entities.Permit>(edit);
            else
                AutoMapper.Mapper.Map(edit, entity);
            var filter = this.BuildFilterEqualID<Entities.Permit>(edit.ID);
            this.Collection.ReplaceOne(filter, entity, UpdateOptions);
        }

        public void Remove(string id)
        {
            var r = this.Collection.DeleteOne(m => m.ID == id);
        }

        public Models.PageResult<Entities.Permit> Query(Models.PageParam<Models.Permit.Condition> pageParam)
        {
            var filter = this.BuildFilter(pageParam.Condition);
            var collection = this.Collection.Find(filter);
            var result = new Models.PageResult<Entities.Permit>();
            result.Total = collection.Count();
            result.Data = collection.Sort(pageParam).Pagination(pageParam).ToList();
            return result;
        }

        protected FilterDefinition<Entities.Permit> BuildFilter(Models.Permit.Condition condition)
        {
            return this.BuildFilter(this.BuildFilters(condition));
        }

        protected List<FilterDefinition<Entities.Permit>> BuildFilters(Models.Permit.Condition condition)
        {
            var list = new List<FilterDefinition<Entities.Permit>>();
            if (condition == null)
                return list;
            if (!String.IsNullOrWhiteSpace(condition.Name))
                list.Add(Builders<Entities.Permit>.Filter.Regex(nameof(Entities.Permit.Name), Helper.FormatQueryString(condition.Name)));
            if (!String.IsNullOrWhiteSpace(condition.ID))
                list.Add(Builders<Entities.Permit>.Filter.Regex(nameof(Entities.Permit.ID), Helper.FormatQueryString(condition.ID)));
            return list;
        }


    }
}
