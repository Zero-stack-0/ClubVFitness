using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Data.Repository
{
    public class GymVDbContext : DbContext
    {
        public GymVDbContext(DbContextOptions<GymVDbContext> options)
            : base(options)
        {
        }

        //Resgiter tables
        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Role { get; set; }
    }
}