using VortexCore.ModelsDB.MongoDB;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace VortexCore.Services.MongoDB
{
    public class ChatService
    {
        private readonly IMongoCollection<ChatMessage> _messages;
        public ChatService(IChatDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _messages = database.GetCollection<ChatMessage>(settings.ChatCollectionName);
        }

        public List<ChatMessage> GetMessages()
        {
            return _messages.Find(message => true).ToList();
        }

        public ChatMessage AddMessage(ChatMessage message)
        {
            _messages.InsertOne(message);
            return message;
        }
    }
}
