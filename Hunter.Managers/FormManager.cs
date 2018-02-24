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
        internal FormManager(Shared shared) : base(shared)
        {
        }

        public IMongoCollection<Entities.Form> Collection
        {
            get
            {
                return this.DefaultDatabase.GetCollection<Entities.Form>(nameof(Entities.Form));
            }
        }

        public Entities.Form Find(string id)
        {
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            return this.Collection.Find(filter).FirstOrDefault();
        }

        public List<Models.Form.MenuItem> GetMenuItems()
        {
            var id = Builders<Entities.Form>.Projection.Include(nameof(Entities.Form.ID));
            var name = Builders<Entities.Form>.Projection.Include(nameof(Entities.Form.Name));
            var projection = Builders<Entities.Form>.Projection.Combine(id, name);
            var list = this.Collection.Find(Builders<Entities.Form>.Filter.Empty).Project(projection).ToList();
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

        public Models.Form.Design GetDesign(string id)
        {
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            var projections = new ProjectionDefinition<Entities.Form>[]
            {
                Builders<Entities.Form>.Projection.Include(nameof(Entities.Form.ID)),
                Builders<Entities.Form>.Projection.Include(nameof(Entities.Form.Html))
            };
            var projection = Builders<Entities.Form>.Projection.Combine(projections);
            var entity = this.Collection.Find(filter).Project(projection).As<Entities.Form>().FirstOrDefault();
            if (entity == null)
                return null;
            return AutoMapper.Mapper.Map<Models.Form.Design>(entity);
        }

        public Models.Form.FlowChart GetFlowChart(string id)
        {
            var entity = this.Find(id);
            return AutoMapper.Mapper.Map<Models.Form.FlowChart>(entity);
        }

        public List<Models.Form.Field> GetFields(string id)
        {
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            var projections = new ProjectionDefinition<Entities.Form>[]
            {
                Builders<Entities.Form>.Projection.Include(nameof(Entities.Form.Fields))
            };
            var projection = Builders<Entities.Form>.Projection.Combine(projections);
            var entity = this.Collection.Find(filter).Project(projection).As<Entities.Form>().FirstOrDefault();
            if (entity == null)
                return null;
            return AutoMapper.Mapper.Map<List<Models.Form.Field>>(entity.Fields);
        }

        public List<Models.Form.Column> GetColumns(string id)
        {
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            var projections = new ProjectionDefinition<Entities.Form>[]
            {
                Builders<Entities.Form>.Projection.Include(nameof(Entities.Form.Columns))
            };
            var projection = Builders<Entities.Form>.Projection.Combine(projections);
            var entity = this.Collection.Find(filter).Project(projection).As<Entities.Form>().FirstOrDefault();
            if (entity == null)
                return null;
            return AutoMapper.Mapper.Map<List<Models.Form.Column>>(entity.Columns);
        }

        public void Save(Models.Form.Edit edit)
        {
            var entity = this.Find(edit.ID);
            if (entity == null)
                entity = AutoMapper.Mapper.Map<Entities.Form>(edit);
            else
                AutoMapper.Mapper.Map(edit, entity);
            entity.Fields = this.ParseHtml(edit.Html);
            this.Collection.ReplaceOne(m => m.ID == edit.ID, entity, UpdateOptions);
        }

        public void Remove(string id)
        {
            var r = this.Collection.DeleteOne(m => m.ID == id);
        }

        /// <summary> 保存Html数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public void Save(Models.Form.Design design)
        {
            var fields = ParseHtml(design.Html);
            var filter = this.BuildFilterEqualID<Entities.Form>(design.ID);
            var setHtml = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Html), design.Html);
            var setFields = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Fields), fields);
            var set = Builders<Entities.Form>.Update.Combine(setHtml, setFields);
            this.Collection.UpdateOne(filter, set, UpdateOptions);
        }

        public void Save(Models.Form.FlowChart model)
        {
            var entity = AutoMapper.Mapper.Map<Entities.Form>(model);
            var filter = this.BuildFilterEqualID<Entities.Form>(model.ID);
            var nodes = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Nodes), entity.Nodes);
            var lines = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Lines), entity.Lines);
            var areas = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Areas), entity.Areas);
            var set = Builders<Entities.Form>.Update.Combine(nodes, lines, areas);
            this.Collection.UpdateOne(filter, set, UpdateOptions);
        }

        public void SaveColumns(string id, List<Models.Form.Column> list)
        {
            var filter = this.BuildFilterEqualID<Entities.Form>(id);
            var set = Builders<Entities.Form>.Update.Set(nameof(Entities.Form.Columns), list);
            this.Collection.UpdateOne(filter, set, UpdateOptions);
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

        public Models.PageResult<Models.Form.QueryResultData> Query(Models.PageParam<Models.Form.Condition> pageParam)
        {
            var filter = this.BuildFilter(pageParam.Condition);
            var collection = this.Collection.Find(filter);

            var result = new Models.PageResult<Models.Form.QueryResultData>();
            result.Total = collection.Count();
            var projections = new ProjectionDefinition<Entities.Form>[]
            {
                Builders<Entities.Form>.Projection.Include(nameof(Entities.Form.ID)),
                Builders<Entities.Form>.Projection.Include(nameof(Entities.Form.Name))
            };
            var projection = Builders<Entities.Form>.Projection.Combine(projections);
            var list = collection.Sort(pageParam).Pagination(pageParam).Project(projection).As<Entities.Form>().ToList();
            result.Data = AutoMapper.Mapper.Map<List<Models.Form.QueryResultData>>(list);
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
