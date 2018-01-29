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

        public List<Models.Form.MenuItem> GetMenuItems()
        {
            var id = Builders<Entities.Form>.Projection.Include(nameof(Entities.Form.ID));
            var name = Builders<Entities.Form>.Projection.Include(nameof(Entities.Form.Name));
            var projection = Builders<Entities.Form>.Projection.Combine(id, name);
            var list = this.Forms.Find(Builders<Entities.Form>.Filter.Empty).Project(projection).ToList();
            var result = new List<Models.Form.MenuItem>();
            foreach (var item in list)
            {
                var temp = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Entities.Form>(item);
                var menuItem = AutoMapper.Mapper.Map<Models.Form.MenuItem>(temp);
                result.Add(menuItem);
            }
            return result;
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

        public void Remove(string id)
        {
            var r = this.Forms.DeleteOne(m => m.ID == id);
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

        public void SaveFlowChart(string id, Models.Form.FlowChart model)
        {
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            var nodes = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Nodes), model.Nodes);
            var lines = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Lines), model.Lines);
            var areas = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Areas), model.Areas);
            var set = Builders<Entities.Form>.Update.Combine(nodes, lines, areas);
            this.Forms.UpdateOne(filter, set, UpdateOptions);
        }

        public Models.Form.FlowChart GetFlowChart(string id)
        {
            var entity = this.Find(id);
            if (entity == null)
                return null;
            var model = new Models.Form.FlowChart()
            {
                Nodes = new Dictionary<string, Models.Form.Node>(),
                Lines = new Dictionary<string, Models.Form.Line>(),
                Areas = new Dictionary<string, Models.Form.Area>()
            };
            AutoMapper.Mapper.Map(entity.Nodes, model.Nodes);
            AutoMapper.Mapper.Map(entity.Lines, model.Lines);
            AutoMapper.Mapper.Map(entity.Areas, model.Areas);
            return model;
        }

        public void SaveColumns(string id, List<Dictionary<string, object>> list)
        {
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            var set = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Columns), list);
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
            result.Data = collection.Sort(pageParam).Pagination(pageParam).ToList();
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
                list.Add(Builders<Entities.Form>.Filter.Regex(nameof(Entities.Form.Name), Helper.FormatQueryString(condition.Name)));
            return list;
        }

    }
}
