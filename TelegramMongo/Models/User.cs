using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TelegramMongo.Models
{
    public class User
    {
        [BsonId] public ObjectId Id { get; set; }

        public long ChatId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BooksGot { get; set; }
        public DateTime BooksGotDate { get; set; }
    }
}