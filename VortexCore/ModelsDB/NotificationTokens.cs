using System;
using System.Collections.Generic;

namespace VortexCore.ModelsDB
{
    public partial class NotificationTokens
    {
        public NotificationTokens()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Token { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
