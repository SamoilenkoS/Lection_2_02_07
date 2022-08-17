using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_DAL
{
    //public class EFCoreDbContextFactory : IDesignTimeDbContextFactory<EFCoreDbContext>
    //{
    //    private readonly string _connectionString = "Server=tcp:mvc-withoutidentitydbserver.database.windows.net,1433;Initial Catalog=MVC_WithoutIdentity_db;Persist Security Info=False;User ID=ITEA;Password=traTaTa111Boom!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    //    public EFCoreDbContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<EFCoreDbContext>();
    //        optionsBuilder.UseSqlServer(_connectionString);

    //        return new EFCoreDbContext(optionsBuilder.Options);
    //    }
    //}
}
