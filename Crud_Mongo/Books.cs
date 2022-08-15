using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Crud_Mongo
{
    public class Books
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Title { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public int? Pages { get; set; }
        public int? Year { get; set; }

    }
}
