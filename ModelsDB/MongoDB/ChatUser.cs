using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VortexCore.DtoClasses;

namespace VortexCore.ModelsDB.MongoDB
{
    public class ChatUser
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int ChatId { get; set; } = 0;
        public string Uid { get; set; }
        public User User { get; set; }
    }
}
