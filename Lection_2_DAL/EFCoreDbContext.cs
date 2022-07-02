using Microsoft.EntityFrameworkCore;

namespace Lection_2_DAL
{
    public class EFCoreDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
