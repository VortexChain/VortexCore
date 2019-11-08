using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VortexCore.ModelsDB;

namespace VortexCore.ManagersDB
{
    public class VortexManager
    {
        private VortexBDContext context;
        public VortexManager(VortexBDContext context)
        {
            this.context = context;
        }

        public IEnumerable<Users> GetUsers()
        {
            return context.Users;
        }

        public bool AddNotificationToken(string token)
        {
            try
            {
                context.NotificationTokens.Add(new NotificationTokens() { Token = token });
                context.SaveChanges();
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }

        public bool SetNotificationToken(int id, string token)
        {
            try
            {
                var existingToken = context.NotificationTokens.SingleOrDefault(t => t.Token == token);
                if (existingToken != null)
                {
                    context.Users.Find(id).NotificatonToken = existingToken;
                }
                else
                {
                    context.Users.Find(id).NotificatonToken = new NotificationTokens() { Token = token };
                }
                context.SaveChanges();
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }

        public string GetNotificationToken(int id)
        {
            try
            {
                return context.Users.Include(q => q.NotificatonToken).SingleOrDefault(x => x.Id == id).NotificatonToken.Token;
            }
            catch //(Exception ex)
            {
                return null;
            }
        }
    }
}
