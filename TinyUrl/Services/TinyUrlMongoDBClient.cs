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

        public async Task UpsertManyAsync(IEnumerable<UrlModel> urlModels)
        {
            List<UpdateOneModel<UrlModel>> requests = new List<UpdateOneModel<UrlModel>>(urlModels.Count());
            foreach (var entity in urlModels)
            {
                var filter = new FilterDefinitionBuilder<UrlModel>().Where(m => m.OriginalUrl == entity.OriginalUrl);
                var update = new UpdateDefinitionBuilder<UrlModel>().Set(m => m.TinyUrl, entity.TinyUrl);
                var request = new UpdateOneModel<UrlModel>(filter, update);
                request.IsUpsert = true;
                requests.Add(request);
            }

            await _urlsCollection.BulkWriteAsync(requests);
        }
    }
}
