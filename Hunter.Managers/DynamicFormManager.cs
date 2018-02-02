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
        public DynamicFormManager(MongoClient mongoClient) : base(mongoClient)
        {
        }

        public IMongoCollection<Entities.DynamicForm> DynamicForms(string formID)
        {
            return this.DefaultDatabase.GetCollection<Entities.DynamicForm>("Dynamic" + formID);
        }

        public Entities.DynamicForm Find(string formID, string id)
        {
            var filter = this.BuildFilterEqualID<Entities.DynamicForm>(id);
            return this.DynamicForms(formID).Find(filter).FirstOrDefault();
        }

        public Models.Result SaveData(string formID, string dataID, Dictionary<string, object> dictionary)
        {
            var entity = this.Find(formID, dataID);
            if (entity == null)
            {
                var form = this.FormManager.Find(formID);
                entity = new Entities.DynamicForm() { ID = dataID, Data = new Dictionary<string, object>() };
                form.CopyTo(entity);
                entity.CurrentNode = entity.Nodes.GetStartNode();
                this.DynamicForms(formID).ReplaceOne(m => m.ID == dataID, entity, UpdateOptions);
            }
            var data = entity.Data;
            if (data == null)
            {
                data = new Dictionary<string, object>();
            }
            var fields = entity.CurrentNode.Fields;
            if (fields != null)
            {
                foreach (var field in fields)
                {
                    if (dictionary.TryGetValue(field, out object value))
                        data[field] = value;
                }
            }
            else
            {
                foreach (var item in dictionary)
                {
                    data[item.Key] = item.Value;
                }
            }
            var filter = this.BuildFilterEqualID<Entities.DynamicForm>(dataID);
            var set = Builders<Entities.DynamicForm>.Update.Set(nameof(Entities.DynamicForm.Data), data);
            this.DynamicForms(formID).UpdateOne(filter, set, UpdateOptions);

            return Models.Result.Create();
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
            return list;
        }

        public Models.Result Next(string formID, string dataID, string lineID)
        {
            var entity = this.Find(formID, dataID);
            if (entity == null)
                return Models.Result.Create(Models.Code.NotFound, "没找到数据");
            if (entity.Finish)
                return Models.Result.Create(Models.Code.Fail, "已结束");
            var line = entity.Lines.Where(l => l.ID == lineID && l.From == entity.CurrentNode.ID).FirstOrDefault();
            if (line == null)
                return Models.Result.Create(Models.Code.NotFound, "没找到线数据");
            var node = entity.Nodes.Where(n => n.ID == line.To).FirstOrDefault();
            if (node == null)
                return Models.Result.Create(Models.Code.NotFound, "没找到下一个节点数据");
            var filter = this.BuildFilterEqualID<Entities.DynamicForm>(dataID);
            var set = Builders<Entities.DynamicForm>.Update.Set(nameof(Entities.DynamicForm.CurrentNode), node);
            this.DynamicForms(formID).UpdateOne(filter, set, UpdateOptions);
            return Models.Result.Create();
        }

        public Models.Result Finish(string formID, string dataID)
        {
            var entity = this.Find(formID, dataID);
            if (entity == null)
                return Models.Result.Create(Models.Code.NotFound, "没找到数据");
            if (entity.Finish)
                return Models.Result.Create(Models.Code.Fail, "已结束");
            if (entity.CurrentNode.IsEndType)
                return Models.Result.Create(Models.Code.Fail, "改节点不是结束节点");
            var filter = this.BuildFilterEqualID<Entities.DynamicForm>(dataID);
            var set = Builders<Entities.DynamicForm>.Update.Set(nameof(Entities.DynamicForm.Finish), true);
            this.DynamicForms(formID).UpdateOne(filter, set, UpdateOptions);
            return Models.Result.Create();
        }

        /*
        public void ParseHTML(iTextSharp.text.Document doc, System.IO.Stream stream, string html)
        {
            var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, stream);
            var xmlWorkerHelper = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
            var cssResolver = new iTextSharp.tool.xml.css.StyleAttrCSSResolver();
            var xmlWorkerFontProvider = new iTextSharp.tool.xml.XMLWorkerFontProvider();
            xmlWorkerFontProvider.RegisterFamily("宋体", "simsun", "C:/Windows/Fonts/simsun.ttc,0");
            var cssAppliers = new iTextSharp.tool.xml.html.CssAppliersImpl(xmlWorkerFontProvider);
            var htmlContext = new iTextSharp.tool.xml.pipeline.html.HtmlPipelineContext(cssAppliers);
            htmlContext.SetTagFactory(iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory());
            var pdfWriterPipeline = new iTextSharp.tool.xml.pipeline.end.PdfWriterPipeline(doc, writer);
            var htmlPipeline = new iTextSharp.tool.xml.pipeline.html.HtmlPipeline(htmlContext, pdfWriterPipeline);
            var cssResolverPipeline = new iTextSharp.tool.xml.pipeline.css.CssResolverPipeline(cssResolver, htmlPipeline);
            var xmlWorker = new iTextSharp.tool.xml.XMLWorker(cssResolverPipeline, true);
            var xmlParser = new iTextSharp.tool.xml.parser.XMLParser(xmlWorker);
            using (var htmlReader = new System.IO.StringReader(html))
            {
                xmlParser.Parse(htmlReader);
            }
        }

        public void ParseHTML(System.IO.Stream stream, string html)
        {
            var doc = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(0, 0, 1000, 800));
            doc.Open();
            this.ParseHTML(doc, stream, html);
            doc.Close();
        }

        public System.IO.MemoryStream ParseHTML(string html)
        {
            var stream = new System.IO.MemoryStream();
            var doc = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(0, 0, 1000, 800));
            doc.Open();
            this.ParseHTML(doc, stream, html);
            doc.Close();
            return stream;
        }
        */

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
