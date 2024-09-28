using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodCat.App.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDaySummaryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DaySummaryId",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Happiness",
                table: "Notes",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DaysSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SummaryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientGeneralFunctioning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalPatientGeneralFunctioning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Interests = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalInterests = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialRelationships = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalSocialRelationships = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Work = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalWork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalFamily = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysicalHealth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalPhysicalHealth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalMemories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportedProblems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalReportedProblems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaysSummaries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_DaySummaryId",
                table: "Notes",
                column: "DaySummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_DaysSummaries_UserId",
                table: "DaysSummaries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_DaysSummaries_DaySummaryId",
                table: "Notes",
                column: "DaySummaryId",
                principalTable: "DaysSummaries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_DaysSummaries_DaySummaryId",
                table: "Notes");

            migrationBuilder.DropTable(
                name: "DaysSummaries");

            migrationBuilder.DropIndex(
                name: "IX_Notes_DaySummaryId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "DaySummaryId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Happiness",
                table: "Notes");
        }
    }
}
