using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class NullColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PatientVitals_Vital_VitalId",
            //    table: "PatientVitals");

            //migrationBuilder.DropIndex(
            //    name: "IX_PatientVitals_VitalId",
            //    table: "PatientVitals");

            migrationBuilder.DropColumn(
                name: "HighLimit",
                table: "Vital");

            migrationBuilder.DropColumn(
                name: "LowLimit",
                table: "Vital");

            //migrationBuilder.AddColumn<int>(
            //    name: "VitalsVitalId",
            //    table: "PatientVitals",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_PatientVitals_VitalsVitalId",
            //    table: "PatientVitals",
            //    column: "VitalsVitalId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PatientVitals_Vital_VitalsVitalId",
            //    table: "PatientVitals",
            //    column: "VitalsVitalId",
            //    principalTable: "Vital",
            //    principalColumn: "VitalId",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PatientVitals_Vital_VitalsVitalId",
            //    table: "PatientVitals");

            //migrationBuilder.DropIndex(
            //    name: "IX_PatientVitals_VitalsVitalId",
            //    table: "PatientVitals");

            //migrationBuilder.DropColumn(
            //    name: "VitalsVitalId",
            //    table: "PatientVitals");

            migrationBuilder.AddColumn<double>(
                name: "HighLimit",
                table: "Vital",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LowLimit",
                table: "Vital",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_PatientVitals_VitalId",
            //    table: "PatientVitals",
            //    column: "VitalId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PatientVitals_Vital_VitalId",
            //    table: "PatientVitals",
            //    column: "VitalId",
            //    principalTable: "Vital",
            //    principalColumn: "VitalId",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
