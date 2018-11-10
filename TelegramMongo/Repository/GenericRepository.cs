using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace TelegramMongo.Repository
{
    public class GenericRepository<T>
    {
        private const string DatabaseName = "bookshelf-db";
        private readonly MongoClient _mongoClient;

        public GenericRepository()
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(PrivateTokens.MongoDbConnectionString));
            settings.SslSettings = new SslSettings {EnabledSslProtocols = SslProtocols.Tls12};
            _mongoClient = new MongoClient(settings);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            var database = _mongoClient.GetDatabase(DatabaseName);
            var collection = database.GetCollection<T>(typeof(T).Name);

            return (await collection.FindAsync(_ => true)).ToList();
        }

        public async Task<IEnumerable<T>> ReadAsync(Expression<Func<T, bool>> filter)
        {
            var database = _mongoClient.GetDatabase(DatabaseName);
            var collection = database.GetCollection<T>(typeof(T).Name);

            return (await collection.FindAsync<T>(filter)).ToList();
        }

        public async Task AddAsync(T item)
        {
            var database = _mongoClient.GetDatabase(DatabaseName);
            var collection = database.GetCollection<T>(typeof(T).Name);

            await collection.InsertOneAsync(item);
        }

        public async Task ReplaceAsync(Expression<Func<T, bool>> filter, T item)
        {
            var database = _mongoClient.GetDatabase(DatabaseName);
            var collection = database.GetCollection<T>(typeof(T).Name);

            await collection.ReplaceOneAsync(filter, item);
        }

        public async Task<int> RemoveAsync(Expression<Func<T, bool>> filter)
        {
            var database = _mongoClient.GetDatabase(DatabaseName);
            var collection = database.GetCollection<T>(typeof(T).Name);

            return (int) (await collection.DeleteManyAsync(filter)).DeletedCount;
        }
    }
}