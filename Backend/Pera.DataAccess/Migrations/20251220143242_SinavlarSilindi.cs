using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pera.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SinavlarSilindi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SinavSonuclar");

            migrationBuilder.DropTable(
                name: "Sinavlar");

            migrationBuilder.RenameColumn(
                name: "DersId",
                table: "Dersler",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "DersId",
                table: "Denemeler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Denemeler_DersId",
                table: "Denemeler",
                column: "DersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Denemeler_Dersler_DersId",
                table: "Denemeler",
                column: "DersId",
                principalTable: "Dersler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Denemeler_Dersler_DersId",
                table: "Denemeler");

            migrationBuilder.DropIndex(
                name: "IX_Denemeler_DersId",
                table: "Denemeler");

            migrationBuilder.DropColumn(
                name: "DersId",
                table: "Denemeler");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Dersler",
                newName: "DersId");

            migrationBuilder.CreateTable(
                name: "Sinavlar",
                columns: table => new
                {
                    SinavId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DersId = table.Column<int>(type: "int", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sinavlar", x => x.SinavId);
                    table.ForeignKey(
                        name: "FK_Sinavlar_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "DersId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SinavSonuclar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SinavId = table.Column<int>(type: "int", nullable: false),
                    OgrenciId = table.Column<int>(type: "int", nullable: false),
                    Puan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TamPuan = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinavSonuclar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SinavSonuclar_Sinavlar_SinavId",
                        column: x => x.SinavId,
                        principalTable: "Sinavlar",
                        principalColumn: "SinavId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sinavlar_DersId",
                table: "Sinavlar",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_SinavSonuclar_SinavId",
                table: "SinavSonuclar",
                column: "SinavId");
        }
    }
}
