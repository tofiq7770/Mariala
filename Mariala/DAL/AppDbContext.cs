using Mariala.Models;
using Microsoft.EntityFrameworkCore;

namespace Mariala.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
