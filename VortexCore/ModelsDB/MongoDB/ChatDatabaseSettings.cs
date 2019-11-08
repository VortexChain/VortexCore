using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VortexCore.ModelsDB.MongoDB
{
    public interface IChatDatabaseSettings
    {
        public string ChatCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public class ChatDatabaseSettings : IChatDatabaseSettings
    {
        public string ChatCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
