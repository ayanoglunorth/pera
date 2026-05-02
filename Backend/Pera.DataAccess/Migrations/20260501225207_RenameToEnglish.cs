using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pera.DataAccess.Migrations
{
    public partial class RenameToEnglish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Rename AspNetUsers columns
            migrationBuilder.RenameColumn(name: "Ad", table: "AspNetUsers", newName: "FirstName");
            migrationBuilder.RenameColumn(name: "Soyad", table: "AspNetUsers", newName: "LastName");
            migrationBuilder.RenameColumn(name: "ProfilResmi", table: "AspNetUsers", newName: "ProfilePicture");

            // 2. Rename Dersler -> Courses
            migrationBuilder.RenameTable(name: "Dersler", newName: "Courses");
            migrationBuilder.RenameColumn(name: "Ad", table: "Courses", newName: "Name");

            // 3. Rename Denemeler -> Exams
            migrationBuilder.RenameTable(name: "Denemeler", newName: "Exams");
            migrationBuilder.RenameColumn(name: "Ad", table: "Exams", newName: "Name");
            migrationBuilder.RenameColumn(name: "Tarih", table: "Exams", newName: "Date");
            migrationBuilder.RenameColumn(name: "Tur", table: "Exams", newName: "Type");
            migrationBuilder.RenameColumn(name: "DersId", table: "Exams", newName: "CourseId");

            migrationBuilder.DropForeignKey(name: "FK_Denemeler_Dersler_DersId", table: "Exams");
            migrationBuilder.DropIndex(name: "IX_Denemeler_DersId", table: "Exams");
            migrationBuilder.CreateIndex(name: "IX_Exams_CourseId", table: "Exams", column: "CourseId");
            migrationBuilder.AddForeignKey(name: "FK_Exams_Courses_CourseId", table: "Exams", column: "CourseId", principalTable: "Courses", principalColumn: "Id");

            // 4. Rename DenemeSonuclar -> ExamResults
            migrationBuilder.RenameTable(name: "DenemeSonuclar", newName: "ExamResults");
            migrationBuilder.RenameColumn(name: "DenemeId", table: "ExamResults", newName: "ExamId");
            migrationBuilder.RenameColumn(name: "OgrenciId", table: "ExamResults", newName: "StudentId");
            migrationBuilder.RenameColumn(name: "TurkceDogru", table: "ExamResults", newName: "TurkishCorrect");
            migrationBuilder.RenameColumn(name: "TurkceYanlis", table: "ExamResults", newName: "TurkishWrong");
            migrationBuilder.RenameColumn(name: "TurkceNet", table: "ExamResults", newName: "TurkishNet");
            migrationBuilder.RenameColumn(name: "MatematikDogru", table: "ExamResults", newName: "MathCorrect");
            migrationBuilder.RenameColumn(name: "MatematikYanlis", table: "ExamResults", newName: "MathWrong");
            migrationBuilder.RenameColumn(name: "MatematikNet", table: "ExamResults", newName: "MathNet");
            migrationBuilder.RenameColumn(name: "FenDogru", table: "ExamResults", newName: "ScienceCorrect");
            migrationBuilder.RenameColumn(name: "FenYanlis", table: "ExamResults", newName: "ScienceWrong");
            migrationBuilder.RenameColumn(name: "FenNet", table: "ExamResults", newName: "ScienceNet");
            migrationBuilder.RenameColumn(name: "InkilapDogru", table: "ExamResults", newName: "HistoryCorrect");
            migrationBuilder.RenameColumn(name: "InkilapYanlis", table: "ExamResults", newName: "HistoryWrong");
            migrationBuilder.RenameColumn(name: "InkilapNet", table: "ExamResults", newName: "HistoryNet");
            migrationBuilder.RenameColumn(name: "DinDogru", table: "ExamResults", newName: "ReligionCorrect");
            migrationBuilder.RenameColumn(name: "DinYanlis", table: "ExamResults", newName: "ReligionWrong");
            migrationBuilder.RenameColumn(name: "DinNet", table: "ExamResults", newName: "ReligionNet");
            migrationBuilder.RenameColumn(name: "Puan", table: "ExamResults", newName: "Score");
            migrationBuilder.RenameColumn(name: "ToplamNet", table: "ExamResults", newName: "TotalNet");

            migrationBuilder.DropForeignKey(name: "FK_DenemeSonuclar_Denemeler_DenemeId", table: "ExamResults");
            migrationBuilder.DropForeignKey(name: "FK_DenemeSonuclar_AspNetUsers_OgrenciId", table: "ExamResults");
            migrationBuilder.DropIndex(name: "IX_DenemeSonuclar_DenemeId", table: "ExamResults");
            migrationBuilder.DropIndex(name: "IX_DenemeSonuclar_OgrenciId", table: "ExamResults");
            migrationBuilder.CreateIndex(name: "IX_ExamResults_ExamId", table: "ExamResults", column: "ExamId");
            migrationBuilder.CreateIndex(name: "IX_ExamResults_StudentId", table: "ExamResults", column: "StudentId");
            migrationBuilder.AddForeignKey(name: "FK_ExamResults_Exams_ExamId", table: "ExamResults", column: "ExamId", principalTable: "Exams", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ExamResults_AspNetUsers_StudentId", table: "ExamResults", column: "StudentId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);

            // 5. Rename Mesajlar -> Messages
            migrationBuilder.RenameTable(name: "Mesajlar", newName: "Messages");
            migrationBuilder.RenameColumn(name: "GondericiId", table: "Messages", newName: "SenderId");
            migrationBuilder.RenameColumn(name: "AliciId", table: "Messages", newName: "ReceiverId");
            migrationBuilder.RenameColumn(name: "Icerik", table: "Messages", newName: "Content");
            migrationBuilder.RenameColumn(name: "Tarih", table: "Messages", newName: "Date");
            migrationBuilder.RenameColumn(name: "OkunduMu", table: "Messages", newName: "IsRead");

            migrationBuilder.DropForeignKey(name: "FK_Mesajlar_AspNetUsers_GondericiId", table: "Messages");
            migrationBuilder.DropForeignKey(name: "FK_Mesajlar_AspNetUsers_AliciId", table: "Messages");
            migrationBuilder.DropIndex(name: "IX_Mesajlar_GondericiId", table: "Messages");
            migrationBuilder.DropIndex(name: "IX_Mesajlar_AliciId", table: "Messages");
            migrationBuilder.CreateIndex(name: "IX_Messages_SenderId", table: "Messages", column: "SenderId");
            migrationBuilder.CreateIndex(name: "IX_Messages_ReceiverId", table: "Messages", column: "ReceiverId");
            migrationBuilder.AddForeignKey(name: "FK_Messages_AspNetUsers_SenderId", table: "Messages", column: "SenderId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(name: "FK_Messages_AspNetUsers_ReceiverId", table: "Messages", column: "ReceiverId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 5. Messages -> Mesajlar
            migrationBuilder.DropForeignKey(name: "FK_Messages_AspNetUsers_SenderId", table: "Messages");
            migrationBuilder.DropForeignKey(name: "FK_Messages_AspNetUsers_ReceiverId", table: "Messages");
            migrationBuilder.DropIndex(name: "IX_Messages_SenderId", table: "Messages");
            migrationBuilder.DropIndex(name: "IX_Messages_ReceiverId", table: "Messages");
            migrationBuilder.RenameColumn(name: "IsRead", table: "Messages", newName: "OkunduMu");
            migrationBuilder.RenameColumn(name: "Date", table: "Messages", newName: "Tarih");
            migrationBuilder.RenameColumn(name: "Content", table: "Messages", newName: "Icerik");
            migrationBuilder.RenameColumn(name: "ReceiverId", table: "Messages", newName: "AliciId");
            migrationBuilder.RenameColumn(name: "SenderId", table: "Messages", newName: "GondericiId");
            migrationBuilder.RenameTable(name: "Messages", newName: "Mesajlar");
            migrationBuilder.CreateIndex(name: "IX_Mesajlar_GondericiId", table: "Mesajlar", column: "GondericiId");
            migrationBuilder.CreateIndex(name: "IX_Mesajlar_AliciId", table: "Mesajlar", column: "AliciId");
            migrationBuilder.AddForeignKey(name: "FK_Mesajlar_AspNetUsers_GondericiId", table: "Mesajlar", column: "GondericiId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(name: "FK_Mesajlar_AspNetUsers_AliciId", table: "Mesajlar", column: "AliciId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Restrict);

            // 4. ExamResults -> DenemeSonuclar
            migrationBuilder.DropForeignKey(name: "FK_ExamResults_Exams_ExamId", table: "ExamResults");
            migrationBuilder.DropForeignKey(name: "FK_ExamResults_AspNetUsers_StudentId", table: "ExamResults");
            migrationBuilder.DropIndex(name: "IX_ExamResults_ExamId", table: "ExamResults");
            migrationBuilder.DropIndex(name: "IX_ExamResults_StudentId", table: "ExamResults");
            migrationBuilder.RenameColumn(name: "TotalNet", table: "ExamResults", newName: "ToplamNet");
            migrationBuilder.RenameColumn(name: "Score", table: "ExamResults", newName: "Puan");
            migrationBuilder.RenameColumn(name: "ReligionNet", table: "ExamResults", newName: "DinNet");
            migrationBuilder.RenameColumn(name: "ReligionWrong", table: "ExamResults", newName: "DinYanlis");
            migrationBuilder.RenameColumn(name: "ReligionCorrect", table: "ExamResults", newName: "DinDogru");
            migrationBuilder.RenameColumn(name: "HistoryNet", table: "ExamResults", newName: "InkilapNet");
            migrationBuilder.RenameColumn(name: "HistoryWrong", table: "ExamResults", newName: "InkilapYanlis");
            migrationBuilder.RenameColumn(name: "HistoryCorrect", table: "ExamResults", newName: "InkilapDogru");
            migrationBuilder.RenameColumn(name: "ScienceNet", table: "ExamResults", newName: "FenNet");
            migrationBuilder.RenameColumn(name: "ScienceWrong", table: "ExamResults", newName: "FenYanlis");
            migrationBuilder.RenameColumn(name: "ScienceCorrect", table: "ExamResults", newName: "FenDogru");
            migrationBuilder.RenameColumn(name: "MathNet", table: "ExamResults", newName: "MatematikNet");
            migrationBuilder.RenameColumn(name: "MathWrong", table: "ExamResults", newName: "MatematikYanlis");
            migrationBuilder.RenameColumn(name: "MathCorrect", table: "ExamResults", newName: "MatematikDogru");
            migrationBuilder.RenameColumn(name: "TurkishNet", table: "ExamResults", newName: "TurkceNet");
            migrationBuilder.RenameColumn(name: "TurkishWrong", table: "ExamResults", newName: "TurkceYanlis");
            migrationBuilder.RenameColumn(name: "TurkishCorrect", table: "ExamResults", newName: "TurkceDogru");
            migrationBuilder.RenameColumn(name: "StudentId", table: "ExamResults", newName: "OgrenciId");
            migrationBuilder.RenameColumn(name: "ExamId", table: "ExamResults", newName: "DenemeId");
            migrationBuilder.RenameTable(name: "ExamResults", newName: "DenemeSonuclar");
            migrationBuilder.CreateIndex(name: "IX_DenemeSonuclar_DenemeId", table: "DenemeSonuclar", column: "DenemeId");
            migrationBuilder.CreateIndex(name: "IX_DenemeSonuclar_OgrenciId", table: "DenemeSonuclar", column: "OgrenciId");
            migrationBuilder.AddForeignKey(name: "FK_DenemeSonuclar_Denemeler_DenemeId", table: "DenemeSonuclar", column: "DenemeId", principalTable: "Denemeler", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_DenemeSonuclar_AspNetUsers_OgrenciId", table: "DenemeSonuclar", column: "OgrenciId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);

            // 3. Exams -> Denemeler
            migrationBuilder.DropForeignKey(name: "FK_Exams_Courses_CourseId", table: "Exams");
            migrationBuilder.DropIndex(name: "IX_Exams_CourseId", table: "Exams");
            migrationBuilder.RenameColumn(name: "CourseId", table: "Exams", newName: "DersId");
            migrationBuilder.RenameColumn(name: "Type", table: "Exams", newName: "Tur");
            migrationBuilder.RenameColumn(name: "Date", table: "Exams", newName: "Tarih");
            migrationBuilder.RenameColumn(name: "Name", table: "Exams", newName: "Ad");
            migrationBuilder.RenameTable(name: "Exams", newName: "Denemeler");
            migrationBuilder.CreateIndex(name: "IX_Denemeler_DersId", table: "Denemeler", column: "DersId");
            migrationBuilder.AddForeignKey(name: "FK_Denemeler_Dersler_DersId", table: "Denemeler", column: "DersId", principalTable: "Dersler", principalColumn: "Id");

            // 2. Courses -> Dersler
            migrationBuilder.RenameColumn(name: "Name", table: "Courses", newName: "Ad");
            migrationBuilder.RenameTable(name: "Courses", newName: "Dersler");

            // 1. AspNetUsers
            migrationBuilder.RenameColumn(name: "ProfilePicture", table: "AspNetUsers", newName: "ProfilResmi");
            migrationBuilder.RenameColumn(name: "LastName", table: "AspNetUsers", newName: "Soyad");
            migrationBuilder.RenameColumn(name: "FirstName", table: "AspNetUsers", newName: "Ad");
        }
    }
}
