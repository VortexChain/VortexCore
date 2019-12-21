using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VortexCore.ModelsDB.MongoDB
{
    public class ChatMessage
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int ChatId { get; set; } = 0;
        public string MessageText { get; set; }
        public string UserId { get; set; }
    }
}
