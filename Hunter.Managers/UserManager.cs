using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Hunter.Managers
{
    public class UserManager : Manager
    {
        public UserManager(MongoClient mongoClient) : base(mongoClient)
        {
        }

        public IMongoCollection<Entities.User> Collection
        {
            get
            {
                return this.DefaultDatabase.GetCollection<Entities.User>(nameof(Entities.User));
            }
        }

        public Entities.User Find(string id)
        {
            var filter = this.BuildFilterEqualID<Entities.User>(id);
            return this.Collection.Find(filter).FirstOrDefault();
        }

        public Models.User.Edit GetEdit(string id)
        {
            var entity = this.Find(id);
            if (entity == null)
                return null;
            return AutoMapper.Mapper.Map<Models.User.Edit>(entity);
        }

        public bool ExistAccount(string account, string id)
        {
            var _account = Builders<Entities.User>.Filter.Eq(nameof(Entities.User.Account), account);
            var _id = Builders<Entities.User>.Filter.Ne(nameof(Entities.User.ID), id);
            var filter = Builders<Entities.User>.Filter.And(_account, _id);
            var entity = this.Collection.Find(filter).FirstOrDefault();
            return entity != null;
        }

        public void Save(Models.User.Edit edit)
        {
            var entity = this.Find(edit.ID);
            if (entity == null)
                entity = AutoMapper.Mapper.Map<Entities.User>(edit);
            else
                AutoMapper.Mapper.Map(edit, entity);
            var filter = this.BuildFilterEqualID<Entities.User>(edit.ID);
            this.Collection.ReplaceOne(filter, entity, UpdateOptions);
        }

        public void Remove(string id)
        {
            var r = this.Collection.DeleteOne(m => m.ID == id);
        }

        public Models.PageResult<Entities.User> Query(Models.PageParam<Models.User.Condition> pageParam)
        {
            var filter = this.BuildFilter(pageParam.Condition);
            var collection = this.Collection.Find(filter);
            var result = new Models.PageResult<Entities.User>();
            result.Total = collection.Count();
            result.Data = collection.Sort(pageParam).Pagination(pageParam).ToList();
            return result;
        }

        protected FilterDefinition<Entities.User> BuildFilter(Models.User.Condition condition)
        {
            return this.BuildFilter(this.BuildFilters(condition));
        }

        protected List<FilterDefinition<Entities.User>> BuildFilters(Models.User.Condition condition)
        {
            var list = new List<FilterDefinition<Entities.User>>();
            if (condition == null)
                return list;
            if (!String.IsNullOrWhiteSpace(condition.Name))
                list.Add(Builders<Entities.User>.Filter.Regex(nameof(Entities.User.Name), Helper.FormatQueryString(condition.Name)));
            if (!String.IsNullOrWhiteSpace(condition.Account))
                list.Add(Builders<Entities.User>.Filter.Regex(nameof(Entities.User.Account), Helper.FormatQueryString(condition.Account)));
            return list;
        }



    }
}
