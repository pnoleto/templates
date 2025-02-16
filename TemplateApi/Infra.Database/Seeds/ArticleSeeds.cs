using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Database.Seeds
{
    internal class ArticleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sources",
                columns: ["Name", "CreatedAt", "UpdateAt", "Active"],
                values: [
                    "Test",
                    DateTime.Now.ToString(),
                    DateTime.Now.ToString(),
                    true
                ]
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Sources");
        }
    }
}
