using System;
using System.Collections.Generic;

namespace VortexCore.ModelsDB
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public int LastLoginId { get; set; }
        public int? NotificatonTokenId { get; set; }

        public virtual UserLogins LastLogin { get; set; }
        public virtual NotificationTokens NotificatonToken { get; set; }
    }
}
