using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAPI.Models
{
    public class VitunDatabaseContext : DbContext
    {
        public VitunDatabaseContext(DbContextOptions<VitunDatabaseContext> options) : base(options) { }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
