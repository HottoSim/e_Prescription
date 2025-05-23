﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class NurseIdLo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HealthcareProfessionalUserId",
                table: "Discharges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nurse",
                table: "Discharges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Discharges_HealthcareProfessionalUserId",
                table: "Discharges",
                column: "HealthcareProfessionalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discharges_HealthcareProfessionals_HealthcareProfessionalUserId",
                table: "Discharges",
                column: "HealthcareProfessionalUserId",
                principalTable: "HealthcareProfessionals",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discharges_HealthcareProfessionals_HealthcareProfessionalUserId",
                table: "Discharges");

            migrationBuilder.DropIndex(
                name: "IX_Discharges_HealthcareProfessionalUserId",
                table: "Discharges");

            migrationBuilder.DropColumn(
                name: "HealthcareProfessionalUserId",
                table: "Discharges");

            migrationBuilder.DropColumn(
                name: "Nurse",
                table: "Discharges");
        }
    }
}
