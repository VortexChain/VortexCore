using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VortexCore.ModelsDB.VortexDB
{
    public partial class SshUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public int ServerId { get; set; }
        public virtual SshServer Server { get; set; }
    }
}
