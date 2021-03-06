using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TinyUrl.Models;

namespace TinyUrl.Services
{
    public class TinyUrlMongoDBClient: ITinyUrlMongoDBClient
    {
        private readonly IMongoCollection<UrlModel> _urlsCollection;

        public TinyUrlMongoDBClient(
            IOptions<UrlsDatabaseSettings> urlsDatabaseSettings)
        {
            MongoClient? mongoClient = new MongoClient(
                urlsDatabaseSettings.Value.ConnectionString);


            IMongoDatabase? mongoDatabase = mongoClient.GetDatabase(
                urlsDatabaseSettings.Value.DatabaseName);

            _urlsCollection = mongoDatabase.GetCollection<UrlModel>(
                urlsDatabaseSettings.Value.UrlsCollectionName);
        }


        public async Task<UrlModel?> GetAsync(UrlModel tinyUrl)
        {
            return await _urlsCollection.Find(x => x.TinyUrl == tinyUrl.TinyUrl).FirstOrDefaultAsync();
        }

        public async Task UpsertAsync(UrlModel entity)
        {
            FilterDefinition<UrlModel>? filter = new FilterDefinitionBuilder<UrlModel>().Where(m => m.OriginalUrl == entity.OriginalUrl);
            UpdateDefinition<UrlModel>? update = new UpdateDefinitionBuilder<UrlModel>().Set(m => m.TinyUrl, entity.TinyUrl);
            UpdateOneModel<UrlModel>? request = new UpdateOneModel<UrlModel>(filter, update);
            request.IsUpsert = true;
            await _urlsCollection.BulkWriteAsync(new List<UpdateOneModel<UrlModel>>() { request });
        }

        //public async Task UpsertAsync(UrlModel entity)
        //{
        //    await _urlsCollection.ReplaceOneAsync(doc => doc.OriginalUrl == entity.OriginalUrl, entity, new ReplaceOptions { IsUpsert = true });
        //}
    }
}
