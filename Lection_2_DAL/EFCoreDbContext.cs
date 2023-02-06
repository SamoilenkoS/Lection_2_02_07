using Lection_2_DAL.Entities;
using Lection_2_DAL.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Lection_2_DAL
{
    public class EFCoreDbContext : DbContext
    {
        public DbSet<Point> Locations { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> Clients { get; set; }
        public DbSet<BookRevision> BookRevisions { get; set; }
        public DbSet<LibraryBook> LibraryBooks { get; set; }
        public DbSet<RentBook> RentBooks { get; set; }
        public DbSet<Role> Roles { get; set; }

        public EFCoreDbContext() { }

        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
