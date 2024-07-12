using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareProfessionals_AspNetUsers_ApplicationUserId",
                table: "HealthcareProfessionals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthcareProfessionals",
                table: "HealthcareProfessionals");

            migrationBuilder.RenameTable(
                name: "HealthcareProfessionals",
                newName: "Surgeons");

            migrationBuilder.RenameIndex(
                name: "IX_HealthcareProfessionals_ApplicationUserId",
                table: "Surgeons",
                newName: "IX_Surgeons_ApplicationUserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseExpiryDate",
                table: "Surgeons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Surgeons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Surgeons",
                table: "Surgeons",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surgeons_AspNetUsers_ApplicationUserId",
                table: "Surgeons",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surgeons_AspNetUsers_ApplicationUserId",
                table: "Surgeons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Surgeons",
                table: "Surgeons");

            migrationBuilder.DropColumn(
                name: "LicenseExpiryDate",
                table: "Surgeons");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Surgeons");

            migrationBuilder.RenameTable(
                name: "Surgeons",
                newName: "HealthcareProfessionals");

            migrationBuilder.RenameIndex(
                name: "IX_Surgeons_ApplicationUserId",
                table: "HealthcareProfessionals",
                newName: "IX_HealthcareProfessionals_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthcareProfessionals",
                table: "HealthcareProfessionals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareProfessionals_AspNetUsers_ApplicationUserId",
                table: "HealthcareProfessionals",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
