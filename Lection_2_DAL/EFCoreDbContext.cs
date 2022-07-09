using Lection_2_DAL.Entities;
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

        protected EFCoreDbContext()
        {
        }

        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
