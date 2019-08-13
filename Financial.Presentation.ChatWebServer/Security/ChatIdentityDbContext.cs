using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial.Presentation.ChatWebServer.Models
{
    public class ChatIdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ChatIdentityDbContext(DbContextOptions<ChatIdentityDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
