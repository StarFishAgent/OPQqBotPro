using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GenshinBotCore.Entities
{
    public partial class GenShinBotContext : DbContext
    {
        public GenShinBotContext()
        {
        }

        public GenShinBotContext(DbContextOptions<GenShinBotContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Pictures> Pictures { get; set; } = null!;
        public virtual DbSet<Users> Users { get; set; } = null!;
        public virtual DbSet<UsersComment> UsersComment { get; set; } = null!;
        public virtual DbSet<UsersCookies> UsersCookies { get; set; } = null!;
        public virtual DbSet<UsersSecret> UsersSecret { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=GenShinBot.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pictures>(entity =>
            {
                entity.Property(e => e.Picture).HasColumnName("Picture");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.QQ).HasColumnName("QQ");
            });

            modelBuilder.Entity<UsersComment>(entity =>
            {
                entity.ToTable("UsersComment");

                entity.Property(e => e.QQ).HasColumnName("QQ");
            });

            modelBuilder.Entity<UsersCookies>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersCookies)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UsersSecret>(entity =>
            {
                entity.ToTable("UsersSecret");

                entity.HasIndex(e => e.UserId, "IX_UsersSecret_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersSecrets)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
