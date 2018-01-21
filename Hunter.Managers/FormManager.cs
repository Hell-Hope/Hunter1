using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

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

    }
}
