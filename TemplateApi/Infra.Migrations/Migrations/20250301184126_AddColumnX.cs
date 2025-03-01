using Infra.Migrations.ModelDbContext;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations.Migrations
{
    [DbContext(typeof(MigrationsDbContext)),
        Migration("20250301181243_AddColumnX")]
    internal class AddColumnX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>("X", "Feeds", "nvarchar(50)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("X", "Feeds", "nvarchar(50)");
        }
    }
}
