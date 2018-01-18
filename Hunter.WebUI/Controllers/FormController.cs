using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hunter.WebUI.Controllers
{
    public class FormController : Controller
    {
        public FormController(MongoClient mongoClient)
        {
            this.MongoClient = mongoClient;
        }

        protected MongoClient MongoClient { get; private set; }

        [HttpGet]
        public IActionResult Design(string id)
        {
            BsonDocument dictionary = null;
            if (String.IsNullOrWhiteSpace(id))
            {
                dictionary = new BsonDocument()
                {
                    { "_id", ObjectId.GenerateNewId() }
                };
            }
            else
            {
                var db = this.MongoClient.GetDatabase("admin");
                var form = db.GetCollection<BsonDocument>("form");
                var objectID = new ObjectId(id);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", objectID);
                dictionary = form.FindSync<BsonDocument>(filter).FirstOrDefault();
            }
            return this.View(dictionary);
        }

        [HttpPost]
        public IActionResult Design(string id, string html)
        {
            var db = this.MongoClient.GetDatabase("admin");
            var form = db.GetCollection<BsonDocument>("form");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            var dictionary = form.FindSync<BsonDocument>(filter).FirstOrDefault();
            if (dictionary == null)
            {
                dictionary = new BsonDocument()
                {
                    { "_id", new ObjectId(id) },
                    { "html", html },
                };
                form.InsertOne(dictionary);
            }
            else
            {
                dictionary["html"] = html;
                var set = Builders<BsonDocument>.Update.Set("html", html);
                form.UpdateOne(filter, set);
            }
            return this.View(dictionary);
        }

        
        [HttpGet]
        public IActionResult Fill(string id, string dataID)
        {
            var db = this.MongoClient.GetDatabase("admin");
            var forms = db.GetCollection<BsonDocument>("form");
            var fills = db.GetCollection<BsonDocument>("F" + id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            var form = forms.FindSync<BsonDocument>(filter).First();
            if (String.IsNullOrWhiteSpace(dataID))
            {
                form["data"] = new BsonDocument()
                {
                    { "_id", ObjectId.GenerateNewId() }
                };
            }
            else
            {
                filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(dataID));
                form["data"] = fills.FindSync<BsonDocument>(filter).First();
            }
            return this.View(form);
        }

        [HttpPost]
        public IActionResult Fill(string id, string dataID, [FromBody]Dictionary<string, object> dictionary)
        {
            var db = this.MongoClient.GetDatabase("admin");
            var forms = db.GetCollection<BsonDocument>("form");
            var fills = db.GetCollection<BsonDocument>("F" + id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            var form = forms.FindSync<BsonDocument>(filter).First();
            filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(dataID));
            var data = fills.FindSync<BsonDocument>(filter).FirstOrDefault();
            if (data == null)
            {
                dictionary["_id"] = new ObjectId(dataID);
                fills.InsertOne(new BsonDocument(dictionary));
            }
            else
            {
                var set = Builders<BsonDocument>.Update.Set("_", "");
                foreach (var item in data)
                {
                    set = set.Set(item.Name, item.Value);
                }
                fills.UpdateOne(filter, set);
            }
            return this.Ok();
        }

        protected Dictionary<string, object> GetObject()
        {
            //this.Request.Body.Seek(0, System.IO.SeekOrigin.Begin);
            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
            var jsonTextReader = new Newtonsoft.Json.JsonTextReader(new System.IO.StreamReader(this.Request.Body));
            var result = jsonSerializer.Deserialize<Dictionary<string, object>>(jsonTextReader);
            return result;
        }

    }
}
