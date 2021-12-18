using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlackHoleCMS.Models;

namespace BlackHoleCMS.Data
{
    public class BlackHoleCmsContext : DbContext
    {
        public BlackHoleCmsContext (DbContextOptions<BlackHoleCmsContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Status> Status { get; set; }
        
        public DbSet<Photo> Photo { get; set; }
    }
}
