using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCVWebb.Library.Data
{
    
    public class MongoDb<T>
    {
        public IMongoDatabase db;
        public MongoDb(string database)
        {
            var connectionString = Environment.GetEnvironmentVariable("MyCvMongoAzure");
            var client = new MongoClient(connectionString);
            db = client.GetDatabase(database);
        }
        //C
        public async Task<T> AddAsync(string table, T t)
        {
            var collection = db.GetCollection<T>(table);
            await collection.InsertOneAsync(t);
            return t;
        }

        //R all
        public async Task<IEnumerable<T>> GetAllAsync(string table)
        {
            var collection = db.GetCollection<T>(table);
            var list = await collection.Find(x => true).ToListAsync();
            return list;
        }


        //R by ID
        public async Task<T> GetByIDAsync<T>(string id, string table) where T : class
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            var result = await collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        //U

        public async Task<T> UpdateAsync<T>(string id, string table, object updates) where T : class
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                var updateDefinition = Builders<T>.Update.Combine(BuildUpdateDefinition<T>(updates));
                await collection.UpdateOneAsync(filter, updateDefinition);
            }

            return result;
        }

        private UpdateDefinition<T> BuildUpdateDefinition<T>(object updates)
        {
            var updateDefinitionBuilder = Builders<T>.Update;
            var properties = updates.GetType().GetProperties();

            var updateDefinition = updateDefinitionBuilder.Combine(
                properties.Select(prop =>
                {
                    var propName = prop.Name;
                    var propValue = prop.GetValue(updates);

                    return updateDefinitionBuilder.Set(propName, propValue);
                })
            );

            return updateDefinition;
        }

        //D
        public async Task<T> DeleteAsync<T>(string table, string id) where T : class
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            var entityToDelete = await collection.Find(filter).FirstOrDefaultAsync();

            if (entityToDelete != null)
            {
                await collection.DeleteOneAsync(filter);
            }

            return entityToDelete;
        }
    }
}
