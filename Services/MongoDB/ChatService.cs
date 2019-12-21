using VortexCore.ModelsDB.MongoDB;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace VortexCore.Services.MongoDB
{
    public class ChatService
    {
        private readonly IMongoCollection<ChatMessage> _messages;
        private readonly IMongoCollection<ChatUser> _users;
        public ChatService(IChatDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _messages = database.GetCollection<ChatMessage>("ChatMessages");
            _users = database.GetCollection<ChatUser>("ChatUsers");

        }

        public List<ChatMessage> GetMessages()
        {
            return _messages.Find(message => true).ToList();
        }

        public List<string> GetUidsFromMessages()
        {
            return _messages.Distinct(new StringFieldDefinition<ChatMessage, string>("UserId"), FilterDefinition<ChatMessage>.Empty).ToList();
        }

        public ChatMessage AddMessage(ChatMessage message)
        {
            _messages.InsertOne(message);
            return message;
        }

        /// <summary>
        /// Add user to DB
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="user"></param>
        /// <returns>Return true if user has been attached</returns>
        public bool AttachUserToChat(int chatId, DtoClasses.User user)
        {
            var existUser = _users.Find(chatUser => chatUser.ChatId == chatId && chatUser.Uid == user.Uid).FirstOrDefault();
            if(existUser == null)
            {
                _users.InsertOne(new ChatUser
                {
                    ChatId = chatId,
                    Uid = user.Uid,
                    User = user
                });
                return true;
            }
            return false;
        }
    
        public List<ChatUser> GetChatUsers(int chatId)
        {
            return _users.Find(user => user.ChatId == chatId).ToList();
        }
    }
}
