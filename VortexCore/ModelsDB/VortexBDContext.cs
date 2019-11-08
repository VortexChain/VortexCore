using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VortexCore.ModelsDB
{
    public partial class VortexBDContext : DbContext
    {
        public VortexBDContext()
        {
        }

        public VortexBDContext(DbContextOptions<VortexBDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<NotificationTokens> NotificationTokens { get; set; }
        public virtual DbSet<UserLogins> UserLogins { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationTokens>(entity =>
            {
                entity.Property(e => e.Token).IsRequired();
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.LastLoginId)
                    .HasName("IX_Users")
                    .IsUnique();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Token).IsRequired();

                entity.HasOne(d => d.LastLogin)
                    .WithOne(p => p.Users)
                    .HasForeignKey<Users>(d => d.LastLoginId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_UserLogins");

                entity.HasOne(d => d.NotificatonToken)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.NotificatonTokenId)
                    .HasConstraintName("FK_Users_NotificationTokens");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
