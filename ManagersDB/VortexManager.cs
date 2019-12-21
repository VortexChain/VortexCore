using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VortexCore.ModelsDB.VortexDB;

namespace VortexCore.ManagersDB
{
    public class VortexManager
    {
        private VortexDBContext context;
        public VortexManager(VortexDBContext context)
        {
            this.context = context;
        }

        public bool AddNotificationToken(string token)
        {
            try
            {
                context.NotificationTokens.Add(new NotificationToken() { Token = token });
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public SshServer GetSshServer(int id)
        {
            return context.SshServers.Find(id);
        }

        public List<SshServer> GetSshServers()
        {
            return context.SshServers.ToList();
        }

        public SshUser GetSshUser(int id)
        {
            return context.SshUsers
                .Include(q => q.Server)
                .First(user => user.Id == id);
        }

        public List<SshUser> GetSshUsers(string uid)
        {
            return context.SshUsers.Include(q => q.Server).Where(user => user.UserId == uid).ToList();
        }

        public bool AddSshUser(SshUser sshUser)
        {
            try
            {
                context.SshUsers.Add(sshUser);
                context.SaveChanges();
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }

        public bool UsernameAvaible(string username, int serverId)
        {
            var user = context.SshUsers.Where(user => user.ServerId == serverId).FirstOrDefault(user => user.Username == username);
            if (user == null) return true;
            return false;
        }
    }
}
