using DesafioBackEndManipulae.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEndManipulae.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Video> Videos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
