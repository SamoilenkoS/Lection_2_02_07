using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lection_2_DAL.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("bf2a9e3d-13c2-45e7-8c99-4e7456ecc2e5"), "Librarian" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("f6eb3e37-2f32-43a4-b662-215693c51490"), "Reader" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("97f735c2-bd0a-42c4-a0c0-12239b5248b3"), "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("97f735c2-bd0a-42c4-a0c0-12239b5248b3"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bf2a9e3d-13c2-45e7-8c99-4e7456ecc2e5"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f6eb3e37-2f32-43a4-b662-215693c51490"));
        }
    }
}
