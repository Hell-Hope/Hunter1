﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Hunter.Managers
{
    public class FormManager : Manager
    {
        public FormManager(MongoClient mongoClient) : base(mongoClient)
        {
        }

        public IMongoCollection<Entities.Form> Forms
        {
            get
            {
                return this.DefaultDatabase.GetCollection<Entities.Form>(nameof(Entities.Form));
            }
        }

        public Entities.Form Find(string id)
        {
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            return this.Forms.Find(filter).FirstOrDefault();
        }

        /// <summary> 保存Html数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public Entities.Entity SaveHtml(string id, string html)
        {
            var entity = this.Find(id);
            if (entity == null)
            {
                this.Forms.InsertOne(entity = new Entities.Form()
                {
                    ID = id,
                    Html = html
                });
            }
            else
            {
                entity.Html = html;
                var filter = this.BuildFilterEqualID<Entities.Form>(id);
                var set = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Html), html);
                this.Forms.UpdateOne(filter, set);
            }
            return entity;
        }

        public Models.PageResult<Entities.Form> Query(Models.PageParam<Models.Form.Condition> pageParam)
        {
            var filter = this.BuildFilter(pageParam.Condition);
            var collection = this.Forms.Find(filter);
            var result = new Models.PageResult<Entities.Form>();
            result.Total = collection.Count();
            result.Data = collection.Pagination(pageParam).ToList();
            return result;
        }

        protected FilterDefinition<Entities.Form> BuildFilter(Models.Form.Condition condition)
        {
            return this.BuildFilter(this.BuildFilters(condition));
        }

        protected List<FilterDefinition<Entities.Form>> BuildFilters(Models.Form.Condition condition)
        {
            var list = new List<FilterDefinition<Entities.Form>>();
            if (condition == null)
                return list;
            if (!String.IsNullOrWhiteSpace(condition.Name))
                list.Add(Builders<Entities.Form>.Filter.Regex(nameof(Entities.Form.Name), new BsonRegularExpression(condition.Name)));
            return list;
        }

    }
}