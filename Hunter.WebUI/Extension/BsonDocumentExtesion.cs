
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Bson
{
    public static class BsonDocumentExtesion
    {

        public static BsonValue TryGetValue(this BsonDocument that, string name)
        {
            if (that.TryGetValue(name, out BsonValue value))
                return value;
            return BsonUndefined.Value;
        }

        public static string TryToString(this BsonValue that)
        {
            switch (that.BsonType)
            {
                case BsonType.EndOfDocument:
                    return null;
                case BsonType.Double:
                    return that.AsDouble.ToString();
                case BsonType.String:
                    return that.AsString.ToString();
                case BsonType.Document:
                    return that.AsBsonDocument.ToString();
                case BsonType.Array:
                    return that.AsBsonArray.ToJson();
                case BsonType.Binary:
                    return that.AsBsonBinaryData.ToString();
                case BsonType.Undefined:
                    return null;
                case BsonType.ObjectId:
                    return that.AsObjectId.ToString();
                case BsonType.Boolean:
                    return that.AsBoolean.ToString();
                case BsonType.DateTime:
                    return that.AsBsonDateTime.ToString();
                case BsonType.Null:
                    return null;
                case BsonType.RegularExpression:
                    break;
                case BsonType.JavaScript:
                    break;
                case BsonType.Symbol:
                    break;
                case BsonType.JavaScriptWithScope:
                    break;
                case BsonType.Int32:
                    return that.AsInt32.ToString();
                case BsonType.Timestamp:
                    return that.AsBsonTimestamp.ToString();
                case BsonType.Int64:
                    return that.AsInt64.ToString();
                case BsonType.Decimal128:
                    return that.AsDecimal128.ToString();
                case BsonType.MinKey:
                    break;
                case BsonType.MaxKey:
                    break;
            }
            return null;
        }

    }
}
