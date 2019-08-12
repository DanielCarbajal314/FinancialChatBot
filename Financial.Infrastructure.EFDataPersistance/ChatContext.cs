using Financial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Financial.Infrastructure.EFDataPersistance
{
    public class ChatContext:DbContext
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
