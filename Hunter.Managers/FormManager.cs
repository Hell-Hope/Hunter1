using System;
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

        public Models.Form.Edit GetEdit(string id)
        {
            var entity = this.Find(id);
            if (entity == null)
                return null;
            return AutoMapper.Mapper.Map<Models.Form.Edit>(entity);
        }

        public void Save(Models.Form.Edit edit)
        {
            var entity = this.Find(edit.ID);
            if (entity == null)
                entity = AutoMapper.Mapper.Map<Entities.Form>(edit);
            else
                AutoMapper.Mapper.Map(edit, entity);
            entity.Fields = this.ParseHtml(edit.Html);
            this.Forms.ReplaceOne(m => m.ID == edit.ID, entity, UpdateOptions);
        }

        /// <summary> 保存Html数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public void SaveHtml(string id, string html)
        {
            var fields = ParseHtml(html);
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            var setHtml = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Html), html);
            var setFields = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Fields), fields);
            var set = Builders<Entities.Form>.Update.Combine(setHtml, setFields);
            this.Forms.UpdateOne(filter, set, UpdateOptions);
        }

        protected Dictionary<string, Entities.Form.Field> ParseHtml(string html)
        {
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);
            var result = new Dictionary<string, Entities.Form.Field>();
            ParseHtml(htmlDocument.DocumentNode, result);
            return result;
        }

        protected void ParseHtml(HtmlAgilityPack.HtmlNode htmlNode, Dictionary<string, Entities.Form.Field> fields)
        {
            if (htmlNode == null)
                return;
            foreach (var childNode in htmlNode.ChildNodes)
            {
                if (childNode.Name == "input")
                {
                    var name = childNode.Attributes["name"]?.Value;
                    if (String.IsNullOrWhiteSpace(name))
                        continue;
                    fields[name] = new Entities.Form.Field()
                    {
                        Name = name,
                        Type = childNode.Attributes["type"]?.Value
                    };
                } 
                else if (childNode.Name == "select")
                {
                    var name = childNode.Attributes["name"]?.Value;
                    if (String.IsNullOrWhiteSpace(name))
                        continue;
                    fields[name] = new Entities.Form.Field()
                    {
                        Name = name,
                        Type = "select"
                    };
                }
                else if (childNode.Name == "textarea")
                {
                    var name = childNode.Attributes["name"]?.Value;
                    if (String.IsNullOrWhiteSpace(name))
                        continue;
                    fields[name] = new Entities.Form.Field()
                    {
                        Name = name,
                        Type = "textarea"
                    };
                }
                else
                {
                    ParseHtml(childNode, fields);
                }
            }
        }


        public Models.PageResult<Entities.Form> Query(Models.PageParam<Models.Form.Condition> pageParam)
        {
            var filter = this.BuildFilter(pageParam.Condition);
            var collection = this.Forms.Find(filter);
            
            var result = new Models.PageResult<Entities.Form>();
            result.Total = collection.Count();
            result.Data = collection.Pagination(pageParam).Sort(pageParam).ToList();
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
