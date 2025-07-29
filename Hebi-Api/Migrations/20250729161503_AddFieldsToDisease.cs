using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hebi_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToDisease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Appointment",
                table: "Diseases",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Diseases",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Diseases",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "DiseaseId",
                table: "Appointments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DiseaseId",
                table: "Appointments",
                column: "DiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Diseases_DiseaseId",
                table: "Appointments",
                column: "DiseaseId",
                principalTable: "Diseases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Diseases_DiseaseId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DiseaseId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Appointment",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "DiseaseId",
                table: "Appointments");
        }
    }
}
