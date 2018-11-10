using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TelegramMongo.Models
{
    public class Book
    {
        public Book()
        {
        }

        public Book(string name)
        {
            Name = name;
        }

        [BsonId] public ObjectId Id { get; set; }

        public string Name { get; set; }
    }
}