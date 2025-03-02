using Infra.Migrations.ModelDbContext;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations.Seeds
{
    [DbContext(typeof(MigrationsDbContext)),
        Migration("20250301181243_Seed_Sources")]
    internal class ArticleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sources",
                columns: ["Name", "CreatedAt", "UpdateAt", "Active"],
                values: ["Test", DateTime.Now.ToString(), DateTime.Now.ToString(), true]
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
