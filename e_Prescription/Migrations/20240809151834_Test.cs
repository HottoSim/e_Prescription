using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PatientVitals_Vital_VitalId",
            //    table: "PatientVitals");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Vital",
            //    table: "Vital");

            //migrationBuilder.RenameTable(
            //    name: "Vital",
            //    newName: "Vitals");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Vitals",
            //    table: "Vitals",
            //    column: "VitalId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_PatientVitals_Vitals_VitalId",
            //    table: "PatientVitals",
            //    column: "VitalId",
            //    principalTable: "Vitals",
            //    principalColumn: "VitalId",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropForeignKey(
        //        name: "FK_PatientVitals_Vitals_VitalId",
        //        table: "PatientVitals");

        //    migrationBuilder.DropPrimaryKey(
        //        name: "PK_Vitals",
        //        table: "Vitals");

        //    migrationBuilder.RenameTable(
        //        name: "Vitals",
        //        newName: "Vital");

        //    migrationBuilder.AddPrimaryKey(
        //        name: "PK_Vital",
        //        table: "Vital",
        //        column: "VitalId");

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_PatientVitals_Vital_VitalId",
        //        table: "PatientVitals",
        //        column: "VitalId",
        //        principalTable: "Vital",
        //        principalColumn: "VitalId",
        //        onDelete: ReferentialAction.Cascade);
        //}
    }
}
