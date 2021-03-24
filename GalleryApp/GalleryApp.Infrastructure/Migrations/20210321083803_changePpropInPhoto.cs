using Microsoft.EntityFrameworkCore.Migrations;

namespace GalleryApp.Infrastructure.Migrations
{
    public partial class changePpropInPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Format",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Photos",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Photos",
                newName: "Path");

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
