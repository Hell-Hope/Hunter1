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
            var entity = new Entities.Form()
            {
                Nodes = new List<Entities.Node>(),
                Lines = new List<Entities.Line>(),
                Areas = new List<Entities.Area>()
            };
            Convert(model, entity);
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            var nodes = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Nodes), entity.Nodes);
            var lines = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Lines), entity.Lines);
            var areas = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Areas), entity.Areas);
            var set = Builders<Entities.Form>.Update.Combine(nodes, lines, areas);
            this.Forms.UpdateOne(filter, set, UpdateOptions);
        }

        private static void Convert(Models.Form.FlowChart model, Entities.Form entity)
        {
            if (model == null)
                return;

            if (model.Nodes != null)
            {
                foreach (var item in model.Nodes)
                {
                    var temp = AutoMapper.Mapper.Map<Entities.Node>(item.Value);
                    temp.ID = item.Key;
                    entity.Nodes.Add(temp);
                }
            }
            if (model.Lines != null)
            {
                foreach (var item in model.Lines)
                {
                    var temp = AutoMapper.Mapper.Map<Entities.Line>(item.Value);
                    temp.ID = item.Key;
                    entity.Lines.Add(temp);
                }
            }
            if (model.Areas != null)
            {
                foreach (var item in model.Areas)
                {
                    var temp = AutoMapper.Mapper.Map<Entities.Area>(item.Value);
                    temp.ID = item.Key;
                    entity.Areas.Add(temp);
                }
            }

        }

        public Models.Form.FlowChart GetFlowChart(string id)
        {
            var entity = this.Find(id);
            return this.Convert(entity);
        }

        public Models.Form.FlowChart Convert(Entities.Form entity)
        {
            if (entity == null)
                return null;
            var model = new Models.Form.FlowChart()
            {
                Nodes = new Dictionary<string, Models.Form.Node>(),
                Lines = new Dictionary<string, Models.Form.Line>(),
                Areas = new Dictionary<string, Models.Form.Area>()
            };
            if (entity.Nodes != null)
            {
                foreach (var item in entity.Nodes)
                {
                    model.Nodes[item.ID] = AutoMapper.Mapper.Map<Models.Form.Node>(item);
                }
            }
            if (entity.Lines != null)
            {
                foreach (var item in entity.Lines)
                {
                    model.Lines[item.ID] = AutoMapper.Mapper.Map<Models.Form.Line>(item);
                }
            }
            if (entity.Areas != null)
            {
                foreach (var item in entity.Areas)
                {
                    model.Areas[item.ID] = AutoMapper.Mapper.Map<Models.Form.Area>(item);
                }
            }
            return model;
        }

        public void SaveColumns(string id, List<Dictionary<string, object>> list)
        {
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            var set = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Columns), list);
            this.Forms.UpdateOne(filter, set, UpdateOptions);
        }

        protected List<Entities.Form.Field> ParseHtml(string html)
        {
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);
            var result = new HashSet<Entities.Form.Field>();
            ParseHtml(htmlDocument.DocumentNode, result);
            return result.ToList();
        }

        protected void ParseHtml(HtmlAgilityPack.HtmlNode htmlNode, ICollection<Entities.Form.Field> fields)
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
                    var temp = new Entities.Form.Field()
                    {
                        Name = name,
                        Type = childNode.Attributes["type"]?.Value
                    };
                    fields.Add(temp);
                }
                else if (childNode.Name == "select")
                {
                    var name = childNode.Attributes["name"]?.Value;
                    if (String.IsNullOrWhiteSpace(name))
                        continue;
                    var temp = new Entities.Form.Field()
                    {
                        Name = name,
                        Type = "select"
                    };
                    fields.Add(temp);
                }
                else if (childNode.Name == "textarea")
                {
                    var name = childNode.Attributes["name"]?.Value;
                    if (String.IsNullOrWhiteSpace(name))
                        continue;
                    var temp = new Entities.Form.Field()
                    {
                        Name = name,
                        Type = "textarea"
                    };
                    fields.Add(temp);
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
