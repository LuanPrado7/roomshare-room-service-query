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

        public async Task<List<Room>> GetAsync(Guid locatorKey) =>
            await _roomsCollection.Find(x => x.Locator.guid == locatorKey).ToListAsync();

        public async Task<Room?> GetAsync(long id, Guid locatorKey)
        {
            var filter = Builders<Room>.Filter.Eq(x => x.Id, id);
            filter &= Builders<Room>.Filter.Eq(x => x.Locator.guid, locatorKey);

            return await _roomsCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Room newRoom) => await _roomsCollection.InsertOneAsync(newRoom);        

        public async Task UpdateAsync(long id, Room updatedRoom) =>
            await _roomsCollection.ReplaceOneAsync(x => x.Id == id, updatedRoom);

        public async Task RemoveAsync(long id) =>
            await _roomsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
