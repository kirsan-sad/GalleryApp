using Microsoft.EntityFrameworkCore.Migrations;

namespace GalleryApp.Infrastructure.Migrations
{
    public partial class AddUserChangeAuthorOnPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Photos");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AuthorId",
                table: "Photos",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Users_AuthorId",
                table: "Photos",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Users_AuthorId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Photos_AuthorId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
