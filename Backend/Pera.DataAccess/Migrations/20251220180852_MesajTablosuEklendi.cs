using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pera.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MesajTablosuEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mesajlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GondericiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AliciId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OkunduMu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesajlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mesajlar_AspNetUsers_AliciId",
                        column: x => x.AliciId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mesajlar_AspNetUsers_GondericiId",
                        column: x => x.GondericiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_AliciId",
                table: "Mesajlar",
                column: "AliciId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_GondericiId",
                table: "Mesajlar",
                column: "GondericiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mesajlar");
        }
    }
}
