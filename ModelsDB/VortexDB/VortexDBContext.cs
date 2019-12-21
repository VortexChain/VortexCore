using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VortexCore.ModelsDB.VortexDB
{
    public partial class VortexDBContext : DbContext
    {
        public VortexDBContext()
        {
        }

        public VortexDBContext(DbContextOptions<VortexDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<NotificationToken> NotificationTokens { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<SshUser> SshUsers { get; set; }
        public virtual DbSet<SshServer> SshServers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
