using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pera.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SinavTuruEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tur",
                table: "Denemeler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tur",
                table: "Denemeler");
        }
    }
}
