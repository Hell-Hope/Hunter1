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

        public Models.Result Save(Models.User.Edit edit)
        {
            if (this.ExistAccount(edit.Account, edit.ID))
                return Models.Result.Create(Models.Code.Exist, "帐号已经存在");
            var entity = this.Find(edit.ID);
            if (entity == null)
                entity = AutoMapper.Mapper.Map<Entities.User>(edit);
            else
                AutoMapper.Mapper.Map(edit, entity);
            var filter = this.BuildFilterEqualID<Entities.User>(edit.ID);
            this.Collection.ReplaceOne(filter, entity, UpdateOptions);
            return Models.Result.Create();
        }

        public Models.Result Remove(string id)
        {
            var r = this.Collection.DeleteOne(m => m.ID == id);
            return Models.Result.Create();
        }

        public Models.Result Login(Models.User.Login login)
        {
            var account = Builders<Entities.User>.Filter.Eq(nameof(Entities.User.Account), login.Account);
            var password = Builders<Entities.User>.Filter.Eq(nameof(Entities.User.Password), login.Password);
            var filter = Builders<Entities.User>.Filter.And(account, password);
            var entity = this.Collection.Find(filter).FirstOrDefault();
            if (entity == null)
                return Models.Result.Create(Models.Code.Fail, "帐号或密码错误");
            var applicationUser = new Models.ApplicationUser()
            {
                ID = entity.ID,
                Account = entity.Account,
                Name = entity.Name
            };
            return Models.Result.CreateDataResult(applicationUser);   
        }

        public Models.PageResult<Models.User.QueryResultData> Query(Models.PageParam<Models.User.Condition> pageParam)
        {
            var filter = this.BuildFilter(pageParam.Condition);
            var collection = this.Collection.Find(filter);
            var result = new Models.PageResult<Models.User.QueryResultData>();
            result.Total = collection.Count();
            var projections = new ProjectionDefinition<Entities.User>[]
            {
                Builders<Entities.User>.Projection.Include(nameof(Entities.User.ID)),
                Builders<Entities.User>.Projection.Include(nameof(Entities.User.Name)),
                Builders<Entities.User>.Projection.Include(nameof(Entities.User.Account))
            };
            var projection = Builders<Entities.User>.Projection.Combine(projections);
            var list = collection.Sort(pageParam).Pagination(pageParam).Project(projection).As<Entities.User>().ToList();
            result.Data = AutoMapper.Mapper.Map<List<Models.User.QueryResultData>>(list);
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
