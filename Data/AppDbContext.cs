using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace API.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
    }
    
}
