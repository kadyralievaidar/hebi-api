using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hebi_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexForSkippedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Diseases",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShiftTemplates_ClinicId",
                table: "ShiftTemplates",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_ClinicId",
                table: "Clinics",
                column: "ClinicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShiftTemplates_ClinicId",
                table: "ShiftTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Clinics_ClinicId",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Diseases");
        }
    }
}
