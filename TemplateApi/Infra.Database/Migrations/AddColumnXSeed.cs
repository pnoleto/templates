using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Database.Migrations
{
    internal class AddColumnXSeed : Migration
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
