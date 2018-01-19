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
                dataID = ObjectId.GenerateNewId().ToString();
            }
            else
            {
            }
            this.ViewData["id"] = id;
            this.ViewData["dataID"] = dataID;
            return this.View(form);
        }

        public IActionResult GetData(string id, string dataID)
        {
            var db = this.MongoClient.GetDatabase("admin");
            var fills = db.GetCollection<BsonDocument>("F" + id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(dataID));
            var data = fills.FindSync<BsonDocument>(filter).FirstOrDefault();
            return this.Json(data?.ToDictionary());
        }

        [HttpPost]
        public IActionResult Fill(string id, string dataID, [FromBody]Dictionary<string, object> dictionary)
        {
            var db = this.MongoClient.GetDatabase("admin");
            var fills = db.GetCollection<BsonDocument>("F" + id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(dataID));
            var data = fills.FindSync<BsonDocument>(filter).FirstOrDefault();
            if (data == null)
            {
                dictionary["_id"] = new ObjectId(dataID);
                fills.InsertOne(new BsonDocument(dictionary));
            }
            else
            {
                var sets = new List<UpdateDefinition<BsonDocument>>();
                foreach (var item in dictionary)
                {
                    sets.Add(Builders<BsonDocument>.Update.Set(item.Key, item.Value));
                }
                fills.UpdateOne(filter, Builders<BsonDocument>.Update.Combine(sets));
            }
            return this.Ok();
        }

    }
}
