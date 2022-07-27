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
                new Role { Id = Guid.Parse("BF2A9E3D-13C2-45E7-8C99-4E7456ECC2E5"), Name = "Librarian" },
                new Role { Id = Guid.Parse("F6EB3E37-2F32-43A4-B662-215693C51490"), Name = "Reader"},
                new Role { Id = Guid.Parse("97F735C2-BD0A-42C4-A0C0-12239B5248B3"), Name = "Admin" });
        }
    }
}
