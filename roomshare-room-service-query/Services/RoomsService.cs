using Microsoft.Extensions.Options;
using MongoDB.Driver;
using roomshare_room_service_query.Model;

namespace roomshare_room_service_query.Services
{
    public class RoomsService
    {
        private readonly IMongoCollection<Room> _roomsCollection;

        public RoomsService(
            IOptions<RoomDatabaseSettings> roomstoreDatabaseSettings)
        {

            var mongoClient = new MongoClient(
                roomstoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                roomstoreDatabaseSettings.Value.DatabaseName);

            _roomsCollection = mongoDatabase.GetCollection<Room>(
                roomstoreDatabaseSettings.Value.RoomsCollectionName);
        }

        public async Task<List<Room>> GetAsync() =>
            await _roomsCollection.Find(_ => true).ToListAsync();

        public async Task<Room?> GetAsync(long id) =>
            await _roomsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Room newBook) =>
            await _roomsCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(long id, Room updatedBook) =>
            await _roomsCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(long id) =>
            await _roomsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
