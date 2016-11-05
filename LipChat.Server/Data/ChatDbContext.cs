using LipChat.Library.Models;
using LipChat.Server.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LipChat.Server.Data
{
    public class ChatDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public ChatDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChatDbContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .Property(p => p.MessageID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}