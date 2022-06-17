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

        public async Task<UrlModel?> GetOrigialUrlAsync(Uri TinyUrl)
        {
            throw new NotImplementedException();
        }

        public async Task<UrlModel> InsertAsync(UrlModel urlModel)
        {
            UrlModel? result = await _urlsCollection.Find(u => u.OriginalUrl == urlModel.OriginalUrl).FirstOrDefaultAsync();
            if (result is null)
            {
                await _urlsCollection.InsertOneAsync(urlModel);
                return urlModel;
            }
            
            return result;
        }

        public async Task<UrlModel?> GetAsync(UrlModel tinyUrl)
        {
            return await _urlsCollection.Find(x => x._id == tinyUrl._id).FirstOrDefaultAsync();
        }

        //public async Task<List<Book>> GetAsync() =>
        //    await _booksCollection.Find(_ => true).ToListAsync();

        //public async Task<Book?> GetAsync(string id) =>
        //    await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        //public async Task CreateAsync(Book newBook) =>
        //    await _booksCollection.InsertOneAsync(newBook);

        //public async Task UpdateAsync(string id, Book updatedBook) =>
        //    await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        //public async Task RemoveAsync(string id) =>
        //    await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
