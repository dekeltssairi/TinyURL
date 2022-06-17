using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TinyUrl.Models
{
    public class UrlModel
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public Uri? OriginalUrl { get; set; }

    }
}
