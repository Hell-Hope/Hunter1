using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Hunter.Managers
{
    public class DynamicFormManager : Manager
    {
        internal DynamicFormManager(Shared shared) : base(shared)
        {
        }

        public IMongoCollection<Entities.DynamicForm> DynamicForms(string formID)
        {
            return this.DefaultDatabase.GetCollection<Entities.DynamicForm>("Dynamic" + formID);
        }

        public Entities.DynamicForm Find(string formID, string dataID)
        {
            var filter = this.BuildFilterEqualID<Entities.DynamicForm>(dataID);
            return this.DynamicForms(formID).Find(filter).FirstOrDefault();
        }

        public Models.Result HasPermit(Entities.DynamicForm entity)
        {
            if (this.ApplicationUser.HasPermit(entity.CurrentNode.Permits) == false)
                return Models.Result.CreateForbidden();
            return Models.Result.Create();
        }

        public Models.Result Create(string formID, string dataID)
        {
            var form = this.FormManager.Find(formID);
            var entity = new Entities.DynamicForm() { ID = dataID, Data = new Dictionary<string, object>() };
            this.Copy(form, entity);
            entity.CurrentNode = entity.Nodes.GetStartNode();
            entity.CreatedUserID = this.ApplicationUser.ID;
            entity.CreatedUserName = this.ApplicationUser.Name;
            var result = this.HasPermit(entity);
            if (result.Success == false)
                return result;
            this.DynamicForms(formID).ReplaceOne(m => m.ID == dataID, entity, UpdateOptions);
            return Models.Result.CreateDataResult(entity);
        }

        public Models.Result SaveData(string formID, string dataID, Dictionary<string, object> dictionary)
        {
            Models.Result result = null;
            var entity = this.Find(formID, dataID);
            if (entity == null)
            {
                result = this.Create(formID, dataID);
                if (result is Models.DataResult<Entities.DynamicForm> temp)
                    entity = temp.Data;
                else
                    return result;
            }
            else
            {
                result = this.HasPermit(entity);
                if (result.Success == false)
                    return result;
            }
            var data = entity.Data ?? new Dictionary<string, object>();
            var fields = entity.CurrentNode.Fields ?? new List<string>();
            foreach (var field in fields)
            {
                if (dictionary.TryGetValue(field, out object value))
                    data[field] = value;
            }
            var filter = this.BuildFilterEqualID<Entities.DynamicForm>(dataID);
            var set = Builders<Entities.DynamicForm>.Update.Set(nameof(Entities.DynamicForm.Data), data);
            this.DynamicForms(formID).UpdateOne(filter, set, UpdateOptions);
            return Models.Result.Create();
        }

        public void Copy(Entities.Form source, Entities.DynamicForm destination)
        {
            destination.Html = (source?.Html) ?? String.Empty;
            destination.Nodes = AutoMapper.Mapper.Map<List<Entities.DynamicForm.Node>>(source.Nodes);
            destination.Lines = AutoMapper.Mapper.Map<List<Entities.DynamicForm.Line>>(source.Lines);
            destination.Areas = AutoMapper.Mapper.Map<List<Entities.DynamicForm.Area>>(source.Areas);
        }

        public void Remove(string formID, string dataID)
        {
            var filter = Builders<Entities.DynamicForm>.Filter.Eq("ID", dataID);
            this.DynamicForms(formID).DeleteOne(filter);
        }

        public Models.PageResult<Entities.DynamicForm> Query(string formID, Models.PageParam<Models.DynamicForm.Condition> pageParam)
        {
            var filter = this.BuildFilter(pageParam.Condition);
            var collection = this.DynamicForms(formID).Find(filter);
            var result = new Models.PageResult<Entities.DynamicForm>();
            result.Total = collection.Count();
            result.Data = collection.Sort(pageParam).Pagination(pageParam).ToList();
            return result;
        }

        protected FilterDefinition<Entities.DynamicForm> BuildFilter(Models.DynamicForm.Condition condition)
        {
            return this.BuildFilter(this.BuildFilters(condition));
        }

        protected List<FilterDefinition<Entities.DynamicForm>> BuildFilters(Models.DynamicForm.Condition condition)
        {
            var list = new List<FilterDefinition<Entities.DynamicForm>>();
            if (condition == null)
                return list;
            foreach (var item in condition)
            {
                var array = item.Key.Split('$');
                if (array.Length != 2 || item.Value == null || String.Empty.Equals(item.Value))
                    continue;
                var field = array[0];
                var comparer = array[1];
                var value = item.Value;
                FilterDefinition<Entities.DynamicForm> filter = null;
                if (comparer == "like")
                    filter = Builders<Entities.DynamicForm>.Filter.Regex(field, Helper.FormatQueryString(value.ToString()));
                else if (comparer == "gt")
                    filter = Builders<Entities.DynamicForm>.Filter.Gt(field, value);
                else if (comparer == "gte")
                    filter = Builders<Entities.DynamicForm>.Filter.Gte(field, value);
                else if (comparer == "lt")
                    filter = Builders<Entities.DynamicForm>.Filter.Lt(field, value);
                else if (comparer == "lte")
                    filter = Builders<Entities.DynamicForm>.Filter.Lte(field, value);
                if (filter != null)
                    list.Add(filter);
            }
            return list;
        }

        /// <summary> 流转到下一个节点
        /// </summary>
        /// <param name="formID"></param>
        /// <param name="dataID"></param>
        /// <param name="lineID"></param>
        /// <returns></returns>
        public Models.Result Next(string formID, string dataID, string lineID)
        {
            var entity = this.Find(formID, dataID);
            if (entity == null)
                return Models.Result.Create(Models.Code.NotFound, "没找到数据");
            if (entity.Finish)
                return Models.Result.Create(Models.Code.Fail, "已结束");
            var result = this.HasPermit(entity);
            if (result.Success == false)
                return result;
            var line = entity.Lines.Where(l => l.ID == lineID && l.From == entity.CurrentNode.ID).FirstOrDefault();
            if (line == null)
                return Models.Result.Create(Models.Code.NotFound, "没找到线数据");
            var node = entity.Nodes.Where(n => n.ID == line.To).FirstOrDefault();
            if (node == null)
                return Models.Result.Create(Models.Code.NotFound, "没找到下一个节点数据");
            var filter = this.BuildFilterEqualID<Entities.DynamicForm>(dataID);
            var set = Builders<Entities.DynamicForm>.Update.Set(nameof(Entities.DynamicForm.CurrentNode), node);
            this.DynamicForms(formID).UpdateOne(filter, set, UpdateOptions);
            this.FlowTraceManager.InsertFlowTrace(entity, lineID);
            return Models.Result.Create();
        }

        /// <summary> 结束流程
        /// </summary>
        /// <param name="formID"></param>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public Models.Result Finish(string formID, string dataID)
        {
            var entity = this.Find(formID, dataID);
            if (entity == null)
                return Models.Result.Create(Models.Code.NotFound, "没找到数据");
            if (entity.Finish)
                return Models.Result.Create(Models.Code.Fail, "已结束");
            if (entity.CurrentNode.IsEndType)
                return Models.Result.Create(Models.Code.Fail, "此节点不是结束节点");
            var result = this.HasPermit(entity);
            if (result.Success == false)
                return result;
            var filter = this.BuildFilterEqualID<Entities.DynamicForm>(dataID);
            var set = Builders<Entities.DynamicForm>.Update.Set(nameof(Entities.DynamicForm.Finish), true);
            this.DynamicForms(formID).UpdateOne(filter, set, UpdateOptions);
            this.FlowTraceManager.InsertFlowTrace(entity, null);
            return Models.Result.Create();
        }

        public Models.DynamicForm.Progress GetProgress(string formID, string dataID)
        {
            var entity = this.Find(formID, dataID);
            var progress = AutoMapper.Mapper.Map<Models.DynamicForm.Progress>(entity);
            return progress;
        }



        public string GetCompleteHtml(string body, Dictionary<string, object> data, string basePath)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(body);
            CompleteHtml(doc.DocumentNode, data, basePath);
            body = doc.DocumentNode.InnerHtml;

            var css = System.IO.File.ReadAllText(System.IO.Path.Combine(basePath, @"Libraries\ckeditor\4.8.0\contents.css"));
            var format = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"" />
    <style>{css}</style>
</head>
<body class=""cke_editable cke_editable_themed cke_contents_ltr cke_show_borders"">
    {body}
</body>
</html>
";
            return format;
        }

        protected void CompleteHtml(HtmlAgilityPack.HtmlNode node, Dictionary<string, object> data, string basePath)
        {
            if (node == null)
                return;
            if (node.Name == "input")
            {
                var type = node.Attributes["type"]?.Value;
                var name = node.Attributes["name"]?.Value;
                if (String.IsNullOrWhiteSpace(name))
                {
                    return;
                }
                if (type == "checkbox")
                {

                }
                else if (type == "radio")
                {

                }
                else
                {
                    var value = Agent.Collection.TryGetClassValue(data, name);
                    node.Attributes.Append("value", Agent.Convert.ToString(value));
                }
            }
            else
            {
                foreach (var item in node.ChildNodes)
                {
                    CompleteHtml(item, data, basePath);
                }
            }
        }

    }
}
