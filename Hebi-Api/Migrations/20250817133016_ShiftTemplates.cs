using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hebi_Api.Migrations
{
    /// <inheritdoc />
    public partial class ShiftTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShiftTemplateId",
                table: "Shifts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShiftTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftTemplates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ShiftTemplateId",
                table: "Shifts",
                column: "ShiftTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_ShiftTemplates_ShiftTemplateId",
                table: "Shifts",
                column: "ShiftTemplateId",
                principalTable: "ShiftTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_ShiftTemplates_ShiftTemplateId",
                table: "Shifts");

            migrationBuilder.DropTable(
                name: "ShiftTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_ShiftTemplateId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "ShiftTemplateId",
                table: "Shifts");
        }
    }
}
