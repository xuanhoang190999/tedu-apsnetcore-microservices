using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Infrastructure.Extensions;
using MongoDB.Driver;
using Shared.Configurations;
using System.Linq.Expressions;

namespace Infrastructure.Common
{
    public class MongoDbRepository<T> : IMongoDbRepositoryBase<T> where T : MongoEntity
    {
        private IMongoDatabase Database { get; }

        public MongoDbRepository(IMongoClient client, MongoDBSettings settings)
        {
            Database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<T> FindAll(ReadPreference readPreference = null)
             => Database.WithReadPreference(readPreference ?? ReadPreference.Primary)
                .GetCollection<T>(GetCollectionName());

        protected virtual IMongoCollection<T> Collection =>
            Database.GetCollection<T>(GetCollectionName());

        public Task CreateAsync(T entity) => Collection.InsertOneAsync(entity);

        public Task UpdateAsync(T entity)
        {
            Expression<Func<T, string>> func = f => f.Id; // Kiểm tra object đó có tồn tại hay không
            var value = (string)entity.GetType()
                .GetProperty(func.Body.ToString()
                    .Split(".")[1])?.GetValue(entity, null);
            var fitler = Builders<T>.Filter.Eq(func, value);

            return Collection.ReplaceOneAsync(fitler, entity);
        }

        public Task DeleteAsync(string id) => Collection.DeleteOneAsync(id);

        private static string GetCollectionName()
        {
            return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as
                BsonCollectionAttribute)?.CollectionName;
        }
    }
}
