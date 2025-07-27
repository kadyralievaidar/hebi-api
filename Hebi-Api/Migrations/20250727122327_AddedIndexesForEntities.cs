using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hebi_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexesForEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ClinicId",
                table: "Shifts",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientCard_ClinicId",
                table: "PatientCard",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Diseases_ClinicId",
                table: "Diseases",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClinicId",
                table: "Appointments",
                column: "ClinicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shifts_ClinicId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_PatientCard_ClinicId",
                table: "PatientCard");

            migrationBuilder.DropIndex(
                name: "IX_Diseases_ClinicId",
                table: "Diseases");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ClinicId",
                table: "Appointments");
        }
    }
}
