namespace roomshare_room_service_query.Model
{
    public class RoomDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string RoomsCollectionName { get; set; } = null!;
    }
}
