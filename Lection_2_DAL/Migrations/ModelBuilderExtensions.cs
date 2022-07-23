using Lection_2_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_DAL.Migrations
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = Guid.NewGuid(), Name = "Librarian" },
                new Role { Id = Guid.NewGuid(), Name = "Reader"},
                new Role { Id = Guid.NewGuid(), Name = "Admin" });
        }
    }
}
