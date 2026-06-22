using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteProject.Migrations
{
    /// <inheritdoc />
    public partial class AddDiaryImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiaryImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaryId = table.Column<int>(type: "int", nullable: false),
                    DiaryId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiaryImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaryImages_Diaries_DiaryId1",
                        column: x => x.DiaryId1,
                        principalTable: "Diaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiaryImages_DiaryImages_DiaryImageId",
                        column: x => x.DiaryImageId,
                        principalTable: "DiaryImages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiaryImages_DiaryId1",
                table: "DiaryImages",
                column: "DiaryId1");

            migrationBuilder.CreateIndex(
                name: "IX_DiaryImages_DiaryImageId",
                table: "DiaryImages",
                column: "DiaryImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaryImages");
        }
    }
}
